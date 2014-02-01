using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
namespace FontCompiler
{
	public class FontRenderer : IDisposable
	{
		static readonly int[] sizes = new int[] { 128, 256, 512, 1024, 2048, 4096 };
		Font font;
		PrivateFontCollection pfx;
		FontCreationParameters parameters;
		public FontRenderer (FontCreationParameters fnt)
		{
			if (fnt.TTF) {
				pfx = new PrivateFontCollection ();
				pfx.AddFontFile (fnt.Name);
				font = new Font (pfx.Families [0], fnt.Size, fnt.Bold ? FontStyle.Bold : FontStyle.Regular);
			} else {
				font = new Font (fnt.Name, fnt.Size, fnt.Bold ? FontStyle.Bold : FontStyle.Regular);
			}
			parameters = fnt;
		}
		public CompiledFont Render ()
		{
			Bitmap globalBitmap = new Bitmap (1, 1);
			Graphics globalGraphics = Graphics.FromImage (globalBitmap);
			var compiled = new CompiledFont ();
			compiled.CharacterMap = new List<char> ();
			compiled.DefaultCharacter = '?';
			//List<Bitmap> glyphs = new List<Bitmap> ();
			List<Point> positions = new List<Point> ();
			int size = 0;
			int lineHeight = 0;

			var glyphs = CreateGlyphs (globalGraphics, parameters.Characters);
			foreach (Glyph g in glyphs) {
				compiled.CharacterMap.Add (g.Character);
			}
			for (int i = 0; i < sizes.Length; i++) {
				if (SizeTest (sizes [i], globalGraphics, out positions, glyphs, out lineHeight)) {
					size = sizes [i];
					break;
				}
			}
			if (size == 0) {
				throw new Exception("Spritefont too large");
			}
			globalBitmap.Dispose ();
			globalGraphics.Dispose ();
			globalBitmap = new Bitmap (size,size);
			globalGraphics = Graphics.FromImage (globalBitmap);
			compiled.Cropping = new List<Rectangle>();
			compiled.Kerning = new List<Vector3>();
			compiled.Glyphs = new List<Rectangle>();
			for (int i = 0; i < glyphs.Count; i++) {
				int x, y;
				x = positions[i].X;
				y = positions[i].Y;
				globalGraphics.DrawImage (glyphs[i].Bitmap, x,y);
				var r = new Rectangle(x,y,glyphs[i].Width,glyphs[i].Height);
				compiled.Glyphs.Add (r);
				r.X = 0;
				r.Y = 0;
				compiled.Cropping.Add (r);
				compiled.Kerning.Add (new Vector3(0f,(float)glyphs[i].Width,0f));
				glyphs[i].Bitmap.Dispose ();
			}
			compiled.VerticalLineSpacing = lineHeight + 2;
			compiled.HorizontalSpacing = 0f;
			compiled.Texture = globalBitmap;
			return compiled;
		}
		List<Glyph> CreateGlyphs(Graphics globalGraphics, string chars)
		{
			var l = new List<Glyph> ();
			foreach (char c in chars) {
				var s = globalGraphics.MeasureString (c.ToString (), font);
				var bmp = new Bitmap ((int)s.Width + 1, (int)s.Height + 1);
				using (var g = Graphics.FromImage(bmp)) {
					g.DrawString (c.ToString (), font, Brushes.White, new PointF (0f, 0f));
				}
				var glyph = new Glyph (bmp.Width, bmp.Height, c, bmp);
				l.Add (glyph);
			}
			l.Sort(new Comparison<Glyph>(delegate(Glyph x, Glyph y) {
				return x.Height.CompareTo(y.Height);
			}));
			return l;
		}
		struct Glyph
		{
			public int Width;
			public int Height;
			public char Character;
			public Bitmap Bitmap;
			public Glyph(int width, int height, char ch, Bitmap bmp)
			{
				Width = width;
				Height = height;
				Character = ch;
				Bitmap = bmp;
			}
		}
		bool SizeTest(int size, Graphics globalGraphics, out List<Point> positions, List<Glyph> glyphs, out int lineHeight)
		{
			int totalWidth = 0;
			int currentWidth = 0;
			int currentY = 0;
			lineHeight = 0;
			positions = new List<Point>();
			foreach (var g in glyphs) {
				lineHeight = Math.Max (lineHeight, g.Height);
				if(currentWidth + g.Width > size) {
					currentY += lineHeight + 1;
					totalWidth = Math.Max (currentWidth,totalWidth);
					currentWidth = 0;
					if (currentY + lineHeight + 1 > size)
						return false;
				}
				positions.Add (new Point(currentWidth,currentY));
				currentWidth += g.Width;
			}
			return true;
		}
		public void Dispose()
		{
			font.Dispose ();
			if (parameters.TTF) {
				pfx.Dispose ();
			}
		}
	}
}