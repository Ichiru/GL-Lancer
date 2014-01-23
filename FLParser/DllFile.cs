﻿/* The contents of this file are subject to the Mozilla Public License
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
 * Modified to support multiple operating systems by Ichiru
 * Portions created by Ichiru are Copyright (C) 2014 Ichiru. All Rights Reserved.
 */

using System;
using System.IO;
using System.Xml;
using FLParser.Dll;

namespace FLParser
{
    public class DllFile
    {
		IDllProvider provider;

        public DllFile(string path)
        {
            if (path == null) 
				throw new ArgumentNullException("path");
			if (!File.Exists (path))
				throw new FileNotFoundException ("path");
			if (Platform.RunningOS == OS.Windows)
				provider = new Win32DllProvider (path);
			else
				provider = new ManagedDllProvider (path);
        }

		[CLSCompliant(false)]
		public string GetString(ushort resourceId)
		{
			return provider.GetString (resourceId);
		}

		[CLSCompliant(false)]
		public XmlDocument GetXml(ushort resourceId)
		{
			return provider.GetXml(resourceId);
		}
    }
}