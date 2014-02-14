//Adapted from OpenTK's Color4, but uses bytes instead of floats
#region License
//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2008 the Open Toolkit library, except where noted.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
namespace FLCommon
{
	/// <summary>
	/// Represents a color with 4 byte components (R, G, B, A).
	/// </summary>
	public struct Color : IEquatable<Color>
	{

		#region Fields

		/// <summary>
		/// The red component of this Color structure.
		/// </summary>
		public byte R;
		/// <summary>
		/// The green component of this Color structure.
		/// </summary>
		public byte G;
		/// <summary>
		/// The blue component of this Color structure.
		/// </summary>
		public byte B;
		/// <summary>
		/// The alpha component of this Color structure.
		/// </summary>
		public byte A;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new Color structure from the specified components.
		/// </summary>
		/// <param name="r">The red component of the new Color structure.</param>
		/// <param name="g">The green component of the new Color structure.</param>
		/// <param name="b">The blue component of the new Color structure.</param>
		/// <param name="a">The alpha component of the new Color structure.</param>
		public Color (byte r, byte g, byte b, byte a)
		{
			R = r;
			G = g;
			B = b;
			A = a;
		}

		/// <summary>
		/// Constructs a new Color structure from the specified components.
		/// </summary>
		/// <param name="r">The red component of the new Color structure.</param>
		/// <param name="g">The green component of the new Color structure.</param>
		/// <param name="b">The blue component of the new Color structure.</param>
		public Color (byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
			A = 255;
		}

		/// <summary>
		/// Constructs a new Color structure from the specified components.
		/// </summary>
		/// <param name="r">The red component of the new Color structure.</param>
		/// <param name="g">The green component of the new Color structure.</param>
		/// <param name="b">The blue component of the new Color structure.</param>
		/// <param name="a">The alpha component of the new Color structure.</param>
		public Color (float r, float g, float b, float a)
		{
			R = (byte)(r * (byte)Byte.MaxValue);
			G = (byte)(g * (byte)Byte.MaxValue);
			B = (byte)(b * (byte)Byte.MaxValue);
			A = (byte)(a * (byte)Byte.MaxValue);
		}

