using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	class Program
	{
		public string Name;
		public int ID;
		public Dictionary<string, GLUniform> Uniforms = new Dictionary<string, GLUniform> ();

		public void ApplyTextures ()
		{
			int i = 0;
			foreach (var str in Uniforms.Keys) {
				var u = Uniforms [str];
				if (u.IsTexture) {
					switch (i) {
						case 0:
						GL.ActiveTexture (TextureUnit.Texture0);
						((Texture)u.Value).Bind ();
						break;
						case 1:
						GL.ActiveTexture (TextureUnit.Texture1);
						((Texture)u.Value).Bind ();
						break;
						case 2:
						GL.ActiveTexture (TextureUnit.Texture2);
						((Texture)u.Value).Bind ();
						break;
						default:
						throw new ArgumentOutOfRangeException ();
					}
					i++;
					((Texture)u.Value).Bind ();
				}

			}
		}

		public void SetTextureUniforms ()
		{
			int i = 0;
			foreach (var str in Uniforms.Keys) {
				var u = Uniforms [str];
				if (u.IsTexture) {
					GL.Uniform1 (u.Location, i);
					i++;
				}
			}
		}
	}
}

