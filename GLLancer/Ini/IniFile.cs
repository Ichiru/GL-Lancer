//Copyright (C) Ichiru 2013
//Check LICENSE for licensing information
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace GLLancer
{
	public class IniFile
	{
		#region BINI Constants
		//The ASCII string BINI
		const uint MAGIC = 0x494E4942;
		//Change this if you want to accept versions other than one
		//(the freelancer default)
		static readonly int[] VERSIONS_ACCEPTED = new int[] { 1 };
		#endregion

		public List<IniSection> Sections { get; private set; }

		public IniFile (string filename)
		{
			Sections = new List<IniSection> ();
			using (var stream = File.OpenRead (filename)) {
				//read in the first 4 bytes of the file. If it equals "BINI" then it is a BINI file
				byte[] magic_buffer = new byte[4];
				stream.Read (magic_buffer, 0, 4);
				if (BitConverter.ToUInt32 (magic_buffer, 0) == MAGIC) {
					//Read in Binary mode
					LoadBinary (stream);
				} else {
					//Read it like a regular INI file
					stream.Seek (0, SeekOrigin.Begin);
					LoadText (stream);
				}
				stream.Close ();
			}
		}

		#region Binary Reading Methods
		void LoadBinary (Stream stream)
		{
			using (var reader = new BinaryReader(stream)) {
				//Check the version of the BINI file
				int version = reader.ReadInt32 ();
				if (!CheckVersion (version))
					throw new Exception (string.Format ("Version {0} is not supported", version));
				//read the strings
				int strings_offset = reader.ReadInt32 ();
				long current_offset = stream.Position;
				stream.Seek (strings_offset, SeekOrigin.Begin);
				byte[] buffer = new byte[stream.Length - strings_offset];
				stream.Read (buffer, 0, (int)stream.Length - strings_offset);
				string strings = Encoding.ASCII.GetString (buffer);
				//Seek back to the section table
				stream.Seek (current_offset, SeekOrigin.Begin);
				do {
					//Read the section
					IniSection section;
					short section_nameIndex = reader.ReadInt16 ();
					section = new IniSection (GetString (strings, section_nameIndex));
					short entryCount = reader.ReadInt16 ();
					//Read the section's entries
					for (int i = 0; i < entryCount; i++) {
						IniEntry entry;
						short entry_nameIndex = reader.ReadInt16 ();
						entry = new IniEntry (GetString (strings, entry_nameIndex));
						byte valueCount = reader.ReadByte ();
						for (int j = 0; j < valueCount; j++) {
							IniValueType valueType = (IniValueType)reader.ReadByte ();
							object data;
							switch (valueType) {
							case IniValueType.Integer:
								data = reader.ReadInt32 ();
								break;
							case IniValueType.Float:
								data = reader.ReadSingle ();
								break;
							case IniValueType.String:
								//All other string table pointers are 16-bit
								//But for values it's 32-bit
								int index = reader.ReadInt32 ();
								data = GetString (strings, index);
								break;
							default:
								throw new Exception ("Ini File Corrupted");
							}
							entry.Values.Add (new IniValue (valueType, data));

						}
						section.Entries.Add (entry);
					}
					Sections.Add (section);
				} while (stream.Position < strings_offset);
			}
		}

		string GetString (string strings, int index)
		{
			int nullindex = strings.IndexOf ('\0', index);
			return strings.Substring (index, nullindex - index);
		}

		bool CheckVersion (int version)
		{
			for (int i = 0; i < VERSIONS_ACCEPTED.Length; i++) {
				if (version == VERSIONS_ACCEPTED [i])
					return true;
			}
			return false;
		}
		#endregion

		#region Text Reading Methods
		void LoadText (Stream stream)
		{
			using (var reader = new StreamReader(stream)) {
				IniSection currentSection = null;
				while (!reader.EndOfStream) {
					string s = reader.ReadLine ().Trim ();
					int comment_index = s.IndexOf (';');
					if (comment_index != -1)
						s = s.Substring (0, comment_index);
					if (string.IsNullOrEmpty (s) || string.IsNullOrWhiteSpace (s))
						continue;
					if (s.StartsWith ("[") && s.EndsWith ("]")) {
						if (currentSection != null)
							Sections.Add (currentSection);
						string sectionName = s.Substring (1, s.Length - 2);
						currentSection = new IniSection (sectionName);
					} else {
						if (currentSection == null)
							throw new Exception ("Broken Ini");
						if (!s.Contains ("="))
							continue;
						string[] split = s.Split ('=');
						string val = split[1].Trim ();
						var entry = new IniEntry (split [0].Trim ());
						IniValueType valuetype;
						object data;
						if (val.Contains (",")) {
							string[] values = val.Split (',');
							for (int i = 0; i < values.Length; i++) {
								val = values [i].Trim ();
								ParseValue (val, out valuetype, out data);
								if(valuetype == IniValueType.Float)
								{ //Floats are only grouped with floats
									entry.Values.Clear ();
									for (int j = 0; j < values.Length;j++)
									{
										val = values[j].Trim ();
										valuetype = IniValueType.Float;
										data = float.Parse (val);
										entry.Values.Add (new IniValue(valuetype,data));
									}
									break;
								}
								entry.Values.Add (new IniValue(valuetype,data));
							}
						currentSection.Entries.Add (entry);
						} else {
							ParseValue (val,out valuetype,out data);
							entry.Values.Add (new IniValue (valuetype, data));
							currentSection.Entries.Add (entry);
						}

					}
				}
			}
		}

		void ParseValue (string val, out IniValueType valuetype, out object data)
		{
			int i;
			float f;
			if (int.TryParse (val, out i)) {
				valuetype = IniValueType.Integer;
				data = i;
			} else if (float.TryParse (val, out f)) {
				valuetype = IniValueType.Float;
				data = f;
			} else {
				valuetype = IniValueType.String;
				data = val;
			}
		}
		#endregion
	
		public IniSection[] GetSections (string name, bool case_sensitive)
		{
			if (!case_sensitive) {
				string nameLower = name.ToLower ();
				var results = from s in Sections where s.Name.ToLower () == nameLower select s;
				return results.ToArray ();
			} else {
				var results = from s in Sections where s.Name == name select s;
				return results.ToArray ();
			}
		}
		/// <summary>
		/// Query an object from the File
		/// </summary>
		/// <param name='query'>
		/// The query used
		/// The format is the section, followed by the entry, and then the value index each separated by forward slashes
		/// Example: WinCamera/fovx/0
		/// You can also specify which section and value indexes you want by putting # and the number after the section/entry's name
		/// Example: WinCamera#0/fovx#0/0
		/// 		 WinCamera#1/fovx/0
		/// </param>
		/// <param name='case_sensitive'>
		/// Specifies whether or not the name lookups are case sensitive
		/// </param>
		public object Query(string query, bool case_sensitive)
		{
			string[] split = query.Split ('/');
			//first split should be section
			object current = null;
			string value;
			int number;
			for(int i = 0; i < split.Length;i++)
			{
				switch(i)
				{
				case 0:
					ParseQueryPart (split[i],out value, out number);
					IniSection[] sections = GetSections (value,case_sensitive);
					if(number != -1)
						current = sections[number];
					else
						current = sections[0];
					break;
				case 1:
					ParseQueryPart (split[i],out value, out number);
					IniEntry[] entries = ((IniSection)current).GetEntries (value,case_sensitive);
					if(number != -1)
						current = entries[number];
					else
						current = entries[0];
					break;
				case 2:
					number = int.Parse (split[i]);
					current = ((IniEntry)current).Values[number];
					return current;
				}
			}
			return current;
		}
		void ParseQueryPart (string part, out string value, out int number)
		{
			if (part.Contains ("#")) {
				string[] split = part.Split ('#');
				value = split [0];
				number = int.Parse (split [1]);
			} else {
				value = part;
				number = -1;
			}
		}
	}
}

