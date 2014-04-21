using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FLCommon;
namespace GLSLProcessor
{
	public static class UniformParser
	{
		static Regex uniformRegex = new Regex ("uniform(.+?);", RegexOptions.Compiled);
		public static List<Uniform> FindUniforms(string source)
		{
			var l = new List<Uniform> ();
			foreach (Match match in uniformRegex.Matches(source)) {
				var m = match.Groups [1].Value.Trim ();
				if (m.Contains ("=")) {
					m = m.Substring (0, m.IndexOf ('=')).Trim ();
				}
				string type = m.Substring (0, m.IndexOf (' '));
				string name = m.Substring (m.IndexOf (' ') + 1);
				if (name.Contains ("[")) {
					string array = name.Substring (name.IndexOf ('[') + 1);
					array = array.Replace ("]", "").Trim ();
					name = name.Substring (0, name.IndexOf ('[')).Trim ();
					var u = new Uniform (name, GLSLTypes.Array);
					u.ArrayType = GetType (type);
					u.ArrayLength = int.Parse (array);
					l.Add (u);
				} else {
					l.Add (new Uniform (name, GetType (type)));
					if (MainClass.debug)
						Console.WriteLine ("Uniform {0} {1}", type, name);
				}
			}
			return l;
		}
		public static GLSLTypes GetType(string name) 
		{
			switch (name) {
			case "vec4":
				return GLSLTypes.Vector4;
			case "vec3":
				return GLSLTypes.Vector3;
			case "mat4":
				return GLSLTypes.Matrix4;
			case "sampler2D":
				return GLSLTypes.Sampler2D;
			case "samplerCube":
				return GLSLTypes.SamplerCube;
			case "int":
				return GLSLTypes.Int;
			case "float":
				return GLSLTypes.Float;
			case "vec2":
				return GLSLTypes.Vector2;
			default:
				throw new NotImplementedException (name);
			}
		}
	}
}

