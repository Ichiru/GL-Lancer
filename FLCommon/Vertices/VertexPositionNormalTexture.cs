using System;
using OpenTK;

namespace FLCommon
{
	public struct VertexPositionNormalTexture : IVertexType
	{
		public Vector3 Position;
		public Vector3 Normal;
		public Vector2 TextureCoordinate;

		public VertexPositionNormalTexture (Vector3 pos, Vector3 normal, Vector2 texcoord)
		{
			Position = pos;
			Normal = normal;
			TextureCoordinate = texcoord;
		}

		public static VertexDeclaration VertexDeclaration = 
			new VertexDeclaration (
			new VertexElement[] { new VertexElement (0, VertexElementFormat.Vector3,
					VertexElementUsage.Position, 0),
				new VertexElement (12, VertexElementFormat.Vector3,
					VertexElementUsage.Normal, 0),
					new VertexElement ( 24, VertexElementFormat.Vector2,
						VertexElementUsage.TextureCoordinate ,0)
				} );

		VertexDeclaration IVertexType.VertexDeclaration {
			get {
				return VertexDeclaration;
			}
		}
	}
}

