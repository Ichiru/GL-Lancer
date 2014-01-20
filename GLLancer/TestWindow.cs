using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
namespace GLLancer
{
	public class TestWindow : GameWindow
	{
		public TestWindow () : base(640,480)
		{

		}

		protected override void OnLoad (EventArgs e)
		{
			GL.ClearColor (Color4.Black);
			base.OnLoad (e);
		}
		protected override void OnRenderFrame (FrameEventArgs e)
		{
			GL.Clear (ClearBufferMask.ColorBufferBit);
			SwapBuffers ();
			base.OnRenderFrame (e);
		}

	}
}

