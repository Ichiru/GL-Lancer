﻿/* The contents of this file a
 * re subject to the Mozilla Public License
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

using FLCommon;
using OpenTK;

using FLParser;
using FLParser.Utf;

using FLApi.Utf.Vms;
using FLApi.Utf.Anm;
using FLApi.Utf.Mat;
using FLApi.Universe;

namespace FLApi.Utf.Cmp
{
    /// <summary>
    /// Represents a UTF Compound File (.cmp)
    /// </summary>
    public class CmpFile : UtfFile, IDrawable, ILibFile
    {
        private ILibFile additionalLibrary;

        public string Path { get; private set; }

        public VmsFile VMeshLibrary { get; private set; }
        public AnmFile Animation { get; private set; }
        public MatFile MaterialLibrary { get; private set; }
        public TxmFile TextureLibrary { get; private set; }

        public Dictionary<int, Part> Parts { get; private set; }
        public ConstructCollection Constructs { get; private set; }
        public Dictionary<string, ModelFile> Models { get; private set; }

        public CmpFile(string path, ILibFile additionalLibrary)
        {
            this.additionalLibrary = additionalLibrary;

            Path = path;

            Models = new Dictionary<string, ModelFile>();
            Constructs = new ConstructCollection();
            Parts = new Dictionary<int, Part>();

            foreach (Node node in parseFile(path))
            {
                switch (node.Name.ToLowerInvariant())
                {
                    case "exporter version":
                        break;
                    case "vmeshlibrary":
                        IntermediateNode vMeshLibraryNode = node as IntermediateNode;
                        if (VMeshLibrary == null) VMeshLibrary = new VmsFile(vMeshLibraryNode, this);
                        else throw new Exception("Multiple vmeshlibrary nodes in cmp root");
                        break;
                    case "animation":
                        IntermediateNode animationNode = node as IntermediateNode;
                        if (Animation == null) Animation = new AnmFile(animationNode, Constructs);
                        else throw new Exception("Multiple animation nodes in cmp root");
                        break;
                    case "material library":
                        IntermediateNode materialLibraryNode = node as IntermediateNode;
                        if (MaterialLibrary == null) MaterialLibrary = new MatFile(materialLibraryNode, this);
                        else throw new Exception("Multiple material library nodes in cmp root");
                        break;
                    case "texture library":
                        IntermediateNode textureLibraryNode = node as IntermediateNode;
                        if (TextureLibrary == null) TextureLibrary = new TxmFile(textureLibraryNode);
                        else throw new Exception("Multiple texture library nodes in cmp root");
                        break;
                    case "cmpnd":
                        IntermediateNode cmpndNode = node as IntermediateNode;
                        foreach (IntermediateNode cmpndSubNode in cmpndNode)
                        {
                            if (cmpndSubNode.Name.Equals("cons", StringComparison.OrdinalIgnoreCase))
                            {
                                Constructs.AddNode(cmpndSubNode);
                            }
                            else if (
                                cmpndSubNode.Name.StartsWith("part_", StringComparison.OrdinalIgnoreCase) ||
                                cmpndSubNode.Name.Equals("root", StringComparison.OrdinalIgnoreCase)
                            )
                            {
                                string objectName = string.Empty, fileName = string.Empty;
                                int index = -1;

                                foreach (LeafNode partNode in cmpndSubNode)
                                {
                                    switch (partNode.Name.ToLowerInvariant())
                                    {
                                        case "object name":
                                            objectName = partNode.StringData;
                                            break;
                                        case "file name":
                                            fileName = partNode.StringData;
                                            break;
                                        case "index":
                                            index = partNode.Int32Data.Value;
                                            break;
                                        default: throw new Exception("Invalid node in " + cmpndSubNode.Name + ": " + partNode.Name);
                                    }
                                }

                                Parts.Add(index, new Part(objectName, fileName, Models, Constructs));
                            }
                            else throw new Exception("Invalid node in " + cmpndNode.Name + ": " + cmpndSubNode.Name);
                        }
                        break;
                    case "materialanim":
                        //TODO cmp materialanim
                        break;
                    default:
                        if (node.Name.EndsWith(".3db", StringComparison.OrdinalIgnoreCase))
                        {
                            ModelFile m = new ModelFile(node as IntermediateNode, this);
                            Models.Add(node.Name, m);
                        }
                        else throw new Exception("Invalid Node in cmp root: " + node.Name);
                        break;
                }
            }
        }

        public void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            for (int i = 0; i < Parts.Count; i++) Parts[i].Initialize(device, content, camera);
        }

        public void Resized()
        {
            for (int i = 0; i < Parts.Count; i++) Parts[i].Resized();
        }

        public void Update()
        {
            if (Animation != null) Animation.Update();
            for (int i = 0; i < Parts.Count; i++) Parts[i].Update();
        }

        public void Draw(Color ambient, List<LightSource> lights, Matrix world)
        {
            for (int i = 0; i < Parts.Count; i++) Parts[i].Draw(ambient, lights, world);

            /*foreach (ModelFile m in cmp.Models.Values)
            {
                int lightCount = lights.Count;
                foreach (Hardpoint h in m.Hardpoints)
                {
                    Light light = null;

                    if (SpaceObject.Loadout != null && SpaceObject.Loadout.Equip.ContainsKey(h.Name))
                        light = SpaceObject.Loadout.Equip[h.Name] as Light;
                    else if (archetype.Loadout != null && archetype.Loadout.Equip.ContainsKey(h.Name))
                        light = archetype.Loadout.Equip[h.Name] as Light;

                    if (light != null)
                    {
                        lights.Add(new RenderLight(light, Vector3.Transform(h.Position, World)));
                        lightCount++;
                        if (lightCount == MAX_LIGHTS) break;
                    }
                }
            }*/
        }

        public TextureData FindTexture(string name)
        {
            if (TextureLibrary != null)
            {
                TextureData texture = TextureLibrary.FindTexture(name);
                if (texture != null) return texture;
            }
            if (additionalLibrary != null) return additionalLibrary.FindTexture(name);
            return null;

        }

        public Material FindMaterial(uint materialId)
        {
            if (MaterialLibrary != null)
            {
                Material material = MaterialLibrary.FindMaterial(materialId);
                if (material != null) return material;
            }
            if (additionalLibrary != null) return additionalLibrary.FindMaterial(materialId);
            return null;
        }

        public VMeshData FindMesh(uint vMeshLibId)
        {
            if (VMeshLibrary != null)
            {
                VMeshData mesh = VMeshLibrary.FindMesh(vMeshLibId);
                if (mesh != null) return mesh;
            }
            if (additionalLibrary != null) return additionalLibrary.FindMesh(vMeshLibId);
            return null;
        }

        public override string ToString()
        {
            return Path;
        }
    }
}