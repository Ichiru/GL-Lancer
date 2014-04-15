using System;
using OpenTK.Graphics.OpenGL;

namespace FLCommon
{
	public class TextureCube : Texture
	{
		public int Size { get; private set; }

		public TextureCube (GraphicsDevice device, int size, bool hasMips, SurfaceFormat format)
		{
			ID = GL.GenTexture ();
			GraphicsDevice = device;
			Size = size;
		}
		public void SetData<T>(CubeMapFace face,int level, Rectangle? rect, T[] data, int start, int count)
		{

		}
		public void SetData<T>(CubeMapFace face, T[] data)
		{
			SetData<T> (face, 0, null, data, 0, data.Length);
		}
		internal override void Bind ()
		{
			throw new NotImplementedException ();
		}
		public override void Dispose ()
		{
			throw new NotImplementedException ();
		}
	}
}

