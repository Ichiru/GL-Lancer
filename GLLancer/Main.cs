using System;
using System.IO;
using System.Reflection;
using FLApi;
namespace GLLancer
{
	class MainClass
	{
		public static string FreelancerDirectory = "";
		public static string AssemblyDirectory;
		public static FreelancerIni FLIni;
		public static void Main (string[] args)
		{
			Console.Write ("Enter Freelancer Directory: ");
			FreelancerDirectory = Console.ReadLine();
			AssemblyDirectory = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			FLIni = new FreelancerIni (FreelancerDirectory);
			using (var game = new MainWindow()) {
				game.Run ();
			}
		}
	}
}
