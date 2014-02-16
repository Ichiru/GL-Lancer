using System;
using FLCommon;
namespace GLSLProcessor
{
	public class Uniform
	{
		public string Name;
		public UniformTypes Type;
		public UniformTypes ArrayType;
		public int ArrayLength;
		public Uniform (string name, UniformTypes type)
		{
			Name = name;
			Type = type;
		}
	}
}

