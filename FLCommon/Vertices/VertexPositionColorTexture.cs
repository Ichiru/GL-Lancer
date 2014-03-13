using System;
using OpenTK;
namespace FLCommon
{
	public struct VertexPositionColorTexture : IVertexType
	{
		public Vector3 Position;
		public Color Color;
		public Vector2 TextureCoordinate;
		public VertexPositionColorTexture(Vector3 pos, Color color, Vector2 texcoord)
		{
			Position = pos;
			Color = color;
			TextureCoordinate = texcoord;
		}
		public static VertexDeclaration VertexDeclaration = new VertexDeclaration(
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), 
			new VertexElement(12, VertexElementFormat.Byte4, VertexElementUsage.BlendWeight, 0), 
			new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0) );
		VertexDeclaration IVertexType.VertexDeclaration {
			get {
				return VertexDeclaration;
			}
		}
	}
}

