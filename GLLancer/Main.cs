using System;

namespace GLLancer
{
	class MainClass
	{
		public static string FreelancerDirectory = "";
		public static void Main (string[] args)
		{
			FreelancerDirectory = Console.ReadLine ();
			using (var window = new TestWindow()) {
				window.Run ();
			}
		}
	}
}
