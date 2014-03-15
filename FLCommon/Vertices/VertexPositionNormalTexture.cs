using System;
using OpenTK;
namespace FLCommon
{
	public struct VertexPositionNormalTexture : IVertexType
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 TextureCoordinate;
		public VertexPositionNormalTexture(Vector3 pos, Vector3 normal, Vector2 texcoord)
		{
			Position = pos;
			Normal = normal;
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

