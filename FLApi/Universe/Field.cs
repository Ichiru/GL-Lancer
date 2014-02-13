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

using FLCommon;

using FLParser.Ini;

namespace FLApi.Universe
{
    public class Field
    {
        public int? CubeSize { get; private set; }
        public int? FillDist { get; private set; }
        public Color? TintField { get; private set; }
        public float? MaxAlpha { get; private set; }
        public Color? DiffuseColor { get; private set; }
        public Color? AmbientColor { get; private set; }
        public Color? AmbientIncrease { get; private set; }
        public float? EmptyCubeFrequency { get; private set; }
        public bool? ContainsFogZone { get; private set; }

        public Field(Section section)
        {
            if (section == null) throw new ArgumentNullException("section");

            foreach (Entry e in section)
            {
                switch (e.Name.ToLowerInvariant())
                {
                    case "cube_size":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (CubeSize != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        CubeSize = e[0].ToInt32();
                        break;
                    case "fill_dist":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (FillDist != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        FillDist = e[0].ToInt32();
                        break;
                    case "tint_field":
                        if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (TintField != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        TintField = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                        break;
                    case "max_alpha":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (MaxAlpha != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            MaxAlpha = e[0].ToSingle();
                        break;
                    case "diffuse_color":
                        if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (DiffuseColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        DiffuseColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                        break;
                    case "ambient_color":
                        if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (AmbientColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        AmbientColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                        break;
                    case "ambient_increase":
                        if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (AmbientIncrease != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        AmbientIncrease = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                        break;
                    case "empty_cube_frequency":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (EmptyCubeFrequency != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        EmptyCubeFrequency = e[0].ToSingle();
                        break;
                    case "contains_fog_zone":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (ContainsFogZone != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        //ContainsFogZone = bool.Parse(e[0].ToString());
                        ContainsFogZone = e[0].ToBoolean();
                        break;
                    default:
                        throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                }
            }
        }
    }
}