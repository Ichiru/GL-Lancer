using System;
using OpenTK;
namespace FLCommon
{
	public struct VertexPositionColor
	{
		public Vector3 Position;
		public Color Color;
		public VertexPositionColor(Vector3 pos, Color color)
		{
			Position = pos;
			Color = color;
		}
	}
}

