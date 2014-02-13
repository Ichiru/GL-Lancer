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
 * Sphere creation code taken form here: http://forums.create.msdn.com/forums/p/11680/61589.aspx
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using FLCommon;

using FLParser;
using FLParser.Utf;

using FLApi.Utf.Mat.Materials;
using FLApi.Utf.Vms;
using FLApi.Universe;

using RenderTools.Primitives;

namespace FLApi.Utf.Mat
{
    /// <summary>
    /// Represents a UTF Sphere File (.sph)
    /// </summary>
    public class SphFile : UtfFile, IDrawable
    {
        private GraphicsDevice device;
        private RenderTools.Camera camera;

        private Quad quad;
        private Ellipsoid sphere;
        private Effect planetEffect;
        private TextureCube planetTexture;
        private bool ready, updatePlanetTexture;

        private ILibFile materialLibrary;

        public float Radius { get; private set; }

        private List<string> sideMaterialNames;
        private Material[] sideMaterials;
        public Material[] SideMaterials
        {
            get
            {
                if (sideMaterials == null)
                {
                    sideMaterials = new Material[sideMaterialNames.Count];
                    for (int i = 0; i < sideMaterialNames.Count; i++)
                    {
                        sideMaterials[i] = materialLibrary.FindMaterial(CrcTool.FLModelCrc(sideMaterialNames[i]));
                        if (sideMaterials[i] == null) sideMaterials[i] = new NullMaterial();
                    }
                }

                return sideMaterials;
            }
        }

        public SphFile(string path, ILibFile materialLibrary)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (materialLibrary == null) throw new ArgumentNullException("materialLibrary");

            ready = false;
            updatePlanetTexture = true;

            this.materialLibrary = materialLibrary;
            sideMaterialNames = new List<string>();

            IntermediateNode sphereNode = parseFile(path)[0] as IntermediateNode;
            if (sphereNode == null) throw new FileContentException(FILE_TYPE, "SphFile without sphere");

            foreach (LeafNode sphereSubNode in sphereNode)
            {
                string name = sphereSubNode.Name.ToLowerInvariant();

                if (name.StartsWith("m", StringComparison.OrdinalIgnoreCase)) sideMaterialNames.Add(sphereSubNode.StringData);
                else if (name == "radius") Radius = sphereSubNode.SingleData.Value;
                else if (name == "sides")
                {
                    int count = sphereSubNode.Int32Data.Value;
                    if (count != sideMaterialNames.Count) throw new Exception("Invalid number of sides in " + sphereNode.Name + ": " + count);
                }
                else throw new Exception("Invalid node in " + sphereNode.Name + ": " + sphereSubNode.Name);
            }
        }

        public void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            if (sideMaterialNames.Count >= 6)
            {
                this.device = device;
                this.camera = camera;

                quad = new Quad(device);
                sphere = new Ellipsoid(device, new Vector3(Radius), 48, 64);

                foreach (Material m in SideMaterials)
                    m.Initialize(device, content, camera);

                planetEffect = content.Load<Effect>("effects/Planet");
                planetEffect.Parameters["Projection"].SetValue(camera.Projection);

                ready = true;
            }
        }

        public void Resized()
        {
            if (ready)
            {
                planetEffect.Parameters["Projection"].SetValue(camera.Projection);
                updatePlanetTexture = true;
            }
        }

        public void Update()
        {
            if (ready)
            {
                planetEffect.Parameters["View"].SetValue(camera.View);
            }
        }

        private void drawPlanetTexture()
        {
            // Generate TextureCube for planet
            planetTexture = new TextureCube(device, 512, true, SurfaceFormat.Color);
			var renderTarget = new RenderTarget2D (device, 512, 512);
            device.SetVertexBuffer(quad.VertexBuffer);
            device.Indices = Quad.IndexBuffer;
			Color[] data = new Color[512 * 512];
			device.SetRenderTarget (renderTarget);
            //device.SetRenderTarget(planetTexture, CubeMapFace.PositiveZ);
            device.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
            SideMaterials[0].Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveType.TriangleList, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Color.White, null, Matrix4.Identity);
			renderTarget.GetData<Color> (data);
			planetTexture.SetData<Color> (CubeMapFace.PositiveZ, data);
            //device.SetRenderTarget(planetTexture, CubeMapFace.PositiveX);
            device.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
            SideMaterials[1].Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveType.TriangleList, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Color.White, null, Matrix4.Identity);
			renderTarget.GetData<Color> (data);
			planetTexture.SetData<Color> (CubeMapFace.PositiveX, data);
            //device.SetRenderTarget(planetTexture, CubeMapFace.NegativeZ);
            device.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
            SideMaterials[2].Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveType.TriangleList, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Color.White, null, Matrix4.Identity);
			renderTarget.GetData<Color> (data);
			planetTexture.SetData<Color> (CubeMapFace.NegativeZ, data);
            //device.SetRenderTarget(planetTexture, CubeMapFace.NegativeX);
            device.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
            SideMaterials[3].Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveType.TriangleList, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Color.White, null, Matrix4.Identity);
			renderTarget.GetData<Color> (data);
			planetTexture.SetData<Color> (CubeMapFace.NegativeX, data);
            //device.SetRenderTarget(planetTexture, CubeMapFace.PositiveY);
            device.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
            SideMaterials[4].Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveType.TriangleList, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Color.White, null, Matrix4.Identity);
			renderTarget.GetData<Color> (data);
			planetTexture.SetData<Color> (CubeMapFace.PositiveY, data);
            //device.SetRenderTarget(planetTexture, CubeMapFace.NegativeY);
            device.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
            SideMaterials[5].Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveType.TriangleList, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Color.White, null, Matrix4.Identity);
			renderTarget.GetData<Color> (data);
			planetTexture.SetData<Color> (CubeMapFace.NegativeY, data);

            device.SetRenderTarget(null);
			renderTarget.Dispose ();
            updatePlanetTexture = false;
        }

        public void Draw(Color ambient, List<LightSource> lights, Matrix4 world)
        {
            if (ready)
            {
                if (updatePlanetTexture) drawPlanetTexture();

                // Draw planet
                device.SetVertexBuffer(sphere.VertexBuffer);
                device.Indices = sphere.IndexBuffer;
				lights = null;
                if (lights != null)
                {
                    planetEffect.Parameters["LightCount"].SetValue(lights.Count);

                    for (int i = 0; i < 9; i++)
                    {
						planetEffect.Parameters["LightsPos"].Elements[i].SetValue(i < lights.Count ? lights[i].Pos.Value : Vector3.Zero);
						planetEffect.Parameters["LightsColor"].Elements[i].SetValue(i < lights.Count ? lights[i].Color.Value.ToVector4() : Vector4.Zero);
						planetEffect.Parameters["LightsRange"].Elements[i].SetValue(i < lights.Count ? lights[i].Range.Value : 0);
						planetEffect.Parameters["LightsAttenuation"].Elements[i].SetValue(i < lights.Count ? lights[i].Attenuation ?? new Vector3(1, 0, 0) : Vector3.Zero);
                    }
                }

                planetEffect.Parameters["AmbientColor"].SetValue(ambient.ToVector4());
                planetEffect.Parameters["World"].SetValue(world);
                planetEffect.Parameters["PlanetTexture"].SetValue(planetTexture);

                planetEffect.CurrentTechnique.Passes[0].Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, sphere.VertexBuffer.VertexCount, 0, sphere.IndexBuffer.IndexCount / 3);
            }
        }
    }
}