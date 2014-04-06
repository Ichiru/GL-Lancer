using System;
using System.IO;
using System.Text.RegularExpressions;
namespace GLSLProcessor
{
	public class Preprocessor
	{
		static Regex includeRegex = new Regex (@"\#include '(.*)'", RegexOptions.Compiled);
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
		static string FindIncludes(string text, string shaderDirectory)
		{
			Match m = includeRegex.Match (text);
			if (m.Success) {
				string path = m.Captures [0].Value.Substring (10).TrimEnd ('\'');
				string includeText = File.ReadAllText(Path.Combine(shaderDirectory,path));
				text = text.Remove (m.Index, m.Length);
				text = text.Insert (m.Index, includeText);
				return FindIncludes (text, shaderDirectory);
			}
			return text;
		}
		public static string Preprocess(string text, string path)
		{
			return "//" + Path.GetFileName(path) + "\n#version 130\n" + StripComments (FindIncludes (text, Path.GetDirectoryName(path)));
		}
	}
}

