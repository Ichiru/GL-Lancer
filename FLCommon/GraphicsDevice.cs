using System;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public class GraphicsDevice
	{
		Color clearColor = Color.Black;
		public IndexBuffer Indices
		{
			set {

			}
		}
		public void SetVertexBuffer(VertexBuffer buffer)
		{

		}
		/*public void SetRenderTarget (CubeMapFace mapFace, TextureCube cube)
		{
			if (cube == null) {
				FramebufferMethods.BindFramebuffer (FramebufferTarget.DrawFramebuffer, 0);
			} else {

			}
		}*/
		public void SetRenderTarget (RenderTarget2D target)
		{
			if (target == null) {
				FramebufferMethods.BindFramebuffer (FramebufferTarget.DrawFramebuffer, 0);
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
		}
	}
}

