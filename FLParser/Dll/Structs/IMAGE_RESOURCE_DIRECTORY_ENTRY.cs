using System;
using System.Runtime.InteropServices;
namespace FLParser.Dll.Structs
{
	[StructLayout(LayoutKind.Sequential)]
	struct IMAGE_RESOURCE_DIRECTORY_ENTRY
	{
		public uint OffsetToData;
		public uint Size;
		public uint CodePage;
		public uint Reserved;
	}
}

