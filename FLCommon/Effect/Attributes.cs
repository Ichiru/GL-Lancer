using System;
using System.Collections.Generic;
namespace FLCommon
{
	public class Attributes
	{
		static GLSLAttribute[] attributes = new GLSLAttribute[] {
			new GLSLAttribute("vertex_position", GLSLTypes.Vector3), new GLSLAttribute("vertex_texcoord0", GLSLTypes.Vector2),
			new GLSLAttribute ("vertex_texcoord1", GLSLTypes.Vector2), new GLSLAttribute("vertex_diffuse", GLSLTypes.Int),
			new GLSLAttribute ("vertex_normal", GLSLTypes.Vector3), new GLSLAttribute("vertex_tangent", GLSLTypes.Vector3),
			new GLSLAttribute ("vertex_binormal", GLSLTypes.Vector3)
		};
		public static bool Recognised(string name, GLSLTypes type)
		{
			for (int i = 0; i < attributes.Length; i++) {
				if (name == attributes[i].Name && type == attributes[i].Type)
					return true;
			}
			return false;
		}
		struct GLSLAttribute
		{
			public string Name;
			public GLSLTypes Type;
			public GLSLAttribute(string name, GLSLTypes type)
			{
				Name = name;
				Type = type;
			}
		}
	}
}

