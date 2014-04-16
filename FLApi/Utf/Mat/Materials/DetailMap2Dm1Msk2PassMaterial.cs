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
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System.Collections.Generic;

using OpenTK;
using FLCommon;

using FLParser.Utf;

using FLApi.Utf.Vms;
using FLApi.Universe;

namespace FLApi.Utf.Mat.Materials
{
    public sealed class DetailMap2Dm1Msk2PassMaterial : AbstractDcDtAcFlipTile
    {
        /// <summary>
        /// Detail Map 1 Flags
        /// </summary>
        public int Dm1Flags { get; private set; }

        private string dm1Name;

        /// <summary>
        /// Detail Map 1
        /// </summary>
        public TextureData Dm1 { get; private set; }

        public DetailMap2Dm1Msk2PassMaterial(IntermediateNode node, ILibFile textureLibrary, string type)
            : base(node, textureLibrary, type)
        {
        }

        protected override bool parentNode(LeafNode n)
        {
            if (!base.parentNode(n))
                switch (n.Name.ToLowerInvariant())
                {
                    case "dm1_flags":
                        Dm1Flags = n.Int32Data.Value;
                        break;
                    case "dm1_name":
                        dm1Name = n.StringData;
                        break;
                    default: return false;
                }

            return true;
        }

        public override void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            if (dm1Name != null) Dm1 = textureLibrary.FindTexture(dm1Name);
            if (Dm1 != null) Dm1.Initialize(device);

            base.Initialize(device, content, camera);
        }

        public override void Draw(D3DFVF vertexFormat, PrimitiveTypes primitiveType, int baseVertex, int numVertices, int startIndex, int primitiveCount, Color ambient, List<LightSource> lights, Matrix4 world)
        {
            if (effect != null)
            {
				if (Dm1 == null)
					effect.SetParameter ("Dm1Sampler", nullTexture);
				else
					effect.SetParameter ("Dm1Sampler", Dm1.Texture);
            }

            base.Draw(vertexFormat, primitiveType, baseVertex, numVertices, startIndex, primitiveCount, ambient, lights, world);
        }
    }
}