﻿/* The contents of this file are subject to the Mozilla Public License
 * Version 1.1 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 * 
 * Software distributed under the License is distributed on an "AS IS"
 * basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
 * License for the specific language governing rights and limitations
 * under the License.
 * 
 * The Original Code is FlLApi code (http://flapi.sourceforge.net/).
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Color = Microsoft.Xna.Framework.Color;

namespace FLParser.Utf
{
    public class LeafNode : Node
    {
        private byte[] data;

        public bool PossibleCompression { get; private set; }

        public byte[] ByteArrayData { get { return data; } }

        public int? Int32Data
        {
            get
            {
                if (data.Length == ByteLen.Int32) return BitConverter.ToInt32(data, 0);
                else return null;
            }
        }

        public int[] Int32ArrayData
        {
            get
            {
                List<int> result = new List<int>();

                using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
                {
                    while (reader.BaseStream.Position <= reader.BaseStream.Length - ByteLen.Int32)
                    {
                        result.Add(reader.ReadInt32());
                    }
                }

                return result.ToArray();
            }
        }

        public ushort? UInt16Data
        {
            get
            {
                if (data.Length == ByteLen.UInt16) return BitConverter.ToUInt16(data, 0);
                else return null;
            }
        }

        public ushort[] UInt16ArrayData
        {
            get
            {
                List<ushort> result = new List<ushort>();

                using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
                {
                    while (reader.BaseStream.Position <= reader.BaseStream.Length - ByteLen.UInt16)
                    {
                        result.Add(reader.ReadUInt16());
                    }
                }

                return result.ToArray();
            }
        }

        public float? SingleData
        {
            get
            {
                if (data.Length == ByteLen.Single) return BitConverter.ToSingle(data, 0);
                else return null;
            }
        }

        public float[] SingleArrayData
        {
            get
            {
                List<float> result = new List<float>();

                using (BinaryReader reader = new BinaryReader(new MemoryStream(data)))
                {
                    while (reader.BaseStream.Position <= reader.BaseStream.Length - ByteLen.Single)
                    {
                        result.Add(reader.ReadSingle());
                    }
                }

                return result.ToArray();
            }
        }

        public string StringData
        {
            get
            {
                return Encoding.ASCII.GetString(data).TrimEnd('\0');
            }
        }

        public Vector2? Vector2Data
        {
            get
            {
                if (data.Length == ByteLen.Single * 2) return ConvertData.ToVector2(data);
                else return null;
            }
        }

        public Vector2[] Vector2ArrayData
        {
            get
            {
                return ConvertData.ToVector2Array(data);
            }
        }

        public Vector3? Vector3Data
        {
            get
            {
                if (data.Length == ByteLen.Single * 3) return ConvertData.ToVector3(data);
                else return null;
            }
        }

        public Vector3[] Vector3ArrayData
        {
            get
            {
                return ConvertData.ToVector3Array(data);
            }
        }

        public Matrix? MatrixData3x3
        {
            get
            {
                if (data.Length == ByteLen.Single * 9) return ConvertData.ToMatrix3x3(data);
                else return null;
            }
        }

        public Matrix? MatrixData4x3
        {
            get
            {
                if (data.Length == ByteLen.Single * 12) return ConvertData.ToMatrix4x3(data);
                else return null;
            }
        }

        public Color? ColorData
        {
            get
            {
                if (data.Length == ByteLen.Single * 3) return ConvertData.ToColor(data);
                else return null;
            }
        }

        public LeafNode(int peerOffset, string name, BinaryReader reader, byte[] dataBlock)
            : base(peerOffset, name)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (dataBlock == null) throw new ArgumentNullException("dataBlock");

            //int zero = reader.ReadInt32();
            reader.BaseStream.Seek(ByteLen.Int32, SeekOrigin.Current);

            int dataOffset = reader.ReadInt32();

            //int allocatedSize = reader.ReadInt32();
            reader.BaseStream.Seek(ByteLen.Int32, SeekOrigin.Current);

            int size = reader.ReadInt32();
            if (size == 0) data = new byte[0];
            else
            {
                data = new byte[size];
                Array.Copy(dataBlock, dataOffset, data, 0, size);
            }

            int size2 = reader.ReadInt32();
            PossibleCompression = size != size2;

            //int timestamp1 = reader.ReadInt32();
            //int timestamp2 = reader.ReadInt32();
            //int timestamp3 = reader.ReadInt32();
        }

        public override string ToString()
        {
            return "{Leaf: " + base.ToString() + ", Data: " + data.Length + "B}";
        }
    }
}
