using System;

namespace GLLancer
{
	[Flags()]
	public enum D3DFVF : ushort
	{
		XYZ = 0x002,
		Normal = 0x010,
		Diffuse = 0x040,
		Tex1 = 0x100,
		Tex2 = 0x200,
	}
}

