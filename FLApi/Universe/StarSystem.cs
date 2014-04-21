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
using System.Xml;

using OpenTK;
using FLCommon;

using FLParser.Ini;

using FLApi.Utf;
using FLApi.Utf.Cmp;
using FLApi.Solar;

namespace FLApi.Universe
{
    public class StarSystem : UniverseElement, IDrawable
    {
        private RenderTools.Camera camera;

        public Vector2? Pos { get; private set; }
        public string MsgIdPrefix { get; private set; }
        public int? Visit { get; private set; }
        public XmlDocument IdsInfo { get; private set; }
        public float? NavMapScale { get; private set; }

        public Color? SpaceColor { get; private set; }
        public string LocalFaction { get; private set; }
        public bool? RpopSolarDetection { get; private set; }

        public string MusicSpace { get; private set; }
        public string MusicDanger { get; private set; }
        public string MusicBattle { get; private set; }

        public List<string> ArchetypeShip { get; private set; }
        public List<string> ArchetypeSimple { get; private set; }
        public List<Archetype> ArchetypeSolar { get; private set; }
        public List<string> ArchetypeEquipment { get; private set; }
        public List<string> ArchetypeSnd { get; private set; }
        public List<List<string>> ArchetypeVoice { get; private set; }

        public string Spacedust { get; private set; }

        public List<Nebula> Nebulae { get; private set; }
        public List<AsteroidField> Asteroids { get; private set; }

        public Color? AmbientColor { get; private set; }

        public List<LightSource> LightSources { get; private set; }
        public List<SystemObject> Objects { get; private set; }
        public List<EncounterParameter> EncounterParameters { get; private set; }

        public TexturePanels TexturePanels { get; private set; }

        private string backgroundBasicStarsPath;
        private CmpFile backgroundBasicStars;
        public CmpFile BackgroundBasicStars
        {
            get
            {
                if (string.IsNullOrEmpty(backgroundBasicStarsPath)) return null;
                if (backgroundBasicStars == null) backgroundBasicStars = new CmpFile(FreelancerIni.DataPath + backgroundBasicStarsPath, null);
                return backgroundBasicStars;
            }
        }

        private string backgroundComplexStarsPath;
        private IDrawable backgroundComplexStars;
        public IDrawable BackgroundComplexStars
        {
            get
            {
                if (string.IsNullOrEmpty(backgroundComplexStarsPath)) return null;
                if (backgroundComplexStars == null)
                {
                    if (backgroundComplexStarsPath.EndsWith(".cmp", StringComparison.OrdinalIgnoreCase))
                        backgroundComplexStars = new CmpFile(FreelancerIni.DataPath + backgroundComplexStarsPath, null);
                    else
                        backgroundComplexStars = new ModelFile(FreelancerIni.DataPath + backgroundComplexStarsPath, null);
                }
                return backgroundComplexStars;
            }
        }

        private string backgroundNebulaePath;
        private CmpFile backgroundNebulae;
        public CmpFile BackgroundNebulae
        {
            get
            {
                if (string.IsNullOrEmpty(backgroundNebulaePath)) return null;
                if (backgroundNebulae == null) backgroundNebulae = new CmpFile(FreelancerIni.DataPath + backgroundNebulaePath, null);
                return backgroundNebulae;
            }
        }

        public List<Zone> Zones { get; private set; }
        public Field Field { get; private set; }
        public AsteroidBillboards AsteroidBillboards { get; private set; }

