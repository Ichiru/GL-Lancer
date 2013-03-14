//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;
using System.Collections.Generic;
namespace GLLancer
{
	public class UtfTreeNode : UtfNode
	{
		public List<UtfNode> Children { get; private set; }
		public UtfTreeNode ()
		{
			Children = new List<UtfNode>();
		}
	}
}

