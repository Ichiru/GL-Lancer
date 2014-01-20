using System;
using System.Collections.Generic;

namespace GLLancer
{
	public class VmsFile : UtfFile
	{
		public Dictionary<uint,VMeshData> Meshes = new Dictionary<uint, VMeshData> ();

		public VmsFile (string path) : base(path)
		{
			foreach (UtfNode node in Root.Children) {
				switch (node.Name.ToLowerInvariant ()) {
				case "vmeshlibrary":
					UtfTreeNode vMeshLibrary = node as UtfTreeNode;
					LoadMeshes (vMeshLibrary);
					break;
				default:
					throw new Exception ("Invalid node in vms root: " + node.Name);
				}
			}
		}

		public VmsFile (UtfTreeNode root) : base(root)
		{
			LoadMeshes (root);
		}

		void LoadMeshes (UtfTreeNode vMeshLibrary)
		{
			foreach (UtfTreeNode vmsNode in vMeshLibrary.Children) {
				if (vmsNode.Children.Count != 1)
					throw new Exception ("Invalid VMesh Node: " + vmsNode.Name);
				UtfLeafNode vMeshDataNode = vmsNode.Children [0] as UtfLeafNode;
				Meshes.Add (FreelancerCRC.ModelCRC (vmsNode.Name), new VMeshData (vMeshDataNode));
			}
		}
	}
}

