using System;
using System.Xml;
namespace FLParser.Dll
{
	/// <summary>
	/// Interface for reading DLL files
	/// </summary>
	interface IDllProvider
	{
		string GetString(ushort resourceId);
		XmlDocument GetXml(ushort resourceId);
	}
}

