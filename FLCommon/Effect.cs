using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace FLCommon
{
	public class Effect
	{
		const uint MAGIC = 0xAFECDE03;
		const byte SOURCE_VERTEX = 0;
		const byte SOURCE_FRAGMENT = 1;
		//glsl caching
		static Dictionary<string,int> glslCompiled = new Dictionary<string, int>();
		static int CompileGLSL(string hash, string source, ShaderType type)
		{
			if (!glslCompiled.ContainsKey (hash)) {
				int status_code;
				string info;
				int obj = GL.CreateShader (type);
				GL.ShaderSource (obj, source);
				GL.CompileShader (obj);
				GL.GetShaderInfoLog(obj, out info);
				GL.GetShader(obj, ShaderParameter.CompileStatus, out status_code);
				if (status_code != 1)
					throw new ApplicationException(info);
				glslCompiled.Add (hash, obj);
			}
			return glslCompiled [hash];
		}
		Dictionary<string, Program> programs = new Dictionary<string, Program>();
		Dictionary<string, Uniform> uniforms = new Dictionary<string, Uniform>();
		Program activeProgram;
		public string ActiveProgram {
			get {
				return activeProgram.Name;
			} set {
				if (value != activeProgram.Name) {
					activeProgram = programs [value];
					UpdateUniforms ();
				}
			}
		}
		public Effect(string filename)
		{
			ProgramDescription[] descriptions;
			string[] sources;
			string[] hashes;
			ShaderType[] types;
			using (var reader = new BinaryReader(File.OpenRead(filename))) {
				if (reader.ReadUInt32 () != MAGIC)
					throw new Exception ("Not a valid effect file");
				var srcLength = reader.ReadUInt16 ();
				types = new byte[srcLength];
				sources = new string[srcLength];
				hashes = new string[srcLength];
				for (int i = 0; i < srcLength; i++) {
					types [i] = reader.ReadByte () == SOURCE_VERTEX ? ShaderType.VertexShader : ShaderType.FragmentShader;
					sources [i] = reader.ReadString ();
					hashes [i] = reader.ReadString ();
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
				int vsID = CompileGLSL (hashes [descriptions[i].VSIndex], sources [descriptions[i].VSIndex], types [descriptions[i].VSIndex]);
				int fsID = CompileGLSL (hashes [descriptions[i].FSIndex], sources [descriptions[i].FSIndex], types [descriptions[i].FSIndex]);
				compiled.ID = GL.CreateProgram();
				GL.AttachShader(compiled.ID, fsID);
				GL.AttachShader(compiled.ID, vsID);
				GL.LinkProgram(compiled);
				int numActiveUniforms = 0;
				int maxActiveUniformLength = 0;
				GL.GetProgram (compiled.ID, GetProgramParameterName.ActiveUniforms, out numActiveUniforms);
				GL.GetProgram (compiled.ID, GetProgramParameterName.ActiveUniformMaxLength, out numActiveUniforms);
				StringBuilder name = new StringBuilder(2 * maxActiveUniformLength);
				for(int j = 0; j < numActiveUniforms; j++) {
					int size;
					ActiveUniformType type;
					GL.GetActiveUniform(compiled.ID, j, out size, out type);
					var uniform = new Uniform (name.ToString (), type);
					if(!uniforms.ContainsKey(uniform))
						uniforms.Add (uniform.Name, uniform);
					compiled.Uniforms.Add (uniform.Name, new GLUniform (uniform.Name, j, uniform.Value));
					name.Clear ();
				}
				programs.Add (compiled.Name, compiled);
			}
		}
		void UpdateUniforms()
		{

		}
		class Program
		{
			public string Name;
			public int ID;
			public Dictionary<string, GLUniform> Uniforms = new Dictionary<string, GLUniform>();
		}
		class GLUniform
		{
			public string Name;
			public int Index;
			public object Value;
			public GLUniform(string name, int index, object value)
			{
				Name = name;
				Index = index;
				Value = value;
			}
		}
		class Uniform
		{
			public string Name;
			public ActiveUniformType Type;
			public object Value;
			public Uniform(string name, ActiveUniformType type)
			{
				Name = name;
				Type = type;
				Value = type.GetDefault();
			}
		}
		struct ProgramDescription
		{
			public string Name;
			public int VSIndex;
			public int FSIndex;
			public ProgramDescription(string name, int vs, int fs)
			{
				Name = name;
				VSIndex = vs;
				FSIndex = fs;
			}
		}
	}
}