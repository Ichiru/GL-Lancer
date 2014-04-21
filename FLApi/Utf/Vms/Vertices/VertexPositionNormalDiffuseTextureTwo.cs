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
 * Data structure from Freelancer UTF Editor by Cannon & Adoxa, continuing the work of Colin Sanby and Mario 'HCl' Brito (http://the-starport.net)
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.IO;

using OpenTK;
using FLCommon;

namespace FLApi.Utf.Vms.Vertices
{
    public struct VertexPositionNormalDiffuseTextureTwo : IVertexType
    {
        public Vector3 Position { get; private set; }
        public Vector3 Normal { get; private set; }
        public uint Diffuse { get; private set; }
        public Vector2 TextureCoordinate { get; private set; }
        public Vector2 TextureCoordinateTwo { get; private set; }

        public VertexPositionNormalDiffuseTextureTwo(BinaryReader reader)
            : this()
        {
            this.Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            this.Normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            this.Diffuse = reader.ReadUInt32();
			this.TextureCoordinate = new Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());
			this.TextureCoordinateTwo = new Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return new VertexDeclaration(
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(sizeof(float) * (3 + 3), VertexElementFormat.Byte4, VertexElementUsage.BlendWeight, 0),
                    new VertexElement(sizeof(float) * (3 + 3) + sizeof(uint), VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                    new VertexElement(sizeof(float) * (3 + 3 + 2) + sizeof(uint), VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1));
            }
        }
    }
}