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
 * Portions created by the Initial Developer are Copyright (C) 2011, 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using OpenTK;

using FLParser;

namespace FLApi.Utf
{
    public class SphereConstruct : AbstractConstruct
    {
        public Vector3 Offset { get; private set; }
        public float Min1 { get; private set; }
        public float Max1 { get; private set; }
        public float Min2 { get; private set; }
        public float Max2 { get; private set; }
        public float Min3 { get; private set; }
        public float Max3 { get; private set; }

        public override Matrix4 Transform { get { return internalGetTransform(Rotation * Matrix4.CreateTranslation(Origin + Offset)); } }

        public SphereConstruct(BinaryReader reader, ConstructCollection constructs)
            : base(reader, constructs)
        {
            Offset = ConvertData.ToVector3(reader);
            Rotation = ConvertData.ToMatrix43x3(reader);

            Min1 = reader.ReadSingle();
            Max1 = reader.ReadSingle();
            Min2 = reader.ReadSingle();
            Max2 = reader.ReadSingle();
            Min3 = reader.ReadSingle();
            Max3 = reader.ReadSingle();
        }

        public override void Update(float distance)
        {
            throw new NotImplementedException();
        }
    }
}
