using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FLCommon;
namespace GLSLProcessor
{
	public static class AttributeChecker
	{
		static Regex uniformRegex = new Regex ("attribute(.+?);", RegexOptions.Compiled);
		public static void CheckAttributes(string source)
		{
			foreach (Match match in uniformRegex.Matches(source)) {
				var m = match.Groups [1].Value.Trim ();
				if (m.Contains ("=")) {
					m = m.Substring (0, m.IndexOf ('=')).Trim ();
				}
				string type = m.Substring (0, m.IndexOf (' '));
				string name = m.Substring (m.IndexOf (' ') + 1);
				GLSLTypes gl_type;
				if (name.Contains ("[")) {
					string array = name.Substring (name.IndexOf ('[') + 1);
					array = array.Replace ("]", "").Trim ();
					name = name.Substring (0, name.IndexOf ('[')).Trim ();
					gl_type = GLSLTypes.Array;
				} else {
					gl_type = UniformParser.GetType (type);
				}
				if (!Attributes.Recognised (name, gl_type))
					throw new Exception (string.Format ("Unrecognised attribute '{0}'", m));
			}
		}

	}
}

