using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public class Attributes
	{
		static GLSLAttribute[] attributes = new GLSLAttribute[] {
			new GLSLAttribute("vertex_position", GLSLTypes.Vector3, VertexElementUsage.Position, 0), 
			new GLSLAttribute("vertex_texcoord0", GLSLTypes.Vector2, VertexElementUsage.TextureCoordinate, 0),
			new GLSLAttribute ("vertex_texcoord1", GLSLTypes.Vector2, VertexElementUsage.TextureCoordinate, 1), 
			new GLSLAttribute("vertex_diffuse", GLSLTypes.Vector4, VertexElementUsage.BlendWeight, 0),
			new GLSLAttribute ("vertex_normal", GLSLTypes.Vector3, VertexElementUsage.Normal, 0),
			 new GLSLAttribute("vertex_tangent", GLSLTypes.Vector3, VertexElementUsage.Tangent, 0),
			new GLSLAttribute ("vertex_binormal", GLSLTypes.Vector3, VertexElementUsage.Binormal, 0)
		};
		internal static List<SupportedAttribute> GetVertexAttributes(int programID)
		{
			var supported = new List<SupportedAttribute> ();
			for (int i = 0; i < attributes.Length; i++) {
				var att = attributes [i];
				int loc = GL.GetAttribLocation (programID, att.Name);
				if (loc != -1) {
					var supportedinfo = new SupportedAttribute (loc, att.Usage, att.UsageNumber);
					supported.Add (supportedinfo);
				}
			}
			return supported;
		}
		public static bool Recognised(string name, GLSLTypes type)
		{
			for (int i = 0; i < attributes.Length; i++) {
				if (name == attributes[i].Name && type == attributes[i].Type)
					return true;
			}
			return false;
		}
		class GLSLAttribute
		{
			public string Name;
			public GLSLTypes Type;
			public VertexElementUsage Usage;
			public int UsageNumber;
			public GLSLAttribute(string name, GLSLTypes type, VertexElementUsage usage, int usagenumber)
			{
				Name = name;
				Type = type;
				Usage = usage;
				UsageNumber = usagenumber;
			}
		}
	}
	class SupportedAttribute
	{
		public int Location;
		public VertexElementUsage Usage;
		public int UsageNumber;
		public SupportedAttribute(int loc, VertexElementUsage usage, int usagenumber)
		{
			Location = loc;
			Usage = usage;
			UsageNumber = usagenumber;
		}
	}
}

