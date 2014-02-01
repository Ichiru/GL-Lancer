using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
namespace FontCompiler
{
	public class FontCreationParameters
	{
		public const string ExtendedChars = 
			" !\"#$%&'()*+,-./0123456789:;<=>?" +
				"@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~" +
				"€ŠŒŽžŸ¿¡¢£§ÀÁÂÃÄÅÆÇÈÉÊËÌÍÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïñòóô" +
				"õùúûüýþÿ";
		public const string Chars = 
			" !\"#$%&'()*+,-./0123456789:;<=>?" +
		    "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		public string Name;
		public int Size;
		public string Characters;
		[Option('b',"bold", HelpText = "Generates a bold font")]
		public bool Bold { get; set; }
		[Option('t',"ttf",HelpText = "Loads from a TTF file")]
		public bool TTF { get; set; }
		[Option('e',"extended", HelpText = "Enables an extended character set")]
		public bool Extended { get; set; }
		[Option('o',"output", MetaValue="FILE", HelpText = "Output to the specified file")]
		public string Output { get; set; }
		[ValueList(typeof(List<string>))]
		public IList<string> Arguments { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var options =  HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
			return options + "Usage: FontCompiler.exe [options] font size";
		}
	}
}

