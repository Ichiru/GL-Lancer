/* The contents of this file a
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
using System.Linq;
using System.Text;

using FLCommon;
using OpenTK;

using FLParser;
using FLParser.Utf;
using FLApi.Universe;
using FLApi.Utf.Mat;
using FLApi.Utf.Vms;

namespace FLApi.Utf.Dfm
{
    public class FaceGroup
    {
        private GraphicsDevice device;
        private ILibFile materialLibrary;

        private string materialName;
        private Material material;
        public Material Material
        {
            get
            {
                if (material == null)
                {
                    material = materialLibrary.FindMaterial(CrcTool.FLModelCrc(materialName));
                }

                return material;
            }
        }

        public ushort[] TriangleStripIndices { get; private set; }
        public ushort[] EdgeIndices { get; private set; }
        public float[] EdgeAngles { get; private set; }

        private IndexBuffer triangleStripIndexBuffer;
        private bool ready = false;

        public FaceGroup(IntermediateNode root, ILibFile materialLibrary)
        {
            this.materialLibrary = materialLibrary;

            foreach (LeafNode node in root)
            {
                switch (node.Name.ToLowerInvariant())
                {
                    case "material_name": materialName = node.StringData;
                        break;
                    case "tristrip_indices": TriangleStripIndices = node.UInt16ArrayData;
                        break;
                    case "edge_indices": EdgeIndices = node.UInt16ArrayData;
                        break;
                    case "edge_angles": EdgeAngles = node.SingleArrayData;
                        break;
                    default: throw new Exception("Invalid node in " + root.Name + ": " + node.Name);
                }
            }
        }

        public void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            this.device = device;

            triangleStripIndexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, TriangleStripIndices.Length, BufferUsage.WriteOnly);
            triangleStripIndexBuffer.SetData(TriangleStripIndices);

            Material.Initialize(device, content, camera);

            ready = true;
        }

        public void Resized()
        {
            if (ready) Material.Resized();
        }

        public void Update()
        {
            if (ready) Material.Update();
        }

        public void Draw(int vertexCount, Color ambient, List<LightSource> lights, Matrix world)
        {
            if (ready)
            {
                device.Indices = triangleStripIndexBuffer;
                Material.Draw(D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX1, PrimitiveTypes.TriangleStrip, 0, vertexCount, 0, TriangleStripIndices.Length - 2, ambient, lights, world);
            }
        }
    }
}
