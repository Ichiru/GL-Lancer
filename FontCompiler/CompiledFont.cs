using System;
using System.Collections.Generic;
using System.Drawing;
namespace FontCompiler
{
	public class CompiledFont
	{
		public Bitmap Texture;
		public List<Rectangle> Glyphs;
		public List<Rectangle> Cropping;
		public List<Char> CharacterMap;
		public int VerticalLineSpacing;
		public float HorizontalSpacing;
		public List<Vector3> Kerning;
		public char? DefaultCharacter;
	}
}

