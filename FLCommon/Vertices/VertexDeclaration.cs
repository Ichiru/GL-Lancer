using System;

namespace FLCommon
{
	public class VertexDeclaration
	{
		public VertexElement[] Elements;
		public VertexDeclaration (params VertexElement[] elements)
		{
			Elements = elements;
		}
	}
}

