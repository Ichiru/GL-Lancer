using System;
using System.IO;
using System.Diagnostics;
namespace ShaderCompiler
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			foreach (var f in Directory.GetFiles("Shaders", "*.fx", SearchOption.AllDirectories)) {
				string dirPath = Path.ChangeExtension (@"GLLancer\Assets\effects" + f.Replace ("Shaders", ""), ".fxg");
				var info = new ProcessStartInfo(@"Third-Party\2MGFX\2MGFX.exe","\"" + f + "\" \"" + dirPath + "\"");
				info.UseShellExecute = false;
				var p = Process.Start (info);
				p.WaitForExit ();
				if (p.ExitCode != 0) {
					Console.WriteLine ("An error occurred");
					return;
				}
			}
		}
	}
}
