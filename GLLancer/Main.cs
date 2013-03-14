using System;

namespace GLLancer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (var window = new TestWindow()) {
				window.Run ();
			}
		}
	}
}
