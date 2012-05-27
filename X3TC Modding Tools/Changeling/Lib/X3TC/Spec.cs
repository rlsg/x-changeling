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
using System.Text.RegularExpressions;
using System.Xml;

namespace Changeling.Lib.X3TC
{
    /// <summary>
    /// This is the base class for all entries coming from TFiles
    /// </summary>
    abstract class Spec : ConfigDatum
    {
        public NumericI displayNameId = -1;
        public String name = "SS_WARE";
        public String type = "-1";

        public NumericFP rateX = 0.0f;
        public NumericFP rateY = 0.0f;
        public NumericFP rateZ = 0.0f;

        public NumericI wareSize = 0;
        public NumericI wareClass = -1;

        public String model = "-1";
        public NumericI pictureId = -1;
        public NumericI videoId = -1;
        public NumericI skinIndex = 0;              // Nominally 0

        public NumericI valuePlayer = 0;            // Nominally 50% of value
        public NumericI value = 0;                  // Average price
        public NumericI[] valueMod = { 1, 1 };
        public NumericI valueNotoriety = 0;         // Minimum Race Rep Requirement

        protected Spec(IConfig config) :
            base(config)
        {
        }

        protected Spec(Spec spec) :
            base(spec)
        {
            displayNameId = spec.displayNameId;
            name = spec.name;
            type = spec.type;
            rateX = spec.rateX;
            rateY = spec.rateY;
            rateZ = spec.rateZ;
            wareSize = spec.wareSize;
            wareClass = spec.wareClass;
            model = spec.model;
            pictureId = spec.pictureId;
            videoId = spec.videoId;
            skinIndex = spec.skinIndex;
            valuePlayer = spec.valuePlayer;
            value = spec.value;
            spec.valueMod.CopyTo(valueMod,0);
            valueNotoriety = spec.valueNotoriety;
        }

        protected Spec(IConfig config, List<String> dataList) :
            this(config)
        {
            {
                int idx = 0;

                model = dataList[idx++];
                pictureId = dataList[idx++];
                rateX = dataList[idx++];
                rateY = dataList[idx++];
                rateZ = dataList[idx++];
                type = dataList[idx++];
                displayNameId = dataList[idx++];
            }

            {
                int idx = dataList.Count()-1;

                name = dataList[--idx];
                skinIndex = dataList[--idx];
                videoId = dataList[--idx];
                valueNotoriety = dataList[--idx];
                valuePlayer = dataList[--idx];
                wareClass = dataList[--idx];
                valueMod[1] = dataList[--idx];
                valueMod[0] = dataList[--idx];
                value = dataList[--idx];
                wareSize = dataList[--idx];
            }
        }

        internal virtual List<String> getSpec()
        {
            List<String> rval = new List<String>();

            rval.Add(model);
            rval.Add(pictureId);
            rval.Add(rateX);
            rval.Add(rateY);
            rval.Add(rateZ);
            rval.Add(type);
            rval.Add(displayNameId);
            rval.Add(wareSize);
            rval.Add(value);
            rval.Add(valueMod[0]);
            rval.Add(valueMod[1]);
            rval.Add(wareClass);
            rval.Add(valuePlayer);
            rval.Add(valueNotoriety);
            rval.Add(videoId);
            rval.Add(skinIndex);
            rval.Add(name);

            return rval;
        }

        protected Spec(IConfig config, System.Xml.XmlNode specNode) :
            this(config)
        {
            name = getAttr(specNode, "name", name);
            model = getAttr(specNode, "model", model);
            pictureId = getAttr(specNode, "pictureId", pictureId);
            rateX = getAttr(specNode, "rateX", rateX);
            rateY = getAttr(specNode, "rateY", rateY);
            rateZ = getAttr(specNode, "rateZ", rateZ);
            type = getAttr(specNode, "type", type);
            displayNameId = getAttr(specNode, "displayNameId", displayNameId);
            wareSize = getAttr(specNode, "wareSize", wareSize);
            value = getAttr(specNode, "value", value);
            valueMod[0] = getAttr(specNode, "valueMod1", valueMod[0]);
            valueMod[1] = getAttr(specNode, "valueMod2", valueMod[1]);
            wareClass = getAttr(specNode, "wareClass", wareClass);
            valuePlayer = getAttr(specNode, "valuePlayer", valuePlayer);
            valueNotoriety = getAttr(specNode, "valueNotoriety", valueNotoriety);
            videoId = getAttr(specNode, "videoId", videoId);
            skinIndex = getAttr(specNode, "skinIndex", skinIndex);
        }

        protected void save(XmlElement specNode)
        {
            specNode.SetAttribute("name", name);
            specNode.SetAttribute("model", model);
            specNode.SetAttribute("pictureId", pictureId);
            specNode.SetAttribute("rateX", rateX);
            specNode.SetAttribute("rateY", rateY);
            specNode.SetAttribute("rateZ", rateZ);
            specNode.SetAttribute("type", type);
            specNode.SetAttribute("displayNameId", displayNameId);
            specNode.SetAttribute("wareSize", wareSize);
            specNode.SetAttribute("value", value);
            specNode.SetAttribute("valueMod1", valueMod[0]);
            specNode.SetAttribute("valueMod2", valueMod[1]);
            specNode.SetAttribute("wareClass", wareClass);
            specNode.SetAttribute("valuePlayer", valuePlayer);
            specNode.SetAttribute("valueNotoriety", valueNotoriety);
            specNode.SetAttribute("videoId", videoId);
            specNode.SetAttribute("skinIndex", skinIndex);
        }

        public override string generateName()
        {
            return name;
        }

        public string getText(int textId)
        {
            return config.getText(17, textId);
        }

        public override string generateLabel()
        {
            String label = getText(displayNameId);

            if (label == null)
            {
                label = name;
            }

            return label;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateTooltip()
        {
            string descr=getText(displayNameId + 1);

            if (descr != null)
            {
                
                string[] lines = descr.Split('\n');

                String tooltip = "Description:-";
                foreach (String line in lines)
                {
                    const int maxLineLen = 60;
                    String temp = line;
                    while (temp.Length > maxLineLen)
                    {
                        string subline = temp.Substring(0, (temp.Length < maxLineLen ? temp.Length : maxLineLen));
                        int whitePos = subline.LastIndexOf(' ');
                        if (whitePos > 0)
                        {
                            subline = subline.Substring(0, whitePos + 1);
                        }
                        tooltip += "\n  "+subline;
                        temp = temp.Substring(subline.Length);
                    }
                    tooltip += "\n  " + temp;
                }

                return tooltip;
            }
            else
            {
                return null;
            }
        }
    }
}
