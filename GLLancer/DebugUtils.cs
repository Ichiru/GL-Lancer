using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GLLancer
{
	public static class DebugUtils
	{
		public static Texture2D CreateWhiteTexture(GraphicsDevice device)
		{
			Color[] c = new Color[2048 * 2048];
			for (int i = 0; i < 2048 * 2048; i++) {
				c [i] = Color.White;
			}
			var tex = new Texture2D (device, 2048, 2048);
			tex.SetData<Color> (c);
			return tex;
		}
	}
}

