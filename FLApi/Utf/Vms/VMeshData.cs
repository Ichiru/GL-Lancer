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

using FLParser;

using FLApi.Utf.Mat;
using FLApi.Utf.Vms.Vertices;
using FLApi.Universe;

namespace FLApi.Utf.Vms
{
    public class VMeshData
    {
        private GraphicsDevice device;
        private bool ready;

        // Data header - 16 bytes long
        public uint MeshType { get; private set; } //0x00000001
        public uint SurfaceType { get; private set; } //0x00000004
        public ushort MeshCount { get; private set; }
        public ushort IndexCount { get; private set; }
        public D3DFVF FlexibleVertexFormat { get; private set; } //0x0112
        public ushort VertexCount { get; private set; }

        /// <summary>
        /// A list of meshes in the mesh data
        /// </summary>
        public List<TMeshHeader> Meshes { get; private set; }

        private ushort[] indices;

        /// <summary>
        /// A list of triangles in the mesh data
        /// </summary>
        public IndexBuffer IndexBuffer { get; private set; }

        public VertexPosition[] verticesVertexPosition { get; private set; }
        public VertexPositionNormal[] verticesVertexPositionNormal { get; private set; }
        public VertexPositionTexture[] verticesVertexPositionTexture { get; private set; }
        public VertexPositionNormalTexture[] verticesVertexPositionNormalTexture { get; private set; }
        public VertexPositionColorTexture[] verticesVertexPositionColorTexture { get; private set; }
        public VertexPositionNormalDiffuseTexture[] verticesVertexPositionNormalDiffuseTexture { get; private set; }
        public VertexPositionNormalTextureTwo[] verticesVertexPositionNormalTextureTwo { get; private set; }
        public VertexPositionNormalDiffuseTextureTwo[] verticesVertexPositionNormalDiffuseTextureTwo { get; private set; }

        /// <summary>
        /// A list of Vertices in the mesh data
        /// </summary>
        public VertexBuffer VertexBuffer { get; private set; }

        public VMeshData(byte[] data, ILibFile materialLibrary)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (materialLibrary == null) throw new ArgumentNullException("materialLibrary");

            ready = false;

