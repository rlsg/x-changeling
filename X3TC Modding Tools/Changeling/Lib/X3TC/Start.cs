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
    class Start : ConfigDatum
    {
        public TextEntryTranslated title=new TextEntryTranslated();
        public TextEntryTranslated description = new TextEntryTranslated();
        public TextEntryTranslated playerName = new TextEntryTranslated();
        public NumericI raceIndex = null;
        public NumericI gender = null;
        public ShipLoadout ship;

        internal Start(IConfig config) :
            base(config)
        {
            ship = new ShipLoadout(config);
        }

        public Start(IConfig config, XmlElement xmlNode) :
            this(config)
        {
        }

        public override bool isValid()
        {
            return config.validate(this);
        }

        public override string generateName()
        {
            return title.getText(44);
        }

        public override string generateLabel()
        {
            return title.getText(config.getLocale());
        }

        public override string generateTooltip()
        {
            int locale = config.getLocale();
            String tooltip="";

            String playerName = description.getText(locale);
            String descriptionText = description.getText(locale);

            if (playerName != null)
            {
                tooltip += "Name: " + playerName + "\n";
            }

            if (descriptionText != null)
            {
                string[] lines = descriptionText.Split('\n');

                tooltip += "Description:-";
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
                        tooltip += "\n  " + subline;
                        temp = temp.Substring(subline.Length);
                    }
                    tooltip += "\n  " + temp;
                }
            }

            return tooltip;
        }

        internal override System.Xml.XmlElement save(System.Xml.XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }
    }
}
