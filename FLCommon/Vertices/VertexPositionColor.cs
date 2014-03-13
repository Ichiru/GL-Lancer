using System;
using OpenTK;

namespace FLCommon
{
	public struct VertexPositionColor : IVertexType
	{
		public static VertexDeclaration VertexDeclaration = new VertexDeclaration (
			new VertexElement (0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), 
			new VertexElement (12, VertexElementFormat.Byte4, VertexElementUsage.BlendWeight, 0));

		VertexDeclaration IVertexType.VertexDeclaration {
			get {
				return VertexDeclaration;
			}
		}

		public Vector3 Position;
		public Color Color;

		public VertexPositionColor (Vector3 pos, Color color)
		{
			Position = pos;
			Color = color;
		}
	}
}

