using System;
using OpenTK;
namespace FLCommon
{
	public struct VertexPositionTexture : IVertexType
	{
		public Vector3 Position;
		public Vector2 TextureCoordinate;
		public VertexPositionTexture(Vector3 pos, Vector2 texcoord)
		{
			Position = pos;
			TextureCoordinate = texcoord;
		}
		public static VertexDeclaration VertexDeclaration = new VertexDeclaration(
			 new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			 new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));

		VertexDeclaration IVertexType.VertexDeclaration {
			get {
				return VertexDeclaration;
			}
		}
	}
}

