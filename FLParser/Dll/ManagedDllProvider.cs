#pragma warning disable 0219 //unused variable warning disable
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using FLParser.Dll.Structs;
namespace FLParser.Dll
{
	class ManagedDllProvider : IDllProvider
	{
		const ushort DOS_MAGIC = 0x5A4D; //MZ
		const uint PE_MAGIC = 0x4550; //PE
		const ushort IMAGE_FILE_32BIT_MACHINE = 0x0100;

		bool hasResources = true; //not all PE files have .rsrc sections
		Dictionary<ushort, string> strings;
		Dictionary<ushort, XmlDocument> xml;

		public ManagedDllProvider (string filename)
		{
			byte[] rsrcSection;
			using (var reader = new BinaryReader(File.OpenRead(filename))) {
				var dosHeader = reader.ReadStruct<IMAGE_DOS_HEADER> ();
				if (dosHeader.e_magic != DOS_MAGIC)
					throw new FileFormatException (filename, "0x" + dosHeader.e_magic.ToString ("X"), "DLL (0x54AD)");
				reader.BaseStream.Seek (dosHeader.e_lfanew, SeekOrigin.Begin);
				var peHeader = reader.ReadStruct<IMAGE_FILE_HEADER> ();
				if(peHeader.Magic != PE_MAGIC)
					throw new FileFormatException(filename, "0x" + peHeader.Magic.ToString("X"), "DLL (0x4550)");
				if ((peHeader.Machine & IMAGE_FILE_32BIT_MACHINE) != IMAGE_FILE_32BIT_MACHINE)
					throw new FileVersionException (filename, "DLL", 64, 32); //TODO: Support 64-bit DLLs? 
				var optionalHeader = reader.ReadStruct<IMAGE_OPTIONAL_HEADER32> ();
				IMAGE_SECTION_HEADER rsrcHeader = new IMAGE_SECTION_HEADER();
				bool found = false;
				for (int i = 0; i < peHeader.NumberOfSections; i++) {
					var header = reader.ReadStruct<IMAGE_SECTION_HEADER> ();
					if (header.Section == ".rsrc") {
						rsrcHeader = header;
						found = true;
						break;
					}
				}
				if (!found) {
					hasResources = false;
					return;
				}
				reader.BaseStream.Seek (rsrcHeader.PointerToRawData, SeekOrigin.Begin);
				//reading it into a memory stream simplifies RVA resolving
				rsrcSection = reader.ReadBytes ((int)rsrcHeader.SizeOfRawData);
			}

			using (var reader = new BinaryReader(new MemoryStream(rsrcSection))) {
				var root = reader.ReadStruct<IMAGE_RESOURCE_DIRECTORY> ();
			}
		}

		public string GetString (ushort resourceId)
		{
			if (!hasResources)
				return "IDS";
			throw new NotImplementedException ();
		}
		public XmlDocument GetXml (ushort resourceId)
		{
			if (!hasResources) {
				var doc = new XmlDocument ();
				doc.LoadXml ("IDS");
				return doc;
			}
			throw new NotImplementedException ();
		}
	}
}