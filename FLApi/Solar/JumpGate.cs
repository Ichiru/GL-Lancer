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
using FLParser.Utf;

namespace FLApi.Solar
{
    public class JumpGate : Archetype
    {
        public JumpGate(Section section, FreelancerIni data)
            : base(section, data)
        {
            foreach (Entry e in section)
            {
                if (!parentEntry(e))
                {
                    /*switch (e.Name.ToLowerInvariant())
                    {
                        case "":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (x != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            x = e[0].ToString();
                            break;
                        default:
                            throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                    }*/
                }
            }
        }
    }
}