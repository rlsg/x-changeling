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
    class SpecLaser : SpecWare
    {
        internal readonly static String[] typeStrings ={
            "SG_LASER_IRE",                 // 00000001
            "SG_LASER_PAC",                 // 00000002
            "SG_LASER_MASS",                // 00000004
            "SG_LASER_ARGON_LIGHT",         // 00000008
            "SG_LASER_TELADI_LIGHT",        // 00000010
            "SG_LASER_PARANID_LIGHT",       // 00000020
            "SG_LASER_HEPT",                // 00000040
            "SG_LASER_BORON_LIGHT",         // 00000080
            "SG_LASER_PBE",                 // 00000100
            "SG_LASER_PIRATE_LIGHT",        // 00000200
            "SG_LASER_TERRAN_LIGHT",        // 00000400
            "SG_LASER_CIG",                 // 00000800
            "SG_LASER_BORON_MEDIUM",        // 00001000
            "SG_LASER_SPLIT_MEDIUM",        // 00002000
            "SG_LASER_TERRAN_MEDIUM",       // 00004000
            "SG_LASER_TELADI_AF",           // 00008000
            "SG_LASER_ARGON_AF",            // 00010000
            "SG_LASER_SPLIT_AF",            // 00020000
            "SG_LASER_PARANID_AF",          // 00040000
            "SG_LASER_TERRAN_AF",           // 00080000
            "SG_LASER_PPC",                 // 00100000
            "SG_LASER_BORON_HEAVY",         // 00200000
            "SG_LASER_TELADI_HEAVY",        // 00400000
            "SG_LASER_PIRATE_HEAVY",        // 00800000
            "SG_LASER_TERRAN_HEAVY",        // 01000000
            "SG_LASER_ARGON_BEAM",          // 02000000
            "SG_LASER_PARANID_BEAM",        // 04000000
            "SG_LASER_TERRAN_BEAM",         // 08000000
            "SG_LASER_SPECIAL",             // 10000000
            "SG_LASER_UNKNOWN1",            // 20000000
            "SG_LASER_UNKNOWN2",            // 40000000
            "SG_LASER_KYON"                 // 80000000
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

        internal static int getBitMask(IEnumerable<String> laserTypes)
        {
            int mask = 0;

            for (int bit = 0; bit < typeStrings.Length; bit++)
            {
                if (laserTypes.Contains(typeStrings[bit]))
                {
                    mask |= (1 << bit);
                }
            }

            return mask;
        }

        internal const String xmlName = "SpecLaser";
        protected override Numeric priceAverageScale { get { return 64.9239M; } }

        public NumericI minShipSize=0;
        public NumericI maxShipSize = 100000;
        public NumericI aoeClass = 0;

        public NumericI defaultBias = 1;
        public NumericI characteristics = 0;

        public NumericI firingSoundId = -1;
        public NumericI bulletIndex = -1;
        public NumericI bulletInterval = 1;
        public NumericI minEnergy = 0;
        public NumericFP chargeRate = 0.1f;

        public String laserIcon = "ICON_LASER";

        public SpecBullet bullet
        {
            get
            {
                return config.getBullet(bulletIndex);
            }
            set
            {
                if (value == null)
                {
                    bulletIndex = -1;
                }
                else
                {
                    Int32 index = config.getIndexOf(value);

                    if (index >= 0)
                    {
                        bulletIndex = index;
                    }
                    else
                    {
                        throw new Exception("SpecLaser::bullet - Attempt to assign an invalid bullet");
                    }
                }
            }
        }

        public String maxBullets
        {
            get
            {
                if (bullet != null)
                {
                    if (bulletInterval > 0)
                    {
                        return (((float)bullet.bulletLife) / bulletInterval).ToString();
                    }
                    else
                    {
                        return "∞";
                    }

                }
                else
                {
                    return "?";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String dmgPerSecond
        {
            get
            {
                if (bullet != null)
                {
                    if (bulletInterval > 0)
                    {
                        return shieldDmgPerSecond.engineer() + "/" + hullDmgPerSecond.engineer();
                    }
                    else
                    {
                        return "-/-";
                    }
                }
                else
                {
                    return "?/?";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String dmgPerShot
        {
            get
            {
                if (bullet != null)
                {
                    return bullet.dmgPerShot;
                }
                else
                {
                    return "?/?";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String dmgPerEnergy
        {
            get
            {
                if (bullet != null)
                {
                    return bullet.dmgPerEnergy;
                }
                else
                {
                    return "?/?";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String range
        {
            get
            {
                if (bullet != null)
                {
                    return bullet.range;
                }
                else
                {
                    return "?";
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String displayName
        {
            get
            {
                return getText(displayNameId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NumericI shieldDmgPerSecond
        {
            get
            {
                if (bullet != null)
                {
                    if (bulletInterval > 0)
                    {
                        double dmgShield = ((bullet.dmgShield * 1000.0) / bulletInterval);

                        return (Int32)Math.Round(dmgShield);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (bullet != null)
                {
                    if (bulletInterval > 0)
                    {
                        double dmgShield = value * (bulletInterval / 1000.0);

                        bullet.dmgShield = (Int32)Math.Round(dmgShield);
                    }
                    else
                    {
                        throw new Exception("SpecLaser::shieldDmgPerSecond - Invalid refire rate");
                    }
                }
                else
                {
                    throw new Exception("SpecLaser::shieldDmgPerSecond - Invalid bullet");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public NumericI hullDmgPerSecond
        {
            get
            {
                if (bullet != null)
                {
                    if (bulletInterval > 0)
                    {
                        double dmgHull = ((bullet.dmgHull * 1000.0) / bulletInterval);

                        return (Int32)Math.Round(dmgHull);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                if (bullet != null)
                {
                    if (bulletInterval > 0)
                    {
                        double dmgHull = value * (bulletInterval / 1000.0);

                        bullet.dmgHull = (Int32)Math.Round(dmgHull);
                    }
                    else
                    {
                        throw new Exception("SpecLaser::hullDmgPerSecond - Invalid refire rate");
                    }
                }
                else
                {
                    throw new Exception("SpecLaser::hullDmgPerSecond - Invalid bullet");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Numeric refireRate
        {
            get
            {
                if (bulletInterval > 0)
                {
                    return 1000.0 / bulletInterval;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                if (value > 0)
                {
                    Double interval = 1000.0 / value;

                    bulletInterval = (Int32)Math.Round(interval);
                }
                else
                {
                    throw new Exception("SpecLaser::refireRate - Attempt to set invalid refire rate");
                }
            }
        }

        internal SpecLaser(IConfig config)
            : base(config)
        {
            model = "27";
            type = "SG_LASER";
            name = "SS_LASER";
        }

        internal SpecLaser(IConfig config, XmlNode specNode)
            : base(config, specNode)
        {
            minShipSize = getAttr(specNode, "minShipSize", minShipSize);
            maxShipSize = getAttr(specNode, "maxShipSize", maxShipSize);
            aoeClass = getAttr(specNode, "aoeClass", aoeClass);
            defaultBias = getAttr(specNode, "defaultBias", defaultBias);
            characteristics = getAttr(specNode, "characteristics", characteristics);
            bulletInterval = getAttr(specNode, "bulletInterval", bulletInterval);
            firingSoundId = getAttr(specNode, "firingSoundId", firingSoundId);
            bulletIndex = getAttr(specNode, "bulletIndex", bulletIndex);
            minEnergy = getAttr(specNode, "minEnergy", minEnergy);
            chargeRate = getAttr(specNode, "chargeRate", chargeRate);
            laserIcon = getAttr(specNode, "laserIcon", laserIcon);
        }

        public SpecLaser(IConfig config, List<String> datLine) :
            base(config, datLine)
        {
            int idx = 7;

            bulletInterval = datLine[idx++];
            firingSoundId = datLine[idx++];
            bulletIndex = datLine[idx++];
            minEnergy = datLine[idx++];
            chargeRate = datLine[idx++];
            laserIcon = datLine[idx++];
        }

        internal override XmlElement save(XmlDocument doc)
        {
            XmlElement specNode = doc.CreateElement(xmlName);

            save(specNode);

            specNode.SetAttribute("minShipSize", minShipSize);
            specNode.SetAttribute("maxShipSize", maxShipSize);
            specNode.SetAttribute("aoeClass", aoeClass);
            specNode.SetAttribute("defaultBias", defaultBias);
            specNode.SetAttribute("characteristics", characteristics);
            specNode.SetAttribute("bulletInterval", bulletInterval);
            specNode.SetAttribute("firingSoundId", firingSoundId);
            specNode.SetAttribute("bulletIndex", bulletIndex);
            specNode.SetAttribute("minEnergy", minEnergy);
            specNode.SetAttribute("chargeRate", chargeRate);
            specNode.SetAttribute("laserIcon", laserIcon);

            return specNode;
        }

        internal override List<String> getSpec()
        {
            List<String> rval=base.getSpec();

            rval.InsertRange(7, new String[] {
                bulletInterval,
                firingSoundId,
                bulletIndex,
                minEnergy,
                chargeRate,
                laserIcon
            });

            return rval;
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        public override string generateTooltip()
        {
            String tooltip =
                "Mount: " + type +
                "\nDamage Per Second (Shield/Hull): " + dmgPerSecond +
                "\nDamage Per Shot (Shield/Hull): " + dmgPerShot +
                "\nDamage Per Energy Unit (Shield/Hull): " + dmgPerEnergy +
                "\nRange: " + range +
                "\nRate of Fire (rpm): " + (refireRate*60).engineer() +
                "\nMaximum Bullets In Flight: " + maxBullets +
                "\nDefault Bias: " + defaultBias.ToString() +
                "\nCharacteristics: " + characteristics.ToString("X8");

            SpecBullet bullet = this.bullet;
            if (bullet != null)
            {
                if (bullet.usesAmmo)
                {
                    SpecWareTech ammo = bullet.ammo;

                    if (ammo != null)
                    {
                        tooltip += "\nAmmo: " + ammo.generateLabel()
                                + "\nAmmo Cost Per Second: " + (ammo.priceAverage * refireRate).ToString() + " Cr"
                                + "\nTime per unit of Cargo Space: " + ((200.0/refireRate)/ammo.wareSize).time();
                    }
                    else
                    {
                        tooltip += "\nAmmo: <invalid>\nCrates Per Second: " + (refireRate/200.0).ToString();
                    }
                }
                tooltip += "\nEnergy Cost Per Shot: " + (bullet.energyCost * 1000000.0).engineer() + "J"
                        +  "\nMean Energy Drain: " + (bullet.energyCost * refireRate*1000000.0).engineer() + "W";
            }

            String descr = base.generateTooltip();

            if (descr!=null)
            {
                tooltip += "\n" + descr;
            }

            return tooltip;
        }

    }
}
