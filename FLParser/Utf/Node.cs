/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Original Code is Starchart code (http://flapi.sourceforge.net/).
 * Data structure from Freelancer UTF Editor by Cannon & Adoxa, continuing the work of Colin Sanby and Mario 'HCl' Brito (http://the-starport.net)
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace FLParser.Utf
{
    public abstract class Node
    {
        public int PeerOffset { get; private set; }
        public string Name { get; private set; }

        protected Node(int peerOffset, string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            this.PeerOffset = peerOffset;
            this.Name = name;
        }

        public static Node FromStream(BinaryReader reader, int offset, string stringBlock, byte[] dataBlock)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (stringBlock == null) throw new ArgumentNullException("stringBlock");
            if (dataBlock == null) throw new ArgumentNullException("dataBlock");

            reader.BaseStream.Seek(offset, SeekOrigin.Begin);

            int peerOffset = reader.ReadInt32();
            int nameOffset = reader.ReadInt32();
            string name = stringBlock.Substring(nameOffset, stringBlock.IndexOf('\0', nameOffset) - nameOffset);

            NodeFlags flags = (NodeFlags)reader.ReadInt32();
            if ((flags & NodeFlags.Intermediate) == NodeFlags.Intermediate)
                return new IntermediateNode(peerOffset, name, reader, stringBlock, dataBlock);
            else if ((flags & NodeFlags.Leaf) == NodeFlags.Leaf)
                return new LeafNode(peerOffset, name, reader, dataBlock);
            else
                throw new FileContentException(UtfFile.FILE_TYPE, "Neither required flag set. Flags: " + flags);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}