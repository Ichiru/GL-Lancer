using System;

namespace FLCommon
{
	public class VertexElement
	{
		public VertexElement (int offset, VertexElementFormat format, VertexElementUsage usage, int pos)
		{
		}
	}
	public enum VertexElementFormat
	{
		Vector2,
		Vector3,
		Byte4
	}
	public enum VertexElementUsage
	{
		Position,
		Normal,
		TextureCoordinate,
		Tangent,
		Binormal,
		BlendWeight
	}
}

