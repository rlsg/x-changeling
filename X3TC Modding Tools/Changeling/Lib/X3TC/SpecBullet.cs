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
    class SpecBullet : Spec
    {
        internal const String xmlName = "SpecBullet";

        internal enum Features: int
        {
            PAC             = 0x00000001,
            Beam            = 0x00000002,
            ZigZag          = 0x00000004,
            Area            = 0x00000008,
            DisableShields  = 0x00000010,
            IgnoreShields   = 0x00000020,
            UseAmmo         = 0x00000040,
            Repair          = 0x00000080,
            Flak            = 0x00000100,
            ReduceSpeed     = 0x00000200,
            DrainWeapons    = 0x00000400,
            Burn            = 0x00000800,
            Fragmentation   = 0x00001000,
            Charged         = 0x00002000,

            Mask_PAC             = ~(Beam | ZigZag | Area),
            Mask_Beam            = ~(PAC | ZigZag | Area | Charged),
            Mask_ZigZag          = ~(PAC | Beam | Area | Charged),
            Mask_Area            = ~(PAC | Beam | ZigZag | Charged),
            Mask_DisableShields  = ~0,
            Mask_IgnoreShields   = ~0,
            Mask_UseAmmo         = ~0,
            Mask_Repair          = ~0,
            Mask_Flak            = ~0,
            Mask_ReduceSpeed     = ~0,
            Mask_DrainWeapons    = ~0,
            Mask_Burn            = ~0,
            Mask_Fragmentation   = ~0,
            Mask_Charged         = ~(Beam | ZigZag | Area)
        }

        private static Features[] compatabilityMaskList =
        {
            Features.Mask_PAC,
            Features.Mask_Beam,
            Features.Mask_ZigZag,
            Features.Mask_Area,
            Features.Mask_DisableShields,
            Features.Mask_IgnoreShields,
            Features.Mask_UseAmmo,
            Features.Mask_Repair,
            Features.Mask_Flak,
            Features.Mask_ReduceSpeed,
            Features.Mask_DrainWeapons,
            Features.Mask_Burn,
            Features.Mask_Fragmentation,
            Features.Mask_Charged
        };

        internal static Features getCompatabilityMask(Features feat)
        {
            switch (feat)
            {
            case Features.PAC: return Features.Mask_PAC;
            case Features.Beam: return Features.Mask_Beam;
            case Features.ZigZag: return Features.Mask_ZigZag;
            case Features.Area: return Features.Mask_Area;
            case Features.DisableShields: return Features.Mask_DisableShields;
            case Features.IgnoreShields: return Features.Mask_IgnoreShields;
            case Features.UseAmmo: return Features.Mask_UseAmmo;
            case Features.Repair: return Features.Mask_Repair;
            case Features.Flak: return Features.Mask_Flak;
            case Features.ReduceSpeed: return Features.Mask_ReduceSpeed;
            case Features.DrainWeapons: return Features.Mask_DrainWeapons;
            case Features.Burn: return Features.Mask_Burn;
            case Features.Fragmentation: return Features.Mask_Fragmentation;
            case Features.Charged: return Features.Mask_Charged;

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

        private new NumericI wareSize { set { } }

        public NumericI ammoIndex
        {
            get
            {
                return base.wareSize;
            }
            
            set
            {
                base.wareSize=value;
            }
        }

        public NumericI dmgShield = 0;
        public NumericI dmgHull = 0;
        public NumericI energyCost = 0;
        public NumericI bulletCharge = 0;
        public NumericI bulletLife = 0;
        public NumericI bulletSpeed = 0;
        public NumericI bulletFlags = 0;
        public NumericI effectIdTrail = 0;
        public NumericI effectIdImpact = 0;
        public NumericI effectIdLaunch = 0;
        public NumericI effectIdEngine = 0;
        public NumericI colorRed = 0;
        public NumericI colorGreen = 0;
        public NumericI colorBlue = 0;
        public NumericFP hitBoxWidth = 1;
        public NumericFP hitBoxHeight = 1;
        public NumericFP hitBoxLength = 1;
        public NumericI soundIdImpact = 0;
        public NumericI soundIdAmbient = 0;
        public NumericI soundVolMin = 0;
        public NumericI soundVolMax = 0;
        public NumericI reduceSpeedPerc = 0;
        public NumericI reduceSpeedTime = 0;
        public NumericI drainEnergy = 0;
        public NumericI burnDamage = 0;
        public NumericI burnDuration = 0;
        public NumericI fragBulletIndex = 0;
        public NumericI fragCount = 0;
        public NumericI chargeEnergy = 0;
        public NumericI chargeSize = 0;

        public Features bulletFeatures
        {
            get
            {
                return (Features)(int)bulletFlags;
            }
            set
            {
                bulletFlags = (int)value;
            }
        }

        public Boolean isBeam
        {
            get
            {
                return (bulletFeatures & Features.Beam)!=0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Beam;
                    bulletFeatures &= Features.Mask_Beam;
                }
                else
                {
                    bulletFeatures &= ~Features.Beam;
                }
            }
        }

        public Boolean isZigZag
        {
            get
            {
                return (bulletFeatures & Features.ZigZag) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.ZigZag;
                    bulletFeatures &= Features.Mask_ZigZag;
                }
                else
                {
                    bulletFeatures &= ~Features.ZigZag;
                }
            }
        }

        public Boolean isArea
        {
            get
            {
                return (bulletFeatures & Features.Area) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Area;
                    bulletFeatures &= Features.Mask_Area;
                }
                else
                {
                    bulletFeatures &= ~Features.Area;
                }
            }
        }

        public Boolean doesDisableShields
        {
            get
            {
                return (bulletFeatures & Features.DisableShields) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.DisableShields;
                    bulletFeatures &= Features.Mask_DisableShields;
                }
                else
                {
                    bulletFeatures &= ~Features.DisableShields;
                }
            }
        }

        public Boolean doesIgnoreShields
        {
            get
            {
                return (bulletFeatures & Features.IgnoreShields) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.IgnoreShields;
                    bulletFeatures &= Features.Mask_IgnoreShields;
                }
                else
                {
                    bulletFeatures &= ~Features.IgnoreShields;
                }
            }
        }

        public Boolean usesAmmo
        {
            get
            {
                return (bulletFeatures & Features.UseAmmo) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.UseAmmo;
                    bulletFeatures &= Features.Mask_UseAmmo;
                }
                else
                {
                    bulletFeatures &= ~Features.UseAmmo;
                }
            }
        }

        public Boolean doesRepair
        {
            get
            {
                return (bulletFeatures & Features.Repair) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Repair;
                    bulletFeatures &= Features.Mask_Repair;
                }
                else
                {
                    bulletFeatures &= ~Features.Repair;
                }
            }
        }

        public Boolean isFlak
        {
            get
            {
                return (bulletFeatures & Features.Flak) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Flak;
                    bulletFeatures &= Features.Mask_Flak;
                }
                else
                {
                    bulletFeatures &= ~Features.Flak;
                }
            }
        }

        public Boolean doesReduceSpeed
        {
            get
            {
                return (bulletFeatures & Features.ReduceSpeed) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.ReduceSpeed;
                    bulletFeatures &= Features.Mask_ReduceSpeed;
                }
                else
                {
                    bulletFeatures &= ~Features.ReduceSpeed;
                }
            }
        }

        public Boolean doesDrainWeapons
        {
            get
            {
                return (bulletFeatures & Features.DrainWeapons) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.DrainWeapons;
                    bulletFeatures &= Features.Mask_DrainWeapons;
                }
                else
                {
                    bulletFeatures &= ~Features.DrainWeapons;
                }
            }
        }

        public Boolean doesBurn
        {
            get
            {
                return (bulletFeatures & Features.Burn) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Burn;
                    bulletFeatures &= Features.Mask_Burn;
                }
                else
                {
                    bulletFeatures &= ~Features.Burn;
                }
            }
        }

        public Boolean isFragmentation
        {
            get
            {
                return (bulletFeatures & Features.Fragmentation) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Fragmentation;
                    bulletFeatures &= Features.Mask_Fragmentation;
                }
                else
                {
                    bulletFeatures &= ~Features.Fragmentation;
                }
            }
        }

        public Boolean isCharged
        {
            get
            {
                return (bulletFeatures & Features.Charged) != 0;
            }
            set
            {
                if (value)
                {
                    bulletFeatures |= Features.Charged;
                    bulletFeatures &= Features.Mask_Charged;
                }
                else
                {
                    bulletFeatures &= ~Features.Charged;
                }
            }
        }

        public SpecWareTech ammo
        {
            get
            {
                return config.getWareTech(ammoIndex);
            }
            set
            {
                ammoIndex = config.getIndexOf(value);
            }
        }

        public String dmgPerShot
        {
            get
            {
                return dmgShield.engineer() + "/" + dmgHull.engineer();
            }
        }

        public String beamDmgRate
        {
            get
            {
                if (bulletLife > 0)
                {
                    Numeric duration=bulletLife*0.001;
                    return (dmgShield/duration).engineer() + "/" + (dmgHull/duration).engineer();
                }
                else
                {
                    return "∞/∞";
                }
            }
        }

        public String dmgPerEnergy
        {
            get
            {
                if (energyCost > 0)
                {
                    double energyCostFlt = energyCost;
                    return (dmgShield / energyCostFlt).engineer() + "/" + (dmgHull / energyCostFlt).engineer();
                }
                else
                {
                    return "∞/∞";
                }
            }
        }

        public String range
        {
            get
            {
                return ((bulletLife/1000.0)*((bulletSpeed*2.0)/1000.0)).engineer()+"m";
            }
        }

        public String speed
        {
            get
            {
                return ((bulletSpeed * 2.0) / 1000.0).engineer() + "m/s";
            }
        }

        public String timeToRange
        {
            get
            {
                return (bulletLife / 1000.0).time();
            }
        }

        public SpecBullet fragBullet
        {
            get
            {
                return config.getBullet(fragBulletIndex);
            }
            set
            {
                if (value == null)
                {
                    fragBulletIndex = -1;
                }
                else
                {
                    Int32 index = config.getIndexOf(value);

                    if (index >= 0)
                    {
                        fragBulletIndex = index;
                    }
                    else
                    {
                        throw new Exception("SpecBullet::fragBullet - Attempt to assign an invalid bullet");
                    }
                }
            }
        }

        protected SpecBullet(IConfig config) :
            base(config)
        {
            name = "SS_BULLET";
            model = "effects\\cockpitWeapons\\bullet_model";
        }

        protected SpecBullet(SpecBullet spec) :
            base(spec)
        {
        }

        internal SpecBullet(IConfig config, XmlNode specNode)
            : base(config, specNode)
        {
            ammoIndex = getAttr(specNode, "ammoIndex", ammoIndex);
            dmgShield = getAttr(specNode, "dmgShield", dmgShield);
            energyCost = getAttr(specNode, "energyCost", energyCost);
            soundIdImpact = getAttr(specNode, "soundIdImpact", soundIdImpact);
            bulletLife = getAttr(specNode, "bulletLife", bulletLife);
            bulletSpeed = getAttr(specNode, "bulletSpeed", bulletSpeed);
            bulletFlags = getAttr(specNode, "bulletFlags", bulletFlags);
            effectIdTrail = getAttr(specNode, "effectIdTrail", effectIdTrail);
            effectIdImpact = getAttr(specNode, "effectIdImpact", effectIdImpact);
            effectIdLaunch = getAttr(specNode, "effectIdLaunch", effectIdLaunch);
            dmgHull = getAttr(specNode, "dmgHull", dmgHull);
            effectIdEngine = getAttr(specNode, "effectIdEngine", effectIdEngine);
            soundIdAmbient = getAttr(specNode, "soundIdAmbient", soundIdAmbient);
            soundVolMin = getAttr(specNode, "soundVolMin", soundVolMin);
            soundVolMax = getAttr(specNode, "soundVolMax", soundVolMax);
            bulletCharge = getAttr(specNode, "bulletCharge", bulletCharge);
            reduceSpeedPerc = getAttr(specNode, "reduceSpeedPerc", reduceSpeedPerc);
            reduceSpeedTime = getAttr(specNode, "reduceSpeedTime", reduceSpeedTime);
            drainEnergy = getAttr(specNode, "drainEnergy", drainEnergy);
            burnDamage = getAttr(specNode, "burnDamage", burnDamage);
            burnDuration = getAttr(specNode, "burnDuration", burnDuration);
            fragBulletIndex = getAttr(specNode, "fragBulletIndex", fragBulletIndex);
            fragCount = getAttr(specNode, "fragCount", fragCount);
            chargeEnergy = getAttr(specNode, "chargeEnergy", chargeEnergy);
            chargeSize = getAttr(specNode, "chargeSize", chargeSize);
            colorRed = getAttr(specNode, "colorRed", colorRed);
            colorGreen = getAttr(specNode, "colorGreen", colorGreen);
            colorBlue = getAttr(specNode, "colorBlue", colorBlue);
            hitBoxHeight = getAttr(specNode, "hitBoxHeight", hitBoxHeight);
            hitBoxWidth = getAttr(specNode, "hitBoxWidth", hitBoxWidth);
            hitBoxLength = getAttr(specNode, "hitBoxLength", hitBoxLength);
        }

        internal override XmlElement save(XmlDocument xmlDoc)
        {
            XmlElement specNode = xmlDoc.CreateElement(xmlName);

            base.save(specNode);

            specNode.RemoveAttribute("wareSize");

            specNode.SetAttribute("ammoIndex", ammoIndex.ToString());
            specNode.SetAttribute("dmgShield", dmgShield.ToString());
            specNode.SetAttribute("energyCost", energyCost.ToString());
            specNode.SetAttribute("soundIdImpact", soundIdImpact.ToString());
            specNode.SetAttribute("bulletLife", bulletLife.ToString());
            specNode.SetAttribute("bulletSpeed", bulletSpeed.ToString());
            specNode.SetAttribute("bulletFlags", bulletFlags.ToString());
            specNode.SetAttribute("effectIdTrail", effectIdTrail.ToString());
            specNode.SetAttribute("effectIdImpact", effectIdImpact.ToString());
            specNode.SetAttribute("effectIdLaunch", effectIdLaunch.ToString());
            specNode.SetAttribute("dmgHull", dmgHull.ToString());
            specNode.SetAttribute("effectIdEngine", effectIdEngine.ToString());
            specNode.SetAttribute("soundIdAmbient", soundIdAmbient.ToString());
            specNode.SetAttribute("soundVolMin", soundVolMin.ToString());
            specNode.SetAttribute("soundVolMax", soundVolMax.ToString());
            specNode.SetAttribute("bulletCharge", bulletCharge.ToString());
            specNode.SetAttribute("reduceSpeedPerc", reduceSpeedPerc.ToString());
            specNode.SetAttribute("reduceSpeedTime", reduceSpeedTime.ToString());
            specNode.SetAttribute("drainEnergy", drainEnergy.ToString());
            specNode.SetAttribute("burnDamage", burnDamage.ToString());
            specNode.SetAttribute("burnDuration", burnDuration.ToString());
            specNode.SetAttribute("fragBulletIndex", fragBulletIndex.ToString());
            specNode.SetAttribute("fragCount", fragCount.ToString());
            specNode.SetAttribute("chargeEnergy", chargeEnergy.ToString());
            specNode.SetAttribute("chargeSize", chargeSize.ToString());
            specNode.SetAttribute("colorRed", colorRed.ToString());
            specNode.SetAttribute("colorGreen", colorGreen.ToString());
            specNode.SetAttribute("colorBlue", colorBlue.ToString());
            specNode.SetAttribute("hitBoxHeight", hitBoxHeight.ToString());
            specNode.SetAttribute("hitBoxWidth", hitBoxWidth.ToString());
            specNode.SetAttribute("hitBoxLength", hitBoxLength.ToString());

            return specNode;
        }

        public SpecBullet(IConfig config, List<String> datLine) :
            base(config, datLine)
        {
            int idx = 7;

            dmgShield = datLine[idx++];
            energyCost = datLine[idx++];
            soundIdImpact = datLine[idx++];
            bulletLife = datLine[idx++];
            bulletSpeed = datLine[idx++];
            bulletFlags = datLine[idx++];
            colorRed = datLine[idx++];
            colorGreen = datLine[idx++];
            colorBlue = datLine[idx++];
            hitBoxWidth = datLine[idx++];
            hitBoxHeight = datLine[idx++];
            hitBoxLength = datLine[idx++];
            effectIdTrail = datLine[idx++];
            effectIdImpact = datLine[idx++];
            effectIdLaunch = datLine[idx++];
            dmgHull = datLine[idx++];
            effectIdEngine = datLine[idx++];
            soundIdAmbient = datLine[idx++];
            soundVolMin = datLine[idx++];
            soundVolMax = datLine[idx++];
            bulletCharge = datLine[idx++];
            reduceSpeedPerc = datLine[idx++];
            reduceSpeedTime = datLine[idx++];
            drainEnergy = datLine[idx++];
            burnDamage = datLine[idx++];
            burnDuration = datLine[idx++];
            fragBulletIndex = datLine[idx++];
            fragCount = datLine[idx++];
            chargeEnergy = datLine[idx++];
            chargeSize = datLine[idx++];
        }

        internal override List<String> getSpec()
        {
            List<String> rval = base.getSpec();

            rval.InsertRange(7, new String[] {
                dmgShield,
                energyCost,
                soundIdImpact,
                bulletLife,
                bulletSpeed,
                bulletFlags,
                colorRed,
                colorGreen,
                colorBlue,
                hitBoxWidth,
                hitBoxHeight,
                hitBoxLength,
                effectIdTrail,
                effectIdImpact,
                effectIdLaunch,
                dmgHull,
                effectIdEngine,
                soundIdAmbient,
                soundVolMin,
                soundVolMax,
                bulletCharge,
                reduceSpeedPerc,
                reduceSpeedTime,
                drainEnergy,
                burnDamage,
                burnDuration,
                fragBulletIndex,
                fragCount,
                chargeEnergy,
                chargeSize
            });

            return rval;
        }

        public override string generateLabel()
        {
            return name;
        }

        public override string generateTooltip()
        {
            String tooltip =
                "Damage Per Shot (Shield/Hull): " + dmgPerShot +
                (isBeam? "\nBeam Damage Rate (Shield/Hull): " + beamDmgRate : "") +
                "\nDamage Per Energy Unit (Shield/Hull): " + dmgPerEnergy +
                (isBeam?"":"\nSpeed: " + speed) +
                "\nRange: " + range +
                (isBeam ? "\nBeam Duration: " : "\nTime to Range: ") + timeToRange +
                "\nEnergy Cost: " + energyCost.engineer();

            SpecWareTech ammo = this.ammo;

            if (ammo != null)
            {
                tooltip += "\nAmmo: " + ammo.generateLabel();
            }

            return tooltip;
        }

        public override bool isValid()
        {
            return config.validate(this);
        }
    }
}
