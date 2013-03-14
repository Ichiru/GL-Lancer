//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;
using System.Collections.Generic;
using System.Linq;
namespace GLLancer
{
	/// <summary>
	/// Defines an entry in an IniSection
	/// </summary>
	public class IniEntry
	{
		public string Name { get; private set; }
		public List<IniValue> Values { get; private set; }
		public IniEntry (string name)
		{
			Name = name;
			Values = new List<IniValue> ();
		}
	}
}

