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
		internal Program ActiveProgram;
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
				return ActiveProgram.Name;
			}
			set {
				if (value != ActiveProgram.Name) {
					ActiveProgram = programs [value];
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
						var type = (GLSLTypes)reader.ReadByte ();
						var u = new UniformDescription (name, type);
						if (type == GLSLTypes.Array) {
							u.ArrayType = (GLSLTypes)reader.ReadByte();
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
						if (uniforms [uniform.Name].Description != uniform) {
							throw new Exception ("Conflicting uniforms");
						}
					} else {
						if (uniform.Type == GLSLTypes.Array) {
							var u = new Uniform (uniform);
							u.Value = new object[uniform.ArrayLength];
							uniforms.Add (uniform.Name, u);
						} else {
							uniforms.Add (uniform.Name, new Uniform (uniform));
						}
					}
					if (loc > 0) {
						compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform, loc,
						                                                  uniform.Type == GLSLTypes.Sampler2D ||
						                                                  uniform.Type == GLSLTypes.SamplerCube));
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
						if (uniform.Type == GLSLTypes.Array) {
							var u = new Uniform (uniform);
							u.Value = new object[uniform.ArrayLength];
							uniforms.Add (uniform.Name, u);
						} else {
							uniforms.Add (uniform.Name, new Uniform (uniform));
						}
					}
					if (loc > 0) {
						compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform, loc,
						                                                    uniform.Type == GLSLTypes.Sampler2D ||
						                                                    uniform.Type == GLSLTypes.SamplerCube));
					} else {
						Console.WriteLine ("Warning: Unused uniform {0}", uniform.Name);
					}
				}
				compiled.SetTextureUniforms ();
				compiled.Attributes = Attributes.GetVertexAttributes (compiled.ID);
				programs.Add (compiled.Name, compiled);
			}
		}

		void InternalSetParameter(string name, GLSLTypes type, object value)
		{
			if (uniforms [name].Description.Type != type)
				throw new InvalidDataException ();
			uniforms [name].Value = value;
			if (ActiveProgram != null)
				ActiveProgram.SetUniform (name, value);
		}

		void InternalSetArrayParameter (string name,GLSLTypes type,int index, object value)
		{
			if (uniforms [name].Description.Type != GLSLTypes.Array)
				throw new InvalidOperationException ();
			if (index >= uniforms [name].Description.ArrayLength)
				throw new IndexOutOfRangeException ();
			if (uniforms [name].Description.ArrayType != type)
				throw new InvalidDataException ();
			((object[])uniforms [name].Value) [index] = value;
			if (ActiveProgram != null)
				ActiveProgram.SetArrayUniform (name, index, value);
		}
		public void SetParameter(string name, Vector4 vec)
		{
			InternalSetParameter (name, GLSLTypes.Vector4, vec);
		}

		public void SetParameter (string name, Matrix4 mat)
		{
			InternalSetParameter (name, GLSLTypes.Matrix4, mat);
		}

		public void SetParameter (string name, int i)
		{
			InternalSetParameter (name, GLSLTypes.Int, i);
		}

		public void SetParameter (string name, Vector3 vec)
		{
			InternalSetParameter (name, GLSLTypes.Vector3, vec);
		}

		public void SetParameter (string name, float f)
		{
			InternalSetParameter (name, GLSLTypes.Float, f);
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
			InternalSetParameter (name, GLSLTypes.Sampler2D, tex);
		}

		public void SetParameter (string name, TextureCube tex)
		{
			InternalSetParameter (name, GLSLTypes.SamplerCube, tex);
		}

		public void SetArrayParameter (string name, int index, Vector4 vec)
		{
			InternalSetArrayParameter (name, GLSLTypes.Vector4, index, vec);
		}

		public void SetArrayParameter (string name, int index, Matrix4 mat)
		{
			InternalSetArrayParameter (name, GLSLTypes.Matrix4, index, mat);
		}

		public void SetArrayParameter (string name, int index, int i)
		{
			InternalSetArrayParameter (name, GLSLTypes.Int, index, i);
		}

		public void SetArrayParameter (string name, int index, Vector3 vec)
		{
			InternalSetArrayParameter (name, GLSLTypes.Vector3, index, vec);
		}

		public void SetArrayParameter (string name, int index, float f)
		{
			InternalSetArrayParameter (name, GLSLTypes.Float, index, f);
		}

		public void Dispose()
		{
			if (!isDisposed) {
				foreach (var program in programs.Values) {
					GL.DeleteProgram (program.ID);
				}
				programs = null;
				ActiveProgram = null;
				isDisposed = true;
			}
		}
		public void Apply ()
		{
			ActiveProgram.ApplyTextures ();
			GraphicsDevice.CurrentEffect = this;
		}

		void UpdateUniforms ()
		{
			foreach (var k in uniforms.Keys) {
				if (uniforms [k].Value != null && ActiveProgram.Uniforms.ContainsKey (k)) {
					if (uniforms [k].Description.Type == GLSLTypes.Array) {
						var array = (object[])uniforms [k].Value;
						for (int i = 0; i < array.Length; i++) {
							if (array [i] != null) {
								ActiveProgram.SetArrayUniform (k, i, array [i]);
							}
						}
					} else {
						ActiveProgram.SetUniform (k, uniforms [k].Value);
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