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
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RenderTools.Primitives
{
    public class Cuboid : IDisposable
    {
        public const int VERTEX_COUNT = 8;
        public static short[] indices = new short[] { 0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7 };

        public static int PrimitiveCount { get; private set; }
        public static IndexBuffer IndexBuffer { get; private set; }

        public VertexBuffer VertexBuffer { get; private set; }

        static Cuboid()
        {
            PrimitiveCount = indices.Length / 2;
        }

        public Cuboid(GraphicsDevice graphicsDevice, Vector3[] corners)
        {
            if (corners.Length != VERTEX_COUNT) throw new ArgumentOutOfRangeException("corners");

            setUpIndexBuffer(graphicsDevice);
            setUpBoxFromCorners(graphicsDevice, corners);
        }

        public Cuboid(GraphicsDevice graphicsDevice, Vector3 size)
        {
            setUpIndexBuffer(graphicsDevice);

            BoundingBox b = new BoundingBox(-(size / 2), size / 2);
            Vector3[] corners = b.GetCorners();

            setUpBoxFromCorners(graphicsDevice, corners);
        }

        private void setUpIndexBuffer(GraphicsDevice graphicsDevice)
        {
            if (IndexBuffer == null)
            {
                IndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
                IndexBuffer.SetData<short>(indices);
            }
        }

        private void setUpBoxFromCorners(GraphicsDevice graphicsDevice, Vector3[] corners)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[VERTEX_COUNT];
            for (int i = 0; i < VERTEX_COUNT; i++)
                vertices[i] = new VertexPositionColor(corners[i], Color.White);

            VertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            VertexBuffer.SetData<VertexPositionColor>(vertices);
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
        }
    }
}