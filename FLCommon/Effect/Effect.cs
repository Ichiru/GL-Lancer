using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace FLCommon
{
	public class Effect : IDisposable
	{
		const uint MAGIC = 0xAFECDE03;
		const byte SOURCE_VERTEX = 0;
		const byte SOURCE_FRAGMENT = 1;
		//glsl caching
		static Dictionary<string,int> glslCompiled = new Dictionary<string, int> ();

		static int CompileGLSL (string hash, string source, ShaderType type)
		{
			if (!glslCompiled.ContainsKey (hash)) {
				int status_code;
				string info;
				int obj = GL.CreateShader (type);
				GL.ShaderSource (obj, source);
				GL.CompileShader (obj);
				GL.GetShaderInfoLog (obj, out info);
				GL.GetShader (obj, ShaderParameter.CompileStatus, out status_code);
				if (status_code != 1)
					throw new ApplicationException (info);
				glslCompiled.Add (hash, obj);
			}
			return glslCompiled [hash];
		}

		Dictionary<string, Program> programs = new Dictionary<string, Program> ();
		Dictionary<string, Uniform> uniforms = new Dictionary<string, Uniform> ();
		Program activeProgram;
		bool isDisposed = false;

		public GraphicsDevice GraphicsDevice {
			get;
			private set;
		}


		public bool IsDisposed {
			get {
				return isDisposed;
			}
		}

		public string CurrentTechnique {
			get {
				return activeProgram.Name;
			}
			set {
				if (value != activeProgram.Name) {
					activeProgram = programs [value];
					UpdateUniforms ();
				}
			}
		}
		static StringHack strhack = new StringHack ();
		public StringHack Techniques
		{
			get {
				return strhack;
			}
		}
		//very dirty hack for XNA-style syntax
		public class StringHack 
		{
			public string this[string index] {
				get {
					return index;
				}
			}
		}

		public Effect (GraphicsDevice device, string filename)
		{
			GraphicsDevice = device;
			ProgramDescription[] descriptions;
			string[] sources;
			string[] hashes;
			ShaderType[] types;
			List<UniformDescription>[] uniformDescriptions;
			using (var reader = new BinaryReader(File.OpenRead(filename))) {
				if (reader.ReadUInt32 () != MAGIC)
					throw new Exception ("Not a valid effect file");
				var srcLength = reader.ReadUInt16 ();
				types = new ShaderType[srcLength];
				sources = new string[srcLength];
				hashes = new string[srcLength];
				uniformDescriptions = new List<UniformDescription>[srcLength];
				for (int i = 0; i < srcLength; i++) {
					types [i] = reader.ReadByte () == SOURCE_VERTEX ? ShaderType.VertexShader : ShaderType.FragmentShader;
					sources [i] = reader.ReadString ();
					hashes [i] = reader.ReadString ();
					int uniformsCount = reader.ReadUInt16 ();
					uniformDescriptions [i] = new List<UniformDescription> (uniformsCount);
					for (int j = 0; j < uniformsCount; j++) {
						var name = reader.ReadString ();
						var type = (UniformTypes)reader.ReadByte ();
						var u = new UniformDescription (name, type);
						if (type == UniformTypes.Array) {
							u.ArrayType = (UniformTypes)reader.ReadByte();
							u.ArrayLength = reader.ReadInt32 ();
						}
						uniformDescriptions [i].Add (u);
					}
				}
				var progLength = reader.ReadUInt16 ();
				descriptions = new ProgramDescription[progLength];
				for (int i = 0; i < progLength; i++) {
					descriptions [i] = new ProgramDescription (reader.ReadString (), reader.ReadUInt16 (), reader.ReadUInt16 ());
				}
			}
			for (int i = 0; i < descriptions.Length; i++) {
				var compiled = new Program ();
				compiled.Name = descriptions [i].Name;
				int vsID = CompileGLSL (hashes [descriptions [i].VSIndex], sources [descriptions [i].VSIndex], types [descriptions [i].VSIndex]);
				int fsID = CompileGLSL (hashes [descriptions [i].FSIndex], sources [descriptions [i].FSIndex], types [descriptions [i].FSIndex]);
				compiled.ID = GL.CreateProgram ();
				GL.AttachShader (compiled.ID, fsID);
				GL.AttachShader (compiled.ID, vsID);
				GL.LinkProgram (compiled.ID);
				int status_code;
				GL.GetProgram (compiled.ID, GetProgramParameterName.LinkStatus, out status_code);
				if (status_code != 1) {
					throw new Exception (GL.GetProgramInfoLog (compiled.ID));
				}
				for (int j = 0; j < uniformDescriptions[descriptions[i].VSIndex].Count; j++) {
					var uniform = uniformDescriptions[descriptions[i].VSIndex][j];
					int loc = GL.GetUniformLocation (compiled.ID, uniform.Name);
					if (uniforms.ContainsKey (uniform.Name)) {
						if (uniforms [uniform.Name].Description != uniform)
							throw new Exception ("Conflicting uniforms");
					} else {
						if (uniform.Type == UniformTypes.Array) {
							var u = new Uniform (uniform);
							u.Value = new object[uniform.ArrayLength];
							uniforms.Add (uniform.Name, u);
						} else {
							uniforms.Add (uniform.Name, new Uniform (uniform));
						}
					}
					if (loc > 0) {
						compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform, loc,
						                                                  uniform.Type == UniformTypes.Sampler2D ||
						                                                  uniform.Type == UniformTypes.SamplerCube));
					} else {
						Console.WriteLine ("Warning: Unused uniform {0}", uniform.Name);
					}
				}
				for (int j = 0; j < uniformDescriptions[descriptions[i].FSIndex].Count; j++) {
					var uniform = uniformDescriptions[descriptions[i].FSIndex][j];
					int loc = GL.GetUniformLocation (compiled.ID, uniform.Name);
					if (uniforms.ContainsKey (uniform.Name)) {
						if (uniforms [uniform.Name].Description != uniform)
							throw new Exception ("Conflicting uniforms");
					} else {
						if (uniform.Type == UniformTypes.Array) {
							var u = new Uniform (uniform);
							u.Value = new object[uniform.ArrayLength];
							uniforms.Add (uniform.Name, u);
						} else {
							uniforms.Add (uniform.Name, new Uniform (uniform));
						}
					}
					if (loc > 0) {
						compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform, loc,
						                                                    uniform.Type == UniformTypes.Sampler2D ||
						                                                    uniform.Type == UniformTypes.SamplerCube));
					} else {
						Console.WriteLine ("Warning: Unused uniform {0}", uniform.Name);
					}
				}
				compiled.SetTextureUniforms ();
				programs.Add (compiled.Name, compiled);
			}
		}

		void InternalSetParameter(string name, UniformTypes type, object value)
		{
			if (uniforms [name].Description.Type != type)
				throw new InvalidDataException ();
			uniforms [name].Value = value;
			if (activeProgram != null)
				activeProgram.SetUniform (name, value);
		}

		void InternalSetArrayParameter (string name,UniformTypes type,int index, object value)
		{
			if (uniforms [name].Description.Type != UniformTypes.Array)
				throw new InvalidOperationException ();
			if (index >= uniforms [name].Description.ArrayLength)
				throw new IndexOutOfRangeException ();
			if (uniforms [name].Description.ArrayType != type)
				throw new InvalidDataException ();
			((object[])uniforms [name].Value) [index] = value;
			if (activeProgram != null)
				activeProgram.SetArrayUniform (name, index, value);
		}
		public void SetParameter(string name, Vector4 vec)
		{
			InternalSetParameter (name, UniformTypes.Vector4, vec);
		}

		public void SetParameter (string name, Matrix4 mat)
		{
			InternalSetParameter (name, UniformTypes.Matrix4, mat);
		}

		public void SetParameter (string name, int i)
		{
			InternalSetParameter (name, UniformTypes.Int, i);
		}

		public void SetParameter (string name, Vector3 vec)
		{
			InternalSetParameter (name, UniformTypes.Vector3, vec);
		}

		public void SetParameter (string name, float f)
		{
			InternalSetParameter (name, UniformTypes.Float, f);
		}

		public void SetParameter (string name, Texture tex)
		{
			if (tex is Texture2D)
				SetParameter (name, (Texture2D)tex);
			else
				SetParameter (name, (TextureCube)tex);
		}

		public void SetParameter (string name, Texture2D tex)
		{
			InternalSetParameter (name, UniformTypes.Sampler2D, tex);
		}

		public void SetParameter (string name, TextureCube tex)
		{
			InternalSetParameter (name, UniformTypes.SamplerCube, tex);
		}

		public void SetArrayParameter (string name, int index, Vector4 vec)
		{
			InternalSetArrayParameter (name, UniformTypes.Vector4, index, vec);
		}

		public void SetArrayParameter (string name, int index, Matrix4 mat)
		{
			InternalSetArrayParameter (name, UniformTypes.Matrix4, index, mat);
		}

		public void SetArrayParameter (string name, int index, int i)
		{
			InternalSetArrayParameter (name, UniformTypes.Int, index, i);
		}

		public void SetArrayParameter (string name, int index, Vector3 vec)
		{
			InternalSetArrayParameter (name, UniformTypes.Vector3, index, vec);
		}

		public void SetArrayParameter (string name, int index, float f)
		{
			InternalSetArrayParameter (name, UniformTypes.Float, index, f);
		}

		public void Dispose()
		{
			if (!isDisposed) {
				foreach (var program in programs.Values) {
					GL.DeleteProgram (program.ID);
				}
				programs = null;
				activeProgram = null;
				isDisposed = true;
			}
		}
		public void Apply ()
		{
			activeProgram.ApplyTextures ();
		}

		void UpdateUniforms ()
		{
			foreach (var k in uniforms.Keys) {
				if (uniforms [k].Value != null && activeProgram.Uniforms.ContainsKey (k)) {
					if (uniforms [k].Description.Type == UniformTypes.Array) {
						var array = (object[])uniforms [k].Value;
						for (int i = 0; i < array.Length; i++) {
							if (array [i] != null) {
								activeProgram.SetArrayUniform (k, i, array [i]);
							}
						}
					} else {
						activeProgram.SetUniform (k, uniforms [k].Value);
					}
				}
			}
		}

		class Uniform
		{
			public UniformDescription Description;
			public object Value;

			public Uniform (UniformDescription description)
			{
				Description = description;
				Value = null;
			}
		}

		struct ProgramDescription
		{
			public string Name;
			public int VSIndex;
			public int FSIndex;

			public ProgramDescription (string name, int vs, int fs)
			{
				Name = name;
				VSIndex = vs;
				FSIndex = fs;
			}
		}
	}
}