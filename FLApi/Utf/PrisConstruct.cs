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
 * Portions created by the Initial Developer are Copyright (C) 2012
 * the Initial Developer. All Rights Reserved.
 */

using System.Collections.Generic;
using System.IO;

using OpenTK;

using FLParser;

namespace FLApi.Utf
{
    public class PrisConstruct : AbstractConstruct
    {
        public Vector3 Offset { get; set; }
        public Vector3 AxisTranslation { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }

        private Matrix4 currentTransform = Matrix4.Identity;

        public override Matrix4 Transform { get { return internalGetTransform(currentTransform * Rotation * Matrix4.CreateTranslation(Origin + Offset)); } }

        public PrisConstruct(BinaryReader reader, ConstructCollection constructs)
            : base(reader, constructs)
        {
            Offset = ConvertData.ToVector3(reader);
            Rotation = ConvertData.ToMatrix3x3(reader);
            AxisTranslation = ConvertData.ToVector3(reader);

            Min = reader.ReadSingle();
            Max = reader.ReadSingle();
        }

        public override void Update(float distance)
        {
            Vector3 currentRotation = AxisTranslation * distance;
            currentTransform = Matrix4.CreateTranslation(currentRotation);
        }
    }
}
