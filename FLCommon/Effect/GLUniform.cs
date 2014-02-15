using System;

namespace FLCommon
{
	class GLUniform
	{
		public string Name;
		public int Location;
		public object Value;
		public bool IsTexture;

		public GLUniform (string name, int loc, object value, bool isTexture)
		{
			Name = name;
			Location = loc;
			Value = value;
			IsTexture = isTexture;
		}
	}
}

