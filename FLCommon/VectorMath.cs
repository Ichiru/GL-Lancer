using System;
using OpenTK;
namespace FLCommon
{
	public static class VectorMath
	{
		public static readonly Vector3 Zero = new Vector3(0f, 0f, 0f);
		public static readonly Vector3 One = new Vector3(1f, 1f, 1f);
		public static readonly Vector3 UnitX = new Vector3(1f, 0f, 0f);
		public static readonly Vector3 UnitY = new Vector3(0f, 1f, 0f);
		public static readonly Vector3 UnitZ = new Vector3(0f, 0f, 1f);
		public static readonly Vector3 Up = new Vector3(0f, 1f, 0f);
		public static readonly Vector3 Down = new Vector3(0f, -1f, 0f);
		public static readonly Vector3 Right = new Vector3(1f, 0f, 0f);
		public static readonly Vector3 Left = new Vector3(-1f, 0f, 0f);
		public static readonly Vector3 Forward = new Vector3(0f, 0f, -1f);
		public static readonly Vector3 Backward = new Vector3(0f, 0f, 1f);

		public static float Distance(Vector3 a, Vector3 b)
		{
			float result;
			result = (a.X - b.X) * (a.X - b.X) +
				(a.Y - b.Y) * (a.Y - b.Y) +
					(a.Z - b.Z) * (a.Z - b.Z);
			return (float)Math.Sqrt(result);
		}

	}
}

