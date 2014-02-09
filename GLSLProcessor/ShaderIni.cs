using System;
using System.Collections.Generic;
using FLParser.Ini;
using FLParser;
namespace GLSLProcessor
{
	public class ShaderIni : IniFile
	{
		public List<string> ShaderPaths;
		public List<ProgramDescription> Programs;
		public ShaderIni (string path)
		{
			var sections = parseFile (path);
			ShaderPaths = new List<string> ();
			Programs = new List<ProgramDescription> ();
			foreach (var section in sections) {
				if (section.Name.ToUpperInvariant () != "PROGRAM") {
					throw new FileContentException (path, "Unexpected section {0}", section.Name);
				}
				var desc = new ProgramDescription ();
				desc.Name = section ["name"] [0].ToString ();
				desc.VertexShader = section ["vertex_shader"] [0].ToString ();
				desc.FragmentShader = section ["fragment_shader"] [0].ToString ();
				Programs.Add (desc);
			}
			foreach (var desc in Programs) {
				if (!ShaderPaths.Contains (desc.FragmentShader))
					ShaderPaths.Add (desc.FragmentShader);
				if (!ShaderPaths.Contains (desc.VertexShader))
					ShaderPaths.Add (desc.VertexShader);
			}
		}
		public class ProgramDescription
		{
			public string Name;
			public string FragmentShader;
			public string VertexShader;
		}
	}
}

