using System;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public static class GLExtensions
	{
		public static string ExtensionList;
		static GLExtensions ()
		{
			ExtensionList = GL.GetString (StringName.Extensions);
		}
		//Global method for checking extensions. Called upon GraphicsDevice creation
		public static void CheckExtensions()
		{
			if(!ExtensionList.Contains("GL_ARB_shading_language")) {
				throw new NotSupportedException ("OPENGL ERROR: Shaders not supported");
			}

			if (ExtensionList.Contains ("GL_ARB_framebuffer_object"))
				FramebufferMethods.UseARB = true;
			else if (ExtensionList.Contains ("GL_EXT_framebuffer_object"))
				FramebufferMethods.UseARB = false;
			else
				throw new NotSupportedException ("OPENGL ERROR: Framebuffers not supported");

			if (!ExtensionList.Contains("GL_ARB_texture_compression") ||
				!ExtensionList.Contains("GL_EXT_texture_compression_s3tc")) {
				throw new NotSupportedException("OPENGL ERROR: Texture Compression (s3tc) not supported");
			}

		}
	}
}

