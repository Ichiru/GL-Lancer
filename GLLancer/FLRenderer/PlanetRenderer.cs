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

using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK;
using FLCommon;

using FLApi.Utf.Mat;
using FLApi.Utf.Vms;
using FLApi.Solar;
using FLApi.Universe;

using RenderTools;
using RenderTools.Primitives;

namespace FLRenderer
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class PlanetRenderer : ObjectRenderer
	{
		//private Effect planetEffect;

		public Planet Planet { get; private set; }
		//private Material[] materials;
		//private RenderTargetCube planetTexture;

		//private Quad faceQuad;

		//private VertexBuffer planetVertexBuffer, atmosphereVertexBuffer;
		//private IndexBuffer planetIndexBuffer, atmosphereIndexBuffer;
		//private int planetPrimitiveCount;

		//private bool updatePlanetTexture;

		private Ellipsoid boundingSphere;

		public PlanetRenderer(GraphicsDevice graphicsDevice, ContentManager content, Camera camera, Matrix4 world, bool useObjectPosAndRotate, SystemObject spaceObject)
			: base(graphicsDevice, content, camera, world, useObjectPosAndRotate, spaceObject, Color.White)
		{
			//faceQuad = new Quad(graphicsDevice);

			Planet = spaceObject.Archetype as Planet;

			SphFile s = Planet.DaArchetype as SphFile;

			boundingSphere = new Ellipsoid(graphicsDevice, new Vector3(s.Radius) * 1.1f, 12, 16);
			Console.WriteLine (Planet.IdsName);
			Console.WriteLine (s.SideMaterials [0].GetType ());
			Console.WriteLine (s.SideMaterials [0].Name);
			spaceObject.Initialize(graphicsDevice, content, camera);

			//Ellipsoid sphere = new Ellipsoid(graphicsDevice, new Vector3(s.Radius), 48, 64);
			//planetVertexBuffer = sphere.VertexBuffer;
			//planetIndexBuffer = sphere.IndexBuffer;
			//planetPrimitiveCount = planetIndexBuffer.IndexCount / 3;

			//materials = s.SideMaterials;
			//foreach (Material material in materials) material.Initialize(graphicsDevice, content, camera, ambientColor, lights);

			//Ellipsoid atmoSphere = new Ellipsoid(graphicsDevice, new Vector3(SpaceObject.AtmosphereRange.Value), 48, 64);
			//atmosphereVertexBuffer = atmoSphere.VertexBuffer;
			//atmosphereIndexBuffer = atmoSphere.IndexBuffer;

			//planetEffect = content.Load<Effect>("effects/Planet");
			//planetEffect.Parameters["AmbientColor"].SetValue(ambientColor.ToVector3());
			//planetEffect.Parameters["DirectionalLightCount"].SetValue(lights.Count);
			//planetEffect.Parameters["DirectionalLightPositions"].SetValue((from LightSource light in lights select light.Pos.Value).ToArray<Vector3>());
			//planetEffect.Parameters["DirectionalLightColors"].SetValue((from LightSource light in lights select light.Color.Value.ToVector3()).ToArray<Vector3>());
			//planetEffect.Parameters["DirectionalLightRanges"].SetValue((from LightSource light in lights select light.Range.Value).ToArray<int>());

			//updatePlanetTexture = true;
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		public override void Update(TimeSpan elapsed)
		{
			//planetEffect.Parameters["View"].SetValue(camera.View);
			//planetEffect.Parameters["Projection"].SetValue(camera.Projection);

			//foreach (Material material in materials) material.Update();

			SpaceObject.Update();

			base.Update(elapsed);
		}

		public void UpdatePlanetTexture()
		{
			//updatePlanetTexture = true;

			SpaceObject.Resized();
		}

		/*private void drawTexture()
        {
            // Generate TextureCube for planet
            if (materials.Length > 0)
            {
                Texture2D t0 = materials[0].Dt.Texture as Texture2D;
                planetTexture = new RenderTargetCube(graphicsDevice, 512, true, t0.Format, DepthFormat.None);

                graphicsDevice.SetVertexBuffer(faceQuad.VertexBuffer);
                graphicsDevice.Indices = Quad.IndexBuffer;

                graphicsDevice.SetRenderTarget(planetTexture, CubeMapFace.PositiveZ);
                graphicsDevice.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
                materials[0].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Matrix4.Identity);

                graphicsDevice.SetRenderTarget(planetTexture, CubeMapFace.PositiveX);
                graphicsDevice.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
                materials[1].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Matrix4.Identity);

                graphicsDevice.SetRenderTarget(planetTexture, CubeMapFace.NegativeZ);
                graphicsDevice.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
                materials[2].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Matrix4.Identity);

                graphicsDevice.SetRenderTarget(planetTexture, CubeMapFace.NegativeX);
                graphicsDevice.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
                materials[3].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Matrix4.Identity);

                graphicsDevice.SetRenderTarget(planetTexture, CubeMapFace.PositiveY);
                graphicsDevice.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
                materials[4].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Matrix4.Identity);

                graphicsDevice.SetRenderTarget(planetTexture, CubeMapFace.NegativeY);
                graphicsDevice.Clear(ClearOptions.Target, Color.Magenta, 0, 0);
                materials[5].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, Quad.VERTEX_COUNT, 0, Quad.PrimitiveCount, Matrix4.Identity);
            }
            else planetTexture = new RenderTargetCube(graphicsDevice, 512, false, SurfaceFormat.Dxt5, DepthFormat.None);

            graphicsDevice.SetRenderTarget(null);

            updatePlanetTexture = false;
        }*/

		public override void Draw(Color ambientColor, List<LightSource> lights)
		{
			//if (updatePlanetTexture) drawTexture();

			// Draw planet
			//graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
			//graphicsDevice.DepthStencilState = DepthStencilState.Default;
			//graphicsDevice.BlendState = BlendState.Opaque;

			//graphicsDevice.SetVertexBuffer(planetVertexBuffer);
			//graphicsDevice.Indices = planetIndexBuffer;

			//planetEffect.Parameters["World"].SetValue(World);
			//planetEffect.Parameters["Dt"].SetValue(planetTexture);

			//planetEffect.CurrentTechnique.Passes[0].Apply();
			//graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, planetVertexBuffer.VertexCount, 0, planetPrimitiveCount);

			// Draw atmosphere
			//if (materials.Length > 6)
			{
				//graphicsDevice.SetVertexBuffer(atmosphereVertexBuffer);
				//graphicsDevice.Indices = atmosphereIndexBuffer;

				//materials[6].Draw(D3DFVF.XYZ | D3DFVF.TEX1, 0, atmosphereVertexBuffer.VertexCount, 0, atmosphereIndexBuffer.IndexCount / 3, World);
			}

			SpaceObject.Draw(Color.White, lights, Matrix4.Identity);

			if (DrawBoundingBoxEnabled)
			{
				boundingBoxEffect.SetParameter ("Dc", UiColor.ToVector3 ());
				boundingBoxEffect.SetParameter ("World", World);
				boundingBoxEffect.Apply ();

				graphicsDevice.SetVertexBuffer(boundingSphere.VertexBuffer);
				graphicsDevice.Indices = boundingSphere.IndexBuffer;

				graphicsDevice.DrawIndexedPrimitives(PrimitiveTypes.LineList, 0, 0, boundingSphere.VertexBuffer.VertexCount, 0, boundingSphere.IndexBuffer.IndexCount / 2);
			}
		}

		public override void Dispose()
		{
			boundingSphere.Dispose();
		}
	}
}