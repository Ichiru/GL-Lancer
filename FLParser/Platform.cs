using System;
using System.IO;

namespace FLParser
{
	public static class Platform
	{
		public static OS RunningOS;
		static Platform ()
		{
			switch (Environment.OSVersion.Platform) {
			case PlatformID.Unix:
				if (Directory.Exists ("/Applications")
				    & Directory.Exists ("/System")
				    & Directory.Exists ("/Users")
				    & Directory.Exists ("/Volumes"))
					RunningOS = OS.Mac;
				else
					RunningOS = OS.Linux;
				break;
			case PlatformID.MacOSX:
				RunningOS = OS.Mac;
				break;
			default:
				RunningOS = OS.Windows;
				break;
			}
		}
	}

	public enum OS
	{
		Windows,
		Mac,
		Linux
	}
}

