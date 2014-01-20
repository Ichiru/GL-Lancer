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
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System;

using Microsoft.Xna.Framework;

using FLParser.Ini;

namespace FLApi.Universe
{
    public class Asteroid
    {
        public string Name { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Size { get; private set; }
        public string Info { get; private set; }

        public Asteroid(Entry e)
        {
            Name = e[0].ToString();
            Rotation =  new Vector3(e[1].ToSingle(), e[2].ToSingle(), e[3].ToSingle());
            Size = new Vector3(e[4].ToSingle(), e[5].ToSingle(), e[6].ToSingle());
            Info = e.Count == 8 ? e[7].ToString() : string.Empty;
        }
    }
}