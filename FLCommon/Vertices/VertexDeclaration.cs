using System;

namespace FLCommon
{
	public class VertexDeclaration
	{
		public VertexElement[] Elements;
		public int VertexStride;
		public VertexDeclaration (params VertexElement[] elements)
		{
			Elements = elements;
		}

	}
}

