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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using RenderTools;

using FLApi;

using Camera = RenderTools.Camera;

namespace FLRenderer
{
	public class Main
	{
		public GraphicsDevice GraphicsDevice { get; private set; }
		public ContentManager Content { get; private set; }

		public FreelancerIni Data { get; private set; }

		public Camera Camera { get; private set; }
		//public SectorMap SectorMap { get; private set; }
		public SystemRenderer SystemMap { get; private set; }
		//public Hud Hud { get; private set; }
		public CoordinateSystem CoordinateSystem { get; private set; }

		public Main(FreelancerIni data)
		{
			Data = data;
		}

		public void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
		{
			this.GraphicsDevice = graphicsDevice;
			this.Content = content;

			Camera = new Camera(GraphicsDevice);
			Camera.Zoom = 5000;
			//SectorMap = new SectorMap(this);
			SystemMap = new SystemRenderer(GraphicsDevice, Content, Camera);
			//Hud = new Hud(this);
			CoordinateSystem = new CoordinateSystem(Camera, Content);
		}

		public void LoadContent()
		{
			//SectorMap.LoadContent();
			SystemMap.LoadContent();
			//Hud.LoadContent();

			CoordinateSystem.LoadContent();
			CoordinateSystem.SetUpVertices(new Vector3(-100000, -10000, -100000), new Vector3(100000, 10000, 100000), new Vector3(10000, 10000, 10000));
		}

		public void Update(TimeSpan elapsed)
		{
			Camera.Update();
			SystemMap.Update(elapsed);
			#if XNA
			Hud.Update(elapsed);
			#endif

			CoordinateSystem.Update();
		}

		public void DeviceReset()
		{
			Camera.UpdateProjection();
			SystemMap.UpdatePlanetTextures();
		}

		public void Draw()
		{
			//SectorMap.Draw();

			GraphicsDevice.Clear(Color.Black);

			CoordinateSystem.Draw();
			SystemMap.Draw();

			//Hud.Draw();
		}
	}
}