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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;

using FLParser.Ini;

namespace FLApi.Universe
{
    public abstract class ZoneReference : DataIniFile
    {
        protected StarSystem parent;
        protected string file;

        private string zoneName;
        private Zone zone;
        public Zone Zone
        {
            get
            {
                if (zone == null) zone = parent.FindZone(zoneName);
                return zone;
            }
        }

        public TexturePanels TexturePanels { get; protected set; }
        public List<string> Properties { get; private set; }
        public List<ExclusionZone> ExclusionZones { get; private set; }

        protected ZoneReference(StarSystem parent, Section section, FreelancerIni data)
            : base(data)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (section == null) throw new ArgumentNullException("section");

            this.parent = parent;

            foreach (Entry e in section)
            {
                switch (e.Name.ToLowerInvariant())
                {
                    case "file":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (file != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        file = e[0].ToString();
                        break;
                    case "zone":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (zoneName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        zoneName = e[0].ToString();
                        break;
                    default:
                        throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                }
            }

            Properties = new List<string>();
            ExclusionZones = new List<ExclusionZone>();
        }
    }
}
