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
 * The Original Code is Starchart code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using OpenTK;
using FLCommon;
namespace FLParser
{
    public static class ConvertData
    {
        public static Vector2 ToVector2(byte[] data)
        {
            Vector2 result = Vector2.Zero;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToVector2(reader);
            }

            return result;
        }

        public static Vector2 ToVector2(BinaryReader reader)
        {
            Vector2 result = Vector2.Zero;

            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();

            return result;
        }

        public static Vector2[] ToVector2Array(byte[] data)
        {
            Vector2[] result;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToVector2Array(reader);
            }

            return result;
        }

        public static Vector2[] ToVector2Array(BinaryReader reader)
        {
            List<Vector2> result = new List<Vector2>();

            while (reader.BaseStream.Position <= reader.BaseStream.Length - ByteLen.Single * 2)
            {
                result.Add(new Vector2(reader.ReadSingle(), -reader.ReadSingle()));
            }

            return result.ToArray();
        }

        public static Vector3 ToVector3(byte[] data)
        {
            Vector3 result = Vector3.Zero;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToVector3(reader);
            }

            return result;
        }

        public static Vector3 ToVector3(BinaryReader reader)
        {
            Vector3 result = Vector3.Zero;

            result.X = reader.ReadSingle();
            result.Y = reader.ReadSingle();
            result.Z = reader.ReadSingle();

            return result;
        }

        public static Vector3[] ToVector3Array(byte[] data)
        {
            Vector3[] result;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToVector3Array(reader);
            }

            return result;
        }

        public static Vector3[] ToVector3Array(BinaryReader reader)
        {
            List<Vector3> result = new List<Vector3>();

            while (reader.BaseStream.Position <= reader.BaseStream.Length - ByteLen.Single * 3)
            {
                result.Add(new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
            }

            return result.ToArray();
        }

        public static Matrix4 ToMatrix3x3(byte[] data)
        {
            Matrix4 result = Matrix4.Identity;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToMatrix3x3(reader);
            }

            return result;
        }

        public static Matrix4 ToMatrix3x3(BinaryReader reader)
        {
            Matrix4 result = Matrix4.Identity;

            result.M11 = reader.ReadSingle();
            result.M21 = reader.ReadSingle();
            result.M31 = reader.ReadSingle();
            result.M41 = 0;
            result.M12 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();
            result.M32 = reader.ReadSingle();
            result.M42 = 0;
            result.M13 = reader.ReadSingle();
            result.M23 = reader.ReadSingle();
            result.M33 = reader.ReadSingle();
            result.M43 = 0;
            result.M14 = 0;
            result.M24 = 0;
            result.M34 = 0;
            result.M44 = 1;

            return result;
        }

        public static Matrix4 ToMatrix4x3(byte[] data)
        {
            Matrix4 result = Matrix4.Identity;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToMatrix4x3(reader);
            }

            return result;
        }

        public static Matrix4 ToMatrix4x3(BinaryReader reader)
        {
            Matrix4 result = Matrix4.Identity;

            result.M11 = reader.ReadSingle();
            result.M21 = reader.ReadSingle();
            result.M31 = reader.ReadSingle();
            result.M41 = 0;
            result.M12 = reader.ReadSingle();
            result.M22 = reader.ReadSingle();
            result.M32 = reader.ReadSingle();
            result.M42 = 0;
            result.M13 = reader.ReadSingle();
            result.M23 = reader.ReadSingle();
            result.M33 = reader.ReadSingle();
            result.M43 = 0;
            result.M14 = reader.ReadSingle();
            result.M24 = reader.ReadSingle();
            result.M34 = reader.ReadSingle();
            result.M44 = 1;

            return result;
        }

        public static Color ToColor(byte[] data)
        {
            Color result = Color.White;

            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryReader reader = new BinaryReader(stream);
                result = ToColor(reader);
            }

            return result;
        }

        public static Color ToColor(BinaryReader reader)
        {
            float r = reader.ReadSingle();
            //r = r < 0 ? 0 : r > 1 ? 1 : r;

            float g = reader.ReadSingle();
            //g = g < 0 ? 0 : g > 1 ? 1 : g;

            float b = reader.ReadSingle();
            //b = b < 0 ? 0 : b > 1 ? 1 : b;

            return new Color(r, g, b);
        }
    }
}