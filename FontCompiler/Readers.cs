using System;

namespace FontCompiler
{
	public static class Readers
	{
		public const string SPRITEFONT = "Microsoft.Xna.Framework.Content.SpriteFontReader, Microsoft.Xna.Framework.Graphics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553";
		public const string TEXTURE2D = "Microsoft.Xna.Framework.Content.Texture2DReader, Microsoft.Xna.Framework.Graphics, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553";
		public const string LIST_RECTANGLE = "Microsoft.Xna.Framework.Content.ListReader`1[[Microsoft.Xna.Framework.Rectangle, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]]";
		public const string RECTANGLE = "Microsoft.Xna.Framework.Content.RectangleReader";
		public const string LIST_CHAR = "Microsoft.Xna.Framework.Content.ListReader`1[[System.Char, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]";
		public const string CHAR = "Microsoft.Xna.Framework.Content.CharReader";
		public const string LIST_VECTOR3 = "Microsoft.Xna.Framework.Content.ListReader`1[[Microsoft.Xna.Framework.Vector3, Microsoft.Xna.Framework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553]]";
		public const string VECTOR3 = "Microsoft.Xna.Framework.Content.Vector3Reader";
	}
}

