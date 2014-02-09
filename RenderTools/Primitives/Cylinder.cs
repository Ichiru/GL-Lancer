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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;

using OpenTK;
using FLCommon;

namespace RenderTools.Primitives
{
    public class Cylinder : IDisposable
    {
        public VertexBuffer VertexBuffer { get; private set; }
        public IndexBuffer IndexBuffer { get; private set; }

        public Cylinder(GraphicsDevice graphicsDevice, Vector3 Size, int slices)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[(slices + 1) * 2];
            ushort[] indices = new ushort[slices * 6];

            float deltaPhi = MathHelper.TwoPi / slices;
            ushort vertexIndex = 0, index = 0;

            for (int slice = 0; slice <= slices; slice++)
            {
                float phi = slice * deltaPhi;

                float x = -Size.X * (float)Math.Sin(phi);
                float y = -(Size.Y / 2);
                float z = -Size.X * (float)Math.Cos(phi);

                Vector3 pos = new Vector3(x, y, z);
                vertices[vertexIndex++] = new VertexPositionColor(pos, Color.White);

                pos.Y = (Size.Y / 2);
                vertices[vertexIndex++] = new VertexPositionColor(pos, Color.White);

                if (slice < slices)
                {
                    indices[index++] = (ushort)(slice * 2);
                    indices[index++] = (ushort)(slice * 2 + 1);

                    indices[index++] = (ushort)(slice * 2);
                    indices[index++] = (ushort)(slice * 2 + 2);

                    indices[index++] = (ushort)(slice * 2 + 1);
                    indices[index++] = (ushort)(slice * 2 + 3);
                }
            }

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData<VertexPositionColor>(vertices);

            IndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            IndexBuffer.SetData<ushort>(indices);
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
        }
    }
}
