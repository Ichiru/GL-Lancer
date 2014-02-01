using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
namespace FontCompiler
{
	public class XNBWriter
	{
		ExtendedWriter writer;
		long sizePos = 0;
		MemoryStream stream;
		FontCreationParameters p;
		public XNBWriter (FontCreationParameters parameters)
		{
			p = parameters;
			stream = new MemoryStream ();
			writer = new ExtendedWriter (stream);
			//Windows XNB file
			writer.Write ((byte)'X');
			writer.Write ((byte)'N');
			writer.Write ((byte)'B');
			writer.Write ((byte)'w');
			//XNA 4.0
			writer.Write ((byte)5);
			//Not compressed
			writer.Write ((byte)0);
			//file size
			sizePos = writer.BaseStream.Position;
			writer.Write ((int)0);
			WriteReaders ();
			writer.Write7Bit (0);
		}

		void WriteReaders()
		{
			writer.Write7Bit (8);
			writer.Write (Readers.SPRITEFONT);
			writer.Write (0);
			writer.Write (Readers.TEXTURE2D);
			writer.Write (0);
			writer.Write (Readers.LIST_RECTANGLE);
			writer.Write (0);
			writer.Write (Readers.RECTANGLE);
			writer.Write (0);
			writer.Write (Readers.LIST_CHAR);
			writer.Write (0);
			writer.Write (Readers.CHAR);
			writer.Write (0);
			writer.Write (Readers.LIST_VECTOR3);
			writer.Write (0);
			writer.Write (Readers.VECTOR3);
			writer.Write (0);
		}
		enum ReaderTable : int {
			SpriteFont = 1,
			Texture2D = 2,
			ListRectangle = 3,
			Rectangle = 4,
			ListChar = 5,
			Char = 6,
			ListVector3 = 7,
			Vector3 = 8
		}
		public unsafe void Write (CompiledFont font)
		{
			writer.Write7Bit (ReaderTable.SpriteFont);
			//Write texture
			writer.Write7Bit (ReaderTable.Texture2D);
			writer.Write (0); //Color surface format (ARGB)
			writer.Write ((uint)font.Texture.Width);
			writer.Write ((uint)font.Texture.Height);
			writer.Write ((uint)1); //mipcount
			uint size = (uint)(font.Texture.Width * font.Texture.Height * 4);
			writer.Write (size);
			var data = font.Texture.LockBits (new Rectangle (0, 0, font.Texture.Width, font.Texture.Height),
			                                  ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			byte* ptr = (byte*)data.Scan0;
			for (uint i = 0; i < size; i++) {
				writer.Write (ptr [i]);
			}
			font.Texture.UnlockBits (data);
			//Glyphs
			writer.Write7Bit (ReaderTable.ListRectangle);
			writer.Write ((uint)font.Glyphs.Count);
			for (int i = 0; i < font.Glyphs.Count; i++) {
				//writer.Write7Bit (ReaderTable.Rectangle);
				writer.Write (font.Glyphs [i].X);
				writer.Write (font.Glyphs [i].Y);
				writer.Write (font.Glyphs [i].Width);
				writer.Write (font.Glyphs [i].Height);
			}
			//Cropping
			writer.Write7Bit (ReaderTable.ListRectangle);
			writer.Write ((uint)font.Cropping.Count);
			for (int i = 0; i < font.Cropping.Count; i++) {
				writer.Write(font.Cropping[i].X);
				writer.Write (font.Cropping[i].Y);
				writer.Write (font.Cropping[i].Width);
				writer.Write (font.Cropping[i].Height);
			}
			//Chars
			writer.Write7Bit (ReaderTable.ListChar);
			writer.Write ((uint)font.CharacterMap.Count);
			for (int i = 0; i < font.CharacterMap.Count; i++) {
				//writer.Write7Bit (ReaderTable.Char);
				writer.Write (font.CharacterMap [i]);
			}
			//Vertical Line Spacing
			writer.Write (font.VerticalLineSpacing);
			//Horizontal Spacing
			writer.Write (font.HorizontalSpacing);
			//Kerning
			writer.Write7Bit (ReaderTable.ListVector3);
			writer.Write ((uint)font.Kerning.Count);
			for (int i = 0; i < font.Kerning.Count; i++) {
				//writer.Write7Bit(ReaderTable.ListVector3);
				writer.Write (font.Kerning[i].X);
				writer.Write (font.Kerning[i].Y);
				writer.Write (font.Kerning[i].Z);
			}
			//Default character
			writer.Write ((byte)1); //has value (nullable)
			writer.Write (font.DefaultCharacter.Value);
		}
		public void Close()
		{
			writer.BaseStream.Seek (sizePos,SeekOrigin.Begin);
			writer.Write ((int)writer.BaseStream.Length);
			byte[] bytes = stream.ToArray ();
			writer.Close ();
			File.WriteAllBytes (p.Output, bytes);
		}
		class ExtendedWriter : BinaryWriter
		{
			public ExtendedWriter(Stream stream) : base(stream) {
			}
			public void Write7Bit(int integer)
			{
				base.Write7BitEncodedInt (integer);
			}
			public void Write7Bit(ReaderTable table)
			{
				Write7Bit ((int)table);
			}
		}
	}
}