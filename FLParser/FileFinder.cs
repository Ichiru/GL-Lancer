using System;
using System.IO;
using System.Collections.Generic;
namespace FLParser
{
	/// <summary>
	/// Provides case-insensitive file finding
	/// </summary>
	public static class FileFinder
	{
		static Dictionary<string,string> properCase = new Dictionary<string, string> ();
		public static void Initialize(string freelancerDir)
		{
			foreach (var f in Directory.GetFiles(freelancerDir, "*", SearchOption.AllDirectories)) {
				string fullPath = Path.GetFullPath (Path.Combine (freelancerDir, f));
				properCase.Add (fullPath.ToUpper (), fullPath);
			}
		}
		public static string GetFile (string filename)
		{
			if (!File.Exists (filename)) {
				var path = Path.GetFullPath (filename.Replace('\\',Path.DirectorySeparatorChar)).ToUpper ();
				if (!properCase.ContainsKey (path))
					throw new FileNotFoundException ();
				return properCase [path];
			} else {
				return filename;
			}
		}
	}
}

