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

namespace Changeling.Lib
{
    class TextEntryTranslated : SortedDictionary<int, TextEntry>
    {
        public TextEntry getRawEntry(int locale)
        {
            if (this.Keys.Contains(locale))
            {
                return this[locale];
            }
            else
            {
                return null;
            }
        }

        public TextEntry getEntry(int locale)
        {
            if (this.Keys.Contains(locale))
            {
                return this[locale];
            }
            else if (this.Keys.Contains(-1))
            {
                return this[-1];
            }
            else
            {
                return null;
            }
        }

        public string getRawText(int locale)
        {
            TextEntry entry = getRawEntry(locale);

            if (entry != null)
            {
                return entry.getText();
            }
            else
            {
                return null;
            }
        }

        public string getText(int locale)
        {
            TextEntry entry = getEntry(locale);

            if (entry != null)
            {
                return entry.getText();
            }
            else
            {
                return null;
            }
        }

        public String whereDefined(int locale)
        {
            String suffix = "";
            TextEntry entry = null;

            if (this.Keys.Contains(locale))
            {
                entry=this[locale];
                suffix = "-L" + locale.ToString();
            }
            else if (this.Keys.Contains(-1))
            {
                entry = this[-1];
            }

            if (entry != null)
            {
                int file=entry.whereDefined();

                if (file >= 0)
                {
                    return file.ToString("04") + suffix;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        internal XmlElement save(XmlDocument doc, int locale, int fileid)
        {
            XmlElement entryXml = null;
            TextEntry entryDefinitions = getRawEntry(locale);

            if (entryDefinitions != null)
            {
                String text=entryDefinitions.getText(fileid);

                if (text != null)
                {
                    entryXml = doc.CreateElement("t");
                    entryXml.InnerText = text;
                }
            }

            return entryXml;
        }

        internal XmlElement saveDefined(XmlDocument doc, int locale, int fileid)
        {
            XmlElement entryXml = null;
            TextEntry entryDefinitions = getRawEntry(locale);

            if (entryDefinitions != null)
            {
                if (fileid == entryDefinitions.whereDefined())
                {
                    String text = entryDefinitions.getText();

                    if (text != null)
                    {
                        entryXml = doc.CreateElement("t");
                        entryXml.InnerText = text;
                    }
                }
            }

            return entryXml;
        }
    }
}
