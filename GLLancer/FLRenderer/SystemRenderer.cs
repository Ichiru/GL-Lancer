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

using FLApi;
using FLApi.Utf.Mat;
using FLApi.Utf.Cmp;
using FLApi.Universe;
using FLApi.Solar;


using Camera = RenderTools.Camera;

using IDrawable = FLApi.IDrawable;
namespace FLRenderer
{
	public class SystemRenderer
	{
		private GraphicsDevice graphicsDevice;
		private ContentManager content;
		private Camera camera;

		public Matrix World { get; private set; }

		private StarSystem starSystem;
		public StarSystem StarSystem
		{
			get { return starSystem; }
			set { loadSystem(value); }
		}

		public List<SunRenderer> Suns { get; private set; }
		public List<PlanetRenderer> Planets { get; private set; }
		public List<ModelRenderer> Models { get; private set; }

		private ModelFile[] starSphereModels;

		public ObjectRenderer SelectedObject { get; set; }

		private Color laneColor = Color.White, gateColor = Color.Lime, holeColor = Color.Red, defaultColor = Color.Turquoise;

		//private BackgroundWorker systemLoader;
		public event SystemLoadedEventHandler SystemLoaded;
		Main starchart;
		public SystemRenderer(GraphicsDevice graphicsDevice, ContentManager content, Camera camera, Main sc)
		{
			this.graphicsDevice = graphicsDevice;
			this.content = content;
			this.camera = camera;
			starchart = sc;
			starSystem = null;

			World = Matrix.Identity;

			Suns = new List<SunRenderer>();
			Planets = new List<PlanetRenderer>();
			Models = new List<ModelRenderer>();

			starSphereModels = new ModelFile[0];
		}

		private void loadSystem(StarSystem system)
		{
			foreach (SunRenderer s in Suns) s.Dispose();
			Suns.Clear();

			foreach (PlanetRenderer p in Planets) p.Dispose();
			Planets.Clear();

			foreach (ModelRenderer r in Models) r.Dispose();
			Models.Clear();

			if (starSphereModels != null)
			{
				starSphereModels = new ModelFile[0];
			}

			GC.Collect();

			//if (!systemLoader.IsBusy)
			//systemLoader.RunWorkerAsync(system);
			loadSystemSynchronously(system);
		}

		//void systemLoader_DoWork(object sender, DoWorkEventArgs e)
		private void loadSystemSynchronously(StarSystem system)
		{
			//StarSystem system = e.Argument as StarSystem;
			starSystem = system;

			List<ModelFile> starSphereRenderData = new List<ModelFile>();

			CmpFile basicStars = system.BackgroundBasicStars;
			if (basicStars != null) starSphereRenderData.AddRange(basicStars.Models.Values);

			IDrawable complexStars = system.BackgroundComplexStars;
			if (complexStars != null)
			{
				if (complexStars is CmpFile)
				{
					CmpFile cmp = complexStars as CmpFile;
					starSphereRenderData.AddRange(cmp.Models.Values);
				}
				else if (complexStars is ModelFile)
				{
					ModelFile model = complexStars as ModelFile;
					starSphereRenderData.Add(model);
				}
			}

			CmpFile nebulae = system.BackgroundNebulae;
			if (nebulae != null)
			{
				starSphereRenderData.AddRange(nebulae.Models.Values);
			}

			starSphereModels = starSphereRenderData.ToArray();

			foreach (ModelFile model in starSphereModels) model.Initialize(graphicsDevice, content, camera);

			foreach (SystemObject o in system.Objects)
			{
				if (o.Archetype is Sun)
				{
					Sun sun = o.Archetype as Sun;

					SunRenderer s = new SunRenderer(graphicsDevice, content, camera, World, true, o);
					Suns.Add(s);
				}
				else if (o.Archetype is Planet)
				{
					Planet planet = o.Archetype as Planet;
					if (planet.DaArchetype is SphFile)
					{
						PlanetRenderer p = new PlanetRenderer(graphicsDevice, content, camera, World, true, o);
						//PlanetRenderer p = new PlanetRenderer (starchart, World, o);
						Planets.Add(p);
					}
					else
					{
						ModelRenderer m = new ModelRenderer(graphicsDevice, content, camera, World, true, o, Color.Blue);
						Models.Add(m);
					}
				}
				else if (o.Archetype is TradelaneRing)
				{
					ModelRenderer m = new ModelRenderer(graphicsDevice, content, camera, World, true, o, laneColor);
					Models.Add(m);
				}
				else if (o.Archetype is JumpGate)
				{
					ModelRenderer m = new ModelRenderer(graphicsDevice, content, camera, World, true, o, gateColor);
					Models.Add(m);
				}
				else if (o.Archetype is JumpHole)
				{
					ModelRenderer m = new ModelRenderer(graphicsDevice, content, camera, World, true, o, holeColor);
					Models.Add(m);
				}
				else
				{
					ModelRenderer m = new ModelRenderer(graphicsDevice, content, camera, World, true, o, defaultColor);
					Models.Add(m);
				}
			}

			SelectedObject = Suns.Count > 0 ? Suns[0] : Planets.Count > 0 ? Planets[0] : (ObjectRenderer)Models[0];



			if (SystemLoaded != null)
				SystemLoaded(this, new EventArgs());
		}

		public void LoadContent()
		{

		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(TimeSpan elapsed)
		{
			foreach (ModelFile model in starSphereModels) model.Update();

			for (int i = 0; i < Suns.Count; i++) Suns[i].Update(elapsed);
			for (int i = 0; i < Planets.Count; i++) Planets[i].Update(elapsed);
			for (int i = 0; i < Models.Count; i++) Models[i].Update(elapsed);
		}

		public void UpdatePlanetTextures()
		{
			for (int i = 0; i < Planets.Count; i++)
				Planets[i].UpdatePlanetTexture();
		}

		public void Draw()
		{
			//StarSphere
			for (int i = 0; i < starSphereModels.Length; i++)
			{
				starSphereModels[i].Draw(Color.White, new List<LightSource>(), Matrix.CreateTranslation(camera.Position));
			}

			//for (int i = 0; i < Suns.Count; i++) Suns[i].Draw(starSystem.AmbientColor.Value, starSystem.LightSources);
			//for (int i = 0; i < Models.Count; i++) Models[i].Draw(starSystem.AmbientColor.Value, starSystem.LightSources);
			//for (int i = 0; i < Planets.Count; i++) Planets[i].Draw(starSystem.AmbientColor.Value, starSystem.LightSources);
		}
	}

	public delegate void SystemLoadedEventHandler(object sender, EventArgs e);
}