        public StarSystem(UniverseIni universe, Section section, FreelancerIni data)
            : base(data)
        {
            if (universe == null) throw new ArgumentNullException("universe");
            if (section == null) throw new ArgumentNullException("section");

            string file = null;

            foreach (Entry e in section)
            {
                switch (e.Name.ToLowerInvariant())
                {
                    case "nickname":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Nickname != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Nickname = e[0].ToString();
                        break;
                    case "file":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (file != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        file = e[0].ToString();
                        break;
                    case "pos":
                        if (e.Count != 2) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Pos != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Pos = new Vector2(e[0].ToSingle(), e[1].ToSingle());
                        break;
                    case "msg_id_prefix":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (MsgIdPrefix != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        MsgIdPrefix = e[0].ToString();
                        break;
                    case "visit":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (Visit != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        Visit = e[0].ToInt32();
                        break;
                    case "strid_name":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (StridName != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        StridName = FreelancerIni.GetStringResource(e[0].ToInt32());
                        break;
                    case "ids_info":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (IdsInfo != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        IdsInfo = FreelancerIni.GetXmlResource(e[0].ToInt32());
                        break;
                    case "navmapscale":
                        if (e.Count != 1) throw new Exception("Invalid number of values in " + section.Name + " Entry " + e.Name + ": " + e.Count);
                        if (NavMapScale != null) throw new Exception("Duplicate " + e.Name + " Entry in " + section.Name);
                        NavMapScale = e[0].ToSingle();
                        break;
                    default:
                        throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                }
            }

            foreach (Section s in ParseFile(FreelancerIni.DataPath + "universe\\" + file))
            {
                switch (s.Name.ToLowerInvariant())
                {
                    case "systeminfo":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "name":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (Name != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    Name = e[0].ToString();
                                    break;
                                case "space_color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (SpaceColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    SpaceColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                case "local_faction":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (LocalFaction != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    LocalFaction = e[0].ToString();
                                    break;
                                case "rpop_solar_detection":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (RpopSolarDetection != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    RpopSolarDetection = e[0].ToBoolean();
                                    break;
						case "space_farclip":
							Console.WriteLine ("[StarSystem]Unimplemented INI thing space_farclip");
							break;
                                default:
                                    throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "music":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "space":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (MusicSpace != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    MusicSpace = e[0].ToString();
                                    break;
                                case "danger":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (MusicDanger != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    MusicDanger = e[0].ToString();
                                    break;
                                case "battle":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (MusicBattle != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    MusicBattle = e[0].ToString();
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + section.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "archetype":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "ship":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ArchetypeShip == null) ArchetypeShip = new List<string>();
                                    ArchetypeShip.Add(e[0].ToString());
                                    break;
                                case "simple":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ArchetypeSimple == null) ArchetypeSimple = new List<string>();
                                    ArchetypeSimple.Add(e[0].ToString());
                                    break;
                                case "solar":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ArchetypeSolar == null) ArchetypeSolar = new List<Archetype>();
                                    ArchetypeSolar.Add(FreelancerIni.Solar.FindSolar(e[0].ToString()));
                                    break;
                                case "equipment":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ArchetypeEquipment == null) ArchetypeEquipment = new List<string>();
                                    ArchetypeEquipment.Add(e[0].ToString());
                                    break;
                                case "snd":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (ArchetypeSnd == null) ArchetypeSnd = new List<string>();
                                    ArchetypeSnd.Add(e[0].ToString());
                                    break;
                                case "voice":
                                    if (ArchetypeVoice == null) ArchetypeVoice = new List<List<string>>();
                                    ArchetypeVoice.Add(new List<string>());
                                    foreach (IValue i in e) ArchetypeVoice[ArchetypeVoice.Count - 1].Add(i.ToString());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "dust":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "spacedust":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (Spacedust != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    Spacedust = e[0].ToString();
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "nebula":
                        if (Nebulae == null) Nebulae = new List<Nebula>();
                        Nebulae.Add(new Nebula(this, s, FreelancerIni));
                        break;
                    case "asteroids":
                        if (Asteroids == null) Asteroids = new List<AsteroidField>();
                        Asteroids.Add(new AsteroidField(this, s, FreelancerIni));
                        break;
                    case "ambient":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "color":
                                    if (e.Count != 3) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (AmbientColor != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    AmbientColor = new Color(e[0].ToInt32(), e[1].ToInt32(), e[2].ToInt32());
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "lightsource":
                        if (LightSources == null) LightSources = new List<LightSource>();
                        LightSources.Add(new LightSource(s, FreelancerIni));
                        break;
                    case "object":
                        if (Objects == null) Objects = new List<SystemObject>();
                        Objects.Add(new SystemObject(universe, this, s, FreelancerIni));
                        break;
                    case "encounterparameters":
                        if (EncounterParameters == null) EncounterParameters = new List<EncounterParameter>();
                        EncounterParameters.Add(new EncounterParameter(s));
                        break;
                    case "texturepanels":
                        TexturePanels = new TexturePanels(s, FreelancerIni);
                        break;
                    case "background":
                        foreach (Entry e in s)
                        {
                            switch (e.Name.ToLowerInvariant())
                            {
                                case "basic_stars":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (backgroundBasicStarsPath != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    backgroundBasicStarsPath = e[0].ToString();
                                    break;
                                case "complex_stars":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    if (backgroundComplexStarsPath != null) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    backgroundComplexStarsPath = e[0].ToString();
                                    break;
                                case "nebulae":
                                    if (e.Count != 1) throw new Exception("Invalid number of values in " + s.Name + " Entry " + e.Name + ": " + e.Count);
                                    string temp = e[0].ToString();
                                    if (backgroundNebulaePath != null && backgroundNebulaePath != temp) throw new Exception("Duplicate " + e.Name + " Entry in " + s.Name);
                                    backgroundNebulaePath = temp;
                                    break;
                                default:
                                    throw new Exception("Invalid Entry in " + s.Name + ": " + e.Name);
                            }
                        }
                        break;
                    case "zone":
                        if (Zones == null) Zones = new List<Zone>();
                        Zones.Add(new Zone(s, FreelancerIni));
                        break;
                    case "field":
                        Field = new Field(s);
                        break;
                    case "asteroidbillboards":
                        AsteroidBillboards = new AsteroidBillboards(s);
                        break;
                    default:
                        throw new Exception("Invalid Section in " + file + ": " + s.Name);
                }
            }
        }

        public SystemObject FindObject(string nickname)
        {
            return (from SystemObject o in Objects where o.Nickname.ToLowerInvariant() == nickname.ToLowerInvariant() select o).First<SystemObject>();
        }

        public SystemObject FindJumpGateTo(StarSystem system)
        {
            return (from SystemObject o in Objects where o.Archetype is JumpGate && o.Goto.System == system select o).First<SystemObject>();
        }

        public SystemObject FindJumpHoleTo(StarSystem system)
        {
            return (from SystemObject o in Objects where o.Archetype is JumpHole && o.Goto.System == system select o).First<SystemObject>();
        }

        public Zone FindZone(string nickname)
        {
            return (from Zone z in Zones where z.Nickname == nickname select z).First<Zone>();
        }

        public void Initialize(GraphicsDevice device, ContentManager content, RenderTools.Camera camera)
        {
            this.camera = camera;

            if (BackgroundBasicStars != null) BackgroundBasicStars.Initialize(device, content, camera);
            if (BackgroundComplexStars != null) BackgroundComplexStars.Initialize(device, content, camera);
            if (BackgroundNebulae != null) BackgroundNebulae.Initialize(device, content, camera);

            foreach (SystemObject o in Objects) o.Initialize(device, content, camera);
        }

        public void Resized()
        {
            if (BackgroundBasicStars != null) BackgroundBasicStars.Resized();
            if (BackgroundComplexStars != null) BackgroundComplexStars.Resized();
            if (BackgroundNebulae != null) BackgroundNebulae.Resized();

            foreach (SystemObject o in Objects) o.Resized();
        }

        public void Update()
        {
            if (BackgroundBasicStars != null) BackgroundBasicStars.Update();
            if (BackgroundComplexStars != null) BackgroundComplexStars.Update();
            if (BackgroundNebulae != null) BackgroundNebulae.Update();

            foreach (SystemObject o in Objects) o.Update();
        }

        public void Draw(Color ambient, List<LightSource> lights, Matrix world)
        {
            Color ambientColor = AmbientColor ?? ambient;

            Matrix starSphereWorld = Matrix.CreateTranslation(camera.Position);
            if (BackgroundBasicStars != null) BackgroundBasicStars.Draw(ambientColor, LightSources, starSphereWorld);
            if (BackgroundComplexStars != null) BackgroundComplexStars.Draw(ambientColor, LightSources, starSphereWorld);
            if (BackgroundNebulae != null) BackgroundNebulae.Draw(ambientColor, LightSources, starSphereWorld);

            foreach (SystemObject o in Objects) o.Draw(ambientColor, LightSources, world);
        }
    }
}