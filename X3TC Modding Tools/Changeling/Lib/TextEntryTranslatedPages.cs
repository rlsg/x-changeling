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
    class TextEntryTranslatedPage : Dictionary<int, TextEntryTranslated>
    {
        public TextEntryTranslated getEntry(int id)
        {
            if (this.Keys.Contains(id))
            {
                return this[id];
            }
            else
            {
                return null;
            }
        }

        internal XmlElement save(XmlDocument doc, int locale, int fileid)
        {
            XmlElement pageXml = doc.CreateElement("page");

            foreach (KeyValuePair<int, TextEntryTranslated> entry in this)
            {
                XmlElement entryXml = entry.Value.save(doc, locale, fileid);

                if (entryXml != null)
                {
                    entryXml.SetAttribute("id", entry.Key.ToString());
                    pageXml.AppendChild(entryXml);
                }
            }

            if (!pageXml.HasChildNodes)
            {
                pageXml = null;
            }

            return pageXml;
        }

        internal XmlElement saveDefined(XmlDocument doc, int locale, int fileid)
        {
            XmlElement pageXml = doc.CreateElement("page");

            foreach (KeyValuePair<int, TextEntryTranslated> entry in this)
            {
                XmlElement entryXml = entry.Value.saveDefined(doc, locale, fileid);

                if (entryXml != null)
                {
                    entryXml.SetAttribute("id", entry.Key.ToString());
                    pageXml.AppendChild(entryXml);
                }
            }

            if (!pageXml.HasChildNodes)
            {
                pageXml = null;
            }

            return pageXml;
        }
    }
}
