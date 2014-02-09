using System;
using System.IO;
namespace GLSLProcessor
{
	public class ShaderOutput
	{
		public const uint MAGIC = 0xAFECDE03;
		public string[] Sources;
		public Program[] Programs;
		public class Program
		{
			public string Name;
			public int VSIndex;
			public int FSIndex;
		}
		public ShaderOutput ()
		{
		}
		public void Save(string filename)
		{
			using (var writer = new BinaryWriter(File.Create(filename))) {
				writer.Write (MAGIC);
				writer.Write (Sources.Length);
				for (int i = 0; i < Sources.Length; i++) {
					writer.Write (Sources [i]);
				}
				writer.Write (Programs.Length);
				for (int i = 0; i < Programs.Length; i++) {
					writer.Write (Programs [i].Name);
					writer.Write (Programs [i].VSIndex);
					writer.Write (Programs [i].FSIndex);
				}
			}
		}
	}
}

