using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace GLSLProcessor
{
	public class ShaderOutput
	{
		const uint MAGIC = 0xAFECDE03;
		public const byte SOURCE_VERTEX = 0;
		public const byte SOURCE_FRAGMENT = 1;
		public string[] Sources;
		public byte[] SourceTypes;
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
			string[] md5s = new string[Sources.Length];
			for (int i = 0; i < md5s.Length; i++) {
				md5s [i] = CalculateMD5Hash (Sources [i]);
			}
			using (var writer = new BinaryWriter(File.Create(filename))) {
				writer.Write (MAGIC);
				writer.Write ((ushort)Sources.Length);
				for (int i = 0; i < Sources.Length; i++) {
					writer.Write (SourceTypes [i]);
					writer.Write (Sources [i]);
					writer.Write (md5s [i]);
				}
				writer.Write ((ushort)Programs.Length);
				for (int i = 0; i < Programs.Length; i++) {
					writer.Write (Programs [i].Name);
					writer.Write ((ushort)Programs [i].VSIndex);
					writer.Write ((ushort)Programs [i].FSIndex);
				}
			}
		}
		static string CalculateMD5Hash(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder(32);
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}
	}
}

