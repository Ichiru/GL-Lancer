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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using FLParser.Utf;

using FLApi.Utf.Vms;
using FLApi.Utf.Mat.Materials;
using FLApi.Universe;

using Nebula = FLApi.Utf.Mat.Materials.Nebula;

namespace FLApi.Utf.Mat
{
    public abstract class Material
    {
        protected GraphicsDevice device;
        private RenderTools.Camera camera;

        protected static Texture2D nullTexture;
        protected ILibFile textureLibrary;

        protected Effect effect;

        public bool IsDisposed { get { return effect == null || effect.IsDisposed || effect.GraphicsDevice.IsDisposed; } }

        public string type { get; private set; }

        /// <summary>
        /// Material Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Diffuse Texture Flags
        /// </summary>
        public int DtFlags { get; private set; }

        public string DtName { get; private set; }

        /// <summary>
        /// Diffuse Texture
        /// </summary>
        public TextureData Dt { get; set; }

        protected Material(IntermediateNode node, ILibFile library, string type)
        {
            this.textureLibrary = library;
            this.type = type;

            Name = node.Name;

            foreach (LeafNode n in node)
            {
                if (!parentNode(n)) throw new Exception("Invalid node in node " + node.Name + ": " + n.Name);
            }
        }

        protected Material()
        {
            textureLibrary = new TxmFile();
            type = "DcDt";

            Name = "NullMaterial";

            DtFlags = -1;
            DtName = null;
        }

        public static Material FromNode(IntermediateNode node, ILibFile textureLibrary)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (textureLibrary == null) throw new ArgumentNullException("textureLibrary");

            LeafNode typeNode = node["Type"] as LeafNode;
            if (typeNode == null) throw new Exception("Invalid or missing type node in " + node.Name);

            string type = typeNode.StringData;
            switch (type)
            {
                case "Nebula": return new Nebula(node, textureLibrary, type);
                case "DcDt": return new DcDt(node, textureLibrary, type);
                case "DcDtTwo": return new DcDt(node, textureLibrary, type);
                case "AtmosphereMaterial": return new AtmosphereMaterial(node, textureLibrary, type);
                case "DetailMapMaterial": return new DetailMapMaterial(node, textureLibrary, type);
                case "DetailMap2Dm1Msk2PassMaterial": return new DetailMap2Dm1Msk2PassMaterial(node, textureLibrary, type);
                case "IllumDetailMapMaterial": return new IllumDetailMapMaterial(node, textureLibrary, type);
                case "Masked2DetailMapMaterial": return new Masked2DetailMapMaterial(node, textureLibrary, type);
                case "DcDtEc": return new DcDtEc(node, textureLibrary, type);
                case "DcDtEt": return new DcDtEt(node, textureLibrary, type);
                case "DcDtEcEt": return new DcDtEt(node, textureLibrary, type);
                case "DcDtOcOt": return new DcDtOcOt(node, textureLibrary, type);
                case "DcDtOcOtTwo": return new DcDtOcOt(node, textureLibrary, type);
                case "DcDtEcOcOt": return new DcDtEcOcOt(node, textureLibrary, type);
                case "DcDtBt": return new DcDtBt(node, textureLibrary, type);
                case "DcDtBtOcOt": return new DcDtBtOcOt(node, textureLibrary, type);
                case "DcDtBtOcOtTwo": return new DcDtBtOcOt(node, textureLibrary, type);
                default: throw new Exception("Invalid material type: " + type);
            }
        }

        protected virtual bool parentNode(LeafNode n)
        {
            switch (n.Name.ToLowerInvariant())
            {
                case "dt_flags":
                    DtFlags = n.Int32Data.Value;
                    break;
                case "dt_name":
                    DtName = n.StringData;
                    break;
                case "type":
                    break;
                default: return false;
            }

            return true;
        }

        public virtual void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            this.device = device;
            this.camera = camera;

