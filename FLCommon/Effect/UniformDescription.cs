using System;

namespace FLCommon
{
	public class UniformDescription
	{
		public string Name;
		public GLSLTypes Type;
		public GLSLTypes ArrayType;
		public int ArrayLength;
		public UniformDescription (string name, GLSLTypes type)
		{
			Name = name;
			Type = type;
		}
		public override bool Equals (object obj)
		{
			if (obj is UniformDescription) {
				var b = (UniformDescription)obj;
				return (Name == b.Name) && (Type == b.Type) && (ArrayType == b.ArrayType) && (ArrayLength == b.ArrayLength);
			} else {
				return false;
			}
		}
		public static bool operator ==(UniformDescription a, UniformDescription b) {
			return a.Equals(b);
		}
		public static bool operator !=(UniformDescription a, UniformDescription b) {
			return !a.Equals(b);
		}
	}
}

