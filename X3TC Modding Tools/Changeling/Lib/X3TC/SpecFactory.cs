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
    class SpecFactory : SpecWare
    {
        internal const String xmlName = "SpecFactory";

        protected override Numeric priceAverageScale
        {
            get
            {
                return 80.7212M;
            }
        }

        public NumericI soundId;
        public NumericFP dockingDistance;
        public NumericFP rendezvousDistance;
        public NumericI soundVolMax;
        public string sceneExternal;
        public string sceneInternal;
        public NumericI raceId;
        public NumericI effectExplodeId;
        public NumericI modelDeadId;
        public NumericI shieldGenerator;
        public NumericI stationSize;
        public string stationSymbol;

        SpecFactory(IConfig config) :
            base(config)
        {

        }

        public SpecFactory(IConfig config, List<string> datLine) :
            base(config, datLine)
        {
            int idx = 7;

            soundId = datLine[idx++];
            dockingDistance = datLine[idx++];
            rendezvousDistance = datLine[idx++];
            soundVolMax = datLine[idx++];
            sceneExternal = datLine[idx++];
            sceneInternal = datLine[idx++];
            raceId = datLine[idx++];
            effectExplodeId = datLine[idx++];
            modelDeadId = datLine[idx++];
            shieldGenerator = datLine[idx++];
            stationSize = datLine[idx++];
            stationSymbol = datLine[idx++];
        }

        internal override List<string> getSpec()
        {
            List<String> rval=base.getSpec();

            rval.InsertRange(7, new String[] {
                soundId,
                dockingDistance,
                rendezvousDistance,
                soundVolMax,
                sceneExternal,
                sceneInternal,
                raceId,
                effectExplodeId,
                modelDeadId,
                shieldGenerator,
                stationSize,
                stationSymbol
            });

            return rval;
        }

        public SpecFactory(IConfig config, XmlElement specNode) :
            base(config, specNode)
        {
            soundId = getAttr(specNode, "soundId", soundId);
            dockingDistance = getAttr(specNode, "dockingDistance", dockingDistance);
            rendezvousDistance = getAttr(specNode, "rendezvousDistance", rendezvousDistance);
            soundVolMax = getAttr(specNode, "soundVolMax", soundVolMax);
            sceneExternal = getAttr(specNode, "sceneExternal", sceneExternal);
            sceneInternal = getAttr(specNode, "sceneInternal", sceneInternal);
            raceId = getAttr(specNode, "raceId", raceId);
            effectExplodeId = getAttr(specNode, "effectExplodeId", effectExplodeId);
            modelDeadId = getAttr(specNode, "modelDeadId", modelDeadId);
            shieldGenerator = getAttr(specNode, "shieldGenerator", shieldGenerator);
            stationSize = getAttr(specNode, "stationSize", stationSize);
            stationSymbol = getAttr(specNode, "stationSymbol", stationSymbol);
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        internal override System.Xml.XmlElement save(System.Xml.XmlDocument xmlDoc)
        {
            XmlElement specNode = xmlDoc.CreateElement(xmlName);

            base.save(specNode);

            specNode.SetAttribute("soundId", soundId);
            specNode.SetAttribute("dockingDistance", dockingDistance);
            specNode.SetAttribute("rendezvousDistance", rendezvousDistance);
            specNode.SetAttribute("soundVolMax", soundVolMax);
            specNode.SetAttribute("sceneExternal", sceneExternal);
            specNode.SetAttribute("sceneInternal", sceneInternal);
            specNode.SetAttribute("raceId", raceId);
            specNode.SetAttribute("effectExplodeId", effectExplodeId);
            specNode.SetAttribute("modelDeadId", modelDeadId);
            specNode.SetAttribute("shieldGenerator", shieldGenerator);
            specNode.SetAttribute("stationSize", stationSize);
            specNode.SetAttribute("stationSymbol", stationSymbol);

            return specNode;
        }

        public override string generateLabel()
        {
            String label = base.generateLabel();

            if (label != null)
            {
                String suffix = null;
                    
                switch ((int)stationSize)
                {
                case 1:
                    break;

                case 2:
                    suffix=config.getText(12, 502);
                    break;

                case 5:
                    suffix = config.getText(12, 503);
                    break;

                case 10:
                    suffix = config.getText(12, 504);
                    break;

                case 20:
                    suffix = config.getText(12, 505);
                    break;

                default:
                    break;
                }

                if (suffix != null)
                {
                    label += " " + suffix;
                }
            }

            return label;
        }

        public override void generateSubItems(System.Windows.Forms.ListViewItem.ListViewSubItemCollection subItems)
        {
            base.generateSubItems(subItems);

            if (raceId > 0)
            {
                subItems.Add(config.getText(1266, raceId));
            }
            else
            {
                subItems.Add("");
            }

            subItems.Add(sceneExternal);
            subItems.Add(type);
        }
    }
}
