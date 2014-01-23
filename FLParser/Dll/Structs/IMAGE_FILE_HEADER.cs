using System;
using System.Runtime.InteropServices;
namespace FLParser.Dll.Structs
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct IMAGE_FILE_HEADER 
	{
		public uint Magic;
		public ushort Machine;
		public ushort NumberOfSections;
		public uint TimeDateStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort SizeOfOptionalHeader;
		public ushort Characteristics;
	}
}

