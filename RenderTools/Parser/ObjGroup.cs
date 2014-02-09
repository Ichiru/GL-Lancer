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
 * The Original Code is RenderTools code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using FLCommon;

namespace RenderTools.Parser
{
    public class ObjGroup
    {
        private string name;
        public ObjMaterial Material { get; private set; }
        private ObjFace[] faces;

        public ObjGroup(string name, ObjMaterial material, ObjFace[] faces)
        {
            this.name = name;
            Material = material;
            this.faces = faces;
        }

        public override string ToString()
        {
            string result = 
                "g " + name + Environment.NewLine + 
                "usemtl " + Material.Name + Environment.NewLine;
            for (int i = 0; i < faces.Length; i++)
            {
                result += 
                    faces[i].ToString() + Environment.NewLine;
            }
            result +=
                "# " + faces.Length + " triangles" + Environment.NewLine;
            return result;
        }
    }
}
