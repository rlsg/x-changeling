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

namespace Changeling.Lib.X3AP
{
    class SpecBullet : X3TC.SpecBullet
    {
        public new IConfig config
        {
            get
            {
                return base.config as IConfig;
            }
        }

        public NumericI dmgShieldOOS = 0;
        public NumericI dmgHullOOS = 0;

        SpecBullet(IConfig config) :
            base(config)
        {
        }

        SpecBullet(SpecBullet spec) :
            base(spec)
        {
            dmgHullOOS = spec.dmgHullOOS;
            dmgShieldOOS = spec.dmgShieldOOS;
        }

        SpecBullet(X3TC.SpecBullet spec) :
            base(spec)
        {
            dmgHullOOS = spec.dmgHull;
            dmgShieldOOS = spec.dmgShield;
        }

        internal SpecBullet(IConfig config, XmlNode specNode)
            : base(config, specNode)
        {
            dmgShieldOOS = getAttr(specNode, "dmgShieldOOS", dmgShield);
            dmgHullOOS = getAttr(specNode, "dmgHullOOS", dmgHull);
        }

        internal override XmlElement save(XmlDocument xmlDoc)
        {
            XmlElement specNode = xmlDoc.CreateElement(xmlName);

            base.save(specNode);

            specNode.SetAttribute("dmgShieldOOS", dmgShieldOOS.ToString());
            specNode.SetAttribute("dmgHullOOS", dmgHull.ToString());

            return specNode;
        }

        public SpecBullet(IConfig config, List<String> datLine) :
            base(config, datLine)
        {
            int idx = 33;

            dmgShieldOOS = datLine[idx++];
            dmgHullOOS = datLine[idx++];
        }

        internal override List<String> getSpec()
        {
            List<String> rval = base.getSpec();

            rval.InsertRange(33, new String[] {
                dmgShieldOOS,
                dmgHullOOS,
            });

            return rval;
        }


        public override string generateTooltip()
        {
            String tooltip =
                "\nDamage Per Energy Unit (Shield/Hull): " + dmgPerEnergy +
                (isBeam ? "" : "\nSpeed: " + speed) +
                "Damage Per Shot (Shield/Hull): " + dmgPerShot +
                (isBeam ? "\nBeam Damage Rate (Shield/Hull): " + beamDmgRate : "") +
                "\nDamage Per Energy Unit (Shield/Hull): " + dmgPerEnergy +
                (isBeam ? "" : "\nSpeed: " + speed) +
                "\nRange: " + range +
                (isBeam ? "\nBeam Duration: " : "\nTime to Range: ") + timeToRange +
                "\nEnergy Cost: " + energyCost.engineer();

            X3TC.SpecWareTech ammo = this.ammo;

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
