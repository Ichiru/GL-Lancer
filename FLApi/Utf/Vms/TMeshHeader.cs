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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using OpenTK;
using FLCommon;

using FLApi.Utf.Mat;
using FLApi.Utf.Mat.Materials;
using FLApi.Universe;

namespace FLApi.Utf.Vms
{
    /// <summary>
    /// Repeated no_meshes times in segment - 12 bytes
    /// </summary>
    public class TMeshHeader
    {
        private ILibFile materialLibrary;
        private static NullMaterial nullMaterial;

        /// <summary>
        /// CRC of texture name for mesh
        /// </summary>
        private uint MaterialId;
        private Material material;
        public Material Material
        {
            get
            {
                if (material == null) material = materialLibrary.FindMaterial(MaterialId);
                return material;
            }
        }

        public ushort StartVertex { get; private set; }
        public ushort EndVertex { get; private set; }
        public ushort NumRefVertices { get; private set; }
        public ushort Padding { get; private set; } //0x00CC

        public int TriangleStart { get; private set; }

        private int numVertices;
        private int primitiveCount;

        public TMeshHeader(BinaryReader reader, int triangleStartOffset, ILibFile materialLibrary)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (materialLibrary == null) throw new ArgumentNullException("materialLibrary");

            this.materialLibrary = materialLibrary;

            MaterialId = reader.ReadUInt32();
            StartVertex = reader.ReadUInt16();
            EndVertex = reader.ReadUInt16();
            NumRefVertices = reader.ReadUInt16();
            Padding = reader.ReadUInt16();

            TriangleStart = triangleStartOffset;

            numVertices = EndVertex - StartVertex + 1;
            primitiveCount = NumRefVertices / 3;
        }

        public void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            if (nullMaterial == null || nullMaterial.IsDisposed)
            {
                nullMaterial = new NullMaterial();
                nullMaterial.Initialize(device, content, camera);
            }

            if (Material != null) Material.Initialize(device, content, camera);
        }

        public void DeviceReset()
        {
            if (Material == null) nullMaterial.Resized();
            else Material.Resized();
        }

        public void Update()
        {
            if (Material == null) nullMaterial.Update();
            else Material.Update();
        }

        public void Draw(D3DFVF vertexFormat, ushort startVertex, Color ambient, List<LightSource> lights, Matrix4 world)
        {
            if (Material == null) nullMaterial.Draw(vertexFormat, PrimitiveTypes.TriangleList, startVertex + StartVertex, numVertices, TriangleStart, primitiveCount, ambient, lights, world);
            else Material.Draw(vertexFormat, PrimitiveTypes.TriangleList, startVertex + StartVertex, numVertices, TriangleStart, primitiveCount, ambient, lights, world);
        }
    }
}
