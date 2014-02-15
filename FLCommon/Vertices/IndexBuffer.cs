using System;

namespace FLCommon
{
	public class IndexBuffer : IDisposable
	{
		public int IndexCount { get; private set; }
		public IndexBuffer (GraphicsDevice device, IndexElementSize size, int length, BufferUsage usage)
		{
		}
		public void SetData<T>(T[] data) where T: struct
		{

		}
		public void Dispose()
		{

		}
	}
}

