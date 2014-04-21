using System;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using FLCommon;
namespace GLLancer
{
	class MainWindow : GameWindow
	{
		FLRenderer.Main starchart;
		GraphicsDevice device;
		ContentManager content;
		public MainWindow()
			: base(800, 600, GraphicsMode.Default, "GL-Lancer")
		{
			VSync = VSyncMode.On;
		}

		/// <summary>Load resources here.</summary>
		/// <param name="e">Not used.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			device = new GraphicsDevice ();
			device.Viewport = new Viewport (ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			content = new ContentManager (device);
			content.RootDirectory = "Assets";
			starchart = new FLRenderer.Main (MainClass.FLIni);
			starchart.Initialize (device, content);
			starchart.DeviceReset ();
			starchart.LoadContent ();
			starchart.Camera.Free = true;
			starchart.SystemMap.StarSystem = MainClass.FLIni.Universe.FindSystem ("Li01");
			//GL.PolygonMode (MaterialFace.FrontAndBack, PolygonMode.Line);
		}

		/// <summary>
		/// Called when your window is resized. Set your viewport here. It is also
		/// a good place to set up your projection matrix (which probably changes
		/// along when the aspect ratio of your window).
		/// </summary>
		/// <param name="e">Not used.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);


			
		}
		private const float ROTATION_SPEED = 3f;
	
		/// <summary>
		/// Called when it is time to setup the next frame. Add you game logic here.
		/// </summary>
		/// <param name="e">Contains timing information for framerate independent logic.</param>
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);
			starchart.Update (TimeSpan.FromSeconds (e.Time));
			if (Keyboard[Key.Escape])
				Exit();

			if (Keyboard[Key.Up] || Keyboard[Key.W])
				starchart.Camera.MoveVector += VectorMath.Forward;
				if (Keyboard[Key.Down] || Keyboard[Key.S])
				starchart.Camera.MoveVector += VectorMath.Backward;
					if (Keyboard[Key.Right] || Keyboard[Key.D])
				starchart.Camera.MoveVector += VectorMath.Right;
						if (Keyboard[Key.Left] || Keyboard[Key.A])
				starchart.Camera.MoveVector += VectorMath.Left;
							if (Keyboard[Key.PageUp])
				starchart.Camera.MoveVector += VectorMath.Up;
								if (Keyboard[Key.PageDown])
				starchart.Camera.MoveVector += VectorMath.Down;

			if (Keyboard[Key.ControlLeft] || Keyboard[Key.ControlRight])
				starchart.Camera.MoveVector *= 5;
			float xDifference = (ClientRectangle.Width / 2) - Mouse.X;
			float yDifference = (ClientRectangle.Height / 2) - Mouse.Y;


			float amount = (float)e.Time / 100.0f;
				starchart.Camera.Rotation = new Vector2 (starchart.Camera.Rotation.X - ROTATION_SPEED * xDifference * amount, starchart.Camera.Rotation.Y - ROTATION_SPEED * yDifference * amount);
			OpenTK.Input.Mouse.SetPosition (ClientRectangle.X + (ClientRectangle.Width / 2),
				ClientRectangle.Y + (ClientRectangle.Height / 2));

		}

		/// <summary>
		/// Called when it is time to render the next frame. Add your rendering code here.
		/// </summary>
		/// <param name="e">Contains timing information.</param>
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			starchart.Draw ();


			SwapBuffers();
		}
	}
}