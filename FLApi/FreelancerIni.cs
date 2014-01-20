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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;

using Microsoft.Win32;

using FLParser;
using FLParser.Ini;

using FLApi.Solar;
using FLApi.Universe;
using FLApi.Equipment;
using FLApi.Characters;

namespace FLApi
{
    public class FreelancerIni : IniFile
    {
        public static string RegistryPath = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Microsoft Games\\Freelancer\\1.0", "AppPath", null) as string;

        public string ExePath { get; private set; }
        public string DataPath { get; private set; }

        public List<DllFile> Resources { get; private set; }

        public SolararchIni Solar { get; private set; }
        public UniverseIni Universe { get; private set; }
        public EquipmentIni Equipment { get; private set; }
        public LoadoutsIni Loadouts { get; private set; }
        public StararchIni Stars { get; private set; }
        public BodypartsIni Bodyparts { get; private set; }
        public CostumesIni Costumes { get; private set; }

        public FreelancerIni(string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            Loadouts = new LoadoutsIni();
            Equipment = new EquipmentIni();

            ExePath = path + "\\EXE\\";

            foreach (Section s in parseFile(ExePath + "freelancer.ini"))
            {
                switch (s.Name.ToLowerInvariant())
                {
                    case "freelancer":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "data path":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (DataPath != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    DataPath = ExePath + e[0].ToString() + "\\";
                                    break;
                            }
                        }
                        break;
                    case "resources":
                        Resources = new List<DllFile>();
                        foreach (Entry e in s)
                        {
                            Resources.Add(new DllFile(ExePath + e[0]));
                        }
                        break;
                    case "data":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "solar":
                                    Solar = new SolararchIni(DataPath + e[0], this);
                                    break;
                                case "universe":
                                    Universe = new UniverseIni(DataPath + e[0], this);
                                    break;
                                case "equipment":
                                    Equipment.AddEquipmentIni(DataPath + e[0], this);
                                    break;
                                case "loadouts":
                                    Loadouts.AddLoadoutsIni(DataPath + e[0], this);
                                    break;
                                case "stars":
                                    Stars = new StararchIni(DataPath + e[0]);
                                    break;
                                case "bodyparts":
                                    Bodyparts = new BodypartsIni(DataPath + e[0], this);
                                    break;
                                case "costumes":
                                    Costumes = new CostumesIni(DataPath + e[0], this);
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        public string GetStringResource(int id)
        {
            int fileId = (id >> 16) - 1;
            if (fileId >= 0 && fileId < Resources.Count)
            {
                ushort resId = (ushort)id;
                return Resources[fileId].GetString(resId);
            }
            else return string.Empty;
        }

        public XmlDocument GetXmlResource(int id)
        {
            int fileId = (id >> 16) - 1;
            if (fileId >= 0 && fileId < Resources.Count)
            {
                ushort resId = (ushort)id;
                XmlDocument result = Resources[fileId].GetXml(resId);
                /*if (result == null)
                {
                    result = new XmlDocument();
                    result.Load(Resources[fileId].GetString(resId));
                }*/
                return result;
            }
            else return null;
        }
    }
}