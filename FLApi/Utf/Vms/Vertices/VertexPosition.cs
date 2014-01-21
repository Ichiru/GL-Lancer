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
 * Data structure from Freelancer UTF Editor by Cannon & Adoxa, continuing the work of Colin Sanby and Mario 'HCl' Brito (http://the-starport.net)
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FLApi.Utf.Vms.Vertices
{
    public struct VertexPosition : IVertexType
    {
        public Vector3 Position { get; private set; }

        public VertexPosition(BinaryReader reader)
            : this()
        {
            this.Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        public VertexPosition(Vector3 position)
            : this()
        {
            this.Position = position;
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0)); }
        }
    }
}