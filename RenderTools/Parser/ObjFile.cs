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
 * The Original Code is RenderTools code (http://flapi.sourceforge.net/).
 * 
 * The Initial Developer of the Original Code is Malte Rupprecht (mailto:rupprema@googlemail.com).
 * Portions created by the Initial Developer are Copyright (C) 2012
 * the Initial Developer. All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using System.IO;

using OpenTK;
using FLCommon;

namespace RenderTools.Parser
{
    public class ObjFile
    {
        private List<Vector3> positions;
        private List<Vector2> textureCoordinates;
        private List<Vector3> normals;
        private List<ObjGroup> groups;

        public ObjFile()
        {
            positions = new List<Vector3>();
            textureCoordinates = new List<Vector2>();
            normals = new List<Vector3>();
            groups = new List<ObjGroup>();
        }

        public void AddGroup(string name, ObjMaterial material, VertexPositionNormalTexture[] vertices, ushort[] indices)
        {
            int posOffset = positions.Count;
            int texOffset = textureCoordinates.Count;
            int norOffset = normals.Count;

            ObjVertex[] objVertices = new ObjVertex[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                objVertices[i] = new ObjVertex(i + posOffset, i + texOffset, i + norOffset);
                positions.Add(vertices[i].Position);
                textureCoordinates.Add(vertices[i].TextureCoordinate);
                normals.Add(vertices[i].Normal);
            }

            ObjFace[] faces = new ObjFace[indices.Length / 3];
            for (int i = 0; i < indices.Length; i += 3)
            {
                faces[i] = new ObjFace(new ObjVertex[] { objVertices[indices[i]], objVertices[indices[i + 1]], objVertices[indices[i + 2]] });
            }

            groups.Add(new ObjGroup(name, material, faces));
        }

        public void Save(string path)
        {
            string filename = Path.GetFileNameWithoutExtension(path);
            path = Path.GetDirectoryName(path);
            string objResult =
                "# Wavefront OBJ exported by RenderTools" + Environment.NewLine +
                Environment.NewLine +
                "mtllib " + filename + ".mtl" + Environment.NewLine +
                Environment.NewLine;
            for (int i = 0; i < positions.Count; i++)
            {
                objResult +=
                    "v " + positions[i].X + " " + positions[i].Y + " " + positions[i].Z + Environment.NewLine;
            }
            objResult +=
                "# " + positions.Count + " vertices" + Environment.NewLine +
                Environment.NewLine;
            for (int i = 0; i < textureCoordinates.Count; i++)
            {
                objResult +=
                    "vt " + textureCoordinates[i].X + " " + textureCoordinates[i].Y + Environment.NewLine;
            }
            objResult +=
                "# " + textureCoordinates.Count + " texture coordinates" + Environment.NewLine +
                Environment.NewLine;
            for (int i = 0; i < normals.Count; i++)
            {
                objResult +=
                    "vn " + normals[i].X + " " + normals[i].Y + " " + normals[i].Z + Environment.NewLine;
            }
            objResult +=
                "# " + normals.Count + " normals" + Environment.NewLine +
                Environment.NewLine;
            for (int i = 0; i < groups.Count; i++)
            {
                objResult +=
                    groups[i].ToString() + Environment.NewLine;
            }
            File.WriteAllText(path+filename+".obj", objResult);

            string mtlResult =
                "# Wavefront MTL exported by RenderTools" + Environment.NewLine +
                "# " + filename + Environment.NewLine +
                Environment.NewLine;
            foreach (ObjGroup g in groups)
            {
                mtlResult +=
                    g.Material.ToString() + Environment.NewLine;
            }
            File.WriteAllText(path + filename + ".mtl", mtlResult);
        }
    }
}
