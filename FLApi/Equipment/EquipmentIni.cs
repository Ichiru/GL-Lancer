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

using FLParser.Ini;

namespace FLApi.Equipment
{
    public class EquipmentIni : IniFile
    {
        public List<AbstractEquipment> Equip { get; private set; }

        public EquipmentIni()
        {
            Equip = new List<AbstractEquipment>();
        }

        public void AddEquipmentIni(string path, FreelancerIni freelancerIni)
        {
            foreach (Section s in parseFile(path))
            {
                switch (s.Name.ToLowerInvariant())
                {
                    case "light":
                        Equip.Add(new Light(s, freelancerIni));
                        break;
                    case "power":
                        break;
                    case "scanner":
                        break;
                    case "tractor":
                        break;
                    case "lootcrate":
                        break;
                    case "repairkit":
                        break;
                    case "countermeasure":
                        break;
                    case "countermeasuredropper":
                        break;
                    case "shieldbattery":
                        break;
                    case "armor":
                        break;
                    case "cargopod":
                        break;
                    case "commodity":
                        break;
                    case "tradelane":
                        break;
                    case "internalfx":
                        break;
                    case "attachedfx":
                        break;
                    case "shieldgenerator":
                        break;
                    case "shield":
                        break;
                    case "engine":
                        break;
                    case "thruster":
                        break;
                    case "cloakingdevice":
                        break;
                    case "motor":
                        break;
                    case "explosion":
                        break;
                    case "munition":
                        break;
                    case "gun":
                        break;
                    case "mine":
                        break;
                    case "minedropper":
                        break;
                    case "lod":
                        break;
                    default: throw new Exception("Invalid Section in " + path + ": " + s.Name);
                }
            }
        }

        public AbstractEquipment FindEquipment(string nickname)
        {
            IEnumerable<AbstractEquipment> candidates = from AbstractEquipment s in Equip where s.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase) select s;
            int count = candidates.Count<AbstractEquipment>();
            if (count == 1) return candidates.First<AbstractEquipment>();
            else if (count == 0) return null;
            else throw new Exception(count + " AbstractEquipments with nickname " + nickname);
        }
    }
}
