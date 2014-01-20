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
 */

using System;
using System.Collections.Generic;
using System.Linq;

using FLParser.Ini;

namespace FLApi.Solar
{
    public class Star
    {
        private StararchIni parent;

        public string Nickname { get; set; } // = med_yellow_sun
        public string StarGlow { get; set; } // = yellow_starglow
        public string StarCenter { get; set; } // = yellow_starcenter
        public string LensFlare { get; set; } // = hex_rainbow_lens_flare
        public string LensGlow { get; set; } //  = default_lens_glow
        public string Spines { get; set; } //  = yellow6_spines
        public int? IntensityFadeIn { get; set; } // = 0
        public int? IntensityFadeOut { get; set; } // = 0
        public float? ZoneOcclusionFadeIn { get; set; } // = 1
        public float? ZoneOcclusionFadeOut { get; set; } // = 1
        public float? Radius { get; set; } // = 3000

        public Star(StararchIni parent, Section section)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            if (section == null) throw new ArgumentNullException("section");

            this.parent = parent;

            foreach (Entry e in section)
            {
                switch (e.Name.ToLowerInvariant())
                {
                    case "nickname":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Nickname != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Nickname = e[0].ToString();
                        break;
                    case "star_glow":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (StarGlow != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        StarGlow = e[0].ToString();
                        break;
                    case "star_center":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (StarCenter != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        StarCenter = e[0].ToString();
                        break;
                    case "lens_flare":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (LensFlare != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        LensFlare = e[0].ToString();
                        break;
                    case "lens_glow":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (LensGlow != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        LensGlow = e[0].ToString();
                        break;
                    case "spines":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Spines != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Spines = e[0].ToString();
                        break;
                    case "intensity_fade_in":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (IntensityFadeIn != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        IntensityFadeIn = e[0].ToInt32();
                        break;
                    case "intensity_fade_out":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (IntensityFadeOut != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        IntensityFadeOut = e[0].ToInt32();
                        break;
                    case "zone_occlusion_fade_in":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (ZoneOcclusionFadeIn != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        ZoneOcclusionFadeIn = e[0].ToSingle();
                        break;
                    case "zone_occlusion_fade_out":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (ZoneOcclusionFadeOut != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        ZoneOcclusionFadeOut = e[0].ToSingle();
                        break;
                    case "radius":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Radius != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Radius = e[0].ToSingle();
                        break;
                    default:
                        throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                }
            }
        }
    }
}