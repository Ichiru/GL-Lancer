//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;
using System.Collections.Generic;
namespace GLLancer
{
	/// <summary>
	/// Defines a section in an IniFile
	/// </summary>
	public class IniSection
	{
		public string Name { get; private set; }
		public List<IniEntry> Entries { get; private set; }
		public IniSection (string name)
		{
			Name = name;
			Entries = new List<IniEntry>();
		}
	}
}

