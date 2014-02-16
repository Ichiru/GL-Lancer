using System;

namespace FLCommon
{
	public class UniformDescription
	{
		public string Name;
		public UniformTypes Type;
		public UniformTypes ArrayType;
		public int ArrayLength;
		public UniformDescription (string name, UniformTypes type)
		{
			Name = name;
			Type = type;
		}
	}
}

