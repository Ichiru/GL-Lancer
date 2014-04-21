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
using System.Linq;

using OpenTK;
using FLCommon;

using RenderTools.Primitives;

using FLParser.Ini;
using FLParser.Utf;

using FLApi.Solar;
using FLApi.Characters;
using FLApi.Utf.Mat;
using FLApi.Utf.Vms;

namespace FLApi.Universe
{
    public class SystemObject : SystemPart, IDrawable
    {
        private GraphicsDevice device;
        private UniverseIni universe;
        private StarSystem system;

        private bool atmosphere = false;
        private VertexBuffer atmosphereVertexBuffer;
        private IndexBuffer atmosphereIndexBuffer;
        private Material atmosphereMaterial;

        public Color? AmbientColor { get; private set; }

        private string archetypeName;
        private Archetype archetype;
        public Archetype Archetype
        {
            get
            {
                if (archetype == null) archetype = FreelancerIni.Solar.FindSolar(archetypeName);
                return archetype;
            }
        }

        public string Star { get; private set; }
        public int? AtmosphereRange { get; private set; }
        public Color? BurnColor { get; private set; }

        private string baseName;
        private Base pBase;
        public Base Base
        {
            get
            {
                if (pBase == null) pBase = universe.FindBase(baseName);
                return pBase;
            }
        }

        public string MsgIdPrefix { get; private set; }
        public string JumpEffect { get; private set; }
        public string Behavior { get; private set; }
        public int? DifficultyLevel { get; private set; }
        public JumpReference Goto { get; private set; }

        private string loadoutName;
        private Loadout loadout;
        public Loadout Loadout
        {
            get
            {
                if (loadout == null) loadout = FreelancerIni.Loadouts.FindLoadout(loadoutName);
                return loadout;
            }
        }

        public string Pilot { get; private set; }

        private string dockWithName;
        private Base dockWith;
        public Base DockWith
        {
            get
            {
                if (dockWith == null) dockWith = universe.FindBase(dockWithName);
                return dockWith;
            }
        }

        public string Voice { get; private set; }
        public Costume SpaceCostume { get; private set; }
        public string Faction { get; private set; }

        public string PrevRing { get; private set; }

        private string nextRingName;
        private SystemObject nextRing;
        public SystemObject NextRing
        {
            get
            {
                if (nextRingName == null) return null;
                if (nextRing == null) nextRing = system.FindObject(nextRingName);
                return nextRing;
            }
        }

        public List<string> TradelaneSpaceName { get; private set; }

        private string parentName;
        private SystemObject parent;
        public SystemObject Parent
        {
            get
            {
                if (parent == null && parentName != null) parent = system.FindObject(parentName);
                return parent;
            }
        }

        public string InfoCardIds { get; private set; }

