using System;

namespace FLCommon
{
	public class Texture3D : Texture
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Depth { get; private set; }
		public Texture3D (GraphicsDevice device, int width, int height, int depth, bool hasMipMaps, SurfaceFormat surfaceFormat)
		{
			GraphicsDevice = device;
			Width = width;
			Height = height;
			Depth = depth;
			Format = surfaceFormat;
		}
		internal override void Bind ()
		{
			throw new NotImplementedException ();
		}
		public void SetData<T> (int level,
		                        int left, int top, int right, int bottom, int front, int back,
		                        T[] data, int startIndex, int elementCount) where T : struct
		{

		}
		public void GetData<T>(int level, int left, int top, int right, int bottom, int front, int back,
		                        T[] data, int startIndex, int elementCount) where T : struct
		{

		}
		public override void Dispose ()
		{
			throw new NotImplementedException ();
		}
	}
}

