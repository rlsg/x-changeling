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
    class SpecCockpit : Spec
    {
        internal const String xmlName = "SpecCockpit";
        internal string modelScene;
        internal NumericI maskLaser;

        internal List<String> mountLaserList
        {
            get
            {
                return SpecLaser.getTypes(maskLaser);
            }
            set
            {
                maskLaser = SpecLaser.getBitMask(value);
            }
        }

        SpecCockpit(IConfig config) :
            base(config)
        {
            model = "28";
        }

        public SpecCockpit(IConfig config, List<string> datLine) :
            base(config, datLine)
        {
            int idx = 7;

            modelScene = datLine[idx++];
            maskLaser = datLine[idx++];
        }

        internal override List<string> getSpec()
        {
            List<string> rval=base.getSpec();

            rval.InsertRange(7, new String[] {
                modelScene,
                maskLaser
            });

            return rval;
        }

        public SpecCockpit(IConfig config, XmlElement xmlNode) :
            base(config, xmlNode)
        {
            modelScene = getAttr(xmlNode, "modelScene", modelScene);
            maskLaser = getAttr(xmlNode, "maskLaser", maskLaser);
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        internal override System.Xml.XmlElement save(System.Xml.XmlDocument xmlDoc)
        {
            XmlElement specNode = xmlDoc.CreateElement(xmlName);

            base.save(specNode);

            specNode.SetAttribute("modelScene", modelScene);
            specNode.SetAttribute("maskLaser", maskLaser.ToString());

            return specNode;
        }

        public override string generateTooltip()
        {
            String tooltip="Compatable Lasers:-";

            List<String> laserTypes = mountLaserList;
            if (laserTypes.Count > 0)
            {
                foreach (String laserMount in laserTypes)
                {
                    tooltip += "\n    " + laserMount;
                }
            }
            else
            {
                tooltip += "\n    None";
            }

            return tooltip;
        }
    }
}
