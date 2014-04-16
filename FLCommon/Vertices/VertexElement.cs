using System;
using OpenTK.Graphics.OpenGL;

namespace FLCommon
{
	public class VertexElement
	{
		//public properties
		public int Offset { get; private set; }
		public VertexElementFormat Format { get; private set; }
		public VertexElementUsage Usage { get; private set; }
		public int UsageNumber { get; private set; }
		//opengl variables (the important stuff)
		internal int GLNumberOfElements;
		internal VertexPointerType GLPointerType;
		internal VertexAttribPointerType GLAttribPointerType; 
		internal bool Normalized;
		public VertexElement (int offset, VertexElementFormat format, VertexElementUsage usage, int usageNumber)
		{
			Offset = offset;
			Format = format;
			Usage = usage;
			UsageNumber = usageNumber;
			GLNumberOfElements = format.GetNumberOfElements ();
			GLPointerType = format.GetPointerType ();
			GLAttribPointerType = format.GetAttribPointerType ();
			Normalized = (format == VertexElementFormat.Byte4);
		}
	}

	public enum VertexElementFormat
	{
		Vector2,
		Vector3,
		Byte4
	}

	public enum VertexElementUsage
	{
		Position,
		Normal,
		TextureCoordinate,
		Tangent,
		Binormal,
		BlendWeight
	}

	static class VertexExtensions
	{
		public static int GetNumberOfElements (this VertexElementFormat elementFormat)
		{
			switch (elementFormat) {
			case VertexElementFormat.Vector2:
				return 2;

			case VertexElementFormat.Vector3:
				return 3;

			case VertexElementFormat.Byte4:
				return 4;
			}

			throw new ArgumentException ();
		}

		public static VertexPointerType GetPointerType (this VertexElementFormat elementFormat)
		{
			switch (elementFormat) {
			case VertexElementFormat.Vector2:
				return VertexPointerType.Float;

			case VertexElementFormat.Vector3:
				return VertexPointerType.Float;


			case VertexElementFormat.Byte4:
				return VertexPointerType.Short;

			}

			throw new ArgumentException ();
		}

		public  static int Size(this VertexElementFormat elementFormat)
		{
			switch (elementFormat)
			{
			case VertexElementFormat.Vector2:
				return 8;
			case VertexElementFormat.Vector3:
				return 12;
			case VertexElementFormat.Byte4:
				return 4;
			}
			return 0;
		}

		public static VertexAttribPointerType GetAttribPointerType (this VertexElementFormat elementFormat)
		{
			switch (elementFormat) {
			case VertexElementFormat.Vector2:
				return VertexAttribPointerType.Float;

			case VertexElementFormat.Vector3:
				return VertexAttribPointerType.Float;

			case VertexElementFormat.Byte4:
				return VertexAttribPointerType.UnsignedByte;
			}

			throw new ArgumentException ();
		}
	}
}