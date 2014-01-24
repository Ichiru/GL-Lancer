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
 * Some data structures and info taken from Freelancer UTF Editor
 * by Cannon & Adoxa, continuing the work of Colin Sanby and Mario 'HCl' Brito (http://the-starport.net)
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

using FLApi;
using FLApi.Utf.Cmp;
using FLApi.Universe;

using RenderTools.Primitives;

using Camera = RenderTools.Camera;

using IDrawable = FLApi.IDrawable;
namespace FLRenderer
{
	public class ModelRenderer : ObjectRenderer
	{
		public ModelFile Model { get; private set; }
		public CmpFile Cmp { get; private set; }

		private Cuboid[] boundingBoxes;

		public ModelRenderer(GraphicsDevice graphicsDevice, ContentManager content, Camera camera, Matrix world, bool useObjectPosAndRotate, SystemObject spaceObject, Color uiColor)
			: base(graphicsDevice, content, camera, world, useObjectPosAndRotate, spaceObject, uiColor)
		{
			boundingBoxes = new Cuboid[0];

			IDrawable archetype = spaceObject.Archetype.DaArchetype;

			if (archetype is ModelFile)
			{
				Model = archetype as ModelFile;
				if (Model != null && Model.Levels.ContainsKey(0))
				{
					Model.Initialize(graphicsDevice, content, camera);

					boundingBoxes = new Cuboid[1];
					boundingBoxes[0] = new Cuboid(graphicsDevice, Model.Levels[0].BoundingBox.GetCorners());
				}
			}
			else if (archetype is CmpFile)
			{
				Cmp = archetype as CmpFile;
				Cmp.Initialize(graphicsDevice, content, camera);

				boundingBoxes = new Cuboid[Cmp.Models.Values.Count];
				int i = 0;
				foreach (ModelFile model in Cmp.Models.Values)
				{
					if (model.Levels.ContainsKey(0))
					{
						boundingBoxes[i] = new Cuboid(graphicsDevice, model.Levels[0].BoundingBox.GetCorners());
						i++;
					}
				}
			}
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		public override void Update(TimeSpan elapsed)
		{
			if (Model != null) Model.Update();
			else if (Cmp != null) Cmp.Update();
			base.Update(elapsed);
		}

		public override void Draw(Color ambientColor, List<LightSource> lights)
		{
			if (Model != null) Model.Draw(ambientColor, lights, World);
			else if (Cmp != null) Cmp.Draw(ambientColor, lights, World);

			if (DrawBoundingBoxEnabled)
			{
				graphicsDevice.Indices = Cuboid.IndexBuffer;

				for (int i = 0; i < boundingBoxes.Length; i++)
				{
					boundingBoxEffect.Parameters["Dc"].SetValue(UiColor.ToVector3());
					boundingBoxEffect.Parameters["World"].SetValue(World);
					boundingBoxEffect.CurrentTechnique.Passes[0].Apply();

					graphicsDevice.SetVertexBuffer(boundingBoxes[i].VertexBuffer);

					graphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, Cuboid.VERTEX_COUNT, 0, Cuboid.PrimitiveCount);
				}
			}
		}

		public override void Dispose()
		{
			foreach (Cuboid b in boundingBoxes) b.Dispose();
		}
	}
}