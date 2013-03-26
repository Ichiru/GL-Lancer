//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;

namespace GLLancer
{
	public abstract class UtfNode
	{
		public string Name { get; private set; }
		public int PeerOffset { get; private set; }
		protected UtfNode (string name, int peerOffset)
		{
			Name = name;
			PeerOffset = peerOffset;
		}
	}
}

