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
			VertexStride = CalculateStride ();
		}
		internal static VertexDeclaration FromType(Type vertexType)
		{
			if (vertexType == null)
				throw new ArgumentNullException("vertexType", "Cannot be null");
			if (!vertexType.IsValueType)
				throw new ArgumentException("vertexType", "Must be value type");
			var type = Activator.CreateInstance(vertexType) as IVertexType;
			if (type == null)
				throw new ArgumentException("vertexData does not inherit IVertexType");
			var vertexDeclaration = type.VertexDeclaration;
			if (vertexDeclaration == null)
				throw new Exception("VertexDeclaration cannot be null");
			return vertexDeclaration;
		}

		int CalculateStride()
		{
			int max = 0;
			for (int i = 0; i < Elements.Length; i += 1)
			{
				int start = Elements [i].Offset + Elements [i].Format.Size ();
				if (max < start)
				{
					max = start;
				}
			}
			return max;
		}

	}
}

