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

using OpenTK;
using FLCommon;

using FLApi.Universe;

using RenderTools;
using RenderTools.Primitives;

namespace FLRenderer
{
	public abstract class ObjectRenderer : IDisposable
	{
		protected GraphicsDevice graphicsDevice;
		protected Camera camera;

		public Matrix World { get; private set; }
		public SystemObject SpaceObject { get; private set; }
		public Color UiColor { get; private set; }
		public bool DrawBoundingBoxEnabled { get; set; }

		//protected Effect boundingBoxEffect;

		protected ObjectRenderer(GraphicsDevice graphicsDevice, ContentManager content, Camera camera, Matrix world, bool useObjectPosAndRotate, SystemObject spaceObject, Color uiColor)
		{
			this.graphicsDevice = graphicsDevice;
			this.camera = camera;

			if (useObjectPosAndRotate)
			{
				World = world * Matrix.CreateTranslation(spaceObject.Pos.Value);
				if (spaceObject.Rotate != null) World =
					Matrix.CreateRotationX(MathConvert.ToRadians(spaceObject.Rotate.Value.X)) *
						Matrix.CreateRotationY(MathConvert.ToRadians(spaceObject.Rotate.Value.Y)) *
						Matrix.CreateRotationZ(MathConvert.ToRadians(spaceObject.Rotate.Value.Z)) *
						World;
			}
			else World = Matrix.Identity;

			SpaceObject = spaceObject;
			UiColor = uiColor;
			//boundingBoxEffect = content.Load<Effect>("effects/Wireframe");

			DrawBoundingBoxEnabled = false;
		}

		public virtual void Update(TimeSpan elapsed)
		{
			//boundingBoxEffect.SetParameter ("View", camera.View);
			//boundingBoxEffect.SetParameter ("Projection", camera.Projection);
		}

		public abstract void Draw(Color ambientColor, List<LightSource> lights);

		public abstract void Dispose();

		public override string ToString()
		{
			string result = string.Empty;
			if (!string.IsNullOrWhiteSpace(SpaceObject.IdsName)) result += SpaceObject.IdsName + " ";

			foreach (string s in SpaceObject.TradelaneSpaceName) result += s + " ";

			return result + "[" + SpaceObject.Nickname + "]";
		}
	}
}