using System;
using System.IO;
namespace FontCompiler
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var options = new FontCreationParameters ();
			var parser = new CommandLine.Parser (with => with.HelpWriter = Console.Error);
			if (parser.ParseArgumentsStrict (args, options, () => Environment.Exit (-2))) {
				Run (options);
			}
		}
		static void Run(FontCreationParameters options)
		{
			if (options.Arguments.Count != 2) {
				Console.Error.WriteLine (options.GetUsage ());
				Console.Error.WriteLine ();
				Console.Error.WriteLine ("Error: Bad number of arguments");
				return;
			}
			if (!int.TryParse (options.Arguments [1], out options.Size)) {
				Console.Error.WriteLine (options.GetUsage ());
				Console.Error.WriteLine ();
				Console.Error.WriteLine ("Error: Size is not a valid integer");
				return;
			}

			options.Name = options.Arguments [0];
			if (options.Extended) {
				options.Characters = FontCreationParameters.ExtendedChars;
			} else {
				options.Characters = FontCreationParameters.Chars;
			}
			if (string.IsNullOrEmpty (options.Output)) {
				options.Output = Path.ChangeExtension (options.Arguments [0], ".xnb");
			}
			CompiledFont rendered;
			using (var fnt = new FontRenderer (options)) {
				rendered = fnt.Render ();
			}
			var xnb = new XNBWriter (options);
			xnb.Write (rendered);
			xnb.Close ();
			Console.Error.WriteLine ("Built: {0}",options.Output);
			Environment.Exit (0);
		}
	}
}
