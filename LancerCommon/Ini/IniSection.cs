//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;
using System.Collections.Generic;
using System.Linq;
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
		public IniEntry[] GetEntries (string name, bool case_sensitive)
		{
			if (!case_sensitive) {
				string nameLower = name.ToLower ();
				var results = from e in Entries where e.Name.ToLower () == nameLower select e;
				return results.ToArray ();
			} else {
				var results = from e in Entries where e.Name == name select e;
				return results.ToArray ();
			}
		}
		public IniEntry[] GetEntries(string name)
		{
			return GetEntries (name,false);
		}
	}
}

