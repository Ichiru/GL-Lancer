using System;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public class RenderTarget2D : Texture2D
	{
		internal int FBO;
		int depthbuffer;
		public RenderTarget2D (GraphicsDevice device, int width, int height) : base(device, width, height)
		{
			//generate the FBO
			FBO = FramebufferMethods.GenFramebuffer ();
			FramebufferMethods.BindFramebuffer (FramebufferTarget.Framebuffer, FBO);
			//make the depth buffer
			depthbuffer = FramebufferMethods.GenRenderbuffer ();
			FramebufferMethods.BindRenderbuffer (RenderbufferTarget.Renderbuffer, depthbuffer);
			FramebufferMethods.RenderbufferStorage (RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, width, height);
			FramebufferMethods.FramebufferRenderbuffer (FramebufferTarget.Framebuffer, 
			                                            FramebufferAttachment.DepthAttachment, 
			                                            RenderbufferTarget.Renderbuffer, depthbuffer);
			//bind the texture
			FramebufferMethods.FramebufferTexture2D (FramebufferTarget.Framebuffer, 
			                                         FramebufferAttachment.ColorAttachment0, 
			                                         TextureTarget.Texture2D, ID, 0);
			//unbind the FBO
			FramebufferMethods.BindFramebuffer (FramebufferTarget.Framebuffer, 0);
		}
	}
}

