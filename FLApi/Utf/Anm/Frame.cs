﻿/* The contents of this file a
 * re subject to the Mozilla Public License
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
 * Data structure from Freelancer UTF Editor by Cannon & Adoxa, continuing the work of Colin Sanby and Mario 'HCl' Brito (http://the-starport.net)
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace FLApi.Utf.Anm
{
    public class Frame
    {
        public float? Time { get; private set; }
        //public byte[] Stuff { get; private set; }
        public float Distance { get; private set; }

        public Frame(BinaryReader reader, bool time, int channelType)
        {
            if (time) Time = reader.ReadSingle();

            /*switch (channelType)
            {
                case 1:
                    Stuff = new byte[4];
                    break;
                case 4:
                    Stuff = new byte[16];
                    break;
                case 34:
                    Stuff = new byte[12];
                    break;
                case 64:
                    Stuff = new byte[6];
                    break;
                case 66:
                    Stuff = new byte[18];
                    break;
                case 80:
                    Stuff = new byte[6];
                    break;
                case 128:
                    Stuff = new byte[6];
                    break;
                case 130:
                    Stuff = new byte[18];
                    break;
                case 144:
                    Stuff = new byte[6];
                    break;
                default: throw new Exception("Invalid channel type: " + channelType);
            }

            for (int i = 0; i < Stuff.Length; i++) Stuff[i] = reader.ReadByte();*/
            Distance = reader.ReadSingle();
        }

        public override string ToString()
        {
            string result = string.Empty;

            //for (int i = 0; i < Stuff.Length; i++) result += Stuff[i] + " ";
            result = (Time == null ? "loop" : Time.ToString()) + " - " + Distance.ToString();

            return result;
        }
    }
}
