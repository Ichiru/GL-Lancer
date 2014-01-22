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
		Matrix World, View, Projection;
		public MainGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			Content.RootDirectory = "Assets";

		}
		protected override void Initialize ()
		{
			World = Matrix.Identity;
			View = Matrix.CreateLookAt(new Vector3(3,13,0),Vector3.Zero,Vector3.Up);
			Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 20);
			base.Initialize ();
		}
		protected override void LoadContent ()
		{
			var efx = Content.Load<Effect> ("effects/Planet");
			base.LoadContent ();
		}
		protected override void Update (GameTime gameTime)
		{
			base.Update (gameTime);
		}
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.Black);

			base.Draw (gameTime);
		}
	}
}

