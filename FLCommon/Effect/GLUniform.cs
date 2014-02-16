using System;
using OpenTK;
namespace FLCommon
{
	class GLUniform
	{
		public UniformDescription Description;
		public int Location;
		public object Value;
		public bool IsTexture;

		public GLUniform (UniformDescription description, int loc, bool isTexture)
		{
			Description = description;
			Location = loc;
			Value = null;
			IsTexture = isTexture;
		}
	}
}

