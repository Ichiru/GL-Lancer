using System;

namespace FLCommon
{
	public struct Viewport
	{
		public int Width;
		public int Height;
		public Viewport(int width, int height)
		{
			Width = width;
			Height = height;
		}
		public float AspectRatio
		{
			get {
				return (float)Width / (float)Height;
			}
		}
	}
}

