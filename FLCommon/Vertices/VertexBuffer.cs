using System;
using System.Collections.Generic;
namespace FLCommon
{
	public class VertexBuffer : IDisposable
	{
		public int VertexCount { get; private set; }
		public int VertexOffset = 0;
		internal int ID;
		public VertexDeclaration Declaration;
		public VertexBuffer (GraphicsDevice device, Type type, int length, BufferUsage usage)
		{
		}
		public VertexBuffer (GraphicsDevice device, VertexDeclaration decl, int length, BufferUsage usage)
		{
			Declaration = decl;
		}
		public void SetData<T>(T[] data) where T: struct
		{

		}
		public void Dispose()
		{

		}
	}
}

