using System;
using OpenTK.Graphics.OpenGL;
namespace FLCommon
{
	public class Texture2D : Texture
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		PixelInternalFormat glInternalFormat;
		PixelFormat glFormat;
		PixelType glType;

		public Texture2D (GraphicsDevice device, int width, int height, bool hasMipMaps, SurfaceFormat format) : this (true)
		{
			GraphicsDevice = device;
			Width = width;
			Height = height;
			Format = format;
			Format.GetGLFormat (out glInternalFormat, out glFormat, out glType);
			LevelCount = hasMipMaps ? CalculateMipLevels (width, height) : 1;
			//Bind the new Texture2D
			Bind ();
			//initialise the texture data
			var imageSize = 0;
			if (glFormat == (PixelFormat)All.CompressedTextureFormats) {
				CheckCompressed ();
				switch (Format) {
				case SurfaceFormat.Dxt1:
				case SurfaceFormat.Dxt3:
				case SurfaceFormat.Dxt5:
					imageSize = ((Width + 3) / 4) * ((Height + 3) / 4) * format.GetSize ();
					break;
				default:
					throw new NotSupportedException ();
				}
				GL.CompressedTexImage2D (TextureTarget.Texture2D, 0, glInternalFormat,
				                        Width, Height, 0,
				                        imageSize, IntPtr.Zero);
			} else {
				GL.TexImage2D(TextureTarget.Texture2D, 0,	           
				              glInternalFormat,
				              Width, Height, 0,
				              glFormat, glType, IntPtr.Zero);
			}
			//enable filtering
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMagFilter.Linear );
			GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Linear );
		}
		public Texture2D (GraphicsDevice device, int width, int height) : this (device, width, height, false, SurfaceFormat.Color)
		{

		}
		protected Texture2D(bool genID)
		{
			if (genID)
				ID = GL.GenTexture ();
		}
		internal override void Bind ()
		{
			GL.BindTexture (TextureTarget.Texture2D, ID);
		}
		public void GetData<T> (int level, Rectangle? rect, T[] data, int start, int count) where T : struct
		{
			GetData<T> (data);
		}
		public void GetData<T> (T[] data) where T : struct
		{
			GL.BindTexture (TextureTarget.Texture2D, ID);
			if (glFormat == (PixelFormat)All.CompressedTextureFormats) {
				throw new NotImplementedException ();
			} else {
				GL.GetTexImage<T> (
					TextureTarget.Texture2D,
					0,
					glFormat,
					glType,
					data
				);
			}
		}
		public void SetData<T> (int level, Rectangle? rect, T[] data, int start, int count) where T : struct
		{
			GL.BindTexture (TextureTarget.Texture2D, ID);
			if (glFormat == (PixelFormat)All.CompressedTextureFormats) {
				GL.CompressedTexImage2D<T> (TextureTarget.Texture2D, level, glInternalFormat,
				                         Width, Height, 0,
				                         count, data);
			} else {
				GL.TexImage2D<T> (TextureTarget.Texture2D, level, glInternalFormat, Width, Height, 0, glFormat, glType, data);
			}
		}
		public void SetData<T> (T[] data) where T : struct
		{
			SetData<T> (0, null, data, 0, data.Length);
		}
		public override void Dispose ()
		{
			GL.DeleteTexture (ID);
			base.Dispose ();
		}
		public static Texture2D FromStream (GraphicsDevice device, System.IO.Stream stream)
		{
			return null;
		}
	}
}

