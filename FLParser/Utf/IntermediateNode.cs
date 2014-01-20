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
using System.Text;
using System.Linq;

namespace FLParser.Utf
{
    public class IntermediateNode : Node, IList<Node>
    {
        private List<Node> children;

        public IntermediateNode(int peerOffset, string name, BinaryReader reader, string stringBlock, byte[] dataBlock)
            : base(peerOffset, name)
        {
            if (reader == null) throw new ArgumentNullException("reader");

            //int zero = reader.ReadInt32();
            reader.BaseStream.Seek(ByteLen.Int32, SeekOrigin.Current);

            children = new List<Node>();

            int childOffset = reader.ReadInt32();
            if (childOffset > 0)
            {
                int next = childOffset;
                do
                {
                    Node n = Node.FromStream(reader, next, stringBlock, dataBlock);
                    children.Add(n);
                    next = n.PeerOffset;
                } while (next > 0);
            }
            //else
            //throw new FileContentsException(UtfFile.FILE_TYPE, "IntermediateNode " + Name + " doesn't have any child nodes.");

            //int allocatedSize = reader.ReadInt32();
            //int size = reader.ReadInt32();
            //int size2 = reader.ReadInt32();
            //int timestamp1 = reader.ReadInt32();
            //int timestamp2 = reader.ReadInt32();
            //int timestamp3 = reader.ReadInt32();
        }

        public int IndexOf(Node item)
        {
            return children.IndexOf(item);
        }

        public void Insert(int index, Node item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public Node this[int index]
        {
            get
            {
                return children[index];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public Node this[string name]
        {
            get
            {
                IEnumerable<Node> candidates = from Node n in children where n.Name.Equals(name, StringComparison.OrdinalIgnoreCase) select n;
                int count = candidates.Count<Node>();
                /*if (count == 1)
                    return candidates.First<Node>();
                else if (count == 0)
                    return null;
                else
                    throw new FileContentsException(UtfFile.FILE_TYPE, count + " Peer nodes with the name " + name);*/
                if (count == 0) return null;
                else return candidates.First<Node>();
            }
        }

        public void Add(Node item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(Node item)
        {
            return children.Contains(item);
        }

        public bool Contains(string name)
        {
            return children.Count<Node>(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == 1;
        }

        public void CopyTo(Node[] array, int arrayIndex)
        {
            children.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return children.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(Node item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        public override string ToString()
        {
            string result = "{Inter: " + base.ToString() + "{";
            if (children != null) foreach (Node n in children) result += n + ", ";
            return result + "}";
        }
    }
}