            using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
            {

                // Read the data header.
                MeshType = reader.ReadUInt32();
                SurfaceType = reader.ReadUInt32();
                MeshCount = reader.ReadUInt16();
                IndexCount = reader.ReadUInt16();
                FlexibleVertexFormat = (D3DFVF)reader.ReadUInt16();
                VertexCount = reader.ReadUInt16();

                // Read the mesh headers.
                Meshes = new List<TMeshHeader>();
                int triangleStartOffset = 0;
                for (int count = 0; count < MeshCount; count++)
                {
                    TMeshHeader item = new TMeshHeader(reader, triangleStartOffset, materialLibrary);
                    triangleStartOffset += item.NumRefVertices;
                    Meshes.Add(item);
                }

                // Read the triangle data
                indices = new ushort[IndexCount];
                for (int i = 0; i < IndexCount; i++) indices[i] = reader.ReadUInt16();

                // Read the vertex data.
                // The FVF defines what fields are included for each vertex.
                switch (FlexibleVertexFormat)
                {
                    case D3DFVF.XYZ: //(D3DFVF)0x0002:
                        verticesVertexPosition = new VertexPosition[VertexCount];
                        for (int i = 0; i < VertexCount; i++) verticesVertexPosition[i] = new VertexPosition(reader);
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL: //(D3DFVF)0x0012:
                        verticesVertexPositionNormal = new VertexPositionNormal[VertexCount];
                        for (int i = 0; i < VertexCount; i++) verticesVertexPositionNormal[i] = new VertexPositionNormal(reader);
                        break;
                    case D3DFVF.XYZ | D3DFVF.TEX1: //(D3DFVF)0x0102:
                        verticesVertexPositionTexture = new VertexPositionTexture[VertexCount];
                        for (int i = 0; i < VertexCount; i++)
                        {
                            Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
						Vector2 textureCoordinate = new Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());

                            verticesVertexPositionTexture[i] = new VertexPositionTexture(position, textureCoordinate);
                        }
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX1: //(D3DFVF)0x0112:
                        verticesVertexPositionNormalTexture = new VertexPositionNormalTexture[VertexCount];
                        for (int i = 0; i < VertexCount; i++)
                        {
                            Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                            Vector3 normal = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
						Vector2 textureCoordinate = new Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());

                            verticesVertexPositionNormalTexture[i] = new VertexPositionNormalTexture(position, normal, textureCoordinate);
                        }
                        break;
                    case D3DFVF.XYZ | D3DFVF.DIFFUSE | D3DFVF.TEX1: //(D3DFVF)0x0142:
                        verticesVertexPositionColorTexture = new VertexPositionColorTexture[VertexCount];
                        for (int i = 0; i < VertexCount; i++)
                        {
                            Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                            int g = reader.ReadByte();
                            int b = reader.ReadByte();
                            int r = reader.ReadByte();
                            int a = reader.ReadByte();
                            Color diffuse = new Color(r, g, b, a);

						Vector2 textureCoordinate = new Vector2(reader.ReadSingle(), 1 - reader.ReadSingle());

                            verticesVertexPositionColorTexture[i] = new VertexPositionColorTexture(position, diffuse, textureCoordinate);
                        }
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.DIFFUSE | D3DFVF.TEX1: //(D3DFVF)0x0152:
                        verticesVertexPositionNormalDiffuseTexture = new VertexPositionNormalDiffuseTexture[VertexCount];
                        for (int i = 0; i < VertexCount; i++) verticesVertexPositionNormalDiffuseTexture[i] = new VertexPositionNormalDiffuseTexture(reader);
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX2: //(D3DFVF)0x0212:
                        verticesVertexPositionNormalTextureTwo = new VertexPositionNormalTextureTwo[VertexCount];
                        for (int i = 0; i < VertexCount; i++) verticesVertexPositionNormalTextureTwo[i] = new VertexPositionNormalTextureTwo(reader);
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.DIFFUSE | D3DFVF.TEX2: //(D3DFVF)0x0252:
                        verticesVertexPositionNormalDiffuseTextureTwo = new VertexPositionNormalDiffuseTextureTwo[VertexCount];
                        for (int i = 0; i < VertexCount; i++) verticesVertexPositionNormalDiffuseTextureTwo[i] = new VertexPositionNormalDiffuseTextureTwo(reader);
                        break;
                    /*case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX4: //(D3DFVF)0x0412:
                        for (int i = 0; i < VertexCount; i++) vertices[i] = new VertexPositionNormalTextureTangentBinormal(reader);
                        break;*/
                    default:
                        throw new FileContentException("UTF:VMeshData", "FVF 0x" + FlexibleVertexFormat + " not supported.");
                }
            }
        }

        public void Initialize(ushort startMesh, int endMesh, GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            this.device = device;

            IndexBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, IndexCount, BufferUsage.WriteOnly);
            IndexBuffer.SetData(indices);

            switch (FlexibleVertexFormat)
            {
                case D3DFVF.XYZ: //(D3DFVF)0x0002:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPosition), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPosition>(verticesVertexPosition);
                    break;
                case D3DFVF.XYZ | D3DFVF.NORMAL: //(D3DFVF)0x0012:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormal), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionNormal>(verticesVertexPositionNormal);
                    break;
                case D3DFVF.XYZ | D3DFVF.TEX1: //(D3DFVF)0x0102:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionTexture), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionTexture>(verticesVertexPositionTexture);
                    break;
                case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX1: //(D3DFVF)0x0112:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTexture), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionNormalTexture>(verticesVertexPositionNormalTexture);
                    break;
                case D3DFVF.XYZ | D3DFVF.DIFFUSE | D3DFVF.TEX1: //(D3DFVF)0x0142:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionColorTexture), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionColorTexture>(verticesVertexPositionColorTexture);
                    break;
                case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.DIFFUSE | D3DFVF.TEX1: //(D3DFVF)0x0152:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalDiffuseTexture), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionNormalDiffuseTexture>(verticesVertexPositionNormalDiffuseTexture);
                    break;
                case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX2: //(D3DFVF)0x0212:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalTextureTwo), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionNormalTextureTwo>(verticesVertexPositionNormalTextureTwo);
                    break;
                case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.DIFFUSE | D3DFVF.TEX2: //(D3DFVF)0x0252:
                    VertexBuffer = new VertexBuffer(device, typeof(VertexPositionNormalDiffuseTextureTwo), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionNormalDiffuseTextureTwo>(verticesVertexPositionNormalDiffuseTextureTwo);
                    break;
                /*case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX4: //(D3DFVF)0x0412:
                    VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTextureTangentBinormal), VertexCount, BufferUsage.WriteOnly);
                    VertexBuffer.SetData<VertexPositionNormalTextureTangentBinormal>(verticesVertexPositionNormalTextureTangentBinormal);
                    break;*/
            }

            for (ushort i = startMesh; i < endMesh; i++)
            {
                Meshes[i].Initialize(device, content, camera);
            }

            ready = true;
        }

        public void DeviceReset(ushort startMesh, int endMesh)
        {
            if (ready)
            {
                for (ushort i = startMesh; i < endMesh; i++)
                {
                    Meshes[i].DeviceReset();
                }
            }
        }

        public void Update(ushort startMesh, int endMesh)
        {
            if (ready)
            {
                for (ushort i = startMesh; i < endMesh; i++)
                {
                    Meshes[i].Update();
                }
            }
        }

        public void Draw(ushort startMesh, int endMesh, ushort startVertex, Color ambient, List<LightSource> lights, Matrix world)
        {
            if (ready)
            {
                device.SetVertexBuffer(VertexBuffer);
                device.Indices = IndexBuffer;

                for (ushort i = startMesh; i < endMesh; i++)
                {
                    Meshes[i].Draw(FlexibleVertexFormat, startVertex, ambient, lights, world);
                }
            }
        }

        public override string ToString()
        {
            return FlexibleVertexFormat.ToString();
        }
    }
}