		/// <summary>
		/// Constructs a new Color structure from the specified components.
		/// </summary>
		/// <param name="r">The red component of the new Color structure.</param>
		/// <param name="g">The green component of the new Color structure.</param>
		/// <param name="b">The blue component of the new Color structure.</param>
		public Color (float r, float g, float b)
		{
			R = (byte)(r * (byte)Byte.MaxValue);
			G = (byte)(g * (byte)Byte.MaxValue);
			B = (byte)(b * (byte)Byte.MaxValue);
			A = 255;
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Converts this color to an integer representation with 8 bits per channel.
		/// </summary>
		/// <returns>A <see cref="System.Int32"/> that represents this instance.</returns>
		/// <remarks>This method is intended only for compatibility with System.Drawing. It compresses the color into 8 bits per channel, which means color information is lost.</remarks>
		public int ToArgb ()
		{
			uint value =
				(uint)(A) << 24 |
				(uint)(R) << 16 |
				(uint)(G) << 8 |
				(uint)(B);

			return unchecked((int)value);
		}

		public Vector4 ToVector4()
		{
			return new Vector4 ((float)R / 255f, (float)G / 255f, (float)B / 255f, (float)A / 255f);
		}

		public Color4 ToColor4()
		{
			return new Color4 ((float)R / 255f, (float)G / 255f, (float)B / 255f, (float)A / 255f);
		}
		/// <summary>
		/// Compares the specified Color structures for equality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is equal to right; false otherwise.</returns>
		public static bool operator == (Color left, Color right)
		{
			return left.Equals (right);
		}

		/// <summary>
		/// Compares the specified Color structures for inequality.
		/// </summary>
		/// <param name="left">The left-hand side of the comparison.</param>
		/// <param name="right">The right-hand side of the comparison.</param>
		/// <returns>True if left is not equal to right; false otherwise.</returns>
		public static bool operator != (Color left, Color right)
		{
			return !left.Equals (right);
		}


		/// <summary>
		/// Compares whether this Color structure is equal to the specified object.
		/// </summary>
		/// <param name="obj">An object to compare to.</param>
		/// <returns>True obj is a Color structure with the same components as this Color; false otherwise.</returns>
		public override bool Equals (object obj)
		{
			if (!(obj is Color))
				return false;

			return Equals ((Color)obj);
		}

		/// <summary>
		/// Calculates the hash code for this Color structure.
		/// </summary>
		/// <returns>A System.Int32 containing the hashcode of this Color structure.</returns>
		public override int GetHashCode ()
		{
			return ToArgb ();
		}

		/// <summary>
		/// Creates a System.String that describes this Color structure.
		/// </summary>
		/// <returns>A System.String that describes this Color structure.</returns>
		public override string ToString ()
		{
			return String.Format ("{{(R, G, B, A) = ({0}, {1}, {2}, {3})}}", R.ToString (), G.ToString (), B.ToString (), A.ToString ());
		}

		#region System colors

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 255, 255, 0).
		/// </summary>
		public static Color Transparent { get { return new Color (255, 255, 255, 0); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (240, 248, 255, 255).
		/// </summary>
		public static Color AliceBlue { get { return new Color (240, 248, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (250, 235, 215, 255).
		/// </summary>
		public static Color AntiqueWhite { get { return new Color (250, 235, 215, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 255, 255, 255).
		/// </summary>
		public static Color Aqua { get { return new Color (0, 255, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (127, 255, 212, 255).
		/// </summary>
		public static Color Aquamarine { get { return new Color (127, 255, 212, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (240, 255, 255, 255).
		/// </summary>
		public static Color Azure { get { return new Color (240, 255, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (245, 245, 220, 255).
		/// </summary>
		public static Color Beige { get { return new Color (245, 245, 220, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 228, 196, 255).
		/// </summary>
		public static Color Bisque { get { return new Color (255, 228, 196, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 0, 0, 255).
		/// </summary>
		public static Color Black { get { return new Color (0, 0, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 235, 205, 255).
		/// </summary>
		public static Color BlanchedAlmond { get { return new Color (255, 235, 205, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 0, 255, 255).
		/// </summary>
		public static Color Blue { get { return new Color (0, 0, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (138, 43, 226, 255).
		/// </summary>
		public static Color BlueViolet { get { return new Color (138, 43, 226, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (165, 42, 42, 255).
		/// </summary>
		public static Color Brown { get { return new Color (165, 42, 42, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (222, 184, 135, 255).
		/// </summary>
		public static Color BurlyWood { get { return new Color (222, 184, 135, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (95, 158, 160, 255).
		/// </summary>
		public static Color CadetBlue { get { return new Color (95, 158, 160, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (127, 255, 0, 255).
		/// </summary>
		public static Color Chartreuse { get { return new Color (127, 255, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (210, 105, 30, 255).
		/// </summary>
		public static Color Chocolate { get { return new Color (210, 105, 30, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 127, 80, 255).
		/// </summary>
		public static Color Coral { get { return new Color (255, 127, 80, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (100, 149, 237, 255).
		/// </summary>
		public static Color CornflowerBlue { get { return new Color (100, 149, 237, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 248, 220, 255).
		/// </summary>
		public static Color Cornsilk { get { return new Color (255, 248, 220, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (220, 20, 60, 255).
		/// </summary>
		public static Color Crimson { get { return new Color (220, 20, 60, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 255, 255, 255).
		/// </summary>
		public static Color Cyan { get { return new Color (0, 255, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 0, 139, 255).
		/// </summary>
		public static Color DarkBlue { get { return new Color (0, 0, 139, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 139, 139, 255).
		/// </summary>
		public static Color DarkCyan { get { return new Color (0, 139, 139, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (184, 134, 11, 255).
		/// </summary>
		public static Color DarkGoldenrod { get { return new Color (184, 134, 11, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (169, 169, 169, 255).
		/// </summary>
		public static Color DarkGray { get { return new Color (169, 169, 169, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 100, 0, 255).
		/// </summary>
		public static Color DarkGreen { get { return new Color (0, 100, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (189, 183, 107, 255).
		/// </summary>
		public static Color DarkKhaki { get { return new Color (189, 183, 107, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (139, 0, 139, 255).
		/// </summary>
		public static Color DarkMagenta { get { return new Color (139, 0, 139, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (85, 107, 47, 255).
		/// </summary>
		public static Color DarkOliveGreen { get { return new Color (85, 107, 47, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 140, 0, 255).
		/// </summary>
		public static Color DarkOrange { get { return new Color (255, 140, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (153, 50, 204, 255).
		/// </summary>
		public static Color DarkOrchid { get { return new Color (153, 50, 204, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (139, 0, 0, 255).
		/// </summary>
		public static Color DarkRed { get { return new Color (139, 0, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (233, 150, 122, 255).
		/// </summary>
		public static Color DarkSalmon { get { return new Color (233, 150, 122, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (143, 188, 139, 255).
		/// </summary>
		public static Color DarkSeaGreen { get { return new Color (143, 188, 139, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (72, 61, 139, 255).
		/// </summary>
		public static Color DarkSlateBlue { get { return new Color (72, 61, 139, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (47, 79, 79, 255).
		/// </summary>
		public static Color DarkSlateGray { get { return new Color (47, 79, 79, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 206, 209, 255).
		/// </summary>
		public static Color DarkTurquoise { get { return new Color (0, 206, 209, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (148, 0, 211, 255).
		/// </summary>
		public static Color DarkViolet { get { return new Color (148, 0, 211, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 20, 147, 255).
		/// </summary>
		public static Color DeepPink { get { return new Color (255, 20, 147, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 191, 255, 255).
		/// </summary>
		public static Color DeepSkyBlue { get { return new Color (0, 191, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (105, 105, 105, 255).
		/// </summary>
		public static Color DimGray { get { return new Color (105, 105, 105, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (30, 144, 255, 255).
		/// </summary>
		public static Color DodgerBlue { get { return new Color (30, 144, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (178, 34, 34, 255).
		/// </summary>
		public static Color Firebrick { get { return new Color (178, 34, 34, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 250, 240, 255).
		/// </summary>
		public static Color FloralWhite { get { return new Color (255, 250, 240, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (34, 139, 34, 255).
		/// </summary>
		public static Color ForestGreen { get { return new Color (34, 139, 34, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 0, 255, 255).
		/// </summary>
		public static Color Fuchsia { get { return new Color (255, 0, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (220, 220, 220, 255).
		/// </summary>
		public static Color Gainsboro { get { return new Color (220, 220, 220, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (248, 248, 255, 255).
		/// </summary>
		public static Color GhostWhite { get { return new Color (248, 248, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 215, 0, 255).
		/// </summary>
		public static Color Gold { get { return new Color (255, 215, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (218, 165, 32, 255).
		/// </summary>
		public static Color Goldenrod { get { return new Color (218, 165, 32, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (128, 128, 128, 255).
		/// </summary>
		public static Color Gray { get { return new Color (128, 128, 128, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 128, 0, 255).
		/// </summary>
		public static Color Green { get { return new Color (0, 128, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (173, 255, 47, 255).
		/// </summary>
		public static Color GreenYellow { get { return new Color (173, 255, 47, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (240, 255, 240, 255).
		/// </summary>
		public static Color Honeydew { get { return new Color (240, 255, 240, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 105, 180, 255).
		/// </summary>
		public static Color HotPink { get { return new Color (255, 105, 180, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (205, 92, 92, 255).
		/// </summary>
		public static Color IndianRed { get { return new Color (205, 92, 92, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (75, 0, 130, 255).
		/// </summary>
		public static Color Indigo { get { return new Color (75, 0, 130, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 255, 240, 255).
		/// </summary>
		public static Color Ivory { get { return new Color (255, 255, 240, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (240, 230, 140, 255).
		/// </summary>
		public static Color Khaki { get { return new Color (240, 230, 140, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (230, 230, 250, 255).
		/// </summary>
		public static Color Lavender { get { return new Color (230, 230, 250, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 240, 245, 255).
		/// </summary>
		public static Color LavenderBlush { get { return new Color (255, 240, 245, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (124, 252, 0, 255).
		/// </summary>
		public static Color LawnGreen { get { return new Color (124, 252, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 250, 205, 255).
		/// </summary>
		public static Color LemonChiffon { get { return new Color (255, 250, 205, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (173, 216, 230, 255).
		/// </summary>
		public static Color LightBlue { get { return new Color (173, 216, 230, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (240, 128, 128, 255).
		/// </summary>
		public static Color LightCoral { get { return new Color (240, 128, 128, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (224, 255, 255, 255).
		/// </summary>
		public static Color LightCyan { get { return new Color (224, 255, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (250, 250, 210, 255).
		/// </summary>
		public static Color LightGoldenrodYellow { get { return new Color (250, 250, 210, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (144, 238, 144, 255).
		/// </summary>
		public static Color LightGreen { get { return new Color (144, 238, 144, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (211, 211, 211, 255).
		/// </summary>
		public static Color LightGray { get { return new Color (211, 211, 211, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 182, 193, 255).
		/// </summary>
		public static Color LightPink { get { return new Color (255, 182, 193, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 160, 122, 255).
		/// </summary>
		public static Color LightSalmon { get { return new Color (255, 160, 122, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (32, 178, 170, 255).
		/// </summary>
		public static Color LightSeaGreen { get { return new Color (32, 178, 170, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (135, 206, 250, 255).
		/// </summary>
		public static Color LightSkyBlue { get { return new Color (135, 206, 250, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (119, 136, 153, 255).
		/// </summary>
		public static Color LightSlateGray { get { return new Color (119, 136, 153, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (176, 196, 222, 255).
		/// </summary>
		public static Color LightSteelBlue { get { return new Color (176, 196, 222, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 255, 224, 255).
		/// </summary>
		public static Color LightYellow { get { return new Color (255, 255, 224, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 255, 0, 255).
		/// </summary>
		public static Color Lime { get { return new Color (0, 255, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (50, 205, 50, 255).
		/// </summary>
		public static Color LimeGreen { get { return new Color (50, 205, 50, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (250, 240, 230, 255).
		/// </summary>
		public static Color Linen { get { return new Color (250, 240, 230, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 0, 255, 255).
		/// </summary>
		public static Color Magenta { get { return new Color (255, 0, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (128, 0, 0, 255).
		/// </summary>
		public static Color Maroon { get { return new Color (128, 0, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (102, 205, 170, 255).
		/// </summary>
		public static Color MediumAquamarine { get { return new Color (102, 205, 170, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 0, 205, 255).
		/// </summary>
		public static Color MediumBlue { get { return new Color (0, 0, 205, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (186, 85, 211, 255).
		/// </summary>
		public static Color MediumOrchid { get { return new Color (186, 85, 211, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (147, 112, 219, 255).
		/// </summary>
		public static Color MediumPurple { get { return new Color (147, 112, 219, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (60, 179, 113, 255).
		/// </summary>
		public static Color MediumSeaGreen { get { return new Color (60, 179, 113, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (123, 104, 238, 255).
		/// </summary>
		public static Color MediumSlateBlue { get { return new Color (123, 104, 238, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 250, 154, 255).
		/// </summary>
		public static Color MediumSpringGreen { get { return new Color (0, 250, 154, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (72, 209, 204, 255).
		/// </summary>
		public static Color MediumTurquoise { get { return new Color (72, 209, 204, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (199, 21, 133, 255).
		/// </summary>
		public static Color MediumVioletRed { get { return new Color (199, 21, 133, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (25, 25, 112, 255).
		/// </summary>
		public static Color MidnightBlue { get { return new Color (25, 25, 112, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (245, 255, 250, 255).
		/// </summary>
		public static Color MintCream { get { return new Color (245, 255, 250, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 228, 225, 255).
		/// </summary>
		public static Color MistyRose { get { return new Color (255, 228, 225, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 228, 181, 255).
		/// </summary>
		public static Color Moccasin { get { return new Color (255, 228, 181, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 222, 173, 255).
		/// </summary>
		public static Color NavajoWhite { get { return new Color (255, 222, 173, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 0, 128, 255).
		/// </summary>
		public static Color Navy { get { return new Color (0, 0, 128, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (253, 245, 230, 255).
		/// </summary>
		public static Color OldLace { get { return new Color (253, 245, 230, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (128, 128, 0, 255).
		/// </summary>
		public static Color Olive { get { return new Color (128, 128, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (107, 142, 35, 255).
		/// </summary>
		public static Color OliveDrab { get { return new Color (107, 142, 35, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 165, 0, 255).
		/// </summary>
		public static Color Orange { get { return new Color (255, 165, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 69, 0, 255).
		/// </summary>
		public static Color OrangeRed { get { return new Color (255, 69, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (218, 112, 214, 255).
		/// </summary>
		public static Color Orchid { get { return new Color (218, 112, 214, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (238, 232, 170, 255).
		/// </summary>
		public static Color PaleGoldenrod { get { return new Color (238, 232, 170, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (152, 251, 152, 255).
		/// </summary>
		public static Color PaleGreen { get { return new Color (152, 251, 152, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (175, 238, 238, 255).
		/// </summary>
		public static Color PaleTurquoise { get { return new Color (175, 238, 238, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (219, 112, 147, 255).
		/// </summary>
		public static Color PaleVioletRed { get { return new Color (219, 112, 147, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 239, 213, 255).
		/// </summary>
		public static Color PapayaWhip { get { return new Color (255, 239, 213, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 218, 185, 255).
		/// </summary>
		public static Color PeachPuff { get { return new Color (255, 218, 185, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (205, 133, 63, 255).
		/// </summary>
		public static Color Peru { get { return new Color (205, 133, 63, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 192, 203, 255).
		/// </summary>
		public static Color Pink { get { return new Color (255, 192, 203, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (221, 160, 221, 255).
		/// </summary>
		public static Color Plum { get { return new Color (221, 160, 221, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (176, 224, 230, 255).
		/// </summary>
		public static Color PowderBlue { get { return new Color (176, 224, 230, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (128, 0, 128, 255).
		/// </summary>
		public static Color Purple { get { return new Color (128, 0, 128, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 0, 0, 255).
		/// </summary>
		public static Color Red { get { return new Color (255, 0, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (188, 143, 143, 255).
		/// </summary>
		public static Color RosyBrown { get { return new Color (188, 143, 143, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (65, 105, 225, 255).
		/// </summary>
		public static Color RoyalBlue { get { return new Color (65, 105, 225, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (139, 69, 19, 255).
		/// </summary>
		public static Color SaddleBrown { get { return new Color (139, 69, 19, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (250, 128, 114, 255).
		/// </summary>
		public static Color Salmon { get { return new Color (250, 128, 114, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (244, 164, 96, 255).
		/// </summary>
		public static Color SandyBrown { get { return new Color (244, 164, 96, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (46, 139, 87, 255).
		/// </summary>
		public static Color SeaGreen { get { return new Color (46, 139, 87, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 245, 238, 255).
		/// </summary>
		public static Color SeaShell { get { return new Color (255, 245, 238, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (160, 82, 45, 255).
		/// </summary>
		public static Color Sienna { get { return new Color (160, 82, 45, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (192, 192, 192, 255).
		/// </summary>
		public static Color Silver { get { return new Color (192, 192, 192, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (135, 206, 235, 255).
		/// </summary>
		public static Color SkyBlue { get { return new Color (135, 206, 235, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (106, 90, 205, 255).
		/// </summary>
		public static Color SlateBlue { get { return new Color (106, 90, 205, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (112, 128, 144, 255).
		/// </summary>
		public static Color SlateGray { get { return new Color (112, 128, 144, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 250, 250, 255).
		/// </summary>
		public static Color Snow { get { return new Color (255, 250, 250, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 255, 127, 255).
		/// </summary>
		public static Color SpringGreen { get { return new Color (0, 255, 127, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (70, 130, 180, 255).
		/// </summary>
		public static Color SteelBlue { get { return new Color (70, 130, 180, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (210, 180, 140, 255).
		/// </summary>
		public static Color Tan { get { return new Color (210, 180, 140, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (0, 128, 128, 255).
		/// </summary>
		public static Color Teal { get { return new Color (0, 128, 128, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (216, 191, 216, 255).
		/// </summary>
		public static Color Thistle { get { return new Color (216, 191, 216, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 99, 71, 255).
		/// </summary>
		public static Color Tomato { get { return new Color (255, 99, 71, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (64, 224, 208, 255).
		/// </summary>
		public static Color Turquoise { get { return new Color (64, 224, 208, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (238, 130, 238, 255).
		/// </summary>
		public static Color Violet { get { return new Color (238, 130, 238, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (245, 222, 179, 255).
		/// </summary>
		public static Color Wheat { get { return new Color (245, 222, 179, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 255, 255, 255).
		/// </summary>
		public static Color White { get { return new Color (255, 255, 255, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (245, 245, 245, 255).
		/// </summary>
		public static Color WhiteSmoke { get { return new Color (245, 245, 245, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (255, 255, 0, 255).
		/// </summary>
		public static Color Yellow { get { return new Color (255, 255, 0, 255); } }

		/// <summary>
		/// Gets the system color with (R, G, B, A) = (154, 205, 50, 255).
		/// </summary>
		public static Color YellowGreen { get { return new Color (154, 205, 50, 255); } }

		#endregion

		#endregion

		#region IEquatable<Color> Members

		/// <summary>
		/// Compares whether this Color structure is equal to the specified Color.
		/// </summary>
		/// <param name="other">The Color structure to compare to.</param>
		/// <returns>True if both Color structures contain the same components; false otherwise.</returns>
		public bool Equals (Color other)
		{
			return
				this.R == other.R &&
			this.G == other.G &&
			this.B == other.B &&
			this.A == other.A;
		}

		#endregion

	}
}
