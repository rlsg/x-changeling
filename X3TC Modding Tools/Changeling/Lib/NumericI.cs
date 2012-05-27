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
    internal class NumericI : Numeric
    {
        private Int32 value;

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
            return Int32.TryParse(text, out value);
        }

        public override bool TryParse(String text, IFormatProvider fp)
        {
            return Int32.TryParse(text, System.Globalization.NumberStyles.Number, fp, out value);
        }

        internal NumericI(Int32 value)
        {
            this.value = value;
        }

        public static implicit operator NumericI(NumericD instance)
        {
            return new NumericI(instance);
        }

        public static implicit operator NumericI(NumericFP instance)
        {
            return new NumericI(instance);
        }

        public static implicit operator NumericI(Int32 value)
        {
            return new NumericI(value);
        }

        public static implicit operator NumericI(String value)
        {
            return new NumericD(Int32.Parse(value, defaultFP));
        }

        public static explicit operator NumericI(Double value)
        {
            return new NumericI((Int32)Math.Round(value));
        }

        public static explicit operator NumericI(Decimal value)
        {
            return new NumericI((Int32)Math.Round(value));
        }

        protected override double fval
        {
            get
            {
                return value;
            }
            set
            {
                this.value = (Int32)value;
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
                this.value = (Int32)value;
            }
        }

        protected override int ival
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
    }
}
