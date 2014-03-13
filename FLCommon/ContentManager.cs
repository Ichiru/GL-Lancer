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
			if (typeof(T) == typeof(Effect)) {
				if (!File.Exists (path + ".effect")) {
					throw new FileNotFoundException (filename);
				}
				return (T)(object)new Effect (device, path + ".effect");
			} else {
				throw new NotImplementedException ();
			}
		}
	}
}

