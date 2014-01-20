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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using FLParser.Utf;

using FLApi.Utf.Vms;
using FLApi.Universe;

namespace FLApi.Utf.Mat.Materials
{
    public class DcDtEt : DcDt
    {
        /// <summary>
        /// Emissive Texture Flags
        /// </summary>
        public int EtFlags { get; private set; }

        private string etName;

        /// <summary>
        /// Emissive Texture
        /// </summary>
        public TextureData Et { get; set; }

        public DcDtEt(IntermediateNode node, ILibFile textureLibrary, string type)
            : base(node, textureLibrary, type)
        {
        }

        protected override bool parentNode(LeafNode n)
        {
            if (!base.parentNode(n))
                switch (n.Name.ToLowerInvariant())
                {
                    case "et_flags":
                        EtFlags = n.Int32Data.Value;
                        break;
                    case "et_name":
                        etName = n.StringData;
                        break;
                    default: return false;
                }

            return true;
        }

        public override void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            if (etName != null) Et = textureLibrary.FindTexture(etName);
            if (Et != null) Et.Initialize(device);

            base.Initialize(device, content, camera);
        }

        public override void Draw(D3DFVF vertexFormat, PrimitiveType primitiveType, int baseVertex, int numVertices, int startIndex, int primitiveCount, Color ambient, List<LightSource> lights, Matrix world)
        {
            if (effect != null)
            {
                if (Et == null) effect.Parameters["Et"].SetValue(nullTexture);
                else effect.Parameters["Et"].SetValue(Et.Texture);
            }

            base.Draw(vertexFormat, primitiveType, baseVertex, numVertices, startIndex, primitiveCount, ambient, lights, world);
        }
    }
}
