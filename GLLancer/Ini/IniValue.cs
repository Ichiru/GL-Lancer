using System;

namespace GLLancer
{
	public class IniValue
	{
		public IniValueType ValueType { get; private set; }
		public object Data { get; private set; }
		public IniValue (IniValueType valueType, object data)
		{
			ValueType = valueType;
			Data = data;
		}
	}
}

