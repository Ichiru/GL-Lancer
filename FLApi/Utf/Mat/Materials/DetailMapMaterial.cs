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
    public sealed class DetailMapMaterial : AbstractDcDtAcFlipTile
    {
        /// <summary>
        /// Detail Map Flags
        /// </summary>
        public int DmFlags { get; private set; }

        private string dmName;

        /// <summary>
        /// Detail Map
        /// </summary>
        public TextureData Dm { get; private set; }

        public DetailMapMaterial(IntermediateNode node, ILibFile textureLibrary, string type)
            : base(node, textureLibrary, type)
        {
        }

        protected override bool parentNode(LeafNode n)
        {
            if (!base.parentNode(n))
                switch (n.Name.ToLowerInvariant())
                {
                    case "dm_flags":
                        DmFlags = n.Int32Data.Value;
                        break;
                    case "dm_name":
                        dmName = n.StringData;
                        break;
                    default: return false;
                }

            return true;
        }

        public override void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            if (dmName != null) Dm = textureLibrary.FindTexture(dmName);
            if (Dm != null) Dm.Initialize(device);

            base.Initialize(device, content, camera);
        }

        public override void Draw(D3DFVF vertexFormat, PrimitiveType primitiveType, int baseVertex, int numVertices, int startIndex, int primitiveCount, Color ambient, List<LightSource> lights, Matrix4 world)
        {
            if (effect != null)
            {
				if (Dm == null)
					effect.SetParameter ("Dm", nullTexture);
				else
					effect.SetParameter ("Dm", Dm.Texture);
            }

            base.Draw(vertexFormat, primitiveType, baseVertex, numVertices, startIndex, primitiveCount, ambient, lights, world);
        }
    }
}