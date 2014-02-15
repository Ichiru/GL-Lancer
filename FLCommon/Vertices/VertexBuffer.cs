using System;

namespace FLCommon
{
	public class VertexBuffer : IDisposable
	{
		public int VertexCount { get; private set; }
		public VertexBuffer (GraphicsDevice device, Type type, int length, BufferUsage usage)
		{
		}
		public VertexBuffer (GraphicsDevice device, VertexDeclaration decl, int length, BufferUsage usage)
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

