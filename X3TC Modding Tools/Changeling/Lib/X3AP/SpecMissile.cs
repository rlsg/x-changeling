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

namespace Changeling.Lib.X3AP
{
    class SpecMissile : X3TC.SpecMissile
    {
        internal new readonly static String[] typeStrings ={
            "SG_MISSILE_LIGHT",             // 00000001
            "SG_MISSILE_MEDIUM",            // 00000002
            "SG_MISSILE_HEAVY",             // 00000004
            "SG_MISSILE_TR_LIGHT",          // 00000008
            "SG_MISSILE_TR_MEDIUM",         // 00000010
            "SG_MISSILE_TR_HEAVY",          // 00000020
            "SG_MISSILE_KHAAK",             // 00000040
            "SG_MISSILE_BOMBER",            // 00000080
            "SG_MISSILE_TORP_CAPITAL",      // 00000100
            "SG_MISSILE_AF_CAPITAL",        // 00000200
            "SG_MISSILE_TR_BOMBER",         // 00000400
            "SG_MISSILE_TR_TORP_CAPITAL",   // 00000800
            "SG_MISSILE_TR_AF_CAPITAL",     // 00001000
            "SG_MISSILE_BOARDINGPOD",       // 00002000
            "SG_MISSILE_DMBF",              // 00004000
            "SG_MISSILE_COUNTER"            // 00008000
        };

        internal new static List<String> getTypes(int typeBitMask = -1)
        {
            List<String> rlist = new List<string>();

            for (int bit = 0; bit < typeStrings.Length; bit++)
            {
                if ((typeBitMask & (1 << bit)) != 0)
                {
                    rlist.Add(typeStrings[bit]);
                }
            }

            return rlist;
        }

        internal new static int getBitMask(IEnumerable<String> types)
        {
            int mask = 0;

            for (int bit = 0; bit < typeStrings.Length; bit++)
            {
                if (types.Contains(typeStrings[bit]))
                {
                    mask |= (1 << bit);
                }
            }

            return mask;
        }

        internal SpecMissile(IConfig config)
            : base(config)
        {
        }

        internal SpecMissile(IConfig config, XmlNode specNode)
            : base(config, specNode)
        {
        }

        public SpecMissile(IConfig config, List<String> datLine) :
            base(config, datLine)
        {
        }
    }
}
