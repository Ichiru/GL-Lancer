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
using System.Linq;

using Microsoft.Xna.Framework;

using FLParser.Ini;

namespace FLApi.Universe
{
    public class UniverseIni : IniFile
    {
        public int? SecondsPerDay { get; private set; }

        public List<Base> Bases { get; private set; }
        public List<StarSystem> Systems { get; private set; }

        public UniverseIni(string path, FreelancerIni freelancerIni)
        {
            Bases = new List<Base>();
            Systems = new List<StarSystem>();

            foreach (Section s in ParseFile(path))
            {
                switch (s.Name.ToLowerInvariant())
                {
                    case "time":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "seconds_per_day":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (SecondsPerDay != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    SecondsPerDay = e[0].ToInt32();
                                    break;
                                default: throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "base":
                        Bases.Add(new Base(s, freelancerIni));
                        break;
                    case "system":
                        Systems.Add(new StarSystem(this, s, freelancerIni));
                        break;
                    default: throw new Exception("Invalid Section in " + path + ": " + s.Name);
                }
            }
        }

        public Base FindBase(string nickname)
        {
            IEnumerable<Base> result = from Base b in Bases where b.Nickname == nickname select b;
            if (result.Count<Base>() == 1) return result.First<Base>();
            else return null;
        }

        public StarSystem FindSystem(string nickname)
        {
            IEnumerable<StarSystem> result = from StarSystem s in Systems where s.Nickname.ToLowerInvariant() == nickname.ToLowerInvariant() select s;
            if (result.Count<StarSystem>() == 1) return result.First<StarSystem>();
            else return null;
        }

        public StarSystem FindSystem(Vector2 pos)
        {
            IEnumerable<StarSystem> result = from StarSystem s in Systems where s.Pos == pos select s;
            if (result.Count<StarSystem>() == 1) return result.First<StarSystem>();
            else return null;
        }
    }
}
