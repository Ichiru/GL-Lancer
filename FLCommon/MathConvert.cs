using System;
using OpenTK;
namespace FLCommon
{
	public class MathConvert
	{
		public static float ToRadians(float degrees)
		{
			return degrees * (MathHelper.Pi / 180f);
		}
	}
}

