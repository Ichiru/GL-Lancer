using System;
using System.IO;
using System.Runtime.InteropServices;
namespace FLParser
{
	public static class BinaryReaderExtensions
	{
		public static T ReadStruct<T>(this BinaryReader reader) where T: struct
		{
			// Read in a byte array
			byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

			// Pin the managed memory while, copy it out the data, then unpin it
			GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();

			return theStructure;
		}
	}
}

