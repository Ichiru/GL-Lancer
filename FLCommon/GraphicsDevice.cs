using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using Microsoft.VisualBasic;


namespace FLCommon
{
	public class GraphicsDevice : IDisposable
	{
		Color clearColor = Color.Black;
		bool isDisposed = false;
		internal EffectInstance CurrentEffect = null;
		VertexBuffer buf = null;
		IndexBuffer ind = null;
		public bool IsDisposed
		{
			get {
				return isDisposed;
			}
		}
		Viewport vp;
		public Viewport Viewport {
			get {
				return vp;
			}
			set {
				vp = value;
				GL.Viewport (vp.X, vp.Y, vp.Width, vp.Height);
			}
		}
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
			//enable caps
			//GL.Enable(EnableCap.DepthTest);
			GL.Enable (EnableCap.Texture2D);
			GL.Enable (EnableCap.TextureCubeMap);
			//GL.Enable (EnableCap.Blend);

		}
		static int currentProgram = 0;
		internal static void UseProgram_Cached(int program)
		{
			if(program != currentProgram) {
				GL.UseProgram (program);
				currentProgram = program;
			}
		}

		public void SetViewport(Viewport vp)
		{

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
			CurrentEffect.ActiveProgram.ApplyTextures ();
			int offset = buf.Declaration.VertexStride * (buf.VertexOffset + baseVertex);
			int enabledCount = 0;
			bool firstEnabled = false;
			for (int i = 0; i < buf.Declaration.Elements.Length; i++) {
				for (int j = 0; j < CurrentEffect.ActiveProgram.Attributes.Count; j++) {
					var decl = buf.Declaration.Elements [i];
					var att = CurrentEffect.ActiveProgram.Attributes [j];
					if (decl.Usage == att.Usage && decl.UsageNumber == att.UsageNumber) {
						GL.EnableVertexAttribArray (att.Location);
						enabledAtt [enabledCount] = att.Location;
						enabledCount++;
						firstEnabled = true;
						break;
					}
				}
				if (firstEnabled)
					break;
			}
			firstEnabled = false;
			GL.BindBuffer (BufferTarget.ArrayBuffer, buf.ID);
			for (int i = 0; i < buf.Declaration.Elements.Length; i++) {
				for (int j = 0; j < CurrentEffect.ActiveProgram.Attributes.Count; j++) {
					var decl = buf.Declaration.Elements [i];
					var att = CurrentEffect.ActiveProgram.Attributes [j];
					if (decl.Usage == att.Usage && decl.UsageNumber == att.UsageNumber) {
						if (firstEnabled) {
							GL.EnableVertexAttribArray (att.Location);
							enabledAtt [enabledCount] = att.Location;
							enabledCount++;
						} else
							firstEnabled = true;
						GL.VertexAttribPointer (att.Location,
							decl.GLNumberOfElements,
							decl.GLAttribPointerType,
							decl.Normalized,
							buf.Declaration.VertexStride,
							(IntPtr)(offset + decl.Offset));
						break;
					}
				}
			}

			GL.BindBuffer (BufferTarget.ElementArrayBuffer, ind.ID);
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

