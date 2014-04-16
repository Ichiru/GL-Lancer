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
			IndexCount = length;
			ID = GL.GenBuffer ();
		}
		public void SetData(short[] data)
		{
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, ID);
			GL.BufferData (BufferTarget.ElementArrayBuffer, new IntPtr (data.Length * 2), data, BufferUsageHint.StaticDraw);
		}
		public void SetData(ushort[] data)
		{
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, ID);
			GL.BufferData (BufferTarget.ElementArrayBuffer, new IntPtr (data.Length * 2), data, BufferUsageHint.StaticDraw);
		}
		public void Dispose()
		{
			GL.DeleteBuffer (ID);
		}
	}
}

