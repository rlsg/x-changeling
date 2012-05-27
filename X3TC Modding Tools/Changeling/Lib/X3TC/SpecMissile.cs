/* 
 * Copyright (c) 2011-2012 Roger L.S. Griffiths
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Changeling.Lib.X3TC
{
    class SpecMissile : SpecWare
    {
        internal readonly static String[] typeStrings ={
            "SG_MISSILE_LIGHT",             // 00000001
            "SG_MISSILE_MEDIUM",            // 00000002
            "SG_MISSILE_HEAVY",             // 00000004
            "SG_MISSILE_TR_LIGHT",          // 00000008
            "SG_MISSILE_TR_MEDIUM",         // 00000010
            "SG_MISSILE_TR_HEAVY",          // 00000020
            "SG_MISSILE_KHAAK",             // 00000040
            "SG_MISSILE_BOMBER",            // 00000080
            "SG_MISSILE_TORP_CAPITAL",      // 00000100
            "SG_MISSILE_AF_CAPITAL",        // 00000200
            "SG_MISSILE_TR_BOMBER",         // 00000400
            "SG_MISSILE_TR_TORP_CAPITAL",   // 00000800
            "SG_MISSILE_TR_AF_CAPITAL",     // 00001000
            "SG_MISSILE_BOARDINGPOD",       // 00002000
            "SG_MISSILE_DMBF"               // 00004000
        };

        internal static List<String> getTypes(int typeBitMask = -1)
        {
            List<String> rlist = new List<string>();

            for (int bit = 0; bit < typeStrings.Length; bit++)
            {
                if ((typeBitMask & (1 << bit)) != 0)
                {
                    rlist.Add(typeStrings[bit]);
                }
            }

            return rlist;
        }

        internal static int getBitMask(IEnumerable<String> types)
        {
            int mask = 0;

            for (int bit = 0; bit < typeStrings.Length; bit++)
            {
                if (types.Contains(typeStrings[bit]))
                {
                    mask |= (1 << bit);
                }
            }

            return mask;
        }

        internal enum Features : int
        {
            IFF             = 0x00000001,
            Dumbfire        = 0x00000002,
            Swarm           = 0x00000004,
            Heatseeker      = 0x00000008,
            MultiWarhead    = 0x00000010,
            Proximity       = 0x00000020,
            ManualDetonate  = 0x00000040,
            Torpedo         = 0x00000080,
            ManualGuidance  = 0x00000100,

            Mask_IFF            = ~0,
            Mask_Dumbfire       = ~(ManualGuidance | Heatseeker),
            Mask_Swarm          = ~0,
            Mask_Heatseeker     = ~(ManualGuidance | Dumbfire),
            Mask_MultiWarhead   = ~0,
            Mask_Proximity      = ~0,
            Mask_ManualDetonate = ~0,
            Mask_Torpedo        = ~0,
            Mask_ManualGuidance = ~(Dumbfire | Heatseeker)
        }

        private static Features[] compatabilityMaskList =
        {
            Features.Mask_IFF,
            Features.Mask_Dumbfire,
            Features.Mask_Swarm,
            Features.Mask_Heatseeker,
            Features.Mask_MultiWarhead,
            Features.Mask_Proximity,
            Features.Mask_ManualDetonate,
            Features.Mask_Torpedo,
            Features.Mask_ManualGuidance
        };

        internal static Features getCompatabilityMask(Features feat)
        {
            switch (feat)
            {
            case Features.IFF: return Features.Mask_IFF;
            case Features.Dumbfire: return Features.Mask_Dumbfire;
            case Features.Swarm: return Features.Mask_Swarm;
            case Features.Heatseeker: return Features.Mask_Heatseeker;
            case Features.MultiWarhead: return Features.Mask_MultiWarhead;
            case Features.Proximity: return Features.Mask_Proximity;
            case Features.ManualDetonate: return Features.Mask_ManualDetonate;
            case Features.Torpedo: return Features.Mask_Torpedo;
            case Features.ManualGuidance: return Features.Mask_ManualGuidance;

            default:
                {
                    int selectedBits = (int)feat;
                    Features mask = (Features)~0;

                    for (int bit = 0; bit < compatabilityMaskList.Length; bit++)
                    {
                        if ((selectedBits & (1 << bit)) != 0)
                        {
                            mask &= compatabilityMaskList[bit];
                            selectedBits &= (int)mask;
                        }
                    }

                    return mask;
                }
            }
        }

        internal const String xmlName = "SpecMissile";
        protected override Numeric priceAverageScale { get { return 28.075M; } }

        public NumericI characteristics = 0;

        public NumericI speed = 0;
        public NumericI accel = 0;
        public NumericI soundIdLauch = 0;
        public NumericI soundIdAmbient = 0;
        public NumericI collision = 0;
        public NumericI damage = 0;
        public NumericI blastRadius = 0;
        public NumericI lifetime = 0;
        public NumericI effectIdTrail = 0;
        public NumericI effectIdGlow = 0;
        public NumericI soundIdExplode = 0;
        public NumericI soundVolMin = 0;
        public NumericI soundVolMax = 0;
        public NumericI soundIdImpact = 0;
        public NumericI effectIdExplode = 0;
        public NumericI effectIdEngine = 0;
        public NumericI missileFlags = 0;
        public NumericI fireInterval = 0;
        public String missileIcon = "ICON_MISSILE";
        public String missileScene = "cockpitWeapons\\missiles\\missile_model_scene";

        public Features missileFeatures
        {
            get
            {
                return (Features)(int)missileFlags;
            }
            set
            {
                missileFlags = (int)value;
            }
        }

        public Numeric minSafeDistance
        {
            get
            {
                return (blastRadius*2.0)/1000.0;
            }
            set
            {
                blastRadius = (NumericI)((value * 1000.0) / 2.0);
            }
        }

        internal SpecMissile(IConfig config)
            : base(config)
        {
            type = "SG_MISSILE";
            name = "SS_MISSILE";
            model = "cockpitWeapons\\missiles\\missile_model";
        }

        internal SpecMissile(IConfig config, XmlNode specNode)
            : base(config, specNode)
        {
            characteristics = getAttr(specNode, "characteristics", characteristics);
            speed = getAttr(specNode, "speed", speed);
            accel = getAttr(specNode, "accel", accel);
            soundIdLauch = getAttr(specNode, "soundIdLauch", soundIdLauch);
            soundIdAmbient = getAttr(specNode, "soundIdAmbient", soundIdAmbient);
            collision = getAttr(specNode, "collision", collision);
            damage = getAttr(specNode, "damage", damage);
            blastRadius = getAttr(specNode, "blastRadius", blastRadius);
            lifetime = getAttr(specNode, "lifetime", lifetime);
            effectIdTrail = getAttr(specNode, "effectIdTrail", effectIdTrail);
            effectIdGlow = getAttr(specNode, "effectIdGlow", effectIdGlow);
            soundIdExplode = getAttr(specNode, "soundIdExplode", soundIdExplode);
            soundVolMin = getAttr(specNode, "soundVolMin", soundVolMin);
            soundVolMax = getAttr(specNode, "soundVolMax", soundVolMax);
            soundIdImpact = getAttr(specNode, "soundIdImpact", soundIdImpact);
            effectIdExplode = getAttr(specNode, "effectIdExplode", effectIdExplode);
            effectIdEngine = getAttr(specNode, "effectIdEngine", effectIdEngine);
            missileFlags = getAttr(specNode, "missileFlags", missileFlags);
            fireInterval = getAttr(specNode, "fireInterval", fireInterval);
            missileIcon = getAttr(specNode, "missileIcon", missileIcon);
            missileScene = getAttr(specNode, "missileScene", missileScene);
        }

        internal override XmlElement save(XmlDocument doc)
        {
            XmlElement specNode = doc.CreateElement(xmlName);

            save(specNode);

            specNode.SetAttribute("characteristics", characteristics.ToString());
            specNode.SetAttribute("speed", speed.ToString());
            specNode.SetAttribute("accel", accel.ToString());
            specNode.SetAttribute("soundIdLauch", soundIdLauch.ToString());
            specNode.SetAttribute("soundIdAmbient", soundIdAmbient.ToString());
            specNode.SetAttribute("collision", collision.ToString());
            specNode.SetAttribute("damage", damage.ToString());
            specNode.SetAttribute("blastRadius", blastRadius.ToString());
            specNode.SetAttribute("lifetime", lifetime.ToString());
            specNode.SetAttribute("effectIdTrail", effectIdTrail.ToString());
            specNode.SetAttribute("effectIdGlow", effectIdGlow.ToString());
            specNode.SetAttribute("soundIdExplode", soundIdExplode.ToString());
            specNode.SetAttribute("soundVolMin", soundVolMin.ToString());
            specNode.SetAttribute("soundVolMax", soundVolMax.ToString());
            specNode.SetAttribute("soundIdImpact", soundIdImpact.ToString());
            specNode.SetAttribute("effectIdExplode", effectIdExplode.ToString());
            specNode.SetAttribute("effectIdEngine", effectIdEngine.ToString());
            specNode.SetAttribute("missileFlags", missileFlags.ToString());
            specNode.SetAttribute("fireInterval", fireInterval.ToString());
            specNode.SetAttribute("missileIcon", missileIcon);
            specNode.SetAttribute("missileScene", missileScene);

            return specNode;
        }

        public SpecMissile(IConfig config, List<String> datLine) :
            base(config, datLine)
        {
            int idx = 7;

            speed = datLine[idx++];
            accel = datLine[idx++];
            soundIdImpact = datLine[idx++];
            soundIdAmbient = datLine[idx++];
            collision = datLine[idx++];
            damage = datLine[idx++];
            blastRadius = datLine[idx++];
            lifetime = datLine[idx++];
            effectIdTrail = datLine[idx++];
            effectIdGlow = datLine[idx++];
            soundIdExplode = datLine[idx++];
            soundVolMin = datLine[idx++];
            soundVolMax = datLine[idx++];
            soundIdImpact = datLine[idx++];
            effectIdExplode = datLine[idx++];
            effectIdEngine = datLine[idx++];
            missileFlags = datLine[idx++];
            fireInterval = datLine[idx++];
            missileIcon = datLine[idx++];
            missileScene = datLine[idx++];
        }

        internal override List<String> getSpec()
        {
            List<String> rval = base.getSpec();

            rval.InsertRange(7, new String[] {
                speed,
                accel,
                soundIdLauch,
                soundIdAmbient,
                collision,
                damage,
                blastRadius,
                lifetime,
                effectIdTrail,
                effectIdGlow,
                soundIdExplode,
                soundVolMin,
                soundVolMax,
                soundIdImpact,
                effectIdExplode,
                effectIdEngine,
                missileFlags,
                fireInterval,
                missileIcon,
                missileScene
            });

            return rval;
        }

        public override string generateTooltip()
        {
            String tooltip =
                "Mount: " + type +
                "\nMaximum Damage: " + damage.engineer() +
                "\nNominal Safe Distance: " + minSafeDistance.engineer() + "m" +
                "\nCharacteristics: " + characteristics.ToString("X8");

            String descr = base.generateTooltip();

            if (descr != null)
            {
                tooltip += "\n" + descr;
            }

            return tooltip;
        }

        public override bool isValid()
        {
            return config.validate(this);
        }
    }
}
