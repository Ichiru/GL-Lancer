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
 * The Original Code is FLApi code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

using FLCommon;
using OpenTK;

using FLParser;
using FLParser.Utf;

using FLApi.Universe;
using FLApi.Utf.Anm;

namespace FLApi.Utf
{
    public abstract class AbstractConstruct
    {
        private ConstructCollection constructs;

        public string ParentName { get; private set; }
        public string ChildName { get; private set; }
        public Vector3 Origin { get; set; }
        public Matrix Rotation { get; set; }

        public abstract Matrix Transform { get; }

        protected AbstractConstruct(BinaryReader reader, ConstructCollection constructs)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (constructs == null) throw new ArgumentNullException("construct");

            this.constructs = constructs;

            byte[] buffer = new byte[ByteLen.ConsString];

            reader.Read(buffer, 0, ByteLen.ConsString);
            ParentName = Encoding.ASCII.GetString(buffer);
            ParentName = ParentName.Substring(0, ParentName.IndexOf('\0'));

            reader.Read(buffer, 0, ByteLen.ConsString);
            ChildName = Encoding.ASCII.GetString(buffer);
            ChildName = ChildName.Substring(0, ChildName.IndexOf('\0'));

            Origin = ConvertData.ToVector3(reader);
        }

        protected Matrix internalGetTransform(Matrix matrix)
        {
            AbstractConstruct parent = constructs.Find(ParentName);
            if (parent != null) matrix *= parent.Transform;

            return matrix;
        }

        public abstract void Update(float distance);
    }
}
