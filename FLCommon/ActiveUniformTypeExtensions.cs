using System;
using OpenTK.Graphics.OpenGL;
using OpenTK;
namespace FLCommon
{
	public static class ActiveUniformTypeExtensions
	{
		public static object GetDefault (this ActiveUniformType type)
		{
			switch (type) {
			case ActiveUniformType.Int:
				return 0;
			case ActiveUniformType.UnsignedInt:
				return 0U;
			case ActiveUniformType.Float:
				return 0f;
			case ActiveUniformType.Double:
				return 0.0;
			case ActiveUniformType.FloatVec2:
				return Vector2.Zero;
			case ActiveUniformType.FloatVec3:
				return Vector3.Zero;
			case ActiveUniformType.FloatVec4:
				return Vector4.Zero;
			case ActiveUniformType.Bool:
				return false;
			case ActiveUniformType.FloatMat2:
				return Matrix2.Zero;
			case ActiveUniformType.FloatMat3:
				return Matrix3.Zero;
			case ActiveUniformType.FloatMat4:
				return Matrix4.Zero;
			case ActiveUniformType.FloatMat2x3:
				return Matrix2x3.Zero;
			case ActiveUniformType.FloatMat2x4:
				return Matrix2x4.Zero;
			case ActiveUniformType.FloatMat3x4:
				return Matrix3x4.Zero;
			case ActiveUniformType.FloatMat4x2:
				return Matrix4x2.Zero;
			case ActiveUniformType.FloatMat4x3:
				return Matrix4x3.Zero;
			case ActiveUniformType.DoubleVec2:
				return Vector2d.Zero;
			case ActiveUniformType.DoubleVec3:
				return Vector3d.Zero;
			case ActiveUniformType.DoubleVec4:
				return Vector4d.Zero;
			default:
				throw new ArgumentOutOfRangeException ();
			}
		}
		public static bool IsTexture (this ActiveUniformType type)
		{
			return false;
		}
	}
}

