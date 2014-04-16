using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public class GraphicsDevice : IDisposable
	{
		Color clearColor = Color.Black;
		bool isDisposed = false;
		internal Effect CurrentEffect = null;
		VertexBuffer buf = null;
		IndexBuffer ind = null;
		public bool IsDisposed
		{
			get {
				return isDisposed;
			}
		}
		public Viewport Viewport { get; set; }
		public IndexBuffer Indices
		{
			set {
				ind = value;
			}
		}
		public void SetVertexBuffer(VertexBuffer buffer)
		{
			buf = buffer;
		}
		public void SetRenderTarget (RenderTarget2D target)
		{
			if (target == null) {
				FramebufferMethods.BindFramebuffer (FramebufferTarget.Framebuffer, 0);
			} else {
				FramebufferMethods.BindFramebuffer (FramebufferTarget.Framebuffer, target.FBO);
			}
		}
		void SetClearColor(Color color)
		{
			if (color != clearColor) {
				GL.ClearColor (color.ToColor4 ());
			}
		}
		public void Clear (ClearOptions options, Color color, int a, int b)
		{
			Clear (color);
		}
		public void Clear (Color color)
		{
			SetClearColor (color);
			GL.Clear (ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
		}
		public GraphicsDevice ()
		{
			GLExtensions.CheckExtensions ();
			//enable texturing
			GL.Enable (EnableCap.Texture2D);
			GL.Enable (EnableCap.TextureCubeMap);
		}
		int[] enabledAtt = new int[20];
		public void DrawIndexedPrimitives(PrimitiveTypes primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount)
		{
			if (CurrentEffect == null)
				throw new Exception ("An effect has not been bound");
			if (buf == null)
				throw new Exception ("No Vertex Buffer bound");
			if (ind == null)
				throw new Exception ("No Index Buffer bound");
			GL.UseProgram (CurrentEffect.ActiveProgram.ID);
			GL.BindBuffer (BufferTarget.ArrayBuffer, buf.ID);
			GL.BindBuffer (BufferTarget.ElementArrayBuffer, ind.ID);
			int offset = buf.Declaration.VertexStride * (buf.VertexOffset + baseVertex);
			int enabledCount = 0;
			for (int i = 0; i < buf.Declaration.Elements.Length; i++) {
				for (int j = 0; j < CurrentEffect.ActiveProgram.Attributes.Count; j++) {
					var decl = buf.Declaration.Elements [i];
					var att = CurrentEffect.ActiveProgram.Attributes [j];
					if (decl.Usage == att.Usage && decl.UsageNumber == att.UsageNumber) {
						GL.VertexAttribPointer (att.Location,
						                       decl.GLNumberOfElements,
						                       decl.GLAttribPointerType,
						                       decl.Normalized,
						                       buf.Declaration.VertexStride,
						                       (IntPtr)(offset + decl.Offset));
						GL.EnableVertexAttribArray (att.Location);
						enabledAtt [enabledCount] = att.Location;
						enabledCount++;
						break;
					}
				}
			}
			GL.DrawRangeElements (primitiveType.GLType (),
			                     minVertexIndex,
			                     numVertices,
			                     primitiveType.GetArrayLength (primitiveCount),
			                     DrawElementsType.UnsignedShort,
			                     (IntPtr)(startIndex * 2));
			for (int i = 0; i < enabledCount; i++) {
				GL.DisableVertexAttribArray (enabledAtt [i]);
			}
		}
		public void Dispose()
		{
			isDisposed = true;
		}
	}
}

