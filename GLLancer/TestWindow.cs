using System;
using OpenTK;
using OpenTK.Input;
namespace GLLancer
{
	public class TestWindow : GameWindow
	{
		public TestWindow () : base(640,480)
		{
			KeyPress += HandleKeyPress;
		}

		void HandleKeyPress (object sender, KeyPressEventArgs e)
		{
			Console.Write (e.KeyChar);
		}

	}
}

