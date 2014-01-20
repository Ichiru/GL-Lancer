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
 * Data structure from Freelancer UTF Editor by Cannon & Adoxa, continuing the work of Colin Sanby and Mario 'HCl' Brito (http://the-starport.net)
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2011
 * the Initial Developer. All Rights Reserved.
 */

using System;

namespace FLApi.Utf.Vms
{
    [Flags()]
    public enum D3DFVF : uint
    {
        //RESERVED0 = 0x0001,

        /// <summary>
        /// Vertex format includes the position of an untransformed vertex. This flag cannot be used with the D3DFVF_XYZRHW flag.
        /// </summary>
        XYZ = 0x0002,

        //XYZRHW = 0x0004,
        //XYZB1 = 0x0006,
        //XYZB2 = 0x0008,
        //XYZB3 = 0x000a,
        //XYZB4 = 0x000c,
        //XYZB5 = 0x000e,

        /// <summary>
        /// Vertex format includes a vertex normal vector. This flag cannot be used with the D3DFVF_XYZRHW flag.
        /// </summary>
        NORMAL = 0x0010,

        //RESERVED1 = 0x0020,

        /// <summary>
        /// Vertex format includes a diffuse color component.
        /// </summary>
        DIFFUSE = 0x0040,

        //SPECULAR = 0x0080,

        //TEXCOUNT_MASK = 0x0f00,
        //TEX0 = 0x0000,

        /// <summary>
        /// Number of texture coordinate sets for this vertex. The actual values for these flags are not sequential.
        /// </summary>
        TEX1 = 0x0100,

        /// <summary>
        /// Number of texture coordinate sets for this vertex. The actual values for these flags are not sequential.
        /// </summary>
        TEX2 = 0x0200,
        
        //TEX3 = 0x0300,
        
        /// <summary>
        /// Number of texture coordinate sets for this vertex. The actual values for these flags are not sequential.
        /// </summary>
        TEX4 = 0x0400,
        
        //TEX5 = 0x0500,
        //TEX6 = 0x0600,
        //TEX7 = 0x0700,
        //TEX8 = 0x0800
    }
}