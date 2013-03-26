//Copyright (C) Ichiru 2013
//See LICENSE for licensing information
using System;
using System.IO;
using System.Text;

namespace GLLancer
{
	public class UtfFile
	{
		const int VERSION = 257;
		public UtfTreeNode Root { get; private set; }


		public UtfFile (string filename)
		{
			byte[] treeBlock;
			string strings;
			byte[] dataBlock;
			using (var reader = new BinaryReader(File.OpenRead (filename))) {
				byte[] magic = reader.ReadBytes (4);
				if (Encoding.ASCII.GetString (magic) != "UTF ")
					throw new Exception ("Not a UTF file");
				if (reader.ReadInt32 () != VERSION)
					throw new Exception ("UTF Version not recognised");
				int treeOffset = reader.ReadInt32 ();
				int treeSize = reader.ReadInt32 ();
				reader.BaseStream.Seek (2 * sizeof(int), SeekOrigin.Current);
				//string table
				int stringsOffset = reader.ReadInt32 ();
				int stringsSize = reader.ReadInt32 ();
				reader.BaseStream.Seek (sizeof(int), SeekOrigin.Current);
				//data table
				int dataOffset = reader.ReadInt32 ();

				//read the tree block
				treeBlock = new byte[treeSize];
				reader.BaseStream.Seek (treeOffset, SeekOrigin.Begin);
				reader.Read (treeBlock, 0, treeSize);

				//read the strings
				byte[] stringsBuffer = new byte[stringsSize];
				reader.BaseStream.Seek (stringsOffset, SeekOrigin.Begin);
				reader.Read (stringsBuffer, 0, stringsSize);
				//encoding.unicode?
				strings = Encoding.ASCII.GetString (stringsBuffer);

				//read the data block
				reader.BaseStream.Seek (dataOffset, SeekOrigin.Begin);
				int dataSize = (int)reader.BaseStream.Length - (int)reader.BaseStream.Position;
				dataBlock = new byte[dataSize];
				reader.Read (dataBlock, 0, dataSize);
			}
			using (var reader = new BinaryReader(new MemoryStream(treeBlock))) {
				Root = (UtfTreeNode)ReadNode (reader,0,strings,dataBlock);
			}
		}
		UtfNode ReadNode (BinaryReader reader, int offset, string strings, byte[] dataBlock)
		{
			reader.BaseStream.Seek (offset, SeekOrigin.Begin);
			int peerOffset = reader.ReadInt32 ();
			string name = GetString (strings, reader.ReadInt32 ());
			int flags = reader.ReadInt32 ();
			if ((flags & 0x10) == 0x10) { //Tree Node
				var node = new UtfTreeNode(name,peerOffset);
				reader.BaseStream.Seek (sizeof(int),SeekOrigin.Current);
				int childOffset = reader.ReadInt32 ();
				int next = childOffset;
				do {
					var child = ReadNode (reader,next,strings,dataBlock);
					node.Children.Add (child);
					next = child.PeerOffset;
				} while(next > 0);
				return node;
			} else if ((flags & 0x80) == 0x80) { //Leaf Node
				var node = new UtfLeafNode(name,peerOffset);
				reader.BaseStream.Seek (sizeof(int),SeekOrigin.Current);
				int dataOffset = reader.ReadInt32 ();
				reader.BaseStream.Seek (sizeof(int),SeekOrigin.Current);
				int size = reader.ReadInt32 ();
				byte[] data = new byte[size];
				if(size != 0)
				{
					Array.Copy (dataBlock,dataOffset,data,0,size);
				}
				node.SetData (data);
				int size2 = reader.ReadInt32 (); //I suppose something should be done with this
				return node;
			}
			return null; //fix compiler issues for now. Definitely replace
		}
		string GetString (string strings, int index)
		{
			int nullindex = strings.IndexOf ('\0', index);
			return strings.Substring (index, nullindex - index);
		}
	}
}

