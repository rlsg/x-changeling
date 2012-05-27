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
using System.IO;
using System.Xml;
using DS.X2Core;

namespace Changeling.Lib.X3TC
{
    abstract class SpecList<T>: ConfigDatumList<T> where T : Spec
    {
        private int id = -1;
        private String fileName = null;
        private FileDetails source = null;
        public String dataSource
        {
            get
            {
                if (source != null)
                {
                    return source.fullFilePath;
                }
                else
                {
                    return null;
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            source = null;
        }

        protected SpecList(IConfig config, int id, String fileName, String xmlName) :
            base(config, xmlName)
        {
            this.id = id;
            this.fileName = fileName;
        }

        protected abstract T create(List<String> datLine);

        public virtual void loadGameData()
        {
            Clear();
            try
            {
                foreach (FileDetails fileDetails in config.findFiles("types", @".*", false))
                {
                    if (fileDetails.isFile && fileDetails.shortFileName.ToLower().Equals(fileName.ToLower()))
                    {
                        source = fileDetails;
                        load(fileDetails.open());
                    }
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }

        public void commitChanges()
        {
            if (source != null)
            {
                source.save(save());
            }
        }

        public void revertChanges()
        {
            if (source != null)
            {
                load(source.open());
            }
        }

        private void load(Stream data)
        {
            using (StreamReader rdr = new StreamReader(data))
            {
                String[] hdrLine = readLine(rdr);
                int countIdx = 1;

                if (int.Parse(hdrLine[0])!=id)
                {
                    throw new Exception("Specification list id mismatch, got "+hdrLine[0]+" but was expecting "+id.ToString());
                }

                if (hdrLine.Length < 3)
                {
                    hdrLine = readLine(rdr);
                    countIdx = 0;
                }

                int count = int.Parse(hdrLine[countIdx]);
                for (int i = 0; i < count; i++)
                {
                    String[] datLine = readLine(rdr);

                    T item = create(new List<String>(datLine));

                    if (item != null)
                    {
                        Add(item);
                    }
                }
            }
        }

        private static String[] readLine(TextReader rdr)
        {
            String line = rdr.ReadLine();

            while (line.StartsWith("//") || line.TrimEnd().Length==0)
            {
                line = rdr.ReadLine();
            }

            return line.Split(new char[] { ';' });
        }

        public void saveGameData(String destinationPath)
        {
            try
            {
                using (X2FD.Catalog cat=config.getCatalog(destinationPath))
                {
                    save(cat.OpenFile("types\\" + fileName+".pck", X2FD.OpenMode.write, X2FD.CreateDisposition.createNew, X2FD.FileType.deflate), true);
                }
            }
            catch (Exception err)
            {
                MainForm.displayError(err);
            }
        }

        private Stream save()
        {
            Stream rval = new MemoryStream();

            save(rval, false);

            return rval;
        }

        private void save(Stream persistantStream, bool autoClose)
        {
            if (autoClose)
            {
                using (StreamWriter writer = new StreamWriter(persistantStream))
                {
                    save(writer);
                }
            }
            else
            {
                using (Stream dataStream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(dataStream))
                    {
                        save(writer);
                        dataStream.Seek(0, SeekOrigin.Begin);
                        dataStream.CopyTo(persistantStream);
                    }
                }
            }
        }

        private void save(StreamWriter writer)
        {
            DateTime saveTime = DateTime.Now;
            writer.WriteLine("// Automatically Generated by Changeling v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                " on " + saveTime.ToLongDateString() + " at " + saveTime.ToLongTimeString());
            writer.WriteLine(id.ToString() + ";" + Count + ";");
            foreach (T item in this)
            {
                foreach (String elem in item.getSpec())
                {
                    writer.Write(elem + ";");
                }
                writer.WriteLine();
            }
            writer.Flush();
        }

        public override string generateName()
        {
            return fileName;
        }

        public override string generateTooltip()
        {
            String tooltip=base.generateTooltip();
            String sourceFileName = dataSource;

            if (sourceFileName != null)
            {
                tooltip += "\nSource: " + sourceFileName;
            }

            return tooltip;
        }
    }
}
