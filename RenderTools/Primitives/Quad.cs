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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;

using OpenTK;
using FLCommon;

namespace RenderTools.Primitives
{
    public class Quad : IDisposable
    {
        public const int VERTEX_COUNT = 4;
        public static short[] indices = new short[] { 0, 2, 1, 1, 2, 3 };

        public static int PrimitiveCount { get; private set; }
        public static IndexBuffer IndexBuffer { get; private set; }

        public VertexBuffer VertexBuffer { get; private set; }

        static Quad()
        {
            PrimitiveCount = indices.Length / 3;
        }

        public Quad(GraphicsDevice graphicsDevice)
        {
            if (IndexBuffer == null)
            {
                IndexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
                IndexBuffer.SetData(indices);
            }

            VertexPositionTexture[] vertices = new VertexPositionTexture[VERTEX_COUNT];

            vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 0));
            vertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 0));

            VertexBuffer = new VertexBuffer(graphicsDevice, VertexPositionTexture.VertexDeclaration, VERTEX_COUNT, BufferUsage.WriteOnly);
            VertexBuffer.SetData<VertexPositionTexture>(vertices);
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
        }
    }
}