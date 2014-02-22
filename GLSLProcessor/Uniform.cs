using System;
using FLCommon;
namespace GLSLProcessor
{
	public class Uniform
	{
		public string Name;
		public GLSLTypes Type;
		public GLSLTypes ArrayType;
		public int ArrayLength;
		public Uniform (string name, GLSLTypes type)
		{
			Name = name;
			Type = type;
		}
	}
}

