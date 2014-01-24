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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using FLApi.Universe;

using RenderTools;
using RenderTools.Primitives;

namespace FLRenderer
{
	public class ZoneRenderer
	{
		private GraphicsDevice graphicsDevice;
		private Camera camera;

		public Matrix World { get; private set; }
		public Zone Zone { get; private set; }

		private VertexBuffer vertexBuffer;
		private IndexBuffer indexBuffer;

		public Color UiColor { get; private set; }

		public bool DrawZone { get; set; }

		private BasicEffect effect;

		public ZoneRenderer(GraphicsDevice graphicsDevice, ContentManager content, Camera camera, Matrix world, bool useObjectPosAndRotate, Zone zone, Color uiColor)
		{
			this.graphicsDevice = graphicsDevice;
			this.camera = camera;

			if (useObjectPosAndRotate)
			{
				World = world * Matrix.CreateTranslation(zone.Pos.Value);
				if (zone.Rotate != null) World =
					Matrix.CreateRotationX(MathHelper.ToRadians(zone.Rotate.Value.X)) *
						Matrix.CreateRotationY(MathHelper.ToRadians(zone.Rotate.Value.Y)) *
						Matrix.CreateRotationZ(MathHelper.ToRadians(zone.Rotate.Value.Z)) *
						World;
			}
			else World = Matrix.Identity;

			this.Zone = zone;
			UiColor = uiColor;

			DrawZone = false;

			effect = new BasicEffect(graphicsDevice);
			effect.World = World;
		}

		public void Update()
		{
			effect.View = camera.View;
			effect.Projection = camera.Projection;
		}

		public void Draw()
		{
			if (DrawZone)
			{
				if (vertexBuffer == null)
				{
					Vector3 size = Zone.Size.Value;

					switch (Zone.Shape)
					{
						case ZoneShape.CYLINDER:
						Cylinder c = new Cylinder(graphicsDevice, size, 8);
						vertexBuffer = c.VertexBuffer;
						indexBuffer = c.IndexBuffer;
						break;
						case ZoneShape.BOX:
						Cuboid b = new Cuboid(graphicsDevice, size);
						vertexBuffer = b.VertexBuffer;
						indexBuffer = Cuboid.IndexBuffer;
						break;
						case ZoneShape.ELLIPSOID:
						Ellipsoid e = new Ellipsoid(graphicsDevice, size, 8, 16);
						vertexBuffer = e.VertexBuffer;
						indexBuffer = e.IndexBuffer;
						break;
						case ZoneShape.SHERE:
						Ellipsoid s = new Ellipsoid(graphicsDevice, size, 8, 8);
						vertexBuffer = s.VertexBuffer;
						indexBuffer = s.IndexBuffer;
						break;
						case ZoneShape.RING:
						Ellipsoid r = new Ellipsoid(graphicsDevice, size, 16, 16);
						vertexBuffer = r.VertexBuffer;
						indexBuffer = r.IndexBuffer;
						break;
					}
				}

				effect.DiffuseColor = Color.Gray.ToVector3();
				effect.CurrentTechnique.Passes[0].Apply();
				graphicsDevice.Indices = indexBuffer;
				graphicsDevice.SetVertexBuffer(vertexBuffer);

				graphicsDevice.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 2);
			}
		}

		public override string ToString()
		{
			string result = string.Empty;
			if (!string.IsNullOrWhiteSpace(Zone.IdsName)) result += Zone.IdsName + " ";
			if (Zone.Comment != null)
			{
				result += "(";
				foreach (string s in Zone.Comment) result += s + ",";
				result += ") ";
			}
			return result + "[" + Zone.Nickname + "]";
		}
	}
}