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

namespace FLApi.Universe
{
    public abstract class UniverseElement : DataIniFile
    {
        public string Nickname { get; protected set; }
        public string StridName { get; protected set; }
        public string Name { get; protected set; }

        public UniverseElement(FreelancerIni data) : base(data) { }

        public override string ToString()
        {
            return Nickname;
        }
    }
}
