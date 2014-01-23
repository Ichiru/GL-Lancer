/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Original Code is Starchart code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 * 
 * Modified to be an interface by Ichiru
 * Portions created by Ichiru are Copyright (C) 2014 Ichiru. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.IO;

namespace FLParser.Dll
{
	class Win32DllProvider : IDllProvider
	{
		private const int LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

		private static class NativeMethods
		{
			[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
			public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

			[DllImport("kernel32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool FreeLibrary(IntPtr hModule);

			[DllImport("User32.dll", CharSet = CharSet.Unicode)]
			public static extern int LoadString(IntPtr hInstance, int uID, StringBuilder lpBuffer, int nBufferMax);

			[DllImport("kernel32.dll")]
			public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

			[DllImport("kernel32.dll", SetLastError = true)]
			public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);
		}

		private string path;
		private Dictionary<ushort, string> stringTable = new Dictionary<ushort, string>();
		private Dictionary<ushort, XmlDocument> xmlData = new Dictionary<ushort, XmlDocument>();

		public Win32DllProvider (string filename)
		{
			path = filename;
		}

		public string GetString(ushort resourceId)
		{
			if (!stringTable.ContainsKey(resourceId))
			{
				IntPtr hInstance = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);

				StringBuilder lpBuffer = new StringBuilder(255);
				int size = NativeMethods.LoadString(hInstance, resourceId, lpBuffer, lpBuffer.Capacity + 1);
				// TODO: improve resource loading
				if (size > lpBuffer.Capacity)
				{
					lpBuffer = new StringBuilder(size);
					size = NativeMethods.LoadString(hInstance, resourceId, lpBuffer, lpBuffer.Capacity + 1);
				}
				stringTable.Add(resourceId, lpBuffer.ToString());

				NativeMethods.FreeLibrary(hInstance);
			}

			return stringTable[resourceId];
		}

		public XmlDocument GetXml(ushort resourceId)
		{
			if (!xmlData.ContainsKey(resourceId))
			{
				IntPtr hInstance = NativeMethods.LoadLibraryEx(path, IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);

				IntPtr hResInfo = NativeMethods.FindResource(hInstance, new IntPtr(resourceId), new IntPtr(23));
				uint size = NativeMethods.SizeofResource(hInstance, hResInfo);
				if (size > 0)
				{
					IntPtr pt = NativeMethods.LoadResource(hInstance, hResInfo);

					byte[] bytes = new byte[size];
					Marshal.Copy(pt, bytes, 0, (int)size);
					NativeMethods.FreeLibrary(hInstance);

					XmlDocument result = new XmlDocument();
					result.Load(new XmlTextReader(new MemoryStream(bytes)));

					xmlData.Add(resourceId, result);
				}
				else
				{
					NativeMethods.FreeLibrary(hInstance);
					return null;
				}
			}

			return xmlData[resourceId];
		}
	}
}