        public SystemObject(UniverseIni universe, StarSystem system, Section section, FreelancerIni freelancerIni)
            : base(section, freelancerIni)
        {
            if (universe == null) throw new ArgumentNullException("universe");
            if (system == null) throw new ArgumentNullException("system");
            if (section == null) throw new ArgumentNullException("section");

            this.universe = universe;
            this.system = system;
            TradelaneSpaceName = new List<string>();

            foreach (Entry e in section)
            {
                if (!parentEntry(e))
                {
                    switch (e.Name.ToLowerInvariant())
                    {
                        case "ambient_color":
                        case "ambient":
                            if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (AmbientColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            AmbientColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                            break;
                        case "archetype":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (archetypeName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            archetypeName = e[0].ToString();
                            break;
                        case "star":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (Star != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            Star = e[0].ToString();
                            break;
                        case "atmosphere_range":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (AtmosphereRange != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            AtmosphereRange = e[0].ToInt32();
                            break;
                        case "burn_color":
                            if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (BurnColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            BurnColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                            break;
                        case "base":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (baseName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            baseName = e[0].ToString();
                            break;
                        case "msg_id_prefix":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (MsgIdPrefix != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            MsgIdPrefix = e[0].ToString();
                            break;
                        case "jump_effect":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (JumpEffect != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            JumpEffect = e[0].ToString();
                            break;
                        case "behavior":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (Behavior != null && Behavior != e[0].ToString()) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            Behavior = e[0].ToString();
                            break;
                        case "difficulty_level":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (DifficultyLevel != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            DifficultyLevel = e[0].ToInt32();
                            break;
                        case "goto":
                            if (e.Count != 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (Goto != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            Goto = new JumpReference(universe, e[0].ToString(), e[1].ToString(), e[2].ToString());
                            break;
                        case "loadout":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (loadoutName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            loadoutName = e[0].ToString();
                            break;
                        case "pilot":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (Pilot != null && Pilot != e[0].ToString()) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            Pilot = e[0].ToString();
                            break;
                        case "dock_with":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (dockWithName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            dockWithName = e[0].ToString();
                            break;
                        case "voice":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (Voice != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            Voice = e[0].ToString();
                            break;
                        case "space_costume":
                            if (e.Count < 1 || e.Count > 3) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (SpaceCostume != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            SpaceCostume = new Costume(e[0].ToString(), e[1].ToString(), e.Count == 3 ? e[2].ToString() : string.Empty, freelancerIni);
                            break;
                        case "faction":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (Faction != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            Faction = e[0].ToString();
                            break;
                        case "prev_ring":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (PrevRing != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            PrevRing = e[0].ToString();
                            break;
                        case "next_ring":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (nextRingName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            nextRingName = e[0].ToString();
                            break;
                        case "tradelane_space_name":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            TradelaneSpaceName.Add(FreelancerIni.GetStringResource(e[0].ToInt32()));
                            break;
                        case "parent":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (parentName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            parentName = e[0].ToString();
                            break;
                        case "info_card_ids":
                        case "info_card":
                        case "info_ids":
                            if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            if (InfoCardIds != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            InfoCardIds = FreelancerIni.GetStringResource(e[0].ToInt32());
                            break;
                        case "ring":
                            if (e.Count != 2) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                            //if ( != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                            //TODO
                            break;
                        case "260800": // Strange error
                            break;
                        default:
                            throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                    }
                }
            }
        }

        public void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            this.device = device;

            Archetype.Initialize(device, content, camera);

            if (AtmosphereRange != null)
            {
                if (Archetype is Planet)
                {
                    SphFile sphere = (Archetype as Planet).DaArchetype as SphFile;

                    if (sphere.SideMaterials.Length > 6)
                    {
                        Ellipsoid e = new Ellipsoid(device, new Vector3(AtmosphereRange.Value), 48, 64);
                        atmosphereVertexBuffer = e.VertexBuffer;
                        atmosphereIndexBuffer = e.IndexBuffer;

                        atmosphereMaterial = sphere.SideMaterials[6];
                        //atmosphere = true;
                    }
                }
            }
        }

        public void Resized()
        {
            Archetype.Resized();
            if (atmosphere) atmosphereMaterial.Resized();
        }

        public void Update()
        {
            Archetype.Update();
            if (atmosphere) atmosphereMaterial.Update();
        }

        public void Draw(Color ambient, List<LightSource> lights, Matrix world)
        {
            Color ambientColor = /*AmbientColor ??*/ ambient;

            Matrix w = world * Matrix.CreateTranslation(Pos.Value);
            if (Rotate != null) w =
                            Matrix.CreateRotationX(MathConvert.ToRadians(Rotate.Value.X)) *
                            Matrix.CreateRotationY(MathConvert.ToRadians(Rotate.Value.Y)) *
                            Matrix.CreateRotationZ(MathConvert.ToRadians(Rotate.Value.Z)) *
                            w;

            Archetype.Draw(ambientColor, lights, w);

            if (atmosphere)
            {
                device.SetVertexBuffer(atmosphereVertexBuffer);
                device.Indices = atmosphereIndexBuffer;

                atmosphereMaterial.Draw(D3DFVF.XYZ | D3DFVF.TEX1, PrimitiveTypes.TriangleList, 0, atmosphereVertexBuffer.VertexCount, 0, atmosphereIndexBuffer.IndexCount / 3, ambientColor, lights, w);
            }
        }
    }
}
