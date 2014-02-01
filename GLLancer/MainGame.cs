using System;
using System.Collections.Generic;
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
		private Dictionary<Keys, bool> keyPressed = new Dictionary<Keys, bool> ();
		MouseState lastMouse = new MouseState();
		bool mouseRotate = false;
		private const float ROTATION_SPEED = 0.4f;
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
			if (Keyboard.GetState ().IsKeyDown (Keys.Escape))
				this.Exit ();
			//Keyboard
			KeyboardState keystate = Keyboard.GetState ();
			MouseState mouse = Mouse.GetState ();
			//Camera
			if (keystate.IsKeyDown (Keys.Home)) {
				if (!keyPressed [Keys.Home]) {
					mouseRotate = !mouseRotate;
					lastMouse = mouse;
				} 
				keyPressed [Keys.Home] = true;
			}
			if (keystate.IsKeyUp (Keys.Home))
				keyPressed [Keys.Home] = false;


			if (keystate.IsKeyDown (Keys.Up) || keystate.IsKeyDown (Keys.W))
				starchart.Camera.MoveVector += Vector3.Forward;
			if (keystate.IsKeyDown (Keys.Down) || keystate.IsKeyDown (Keys.S))
				starchart.Camera.MoveVector += Vector3.Backward;
			if (keystate.IsKeyDown (Keys.Right) || keystate.IsKeyDown (Keys.D))
				starchart.Camera.MoveVector += Vector3.Right;
			if (keystate.IsKeyDown (Keys.Left) || keystate.IsKeyDown (Keys.A))
				starchart.Camera.MoveVector += Vector3.Left;
			if (keystate.IsKeyDown (Keys.PageUp))
				starchart.Camera.MoveVector += Vector3.Up;
			if (keystate.IsKeyDown (Keys.PageDown))
				starchart.Camera.MoveVector += Vector3.Down;

			if (keystate.IsKeyDown (Keys.LeftControl) || keystate.IsKeyDown (Keys.RightControl))
				starchart.Camera.MoveVector *= 5;
			//Camera
			if (mouseRotate) {
				float xDifference = mouse.X - lastMouse.X;
				float yDifference = mouse.Y - lastMouse.Y;

				float amount = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
				starchart.Camera.Rotation = new Vector2 (starchart.Camera.Rotation.X - ROTATION_SPEED * xDifference * amount, starchart.Camera.Rotation.Y - ROTATION_SPEED * yDifference * amount);
				Mouse.SetPosition (lastMouse.X, lastMouse.Y);
			}
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

