using System;

namespace GLLancer
{
	public class CmpFile : UtfFile
	{
		public VmsFile VMeshLibrary;

		public CmpFile (string path) : base (path)
		{
			LoadCmp ();
		}

		public CmpFile (UtfTreeNode root) : base(root)
		{
			LoadCmp ();
		}

		void LoadCmp ()
		{
			foreach (UtfNode node in Root.Children) {
				switch (node.Name.ToLowerInvariant ()) {
				case "exporter version":
					break;
				case "vmeshlibrary":
					UtfTreeNode vMeshLibraryNode = node as UtfTreeNode;
					if (VMeshLibrary == null)
						VMeshLibrary = new VmsFile (vMeshLibraryNode);
					else
						throw new Exception ("Multiple vmeshlibrary nodes in cmp root");
					break;
				case "animation":
					//TODO animation
					break;
				case "material library":
					//TODO material
					break;
				case "texture library":
					//TODO texturing
					break;
				case "cmpnd":
					//TODO cmpnd
					break;
				case "materialanim":
					//TODO cmp materialanim
					break;
				default:
					if (node.Name.EndsWith (".3db", StringComparison.OrdinalIgnoreCase)) {
						//TODO .3db
					} else
						throw new Exception ("Invalid Node in cmp root: " + node.Name);
					break;
				}
			}
		}
	}
}

