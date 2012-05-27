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
    class CriteriaMissile : ConfigDatum
    {
        internal const String xmlName = "CriteriaMissile";

        public NumericI missileIndex=-1;

        /// <summary>
        /// 
        /// </summary>
        public SpecMissile missile
        {
            get
            {
                return config.getMissile(missileIndex);
            }
            set
            {
                missileIndex = config.getIndexOf(value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        internal CriteriaMissile(IConfig config)
            : base(config)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="critNode"></param>
        internal CriteriaMissile(IConfig config, XmlNode critNode)
            : base(config)
        {
            missileIndex = getAttr(critNode, "missileIndex", missileIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        internal override XmlElement save(XmlDocument doc)
        {
            XmlElement critNode = doc.CreateElement(xmlName);

            critNode.SetAttribute("missileIndex", missileIndex.ToString());

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
            SpecMissile spec = config.getMissile(missileIndex);

            return spec.generateName();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string generateLabel()
        {
            SpecMissile spec=config.getMissile(missileIndex);

            return spec.generateLabel();
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
