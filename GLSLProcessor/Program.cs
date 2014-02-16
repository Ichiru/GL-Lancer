using System;
using System.Collections.Generic;
using System.IO;
namespace GLSLProcessor
{
	class MainClass
	{
		static void PrintUsage()
		{
			Console.WriteLine ("Usage GLSLProcessor.exe input output");
		}
		public static int Main (string[] args)
		{
			if (args.Length != 2) {
				Console.WriteLine ("Error: Wrong number of arguments");
				PrintUsage ();
				return 1;
			}
			if (!File.Exists (args [0])) {
				Console.WriteLine ("Couldn't find file {0}", args [0]);
				return 2;
			}
			var ini = new ShaderIni (args [0]);
			var output = new ShaderOutput ();
			output.Sources = new string[ini.ShaderPaths.Count];
			output.SourceTypes = ini.ShaderTypes.ToArray ();
			output.UniformLists = new List<Uniform>[ini.ShaderPaths.Count];
			output.Programs = new ShaderOutput.Program[ini.Programs.Count];
			for (int i = 0; i < ini.ShaderPaths.Count; i++) {
				var path = Path.Combine (Path.GetDirectoryName (args [0]), ini.ShaderPaths [i]);
				var preprocessed = Preprocessor.Preprocess (File.ReadAllText (path), path);
				output.UniformLists[i] = UniformParser.FindUniforms (preprocessed);
				output.Sources [i] = preprocessed;
			}
			for (int i = 0; i < ini.Programs.Count; i++) {
				var source = ini.Programs [i];
				output.Programs [i] = new ShaderOutput.Program ();
				output.Programs [i].Name = source.Name;
				output.Programs [i].VSIndex = ini.ShaderPaths.IndexOf (source.VertexShader);
				output.Programs [i].FSIndex = ini.ShaderPaths.IndexOf (source.FragmentShader);
			}
			output.Save (args [1]);
			return 0;
		}
	}
}
