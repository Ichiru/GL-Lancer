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
 * The Original Code is RenderTools code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2012
 * the Initial Developer. All Rights Reserved.
 */

namespace RenderTools.Parser
{
    public class ObjFace
    {
        private ObjVertex[] vertices;

        public ObjFace(ObjVertex[] vertices)
        {
            this.vertices = vertices;
        }

        public override string ToString()
        {
            string result = "f ";

            for (int i = 0; i < vertices.Length; i++)
            {
                result += vertices[i].ToString();
                if (i < vertices.Length - 1) result += " ";
            }

            return result;
        }
    }
}
