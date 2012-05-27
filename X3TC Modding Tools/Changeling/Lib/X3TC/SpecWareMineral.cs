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
    class SpecWareMineral : SpecWare
    {
        internal const String xmlName = "SpecWareMineral";
        protected override Numeric priceAverageScale { get { return 5.25M; } }

        SpecWareMineral(IConfig config) :
            base(config)
        {
            model = "180";
        }

        public SpecWareMineral(IConfig config, List<string> datLine) :
            base(config, datLine)
        {
        }

        public SpecWareMineral(IConfig config, XmlElement xmlNode) :
            base(config, xmlNode)
        {
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        internal override System.Xml.XmlElement save(System.Xml.XmlDocument xmlDoc)
        {
            XmlElement specNode = xmlDoc.CreateElement(xmlName);

            base.save(specNode);

            return specNode;
        }
    }
}
