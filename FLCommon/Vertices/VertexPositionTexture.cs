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
		public static VertexDeclaration VertexDeclaration = null;

		VertexDeclaration IVertexType.VertexDeclaration {
			get {
				return VertexDeclaration;
			}
		}
	}
}

