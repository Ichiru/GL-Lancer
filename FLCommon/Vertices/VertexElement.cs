using System;

namespace FLCommon
{
	public class VertexElement
	{
		public int Offset;
		public VertexElementFormat Format;
		public VertexElementUsage Usage;
		public int UsageNumber;
		public VertexElement (int offset, VertexElementFormat format, VertexElementUsage usage, int usageNumber)
		{
			Offset = offset;
			Format = format;
			Usage = usage;
			UsageNumber = usageNumber;
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

