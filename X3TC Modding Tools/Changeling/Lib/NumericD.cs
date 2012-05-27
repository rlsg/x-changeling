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

namespace Changeling.Lib
{
    internal class NumericD : Numeric
    {
        private Decimal value;

        public override string ToString()
        {
            return value.ToString();
        }

        public override string ToString(String fmt)
        {
            return value.ToString(fmt);
        }

        public override string ToString(String fmt, IFormatProvider fp)
        {
            return value.ToString(fmt, fp);
        }

        public override string ToString(IFormatProvider fp)
        {
            return value.ToString(fp);
        }

        public override bool TryParse(String text)
        {
            return Decimal.TryParse(text, out value);
        }

        public override bool TryParse(String text, IFormatProvider fp)
        {
            return Decimal.TryParse(text, System.Globalization.NumberStyles.Number, fp, out value);
        }

        internal NumericD(Decimal value)
        {
            this.value = value;
        }

        public static implicit operator NumericD(NumericI instance)
        {
            return new NumericD(instance);
        }

        public static implicit operator NumericD(NumericFP instance)
        {
            return new NumericD(instance);
        }

        public static implicit operator NumericD(Decimal value)
        {
            return new NumericD(value);
        }

        public static implicit operator NumericD(String value)
        {
            return new NumericD(Decimal.Parse(value, defaultFP));
        }

        public static explicit operator NumericD(Int32 value)
        {
            return new NumericD(value);
        }

        public static explicit operator NumericD(Double value)
        {
            return new NumericD((Decimal)value);
        }

        protected override double fval
        {
            get
            {
                return (double)value;
            }
            set
            {
                this.value = (Decimal)value;
            }
        }

        protected override decimal dval
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        protected override int ival
        {
            get
            {
                return (int)value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
