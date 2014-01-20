//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
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
		public static implicit operator string (IniValue i)
		{
			if (i.ValueType == IniValueType.String) {
				return (string)i.Data;
			} else {
				return i.Data.ToString ();
			}
		}
	}
}

