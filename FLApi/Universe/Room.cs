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

using FLParser.Ini;

namespace FLApi.Universe
{
    public class Room : DataIniFile
    {
        public string Nickname { get; private set; }

        public Room(Section section, FreelancerIni data)
            : base(data)
        {
            if (section == null) throw new ArgumentNullException("section");
            string file = null;

            foreach (Entry e in section)
            {
                switch (e.Name.ToLowerInvariant())
                {
                    case "nickname":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Nickname != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Nickname = e[0].ToString();
                        break;
                    case "file":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (file != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        file = e[0].ToString();
                        break;
                    default: throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                }
            }

            foreach (Section s in ParseFile(FreelancerIni.DataPath + file))
            {
                switch (s.Name.ToLowerInvariant())
                {
                    case "room_info":
                        // TODO Room room_info
                        break;
                    case "room_sound":
                        // TODO Room room_sound
                        break;
                    case "camera":
                        // TODO Room camera
                        break;
                    case "spiels":
                        // TODO Room spiels
                        break;
                    case "playershipplacement":
                        // TODO Room playershipplacement
                        break;
                    case "characterplacement":
                        // TODO Room characterplacement
                        break;
                    case "forsaleshipplacement":
                        // TODO Room forsaleshipplacement
                        break;
                    case "hotspot":
                        // TODO Room hotspot
                        break;
                    case "flashlightset":
                        // TODO Room flashlightset
                        break;
                    case "flashlightline":
                        // TODO Room flashlightline
                        break;
                    default: throw new Exception("Invalid Section in " + file + ": " + s.Name);
                }
            }
        }
    }
}
