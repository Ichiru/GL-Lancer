using System;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public class IndexBuffer : IDisposable
	{
		public int IndexCount { get; private set; }
		internal int ID;
		public IndexBuffer (GraphicsDevice device, IndexElementSize size, int length, BufferUsage usage)
		{
			ID = GL.GenBuffer ();
		}
		public void SetData<T>(T[] data) where T: struct
		{

		}
		public void Dispose()
		{

		}
	}
}

