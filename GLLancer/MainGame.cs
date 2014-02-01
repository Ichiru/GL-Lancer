using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GLLancer
{
	public class MainGame : Game
	{
		GraphicsDeviceManager graphics;
		FLRenderer.Main starchart;
		public MainGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			Content.RootDirectory = "Assets";
			starchart = new FLRenderer.Main (MainClass.FLIni);
		}
		protected override void Initialize ()
		{
			starchart.Initialize (GraphicsDevice, Content);
			starchart.DeviceReset ();
			starchart.Camera.Free = true;
			base.Initialize ();
		}
		protected override void LoadContent ()
		{
			starchart.LoadContent ();
			starchart.SystemMap.StarSystem = MainClass.FLIni.Universe.FindSystem ("Li01");
			base.LoadContent ();
		}
		protected override void Update (GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();
			starchart.Update (gameTime.ElapsedGameTime);
			base.Update (gameTime);
		}
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.Black);
			starchart.Draw ();
			base.Draw (gameTime);
		}
	}
}

