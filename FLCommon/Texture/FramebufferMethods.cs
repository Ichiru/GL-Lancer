using System;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	/// <summary>
	/// Helper class that allows both EXT and ARB framebuffers (ARB preferred)
	/// </summary>
	static class FramebufferMethods
	{
		static bool UseARB;
		static FramebufferMethods()
		{
			if (GLExtensions.ExtensionList.Contains ("GL_ARB_framebuffer_object"))
				UseARB = true;
			else if (GLExtensions.ExtensionList.Contains ("GL_EXT_framebuffer_object"))
				UseARB = false;
			else
				throw new Exception ("Hardware doesn't support GL_EXT_framebuffer_object or OpenGL 3.0");
		}
		public static void FramebufferTexture2D(FramebufferTarget fbTarget, FramebufferAttachment attachment, TextureTarget texTarget,
		                                         int texture, int level)
		{
			if (UseARB) {
				GL.FramebufferTexture2D (fbTarget, attachment, texTarget, texture, level);
			} else {
				GL.Ext.FramebufferTexture2D (fbTarget, attachment, texTarget, texture, level);
			}
		}
		public static int GenFramebuffer()
		{
			if (UseARB) {
				return GL.GenFramebuffer ();
			} else {
				return GL.Ext.GenFramebuffer ();
			}
		}
		public static void BindFramebuffer(FramebufferTarget target, int framebuffer)
		{
			if (UseARB) {
				GL.BindFramebuffer (target, framebuffer);
			} else {
				GL.Ext.BindFramebuffer (target, framebuffer);
			}
		}
		public static int GenRenderbuffer ()
		{
			if (UseARB) {
				return GL.GenRenderbuffer ();
			} else {
				return GL.Ext.GenRenderbuffer ();
			}
		}
		public static void BindRenderbuffer(RenderbufferTarget target, int buffer)
		{
			if (UseARB) {
				GL.BindRenderbuffer (target, buffer);
			} else {
				GL.Ext.BindRenderbuffer (target, buffer);
			}
		}
		public static void FramebufferRenderbuffer(FramebufferTarget fboTarget, FramebufferAttachment attachment, RenderbufferTarget rboTarget, int rbo)
		{
			if (UseARB) {
				GL.FramebufferRenderbuffer (fboTarget, attachment, rboTarget, rbo);
			} else {
				GL.Ext.FramebufferRenderbuffer (fboTarget, attachment, rboTarget, rbo);
			}
		}
		public static void RenderbufferStorage (RenderbufferTarget target, RenderbufferStorage storage, int width, int height)
		{
			if (UseARB) {
				GL.RenderbufferStorage (target, storage, width, height);
			} else {
				GL.Ext.RenderbufferStorage (target, storage, width, height);
			}
		}
		public static void DeleteFramebuffer (int fbo)
		{
			if (UseARB) {
				GL.DeleteFramebuffer (fbo);
			} else {
				GL.Ext.DeleteFramebuffer (fbo);
			}
		}
		public static void DeleteRenderbuffer (int rbo)
		{
			if (UseARB) {
				GL.DeleteRenderbuffer (rbo);
			} else {
				GL.Ext.DeleteRenderbuffer (rbo);
			}
		}
	}
}