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

namespace Changeling.Lib
{
    class FileDetails : ConfigDatum
    {
        internal const String xmlName = "FileDetails";

        internal String fullFilePath;
        internal String shortFileName;
        internal FileDetailsList subFiles;
        internal String error=null;

        internal Boolean isModified
        {
            get
            {
                return config.isModified(fullFilePath);
            }
        }

        internal Boolean isDirectory
        {
            get
            {
                return (subFiles != null);
            }
        }

        internal Boolean isFile
        {
            get
            {
                return (subFiles == null);
            }
        }

        public FileDetails(IConfig config, VFS.FileInfo fileInfo) :
            base(config)
        {
            fullFilePath = fileInfo.realName;
            shortFileName = fileInfo.fileName;

            switch (fileInfo.attributes)
            {
            case VFS.FileAttribute.directory:
                subFiles = new FileDetailsList(config, fileInfo.fileName, fileInfo.realName);
                break;

            case VFS.FileAttribute.file:
                subFiles = null;
                break;
            }
        }

        public FileDetails(IConfig config, XmlElement xmlNode) :
            base(config)
        {
            fullFilePath = getAttr(xmlNode, "fullFilePath", fullFilePath);
            shortFileName = getAttr(xmlNode, "shortFileName", shortFileName);

            if (getAttr(xmlNode, "isDirectory", false))
            {
                subFiles = new FileDetailsList(config, xmlNode, shortFileName, fullFilePath);
            }
            else
            {
                subFiles = null;
            }
        }

        public FileDetails(IConfig config, string fileName) :
            base(config)
        {
            fullFilePath = fileName;
            int startIndex = fileName.LastIndexOf('\\') + 1;
            int length = fileName.LastIndexOf('.')-startIndex;
            shortFileName = fileName.Substring(startIndex, length);
            subFiles = null;
        }

        public virtual Stream open()
        {
            return config.readFile(fullFilePath);
        }

        public virtual void save(Stream dataStream)
        {
            fullFilePath = config.writeFile(fullFilePath, dataStream);
        }

        public override bool isValid()
        {
            return true;
        }

        public override string generateName()
        {
            return fullFilePath;
        }

        public override string generateLabel()
        {
            return shortFileName;
        }

        public override string generateTooltip()
        {
            String tooltip =
                "Full Path: " + fullFilePath +
                "\nModified: " + isModified.ToString();

            if (subFiles!=null)
            {
                tooltip += "\nFile Type: Directory";
            }
            else
            {
                tooltip += "\nFile Type: File";
            }

            if (error != null)
            {
                tooltip += "\nLast Error: " + error;
            }

            return tooltip;
        }

        internal override XmlElement save(XmlDocument xmlDoc)
        {
            return null;
        }

        public override void Dispose()
        {
            if (subFiles != null)
            {
                subFiles.Dispose();
            }
            base.Dispose();
        }

        internal void revert()
        {
            fullFilePath = config.revertFile(fullFilePath);
        }

        internal void exportModifications(Ionic.Zip.ZipFile archive)
        {
            if (isDirectory)
            {
                subFiles.exportModifications(archive);
            }
            else if (isModified)
            {
                archive.AddEntry(fullFilePath, open());
            }
        }
    }
}
