using System;
using System.IO;
namespace FLCommon
{
	public class ContentManager
	{
		public string RootDirectory;
		GraphicsDevice device;
		public ContentManager(GraphicsDevice device)
		{
			this.device = device;
		}
		public T Load<T> (string filename)
		{
			var path = Path.Combine (RootDirectory, filename);
			if (typeof(T) == typeof(EffectInstance)) {
				if (!File.Exists (path + ".effect")) {
					throw new FileNotFoundException (filename);
				}
				//Console.WriteLine (Path.GetFileName (path));
				return (T)(object)new EffectInstance (device, path + ".effect");
			} else {
				throw new NotImplementedException ();
			}
		}
	}
}

