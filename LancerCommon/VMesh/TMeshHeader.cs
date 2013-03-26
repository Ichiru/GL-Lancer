using System;

namespace GLLancer
{
	public struct TMeshHeader
	{
		public uint MaterialCrc;
		public int StartVertex;
		public int EndVertex;
		public int NumRefVertices;
		public int Padding;

		public int BaseVertex;
		public int TriangleStart;
	}
}

