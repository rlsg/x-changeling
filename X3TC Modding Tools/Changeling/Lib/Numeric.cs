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
    /// <summary>
    /// This is a wrapper class to support the default
    /// implicit string conversion to be in the
    /// engineering format.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Engineer<T> where T : Numeric
    {
        private T value;

        public Engineer(T value)
        {
            this.value = value;
        }

        public static implicit operator String(Engineer<T> instance)
        {
            return instance.value.engineer();
        }

        public static implicit operator Engineer<T>(T value)
        {
            return new Engineer<T>(value);
        }

        public static implicit operator T(Engineer<T> instance)
        {
            return instance.value;
        }
    }

    /// <summary>
    /// This is a data type neutral representation of a numeric value
    /// </summary>
    abstract class Numeric 
    {
        /// <summary>
        /// Array containing the time units supported by this class
        /// </summary>
        private readonly static string[] timeUnits = { "ms", "s", "min", "hr" };
        /// <summary>
        /// Array containing the engineering suffixes supported by this class
        /// </summary>
        private readonly static string[] engineerUnits = { "", "K", "M", "G", "T", "P" };
        /// <summary>
        /// The instance of the IFormatProvider interface that will be used for
        /// all implicit data conversions to/from the string data type
        /// </summary>
        protected static IFormatProvider defaultFP = new System.Globalization.CultureInfo("en-GB", false);

        /// <summary>
        /// The locale to be used for implicit conversion to/from string data (default "en-GB")
        /// </summary>
        internal static String locale
        {
            get
            {
                if (defaultFP is System.Globalization.CultureInfo)
                {
                    System.Globalization.CultureInfo cultureInfo = defaultFP as System.Globalization.CultureInfo;

                    return cultureInfo.Name;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                defaultFP = new System.Globalization.CultureInfo(value, false);
            }
        }

        /// <summary>
        /// Property used to access/modify the underlying numeric data using an IEEE 64-bit Floating Point representation
        /// consisting of a normalised 48-bit signed mantissa and a 16-bit signed exponent
        /// </summary>
        protected abstract Double fval { get; set; }
        /// <summary>
        /// Property used to access/modify the underlying numeric data using a decimal representation consisting of a
        /// 96-bit signed integer and a decimal point position indicator (0 to 28)
        /// </summary>
        protected abstract Decimal dval { get; set; }
        /// <summary>
        /// Property used to access/modify the underlying numeric data using a standard 32-bit signed integer representation
        /// </summary>
        protected abstract Int32 ival { get; set; }

        /// <summary>
        /// This function allows for the generic explicit conversion of
        /// the underlying value into an appropriate textual representation
        /// using the system default locale
        /// </summary>
        /// <returns></returns>
        public abstract override string ToString();
        /// <summary>
        /// This function allows for the generic explicit conversion of
        /// the underlying value into an appropriate textual representation
        /// using the system default locale
        /// </summary>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public abstract string ToString(String fmt);
        /// <summary>
        /// This function allows for the generic explicit conversion of
        /// the underlying value into an appropriate textual representation
        /// using the given locale
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="fp"></param>
        /// <returns></returns>
        public abstract string ToString(String fmt, IFormatProvider fp);
        /// <summary>
        /// This function allows for the generic explicit conversion of
        /// the underlying value into an appropriate textual representation
        /// using the given locale
        /// </summary>
        /// <param name="fp"></param>
        /// <returns></returns>
        public abstract string ToString(IFormatProvider fp);
        /// <summary>
        /// This function allows for the generic explicit conversion of
        /// textual data into the current underlying numeric data type
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract bool TryParse(String text);
        /// <summary>
        /// This function allows for the generic explicit conversion of
        /// textual data into the current underlying numeric data type
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fp"></param>
        /// <returns></returns>
        public abstract bool TryParse(String text, IFormatProvider fp);

        /// <summary>
        /// Implicit initialisation of a generic instance of
        /// Numeric with a Double value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Numeric(Double value)
        {
            return new NumericFP(value);
        }

        /// <summary>
        /// Implicit initialisation of a generic instance of
        /// Numeric with a Decimal value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Numeric(Decimal value)
        {
            return new NumericD(value);
        }

        /// <summary>
        /// Implicit initialisation of a generic instance of
        /// Numeric with an Integer value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static implicit operator Numeric(Int32 value)
        {
            return new NumericI(value);
        }

        /// <summary>
        /// Implicit usage of a generic instance of
        /// Numeric as a Double value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static implicit operator Double(Numeric instance)
        {
            return instance.fval;
        }

        /// <summary>
        /// Implicit usage of a generic instance of
        /// Numeric as a Decimal value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static implicit operator Decimal(Numeric instance)
        {
            return instance.dval;
        }

        /// <summary>
        /// Implicit usage of a generic instance of
        /// Numeric as an Integer value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static implicit operator Int32(Numeric instance)
        {
            return instance.ival;
        }

        /// <summary>
        /// Implicit conversion of a generic instance of
        /// Numeric to an appropriate string representation
        /// of the underlying value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static implicit operator String(Numeric instance)
        {
            return instance.ToString(defaultFP);
        }

        /// <summary>
        /// Addition support with automatic type conversion
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Numeric operator +(Numeric a, Numeric b)
        {
            if (a is NumericFP || b is NumericFP)
            {
                return a.fval + b.fval;
            }
            else if (a is NumericD || b is NumericD)
            {
                return a.dval + b.dval;
            }
            else
            {
                return a.ival + b.ival;
            }
        }

        /// <summary>
        /// Subtraction support with automatic type conversion
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Numeric operator -(Numeric a, Numeric b)
        {
            if (a is NumericFP || b is NumericFP)
            {
                return a.fval - b.fval;
            }
            else if (a is NumericD || b is NumericD)
            {
                return a.dval - b.dval;
            }
            else
            {
                return a.ival - b.ival;
            }
        }

        /// <summary>
        /// Multiplication support with automatic type conversion
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Numeric operator *(Numeric a, Numeric b)
        {
            if (a is NumericFP || b is NumericFP)
            {
                return a.fval * b.fval;
            }
            else if (a is NumericD || b is NumericD)
            {
                return a.dval * b.dval;
            }
            else
            {
                return a.ival * b.ival;
            }
        }

        /// <summary>
        /// Division support with automatic type conversion
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Numeric operator /(Numeric a, Numeric b)
        {
            if (a is NumericFP || b is NumericFP)
            {
                return a.fval / b.fval;
            }
            else if (a is NumericD || b is NumericD)
            {
                return a.dval / b.dval;
            }
            else
            {
                return a.ival / b.ival;
            }
        }

        /// <summary>
        /// Modulo support with automatic type conversion
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Numeric operator %(Numeric a, Numeric b)
        {
            if (a is NumericFP || b is NumericFP)
            {
                return a.fval % b.fval;
            }
            else if (a is NumericD || b is NumericD)
            {
                return a.dval % b.dval;
            }
            else
            {
                return a.ival % b.ival;
            }
        }

        /// <summary>
        /// Explicit truncation to integer of the generic numeric value
        /// </summary>
        /// <returns></returns>
        public NumericI Truncate()
        {
            if (this is NumericFP)
            {
                return (NumericI)Math.Truncate(fval);
            }
            else if (this is NumericD)
            {
                return (NumericI)Math.Truncate(dval);
            }
            else
            {
                return ival;
            }
        }

        /// <summary>
        /// Explicit rounding to nearest integer of the generic numeric value
        /// </summary>
        /// <returns></returns>
        public NumericI Round()
        {
            if (this is NumericFP)
            {
                return (NumericI)Math.Round(fval);
            }
            else if (this is NumericD)
            {
                return (NumericI)Math.Round(dval);
            }
            else
            {
                return ival;
            }
        }

        /// <summary>
        /// Generation of an abbreviated textual engineering
        /// representation from the given generic numeric value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static String engineer(Numeric instance)
        {
            return instance.engineer();
        }

        /// <summary>
        /// Generation of an abbreviated textual engineering
        /// representation of the underlying numeric data
        /// </summary>
        /// <returns></returns>
        public String engineer()
        {
            String rval = "0";
            double value = fval;
            double absVal = Math.Abs(value);

            if (absVal > 0.01)
            {
                int unitsIdx = Math.Max(0, (int)Math.Truncate(Math.Log10(absVal) / 3));

                rval = (value / (Math.Pow(1000, unitsIdx))).ToString("###.#") + engineerUnits[unitsIdx];
            }

            return rval;
        }

        /// <summary>
        /// Generation of an abbreviated textual time
        /// representation from the given generic numeric
        /// value
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static String time(Numeric instance)
        {
            return instance.time();
        }

        /// <summary>
        /// Generation of an abbreviated textual time
        /// representation of the underlying numeric data
        /// </summary>
        /// <returns></returns>
        public String time()
        {
            String rval = "-";
            double value = fval;

            if (value >= 0)
            {
                rval = "(instant)";

                if (value > 0.001)
                {
                    int unitsIdx = 1;

                    if (value < 1)
                    {
                        value *= 1000;
                        unitsIdx = 0;
                        rval = value.ToString("###.#");
                    }
                    else
                    {
                        if (value > 60)
                        {
                            value /= 60;
                            unitsIdx++;

                            if (value > 60)
                            {
                                value /= 60;
                                unitsIdx++;
                            }
                        }
                        rval = value.ToString("##.##");
                    }

                    rval += timeUnits[unitsIdx];
                }
            }

            return rval;
        }

    }
}
