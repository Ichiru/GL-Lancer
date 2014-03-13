using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
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
			Declaration = VertexDeclaration.FromType (type);
			CreateVBO ();
		}
		public VertexBuffer (GraphicsDevice device, VertexDeclaration decl, int length, BufferUsage usage)
		{
			Declaration = decl;
			CreateVBO ();
		}
		void CreateVBO()
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

