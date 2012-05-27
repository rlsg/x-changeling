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
    /// <summary>
    /// 
    /// </summary>
    class CriteriaLaser : ConfigDatum
    {
        internal const String xmlName = "CriteriaLaser";

        public NumericI laserIndex = -1;
        public NumericI bias = 0;

        /// <summary>
        /// 
        /// </summary>
        public SpecLaser laser
        {
            get
            {
                return config.getLaser(laserIndex);
            }
            set
            {
                laserIndex = config.getIndexOf(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        internal CriteriaLaser(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="critNode"></param>
        internal CriteriaLaser(IConfig config, XmlNode critNode)
            : base(config)
        {
            laserIndex = getAttr(critNode, "laserIndex", laserIndex);
            bias = getAttr(critNode, "bias", bias);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        internal override XmlElement save(XmlDocument doc)
        {
            XmlElement critNode = doc.CreateElement(xmlName);

            critNode.SetAttribute("laserIndex", laserIndex);
            critNode.SetAttribute("bias", bias);

            return critNode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool isValid()
        {
            return config.validate(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateName()
        {
            SpecLaser spec = config.getLaser(laserIndex);

            return spec.name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateLabel()
        {
            SpecLaser spec = config.getLaser(laserIndex);

            return spec.generateLabel() + " ["+bias.ToString()+"]";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateTooltip()
        {
            return null;
        }
    }
}
