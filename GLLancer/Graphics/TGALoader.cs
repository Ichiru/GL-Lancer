using System;
using System.IO;
using OpenTK.Graphics.OpenGL;
namespace GLLancer
{
	public static class TGALoader
	{
		enum DataFormats : byte {
			NoImage = 0,
			ColorMapped = 1,
			RGB = 2,
			BW = 3,
			RLEColorMapped = 9,
			RLE_RGB = 10,
			CompressedBW = 11,
			CompressedColorMapped1 = 32,
			CompressedColorMapped2 = 33
		}
		public static int Load(Stream stream, out int width, out int height)
		{
			using(var reader = new BinaryReader(stream))
			{
				//Read the TGA Header
				byte imageIDLength = reader.ReadByte ();
				byte colorMapType = reader.ReadByte ();
				DataFormats imageType = (DataFormats)reader.ReadByte ();
				short colorMapOrigin = reader.ReadInt16();
				short colorMapLength = reader.ReadInt16 ();
				byte colorMapDepth = reader.ReadByte ();
				short x_origin = reader.ReadInt16 ();
				short y_origin = reader.ReadInt16 ();
				width = reader.ReadInt16 ();
				height = reader.ReadInt16 ();
				byte bitsperpixel = reader.ReadByte ();
				byte imagedescriptor = reader.ReadByte ();
				//Image ID string
				byte[] idData = reader.ReadBytes (imageIDLength);
				switch(imageType) {
				case DataFormats.RGB:
					if(bitsperpixel == 32)
					{
						byte[] data = reader.ReadBytes(width * height * 4);
						int id = GL.GenTexture();
						GL.BindTexture (TextureTarget.Texture2D,id);
						GL.TexImage2D (TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,
						               width,height,0,PixelFormat.Bgra,PixelType.UnsignedByte,data);
						return id;
					}
					else if (bitsperpixel == 24)
					{
						byte[] data = reader.ReadBytes (width * height * 3);
						int id = GL.GenTexture ();
						GL.BindTexture (TextureTarget.Texture2D,id);
						GL.TexImage2D (TextureTarget.Texture2D,0,PixelInternalFormat.Rgb,
						               width,height,0,PixelFormat.Bgr,PixelType.UnsignedByte,data);
						return id;
					}
					else if(bitsperpixel == 16)
					{
						byte[] read_data = reader.ReadBytes (width * height * 2);
						byte[] data = new byte[width * height * 4];
						int j = 0;
						for(int i = 0; i < read_data.Length; i += 2)
						{
							byte b1 = read_data[i + 1];
							byte b0 = read_data[i];
							byte r = (byte)((b1 & 0x7c) << 1);
							byte g = (byte)(((b1 & 0x03) << 6) | ((b0 & 0xe0) << 2));
							byte b = (byte)((b0 & 0x1f) << 3);
							byte a = (byte)((b1 & 0x80));
							data[j++] = b;
							data[j++] = g;
							data[j++] = r;
							data[j++] = a;
						}
						int id = GL.GenTexture ();
						GL.BindTexture (TextureTarget.Texture2D,id);
						GL.TexImage2D (TextureTarget.Texture2D,0,PixelInternalFormat.Rgba,
						               width,height,0,PixelFormat.Bgra,PixelType.UnsignedByte,data);
						return id;
					}
					else
						throw new Exception("Unsupported image format");
				default:
					throw new Exception("Unsupported image format");
				}
			}
		}
	}
}