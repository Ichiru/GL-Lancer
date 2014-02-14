using System;

namespace FLCommon
{
	public abstract class Texture : IDisposable
	{
		internal int ID;
		public SurfaceFormat Format { get; protected set; }
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
		internal static int CalculateMipLevels(int width, int height = 0, int depth = 0)
		{
			int levels = 1;
			int size = Math.Max(Math.Max(width, height), depth);
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

