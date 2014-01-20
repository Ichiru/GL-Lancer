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

using Microsoft.Xna.Framework;

using FLParser.Ini;

namespace FLApi.Universe
{
    public class Nebula : ZoneReference
    {
        public int? FogEnabled { get; private set; }
        public int? FogNear { get; private set; }
        public int? FogDistance { get; private set; }
        public Color? FogColor { get; private set; }

        public List<string> ExteriorShape { get; private set; }
        public List<int> ExteriorShapeWeights { get; private set; }
        public string ExteriorFillShape { get; private set; }
        public int? ExteriorPlaneSlices { get; private set; }
        public int? ExteriorBitRadius { get; private set; }
        public float? ExteriorBitRadiusRandomVariation { get; private set; }
        public int? ExteriorMinBits { get; private set; }
        public int? ExteriorMaxBits { get; private set; }
        public float? ExteriorMoveBitPercent { get; private set; }
        public float? ExteriorEquatorBias { get; private set; }
        public Color? ExteriorColor { get; private set; }

        public List<NebulaLight> NebulaLights { get; private set; }

        public int? CloudsMaxDistance { get; private set; }
        public int? CloudsPuffCount { get; private set; }
        public int? CloudsPuffRadius { get; private set; }
        public Color? CloudsPuffColorA { get; private set; }
        public Color? CloudsPuffColorB { get; private set; }
        public float? CloudsPuffMaxAlpha { get; private set; }
        public List<string> CloudsPuffShape { get; private set; }
        public List<int> CloudsPuffWeights { get; private set; }
        public float? CloudsPuffDrift { get; private set; }
        public Vector2? CloudsNearFadeDistance { get; private set; }
        public float? CloudsLightningIntensity { get; private set; }
        public Color? CloudsLightningColor { get; private set; }
        public float? CloudsLightningGap { get; private set; }
        public float? CloudsLightningDuration { get; private set; }

        public float? BackgroundLightningDuration { get; private set; }
        public float? BackgroundLightningGap { get; private set; }
        public Color? BackgroundLightningColor { get; private set; }

        public float? DynamicLightningGap { get; private set; }
        public float? DynamicLightningDuration { get; private set; }
        public Color? DynamicLightningColor { get; private set; }
        public float? DynamicLightningAmbientIntensity { get; private set; }
        public int? DynamicLightningIntensityIncrease { get; private set; }

