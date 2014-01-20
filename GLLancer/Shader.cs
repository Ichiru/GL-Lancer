using System;
using System.Text.RegularExpressions;
using System.IO;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
namespace GLLancer
{
	public class Shader
	{
		#region Text Processing
		static Regex includeRegex = new Regex (@"\@include '(.*)'", RegexOptions.Compiled);
		static string blockComments = @"/\*(.*?)\*/";
		static string lineComments = @"//(.*?)\r?\n";
		static string strings = @"""((\\[^\n]|[^""\n])*)""";
		static string verbatimStrings = @"@(""[^""]*"")+";
		static string StripComments (string text)
		{
			string noComments = Regex.Replace(text,
			                                  blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
			                                  me => {
				if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
					return me.Value.StartsWith("//") ? Environment.NewLine : "";
				// Keep the literal strings
				return me.Value;
			},
			RegexOptions.Singleline);
			return noComments;
		}
		static string FindIncludes(string text)
		{
			Match m = includeRegex.Match (text);
			if (m.Success) {
				string path = m.Captures [0].Value.Substring (10).TrimEnd ('\'');
				string includeText = File.ReadAllText(Path.Combine(ShaderDirectory,path));
				return FindIncludes (m.Result (includeText));
			}
			return text;
		}
		#endregion
		static string ShaderDirectory;
		public Shader (string name)
		{
			ShaderDirectory = Path.Combine (MainClass.AssemblyDirectory, "Shaders");
			var filename = Path.Combine (ShaderDirectory, name) + ".ini";
			if (!File.Exists (filename))
				throw new FileNotFoundException ();
			var file = new IniFile (filename);
			var s = file.GetSections ("Shader", false);
			if (s.Length < 1)
				throw new Exception ("Empty or Corrupt Shader Ini");
			string vsname = Path.Combine (ShaderDirectory, s [0].GetEntries ("vertex") [0].Values[0]);
			string vertexShader = Preprocess (File.ReadAllText (vsname));
			string fsname = Path.Combine (ShaderDirectory, s [0].GetEntries ("fragment") [0].Values[0]);
			string fragmentShader = Preprocess (File.ReadAllText (fsname));
		}
		string Preprocess(string text)
		{
			var stripped = StripComments (FindIncludes (text));
			Console.WriteLine (stripped);
			return text;
		}


	}
}

