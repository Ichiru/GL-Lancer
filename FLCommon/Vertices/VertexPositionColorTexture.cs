using System;
using OpenTK;
namespace FLCommon
{
	public struct VertexPositionColorTexture
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
		public static VertexDeclaration VertexDeclaration = null;
	}
}

