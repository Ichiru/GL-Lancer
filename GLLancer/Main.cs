using System;
using System.IO;
using System.Reflection;
namespace GLLancer
{
	class MainClass
	{
		public static string FreelancerDirectory = "";
		public static string AssemblyDirectory;
		public static void Main (string[] args)
		{
			Console.Write ("Enter Freelancer Directory: ");
			FreelancerDirectory = Console.ReadLine ();
			AssemblyDirectory = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			//using (var game = new MainGame()) {
				//game.Run ();
			//}
			var file = new FLParser.DllFile (Path.Combine (FreelancerDirectory, "EXE/infocards.dll"));
			Console.WriteLine ("");
		}
	}
}
