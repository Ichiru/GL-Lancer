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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using FLApi.Utf.Mat;
using FLApi.Universe;
using FLApi.Solar;

using RenderTools;
using RenderTools.Primitives;

namespace FLRenderer
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class SunRenderer : ObjectRenderer
	{
		public Sun Sun { get; private set; }

		private BasicEffect sunEffect;
		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;
		private int primitiveCount;

		private Ellipsoid boundingSphere;

		public SunRenderer(GraphicsDevice graphicsDevice, ContentManager content, Camera camera, Matrix world, bool useObjectPosAndRotate, SystemObject sun)
			: base(graphicsDevice, content, camera, world, useObjectPosAndRotate, sun, Color.Yellow)
		{
			Sun = SpaceObject.Archetype as Sun;
			SphFile s = Sun.DaArchetype as SphFile;

			boundingSphere = new Ellipsoid(graphicsDevice, new Vector3(s.Radius) * 1.1f, 6, 8);

			sunEffect = new BasicEffect(graphicsDevice);

			Ellipsoid sphere = new Ellipsoid(graphicsDevice, new Vector3(s.Radius), 100, 100);
			vertexBuffer = sphere.VertexBuffer;
			indexBuffer = sphere.IndexBuffer;
			primitiveCount = indexBuffer.IndexCount / 3;
		}


		/// <summary>
		/// Allows the game component to update itself.
		public override void Update(TimeSpan elapsed)
		{
			sunEffect.View = camera.View;
			sunEffect.Projection = camera.Projection;

			base.Update(elapsed);
		}

		public override void Draw(Color ambientColor, List<LightSource> lights)
		{
			sunEffect.World = World;

			graphicsDevice.BlendState = BlendState.Opaque;
			graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
			graphicsDevice.DepthStencilState = DepthStencilState.Default;

			graphicsDevice.SetVertexBuffer(vertexBuffer);
			graphicsDevice.Indices = indexBuffer;

			sunEffect.CurrentTechnique.Passes[0].Apply();
			graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, primitiveCount);
		}

		public override void Dispose()
		{
			boundingSphere.Dispose();
		}
	}
}