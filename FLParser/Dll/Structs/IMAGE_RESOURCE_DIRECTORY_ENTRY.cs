using System;
using System.Runtime.InteropServices;
namespace FLParser.Dll.Structs
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct IMAGE_RESOURCE_DIRECTORY_ENTRY
	{
		public uint Name;
		public uint OffsetToData;
	}
}

