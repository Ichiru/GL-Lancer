using System;
using OpenTK;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
namespace FLCommon
{
	internal class Effect
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

		public Dictionary<string, Program> Programs = new Dictionary<string, Program> ();
		public Dictionary<string, Uniform> Uniforms = new Dictionary<string, Uniform> ();

		bool isDisposed = false;

		public Effect (string filename)
		{
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
					if (Uniforms.ContainsKey (uniform.Name)) {
						if (Uniforms [uniform.Name].Description != uniform) {
							throw new Exception ("Conflicting uniforms");
						}
					} else {
						if (uniform.Type == GLSLTypes.Array) {
							var u = new Uniform (uniform);
							u.Value = new object[uniform.ArrayLength];
							Uniforms.Add (uniform.Name, u);
						} else {
							Uniforms.Add (uniform.Name, new Uniform (uniform));
						}
					}
					if (loc > 0) {
						compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform, loc,
							uniform.Type == GLSLTypes.Sampler2D ||
							uniform.Type == GLSLTypes.SamplerCube));
					} else {
						//Console.WriteLine ("Warning: Unused uniform {0}", uniform.Name);
					}
				}
				for (int j = 0; j < uniformDescriptions[descriptions[i].FSIndex].Count; j++) {
					var uniform = uniformDescriptions[descriptions[i].FSIndex][j];
					int loc = GL.GetUniformLocation (compiled.ID, uniform.Name);
					if (Uniforms.ContainsKey (uniform.Name)) {
						if (Uniforms [uniform.Name].Description != uniform)
							throw new Exception ("Conflicting uniforms");
					} else {
						if (uniform.Type == GLSLTypes.Array) {
							var u = new Uniform (uniform);
							u.Value = new object[uniform.ArrayLength];
							Uniforms.Add (uniform.Name, u);
						} else {
							Uniforms.Add (uniform.Name, new Uniform (uniform));
						}
					}
					if (loc > 0) {
						compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform, loc,
							uniform.Type == GLSLTypes.Sampler2D ||
							uniform.Type == GLSLTypes.SamplerCube));
					} else {
						//Console.WriteLine ("Warning: Unused uniform {0}", uniform.Name);
					}
				}
				GL.UseProgram (compiled.ID);
				compiled.SetTextureUniforms ();
				compiled.Attributes = Attributes.GetVertexAttributes (compiled.ID);
				Programs.Add (compiled.Name, compiled);
			}
		}

		public class Uniform
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

