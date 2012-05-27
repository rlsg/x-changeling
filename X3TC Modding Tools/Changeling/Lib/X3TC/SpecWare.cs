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
    /// This is a derivative of the generic base class for TFile entries
    /// which includes support for the calculation of value of the
    /// represented item and display of said information as a tooltip.
    /// </summary>
    abstract class SpecWare : Spec
    {
        /// <summary>
        /// This property is used to specify the offset that is used
        /// to calculate the cost of the ware using the basic default
        /// price calculation formula (default of 0)
        /// </summary>
        protected virtual Numeric priceAverageOffset { get { return 0; } }
        /// <summary>
        /// This property is used to specify the scale that is used
        /// to calculate the cost of the ware using the basic default
        /// price calculation formula
        /// </summary>
        protected abstract Numeric priceAverageScale { get; }
        /// <summary>
        /// This property is used to convert between the average in-game
        /// price of a ware and it's cost calculation parameters.
        /// </summary>
        public virtual Numeric priceAverage
        {
            get
            {
                return (priceAverageOffset+(this.value * priceAverageScale)).Truncate();
            }
            set
            {
                this.value = ((value-priceAverageOffset) / priceAverageScale).Round();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public SpecWare(IConfig config) :
            base(config)
        {
            // Deliberatly does nothing
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="datLine"></param>
        public SpecWare(IConfig config, List<string> datLine) :
            base(config, datLine)
        {
            // Deliberatly does nothing
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="xmlNode"></param>
        public SpecWare(IConfig config, XmlNode xmlNode) :
            base(config, xmlNode)
        {
            // Deliberatly does nothing
        }

        /// <summary>
        /// Prefixes the default TFile entry tooltip with
        /// ware internal identifier, size, and price
        /// information
        /// </summary>
        /// <returns></returns>
        public override string generateTooltip()
        {

            String tooltip = "Internal Name: " + name;

            String wareClassText = config.getWareClassText(wareClass);

            if (wareClassText != null)
            {
                tooltip += "\nWare Size: " + wareSize.ToString() + " " + wareClassText;
            }

            tooltip += "\nPrice (Average): " + priceAverage.engineer() + " Cr";

            String descr = base.generateTooltip();

            if (descr != null)
            {
                tooltip += "\n" + descr;
            }
            
            return tooltip;
        }
    }
}
