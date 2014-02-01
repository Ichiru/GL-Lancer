using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FLApi.Utf.Mat
{
	public static class EffectExtensions
	{
		public static void SetParameter(this Effect effect, string parameter, Vector3 value)
		{
			var param = effect.Parameters [parameter];
			if (param != null)
				param.SetValue (value);
		}
		public static void SetParameter(this Effect effect, string parameter, Vector4 value)
		{
			var param = effect.Parameters [parameter];
			if (param != null)
				param.SetValue (value);
		}
	}
}

