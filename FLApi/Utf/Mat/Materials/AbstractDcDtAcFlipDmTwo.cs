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

using FLCommon;
using OpenTK;

using FLParser.Utf;

using FLApi.Utf.Vms;
using FLApi.Universe;

namespace FLApi.Utf.Mat.Materials
{
    public abstract class AbstractDcDtAcFlipDmTwo : AbstractDcDtAcFlip
    {
        /// <summary>
        /// Detail Map 0 Flags
        /// </summary>
        public int Dm0Flags { get; private set; }

        private string dm0Name;

        /// <summary>
        /// Detail Map 0
        /// </summary>
        public TextureData Dm0 { get; set; }

        /// <summary>
        /// Detail Map 1 Flags
        /// </summary>
        public int Dm1Flags { get; private set; }

        private string dm1Name;

        /// <summary>
        /// Detail Map 1
        /// </summary>
        public TextureData Dm1 { get; set; }

        /// <summary>
        /// Tile Rate 0 tile amount (1=no tiling, >1 creates multiple tiles), 0 (f)
        /// </summary>
        public float TileRate0 { get; set; }

        /// <summary>
        /// Tile Rate 1 tile amount (1=no tiling, >1 creates multiple tiles), 0 (f)
        /// </summary>
        public float TileRate1 { get; set; }

        public AbstractDcDtAcFlipDmTwo(IntermediateNode node, ILibFile textureLibrary, string type)
            : base(node, textureLibrary, type)
        {
        }

        protected override bool parentNode(LeafNode n)
        {
            if (!base.parentNode(n))
                switch (n.Name.ToLowerInvariant())
                {
                    case "dm0_flags":
                        Dm0Flags = n.Int32Data.Value;
                        break;
                    case "dm0_name":
                        dm0Name = n.StringData;
                        break;
                    case "dm1_flags":
                        Dm1Flags = n.Int32Data.Value;
                        break;
                    case "dm1_name":
                        dm1Name = n.StringData;
                        break;
                    case "tilerate0":
                        TileRate0 = n.SingleData.Value;
                        break;
                    case "tilerate1":
                        TileRate1 = n.SingleData.Value;
                        break;
                    default: return false;
                }

            return true;
        }

        public override void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            if (dm0Name != null) Dm0 = textureLibrary.FindTexture(dm0Name);
            if (Dm0 != null) Dm0.Initialize(device);
            if (dm1Name != null) Dm1 = textureLibrary.FindTexture(dm1Name);
            if (Dm1 != null) Dm1.Initialize(device);

            base.Initialize(device, content, camera);
        }

        public override void Draw(D3DFVF vertexFormat, PrimitiveTypes primitiveType, int baseVertex, int numVertices, int startIndex, int primitiveCount, Color ambient, List<LightSource> lights, Matrix4 world)
        {
            if (effect != null)
            {
				if (Dm0 == null) effect.SetParameter ("Dm0", nullTexture);
				else effect.SetParameter ("Dm0", Dm0.Texture);

				if (Dm1 == null) effect.SetParameter ("Dm1", nullTexture);
				else effect.SetParameter ("Dm1", Dm1.Texture);

				effect.SetParameter ("TileRate0", TileRate0);
				effect.SetParameter ("TileRate1", TileRate1);
            }

            base.Draw(vertexFormat, primitiveType, baseVertex, numVertices, startIndex, primitiveCount, ambient, lights, world);
        }
    }
}
