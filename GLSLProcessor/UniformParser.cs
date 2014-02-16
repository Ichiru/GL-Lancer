using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniformTypes = FLCommon.UniformTypes;
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
					var u = new Uniform (name, UniformTypes.Array);
					u.ArrayType = GetType (type);
					u.ArrayLength = int.Parse (array);
					l.Add (u);
				} else {
					l.Add (new Uniform (name, GetType (type)));
				}
			}
			return l;
		}
		static UniformTypes GetType(string name) 
		{
			switch (name) {
			case "vec4":
				return UniformTypes.Vector4;
			case "vec3":
				return UniformTypes.Vector3;
			case "mat4":
				return UniformTypes.Matrix4;
			case "sampler2D":
				return UniformTypes.Sampler2D;
			case "samplerCube":
				return UniformTypes.SamplerCube;
			case "int":
				return UniformTypes.Int;
			case "float":
				return UniformTypes.Float;
			default:
				throw new NotImplementedException (name);
			}
		}
	}
}