        public Nebula(StarSystem parent, Section section, FreelancerIni data)
            : base(parent, section, data)
        {
            ExteriorShape = new List<string>();
            NebulaLights = new List<NebulaLight>();
            CloudsPuffShape = new List<string>();

            foreach (Section s in parseFile(FreelancerIni.DataPath + file))
            {
                switch (s.Name.ToLowerInvariant())
                {
                    case "texturepanels":
                        TexturePanels = new TexturePanels(s, FreelancerIni);
                        break;
                    case "properties":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "flag":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    Properties.Add(e[0].ToString());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "fog":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "fog_enabled":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (FogEnabled != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    FogEnabled = e[0].ToInt32();
                                    break;
                                case "near":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (FogNear != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    FogNear = e[0].ToInt32();
                                    break;
                                case "distance":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (FogDistance != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    FogDistance = e[0].ToInt32();
                                    break;
                                case "color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (FogColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    FogColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "exclusion zones":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "exclusion":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    ExclusionZones.Add(new ExclusionZone(parent, e[0].ToString()));
                                    break;
                                case "fog_far":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExclusionZones.Count == 0) throw new Exception(e.Name + " before exclusion");
                                    ExclusionZones[ExclusionZones.Count - 1].FogFar = e[0].ToSingle();
                                    break;
                                case "zone_shell":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExclusionZones.Count == 0) throw new Exception(e.Name + " before exclusion");
                                    //TODO:ExclusionZones[ExclusionZones.Count - 1].ZoneShellPath = e[0].ToString();
                                    break;
                                case "shell_scalar":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExclusionZones.Count == 0) throw new Exception(e.Name + " before exclusion");
                                    ExclusionZones[ExclusionZones.Count - 1].ShellScalar = e[0].ToSingle();
                                    break;
                                case "max_alpha":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExclusionZones.Count == 0) throw new Exception(e.Name + " before exclusion");
                                    ExclusionZones[ExclusionZones.Count - 1].MaxAlpha = e[0].ToSingle();
                                    break;
                                case "color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExclusionZones.Count == 0) throw new Exception(e.Name + " before exclusion");
                                    ExclusionZones[ExclusionZones.Count - 1].Color = new Color(e[0].ToInt32(), e[0].ToInt32(), e[0].ToInt32());
                                    break;
                                case "exclusion_tint":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExclusionZones.Count == 0) throw new Exception(e.Name + " before exclusion");
                                    ExclusionZones[ExclusionZones.Count - 1].Tint = new Color(e[0].ToInt32(), e[0].ToInt32(), e[0].ToInt32());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "exterior":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "shape":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    ExteriorShape.Add(e[0].ToString());
                                    break;
                                case "shape_weights":
                                    if (e.Count != 4) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorShapeWeights != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorShapeWeights = new List<int>();
                                    foreach (IValue i in e) ExteriorShapeWeights.Add(i.ToInt32());
                                    break;
                                case "fill_shape":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorFillShape != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorFillShape = e[0].ToString();
                                    break;
                                case "plane_slices":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorPlaneSlices != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorPlaneSlices = e[0].ToInt32();
                                    break;
                                case "bit_radius":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorBitRadius != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorBitRadius = e[0].ToInt32();
                                    break;
                                case "bit_radius_random_variation":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorBitRadiusRandomVariation != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorBitRadiusRandomVariation = e[0].ToSingle();
                                    break;
                                case "min_bits":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorMinBits != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorMinBits = e[0].ToInt32();
                                    break;
                                case "max_bits":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorMaxBits != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorMaxBits = e[0].ToInt32();
                                    break;
                                case "move_bit_percent":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorMoveBitPercent != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorMoveBitPercent = e[0].ToSingle();
                                    break;
                                case "equator_bias":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorEquatorBias != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorEquatorBias = e[0].ToSingle();
                                    break;
                                case "color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ExteriorColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    ExteriorColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "nebulalight":
                        NebulaLights.Add(new NebulaLight(s));
                        break;
                    case "clouds":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "max_distance":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsMaxDistance != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsMaxDistance = e[0].ToInt32();
                                    break;
                                case "puff_count":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffCount != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffCount = e[0].ToInt32();
                                    break;
                                case "puff_radius":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffRadius != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffRadius = e[0].ToInt32();
                                    break;
                                case "puff_colora":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffColorA != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffColorA = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                case "puff_colorb":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffColorB != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffColorB = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                case "puff_max_alpha":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffMaxAlpha != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffMaxAlpha = e[0].ToSingle();
                                    break;
                                case "puff_shape":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    CloudsPuffShape.Add(e[0].ToString());
                                    break;
                                case "puff_weights":
                                    if (e.Count != 4) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffWeights != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffWeights = new List<int>();
                                    foreach (IValue i in e) CloudsPuffWeights.Add(i.ToInt32());
                                    break;
                                case "puff_drift":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsPuffDrift != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsPuffDrift = e[0].ToSingle();
                                    break;
                                case "near_fade_distance":
                                    if (e.Count != 2) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsNearFadeDistance != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsNearFadeDistance = new Vector2(e[0].ToSingle(), e[1].ToSingle());
                                    break;
                                case "lightning_intensity":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsLightningIntensity != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsLightningIntensity = e[0].ToSingle();
                                    break;
                                case "lightning_color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsLightningColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsLightningColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                case "lightning_gap":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsLightningGap != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsLightningGap = e[0].ToSingle();
                                    break;
                                case "lightning_duration":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (CloudsLightningDuration != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    CloudsLightningDuration = e[0].ToSingle();
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "backgroundlightning":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "duration":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (BackgroundLightningDuration != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    BackgroundLightningDuration = e[0].ToSingle();
                                    break;
                                case "gap":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (BackgroundLightningGap != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    BackgroundLightningGap = e[0].ToSingle();
                                    break;
                                case "color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (BackgroundLightningColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    BackgroundLightningColor = BackgroundLightningColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "dynamiclightning":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "gap":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (DynamicLightningGap != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    DynamicLightningGap = e[0].ToSingle();
                                    break;
                                case "duration":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (DynamicLightningDuration != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    DynamicLightningDuration = e[0].ToSingle();
                                    break;
                                case "color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (DynamicLightningColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    DynamicLightningColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                case "ambient_intensity":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (DynamicLightningAmbientIntensity != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    DynamicLightningAmbientIntensity = e[0].ToSingle();
                                    break;
                                case "intensity_increase":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (DynamicLightningIntensityIncrease != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    DynamicLightningIntensityIncrease = e[0].ToInt32();
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    default:
                        throw new Exception("Invalid Section in " + file + ": " + s.Name);
                }
            }
        }
    }
}