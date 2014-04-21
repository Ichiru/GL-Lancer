using System;
using System.IO;
using System.Reflection;
using FLApi;
namespace GLLancer
{
	class MainClass
	{
		public static string AssemblyDirectory;
		public static FreelancerIni FLIni;
		public static ConfigIni Configuration;
		public static void Main (string[] args)
		{
			Configuration = new ConfigIni ("config.ini");
			AssemblyDirectory = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			FLIni = new FreelancerIni (Configuration.FreelancerDirectory);
			using (var game = new MainWindow()) {
				game.Run ();
			}
		}
	}
}
