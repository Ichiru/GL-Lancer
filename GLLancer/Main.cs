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
			//FreelancerDirectory = Console.ReadLine ();
			AssemblyDirectory = Path.GetDirectoryName (Assembly.GetExecutingAssembly ().Location);
			var s = new Shader ("quadtexture");
			/*using (var window = new TestWindow()) {
				window.Run ();
			}*/
		}
	}
}
