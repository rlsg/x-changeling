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
using System.IO;
using DS.X2Core;
using Ionic.Zip;

namespace Changeling.Lib
{
    class FileDetailsList : ConfigDatumList<FileDetails>
    {
        protected String vfsLocation;

        internal FileDetailsList(IConfig config, String listName, String vfsLocation, Boolean recurse=true) :
            base(config, listName)
        {
            this.vfsLocation = vfsLocation;

            try
            {
                AddRange(config.findFiles(vfsLocation, @".*", recurse));

                this.Sort(new ConfigDatum.LabelComparer());
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }

        public FileDetailsList(IConfig config, XmlElement xmlNode, string listName, string vfsLocation) :
            base(config, listName)
        {
            this.vfsLocation = vfsLocation;

            load(xmlNode);
        }

        protected override FileDetails create(XmlElement xmlNode)
        {
            return new FileDetails(config, xmlNode);
        }

        protected override void loadAttributes(XmlElement listRoot)
        {
            listRoot.SetAttribute("vfsLocation", vfsLocation); 
        }

        protected override void saveAttributes(XmlElement listRoot)
        {
            vfsLocation = listRoot.GetAttribute("vfsLocation");
        }

        internal void exportModifications(ZipFile archive)
        {
            foreach (FileDetails details in this)
            {
                details.exportModifications(archive);
            }
        }
    }
}
