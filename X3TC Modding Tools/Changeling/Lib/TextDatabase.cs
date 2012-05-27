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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DS.X2Core;

namespace Changeling.Lib
{
    class TextDictionary : SortedDictionary<int, TextEntryTranslatedPage>
    {
        const string xmlNodeName = "TextDatabase";
        const string xmlLocaleNodeName = "Locale";
        const string xmlFileNodeName = "Book";
        const int PAGE_BASE = 10000;
        const int PAGE_X3R = 30 * PAGE_BASE;
        const int PAGE_X3TC = 35 * PAGE_BASE;

        internal static XmlElement createTextEntry(XmlDocument doc, Int32 id, String text)
        {
            XmlElement entry = doc.CreateElement("t");

            entry.SetAttribute("id", id.ToString());
            entry.InnerText = text;

            return entry;
        }

        internal static XmlElement createTextEntry(XmlDocument doc, Int32 id, Int32 value)
        {
            return createTextEntry(doc, id, value.ToString());
        }

        internal static XmlElement createTextEntry(XmlDocument doc, Int32 id, Boolean value)
        {
            return createTextEntry(doc, id, (value ? "1" : "0"));
        }

        internal static int getTextEntryId(XmlNode node)
        {
            int rval=-1;

            try
            {
                if (node.Name.Equals("t"))
                {
                    Int32.TryParse(node.Attributes["id"].Value, out rval);
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }

            return rval;
        }

        internal static void getTextEntryValue(XmlNode node, ref int ival)
        {
            try
            {
                if (node.Name.Equals("t"))
                {
                    Int32.TryParse(node.InnerText, out ival);
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }

        internal static void getTextEntryValue(XmlNode node, ref bool bval)
        {
            int ival = -1;

            getTextEntryValue(node, ref ival);

            switch (ival)
            {
            case 0:
                bval = false;
                break;

            case 1:
                bval = true;
                break;
            }
        }

        private FileDetailsList _fileDetailsList = null;
        private SortedSet<Int32> definedLocales = new SortedSet<int>();
        private SortedSet<Int32> definedFiles = new SortedSet<int>();
        private IConfig config = null;
        public FileDetailsList fileDetailsList
        {
            get
            {
                return _fileDetailsList;
            }
            private set
            {
                _fileDetailsList=value;
            }
        }

        internal TextDictionary(IConfig config)
        {
            this.config = config;
            enumerateFiles();
        }

        private void enumerateFiles()
        {
            fileDetailsList = new FileDetailsList(config, "Text Files", "t", false);
        }

        public new void Clear()
        {
            base.Clear();
            fileDetailsList.Clear();
            definedLocales.Clear();
            GC.Collect();
        }

        public TextEntry getEntry(int page, int id, int locale)
        {
            TextEntryTranslated entry = getEntry(page, id);

            if (entry != null)
            {
                return entry.getEntry(locale);
            }
            else
            {
                return null;
            }
        }

        public TextEntryTranslated getEntry(int page, int id)
        {
            if (this.Keys.Contains(page))
            {
                return this[page].getEntry(id);
            }
            else
            {
                return null;
            }
        }

        public String getText(int page, int id, int locale, bool raw=false)
        {
            String rval = null;

            if (raw)
            {
                rval=getText(page, id, locale, null);
            }
            else
            {
                if (page < PAGE_X3R)
                {
                    rval = getText((page % PAGE_BASE) + PAGE_X3R, id, locale);
                }
                else if (page < PAGE_X3TC)
                {
                    rval = getText((page % PAGE_BASE) + PAGE_X3TC, id, locale);
                }

                if (rval == null)
                {
                    rval=getText(page, id, locale, new CrossReferenceList(this, locale));
                }
            }

            return rval;

        }

        private class CrossReferenceList : Dictionary<String, String>
        {
            internal const String regex = @"(^|[^\\])\{\d+,\d+\}";
            private TextDictionary context;
            private int locale;

            public CrossReferenceList(TextDictionary textDictionary, int locale)
            {
                this.context = textDictionary;
                this.locale = locale;
            }

            public string Resolve(Match match)
            {
                string rval = match.Value;
                string prefix = "";

                if (rval[0] != '{')
                {
                    prefix += rval[0];
                    rval = rval.Substring(1);
                }

                int sepPos = rval.IndexOf(',');
                int refPage = int.Parse(rval.Substring(1, sepPos - 1));
                int refId = int.Parse(rval.Substring(sepPos + 1, rval.Length - (sepPos + 2)));
                string key = "{" + refPage.ToString() + "," + refId.ToString() + "}";

                lock (this)
                {
                    if (!ContainsKey(key))
                    {
                        Add(key, key);

                        if (refPage < PAGE_X3TC)
                        {
                            rval = context.getText((refPage % PAGE_BASE) + PAGE_X3TC, refId, locale, this);
                        }

                        if (rval == null && refPage < PAGE_X3R)
                        {
                            rval = context.getText((refPage % PAGE_BASE) + PAGE_X3R, refId, locale, this);
                        }

                        if (rval == null)
                        {
                            rval = context.getText(refPage, refId, locale, this);
                        }

                        if (rval != null)
                        {
                            this[key] = rval;
                        }
                        else
                        {
                            rval = this[key];
                        }
                    }
                    else
                    {
                        rval = this[key];
                    }
                }

                return prefix + rval;
            }
        };

        private static class EmbeddedComment
        {
            internal const String regex = @"(^|[^\\])\([^\)]*\)";

            public static string Remove(Match match)
            {
                string rval = "";

                if (match.Value[0] != '(')
                {
                    rval += match.Value[0];
                }

                return rval;
            }
        };

        private static class Escape
        {
            internal const String regex = @"\\[\(\{\}\)]";

            public static string Remove(Match match)
            {
                return match.Value.Substring(1);
            }
        };

        private String getText(int page, int id, int locale, CrossReferenceList crossReferences)
        {
            TextEntryTranslated entry = this.getEntry(page, id);

            if (entry != null)
            {
                String text=entry.getText(locale);

                if (text!=null && crossReferences != null)
                {
                    text = Regex.Replace(text, EmbeddedComment.regex, EmbeddedComment.Remove);
                    text = Regex.Replace(text, CrossReferenceList.regex, crossReferences.Resolve);
                    text = Regex.Replace(text, CrossReferenceList.regex, crossReferences.Resolve);
                    text = Regex.Replace(text, Escape.regex, Escape.Remove);
                    text = text.Replace("\\n", "\n");
                }
                
                return text;
            }
            else
            {
                return null;
            }
        }

        public String whereDefined(int page, int id, int locale)
        {
            TextEntryTranslated entry = this.getEntry(page, id);

            if (entry != null)
            {
                return entry.whereDefined(locale);
            }
            else
            {
                return null;
            }
        }

        public void loadGameData()
        {
            Clear();
            try
            {
                enumerateFiles();

                foreach (FileDetails fileDetails in fileDetailsList)
                {
                    String fileName = fileDetails.fullFilePath;

                    if (fileDetails.isFile)
                    {
                        if (Regex.IsMatch(fileName, @"t\\\d{4}(-L\d{3})?\.(xml|pck)$", RegexOptions.IgnoreCase))
                        {
                            int offset = fileName.LastIndexOf('\\');
                            int fileId = int.Parse(fileName.Substring(offset + 1, 4));
                            int locale = -1;

                            if (fileName.Length > (offset + 9))
                            {
                                locale = int.Parse(fileName.Substring(offset + 7, 3));
                            }

                            try
                            {
                                definedLocales.Add(locale);
                                definedFiles.Add(fileId);

                                load(fileDetails.open(), fileId, locale);
                            }
                            catch (Exception err)
                            {
                                fileDetails.error = err.Message;
                            }
                        }
                        else
                        {
                            fileDetails.error = "*** IGNORED ***";
                        }
                    }
                    GC.Collect();
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }

        private void load(Stream xmlDataStream, int fileId, int locale)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(xmlDataStream);

            foreach (XmlNode langNode in doc.ChildNodes)
            {
                if (langNode.Name.Equals("language"))
                {
                    loadPages(langNode, fileId, locale);
                }
            }
        }

        private bool loadPages(XmlNode bookNode, int fileId, int locale)
        {
            bool loadedData = false;

            foreach (XmlNode pageNode in bookNode.ChildNodes)
            {
                if (pageNode.Name.Equals("page"))
                {
                    XmlAttribute pageId = pageNode.Attributes["id"];
                    Int32 page = -1;

                    if (Int32.TryParse(pageId.Value, out page))
                    {
                        if (!this.Keys.Contains(page))
                        {
                            this.Add(page, new TextEntryTranslatedPage());
                        }
                        TextEntryTranslatedPage textPage = this[page];
                        foreach (XmlNode textNode in pageNode.ChildNodes)
                        {
                            if (textNode.Name.Equals("t"))
                            {
                                XmlAttribute textId = textNode.Attributes["id"];
                                Int32 text = -1;

                                if (Int32.TryParse(textId.Value, out text))
                                {
                                    if (!textPage.Keys.Contains(text))
                                    {
                                        textPage.Add(text, new TextEntryTranslated());
                                    }
                                    TextEntryTranslated transText = textPage[text];

                                    if (!transText.Keys.Contains(locale))
                                    {
                                        transText.Add(locale, new TextEntry());
                                    }
                                    TextEntry entry = transText[locale];

                                    if (!entry.Keys.Contains(fileId))
                                    {
                                        entry.Add(fileId, textNode.InnerXml);
                                    }
                                    else
                                    {
                                        entry[fileId] = textNode.InnerXml;
                                    }
                                    loadedData = true;
                                }
                            }
                        }
                    }
                }
            }

            return loadedData;
        }

        public void saveGameData(String destinationPath)
        {
            try
            {
                using (X2FD.Catalog cat = config.getCatalog(destinationPath))
                {
                    foreach (Int32 locale in definedLocales)
                    {
                        String extension = (locale >= 0 ? "-L" + locale.ToString("D3") : "");

                        foreach (Int32 fileid in definedFiles)
                        {
                            String fileName = fileid.ToString("D4") + extension;

                            try
                            {
                                XmlDocument doc = new XmlDocument();
                                DateTime saveTime = DateTime.Now;
                                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
                                doc.AppendChild(doc.CreateComment(
                                    "Automatically Generated by Changeling v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                                    " on " + saveTime.ToLongDateString() + " at " + saveTime.ToLongTimeString()));

                                XmlElement root = doc.CreateElement("language");
                                root.SetAttribute("id", locale.ToString());

                                foreach (KeyValuePair<int, TextEntryTranslatedPage> page in this)
                                {
                                    XmlElement pageXml = page.Value.save(doc, locale, fileid);

                                    if (pageXml != null)
                                    {
                                        pageXml.SetAttribute("id", page.Key.ToString());
                                        root.AppendChild(pageXml);
                                    }
                                }

                                if (root.HasChildNodes)
                                {
                                    using (X2FD.File file = cat.OpenFile("t\\" + fileName + ".pck", X2FD.OpenMode.write, X2FD.CreateDisposition.createNew, X2FD.FileType.deflate))
                                    {
                                        doc.AppendChild(root);

                                        doc.Save(file);
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                MainForm.displayError(err);
                            }
                            GC.Collect();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }

        internal void save(XmlElement parentXml)
        {
            XmlDocument doc = parentXml.OwnerDocument;

            XmlElement textDatabaseXml = doc.CreateElement(xmlNodeName);

            try
            {
                foreach (Int32 fileid in definedFiles)
                {
                    XmlElement fileXml = doc.CreateElement(xmlFileNodeName);
                    fileXml.SetAttribute("id", fileid.ToString());

                    foreach (Int32 locale in definedLocales)
                    {
                        XmlElement localeXml = doc.CreateElement(xmlLocaleNodeName);
                        localeXml.SetAttribute("id", locale.ToString());

                        foreach (KeyValuePair<int, TextEntryTranslatedPage> page in this)
                        {
                            XmlElement pageXml = page.Value.saveDefined(doc, locale, fileid);

                            if (pageXml != null)
                            {
                                pageXml.SetAttribute("id", page.Key.ToString());
                                localeXml.AppendChild(pageXml);
                            }
                        }

                        if (localeXml.HasChildNodes)
                        {
                            fileXml.AppendChild(localeXml);
                        }
                    }

                    if (fileXml.HasChildNodes)
                    {
                        textDatabaseXml.AppendChild(fileXml);
                    }
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }

            parentXml.AppendChild(textDatabaseXml);
        }

        internal void load(XmlNode parentXml)
        {
            Clear();
            try
            {
                foreach (XmlNode textDatabaseXml in parentXml.ChildNodes)
                {
                    if (textDatabaseXml.Name.Equals(xmlNodeName))
                    {
                        foreach (XmlNode localeXml in textDatabaseXml.ChildNodes)
                        {
                            if (localeXml.Name.Equals(xmlLocaleNodeName))
                            {
                                Int32 locale = Int32.Parse(localeXml.Attributes["id"].Value);
                                definedLocales.Add(locale);

                                foreach (XmlNode fileXml in localeXml.ChildNodes)
                                {
                                    if (fileXml.Name.Equals(xmlFileNodeName))
                                    {
                                        Int32 fileId = Int32.Parse(fileXml.Attributes["id"].Value);
                                        definedFiles.Add(fileId);

                                        if (loadPages(fileXml, fileId, locale))
                                        {
                                            //fileNameList.Add("t\\" + fileId.ToString("D4") + (locale>=0?"-L"+locale.ToString("D3"):""));
                                        }
                                    }
                                }
                            }
                            else if (localeXml.Name.Equals(xmlFileNodeName))
                            {
                                Int32 fileId = Int32.Parse(localeXml.Attributes["id"].Value);
                                definedFiles.Add(fileId);

                                foreach (XmlNode fileXml in localeXml.ChildNodes)
                                {
                                    if (fileXml.Name.Equals(xmlLocaleNodeName))
                                    {
                                        Int32 locale = Int32.Parse(fileXml.Attributes["id"].Value);
                                        definedLocales.Add(locale);

                                        if (loadPages(fileXml, fileId, locale))
                                        {
                                            //fileNameList.Add("t\\" + fileId.ToString("D4") + (locale >= 0 ? "-L" + locale.ToString("D3") : ""));
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }
    }
}