            if (DtName != null) Dt = textureLibrary.FindTexture(DtName);
            if (Dt != null) Dt.Initialize(device);

            if (nullTexture == null || nullTexture.IsDisposed || nullTexture.GraphicsDevice.IsDisposed)
            {
                nullTexture = new Texture2D(device, 256, 256, false, SurfaceFormat.Color);
                Color[] colors = new Color[nullTexture.Width * nullTexture.Height];
                for (int i = 0; i < colors.Length; i++) colors[i] = Color.White;
                nullTexture.SetData<Color>(colors);
            }

            effect = content.Load<Effect>("effects/materials/" + type);
			//Console.WriteLine (type);
            //effect.Parameters["Projection"].SetValue(camera.Projection);
			effect.SetParameter ("Projection", camera.Projection);
        }

        public void Resized()
        {
            if (effect != null) effect.Parameters["Projection"].SetValue(camera.Projection);
        }

        public void Update()
        {
            if (effect != null)
            {
                effect.Parameters["View"].SetValue(camera.View);
                //effect.Parameters["CameraPosition"].SetValue(camera.Position);
				effect.SetParameter ("CameraPosition", camera.Position);
            }
        }
		public virtual void DrawPlanet()
		{

		}
        public virtual void Draw(D3DFVF vertexFormat, PrimitiveType primitiveType, int baseVertex, int numVertices, int startIndex, int primitiveCount, Color ambient, List<LightSource> lights, Matrix world)
        {
            if (effect != null)
            {
                if (lights != null)
                {
                    effect.Parameters["LightCount"].SetValue(lights.Count);

                    for (int i = 0; i < 9; i++)
                    {
                        effect.Parameters["LightsPos"].Elements[i].SetValue(i < lights.Count ? lights[i].Pos.Value : Vector3.Zero);
                        effect.Parameters["LightsColor"].Elements[i].SetValue(i < lights.Count ? lights[i].Color.Value.ToVector4() : Vector4.Zero);
                        effect.Parameters["LightsRange"].Elements[i].SetValue(i < lights.Count ? lights[i].Range.Value : 0);
                        effect.Parameters["LightsAttenuation"].Elements[i].SetValue(i < lights.Count ? lights[i].Attenuation ?? new Vector3(1, 0, 0) : Vector3.Zero);
                    }
                }

                //effect.Parameters["AmbientColor"].SetValue(ambient.ToVector4());
				effect.SetParameter ("AmbientColor", ambient.ToVector4 ());
                //effect.Parameters["World"].SetValue(world);
				effect.SetParameter ("World", world);
                if (Dt == null) effect.Parameters["Dt"].SetValue(nullTexture);
                else effect.Parameters["Dt"].SetValue(Dt.Texture);

                switch (vertexFormat)
                {
                    case D3DFVF.XYZ:
                        effect.CurrentTechnique = effect.Techniques["Position"];
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL:
                        effect.CurrentTechnique = effect.Techniques["PositionNormal"];
                        break;
                    case D3DFVF.XYZ | D3DFVF.TEX1:
                        effect.CurrentTechnique = effect.Techniques["PositionTexture"];
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX1:
                        effect.CurrentTechnique = effect.Techniques["PositionNormalTexture"];
                        break;
                    case D3DFVF.XYZ | D3DFVF.NORMAL | D3DFVF.TEX2:
                        effect.CurrentTechnique = effect.Techniques["PositionNormalTextureTwo"];
                        break;
                    case D3DFVF.XYZ | D3DFVF.DIFFUSE | D3DFVF.TEX1:
                        effect.CurrentTechnique = effect.Techniques["PositionDiffuseTexture"];
                        break;
                    default: throw new Exception("Invalid vertex format: " + vertexFormat);
                }

                effect.CurrentTechnique.Passes[0].Apply();
                device.DrawIndexedPrimitives(primitiveType, baseVertex, 0, numVertices, startIndex, primitiveCount);
            }
        }
    }
}
