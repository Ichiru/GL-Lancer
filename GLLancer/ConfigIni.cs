using System;
using FLParser.Ini;

namespace GLLancer
{
	public class ConfigIni : IniFile
	{
		public string FreelancerDirectory;
		public ConfigIni (string filename)
		{
			var sections = ParseFile (filename);
			FreelancerDirectory = sections [0] ["freelancer_directory"] [0].ToString ();
		}
	}
}

