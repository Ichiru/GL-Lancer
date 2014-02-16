using System;

namespace FLCommon
{
	public abstract class Texture : IDisposable
	{
		internal int ID;
		public SurfaceFormat Format { get; protected set; }
		public GraphicsDevice GraphicsDevice { get; protected set; }
		static bool compressedChecked = false;
		protected static void CheckCompressed()
		{
			if (!compressedChecked) {
				if (!GLExtensions.ExtensionList.Contains ("GL_ARB_texture_compression") ||
				    !GLExtensions.ExtensionList.Contains ("GL_EXT_texture_compression_s3tc")) {
					throw new Exception ("Texture Compression isn't supported by your driver");
				}
				compressedChecked = true;
			}
		}
		public int LevelCount {
			get;
			protected set;
		}
		bool isDisposed = false;
		public bool IsDisposed {
			get {
				return isDisposed;
			}
		}
		internal abstract void Bind ();
		internal static int CalculateMipLevels(int width, int height = 0)
		{
			int levels = 1;
			int size = Math.Max (width, height);
			while (size > 1)
			{
				size = size / 2;
				levels++;
			}
			return levels;
		}
		public virtual void Dispose()
		{
			isDisposed = true;
		}
	}
}

