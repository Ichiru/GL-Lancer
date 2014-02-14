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
	}
}

