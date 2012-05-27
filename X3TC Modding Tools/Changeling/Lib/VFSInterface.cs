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
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using DS.X2Core;
using Ionic.Zip;

namespace Changeling.Lib
{
    /// <summary>
    /// 
    /// </summary>
    class VFSInterface
    {
        internal const String xmlName = "ModifiedFile";
        private SortedDictionary<String, MemoryStream> changedFiles = new SortedDictionary<String, MemoryStream>();
        
        private String _vfsRoot;
        internal String vfsRoot
        {
            get
            {
                return _vfsRoot;
            }
        }

        private IConfig _config;
        internal IConfig config
        {
            get
            {
                return _config;
            }
        }

        private IConfig _baseline;
        internal IConfig baseline
        {
            get
            {
                return _baseline;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="vfsRoot"></param>
        public VFSInterface(IConfig config, String vfsRoot)
        {
            _config = config;
            _vfsRoot = vfsRoot;
        }

        public VFSInterface(IConfig modification, IConfig baseline)
        {
            _config = modification;
            _baseline = baseline;
        }

        /// <summary>
        /// 
        /// </summary>
        public void revertAll()
        {
            changedFiles.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public String revertFile(String fileName)
        {
            String vfsPath = getVfsPath(fileName);
            changedFiles.Remove(vfsPath);

            return findFile(fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public String getVfsPath(String fileName)
        {
            String catPath = null;
            String vfsPath = null;
            X2FD.Path.Parse(fileName, out catPath, out vfsPath);

            if (catPath == null && vfsRoot != null)
            {
                if (fileName.StartsWith(vfsRoot + "\\"))
                {
                    vfsPath = fileName.Substring(vfsRoot.Length + 1);
                }
            }

            if (vfsPath == null)
            {
                vfsPath = fileName;
            }

            return vfsPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Boolean isModified(String fileName)
        {
            return changedFiles.ContainsKey(getVfsPath(fileName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream readFile(String fileName)
        {
            Stream rval = null;
            String vfsPath = getVfsPath(fileName);

            if (changedFiles.ContainsKey(vfsPath))
            {
                rval = new MemoryStream();
                Stream persistentStream = changedFiles[vfsPath];

                persistentStream.CopyTo(rval);

                persistentStream.Seek(0, SeekOrigin.Begin);
                rval.Seek(0, SeekOrigin.Begin);
            }
            else if (baseline != null)
            {
                rval = baseline.readFile(fileName);
            }
            else
            {
                rval = readFileDirect(fileName);
            }


            return rval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataStream"></param>
        /// <returns></returns>
        public String writeFile(String fileName, Stream dataStream)
        {
            String vfsPath = getVfsPath(fileName);

            if (!changedFiles.ContainsKey(vfsPath))
            {
                changedFiles.Add(vfsPath, new MemoryStream());
            }
            Stream persistentStream = changedFiles[vfsPath];

            persistentStream.SetLength(0);
            dataStream.CopyTo(persistentStream);

            persistentStream.Seek(0, SeekOrigin.Begin);

            return vfsPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Stream readFileDirect(String fileName)
        {
            Stream rval = new MemoryStream();
            String catPath = null;
            String vfsPath = null;
            X2FD.Path.Parse(fileName, out catPath, out vfsPath);

            if (catPath != null && vfsPath != null)
            {
                using (X2FD.Catalog cat = X2FD.Catalog.Open(catPath, X2FD.CreateDisposition.openExisting))
                {
                    Stream file = cat.OpenFile(vfsPath, X2FD.OpenMode.read, X2FD.CreateDisposition.openExisting, X2FD.FileType.auto);
                    file.CopyTo(rval);
                    file.Close();
                }
            }
            else if (vfsRoot!=null)
            {
                using (VFS.DirectoryFileSystem vfs = new VFS.DirectoryFileSystem())
                {
                    vfs.loadingBehaviour = VFS.LoadingBehaviour.x3tc;
                    vfsPath = X2FD.Path.GetVfsRelativePath(fileName, vfsRoot + "\\");

                    vfs.Create(vfsRoot);

                    Stream file = vfs.OpenFile(vfsPath, X2FD.OpenMode.read);
                    file.CopyTo(rval);
                    file.Close();
                }
            }

            rval.Seek(0, SeekOrigin.Begin);

            return rval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="dataStream"></param>
        public void writeFileDirect(String fileName, Stream dataStream)
        {
            String catPath = null;
            String vfsPath = null;
            X2FD.Path.Parse(fileName, out catPath, out vfsPath);

            if (catPath != null && vfsPath != null)
            {
                using (X2FD.Catalog cat = X2FD.Catalog.Open(catPath, X2FD.CreateDisposition.openExisting))
                {
                    Stream file = cat.OpenFile(vfsPath, X2FD.OpenMode.write, X2FD.CreateDisposition.createNew, X2FD.FileType.deflate);
                    dataStream.CopyTo(file);
                    file.Close();
                }
            }
            else if (vfsRoot != null)
            {
                using (VFS.DirectoryFileSystem vfs = new VFS.DirectoryFileSystem())
                {
                    vfs.loadingBehaviour = VFS.LoadingBehaviour.x3tc;
                    vfsPath = X2FD.Path.GetVfsRelativePath(fileName, vfsRoot + "\\");

                    vfs.Create(vfsRoot);

                    Stream file = vfs.OpenFile(vfsPath, X2FD.OpenMode.write);
                    dataStream.CopyTo(file);
                    file.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docRoot"></param>
        internal void load(XmlNode docRoot)
        {
            revertAll();

            foreach (XmlNode xmlNode in docRoot.ChildNodes)
            {
                try
                {
                    switch (xmlNode.Name)
                    {
                    case xmlName:
                        if (xmlNode is XmlElement)
                        {
                            XmlElement xmlElem = (XmlElement)xmlNode;
                            String fileName = xmlElem.GetAttribute("fileName");
                            MemoryStream persistantStream = new MemoryStream();

                            using (Stream dataStream = new MemoryStream())
                            {
                                using (StreamWriter writer = new StreamWriter(dataStream))
                                {
                                    writer.Write(xmlElem.InnerText);
                                    writer.Flush();
                                    dataStream.Seek(0, SeekOrigin.Begin);
                                    dataStream.CopyTo(persistantStream);
                                }
                            }
                            persistantStream.Seek(0, SeekOrigin.Begin);

                            changedFiles.Add(fileName, persistantStream);
                        }
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configRoot"></param>
        internal void save(XmlElement configRoot)
        {
            XmlDocument doc=configRoot.OwnerDocument;

            foreach (KeyValuePair<String, MemoryStream> entry in changedFiles)
            {
                try
                {
                    XmlElement xmlElem = doc.CreateElement(xmlName);
                    xmlElem.SetAttribute("fileName", entry.Key);
                    using (Stream dataStream = new MemoryStream())
                    {
                        entry.Value.CopyTo(dataStream);
                        dataStream.Seek(0, SeekOrigin.Begin);

                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            xmlElem.InnerText=reader.ReadToEnd();
                        }
                    }
                    entry.Value.Seek(0, SeekOrigin.Begin);

                    configRoot.AppendChild(xmlElem);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="vfsLocation"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private Boolean doesFileMatch(string fileName, string vfsLocation, string pattern, bool recurse)
        {
            Boolean rval = false;
            String prefix = vfsLocation.Equals("")?"":(vfsLocation + "\\");

            if (fileName.StartsWith(prefix))
            {
                String targetPath=fileName.Substring(vfsLocation.Length+1);
                if (recurse || !targetPath.Contains('\\'))
                {
                    rval = Regex.IsMatch(targetPath.Substring(targetPath.LastIndexOf('\\') + 1), pattern);
                }
            }

            return rval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vfsPath"></param>
        /// <returns></returns>
        private String findFile(string vfsPath)
        {
            String realFileName = null;

            if (vfsRoot != null)
            {
                using (VFS.DirectoryFileSystem vfs = new VFS.DirectoryFileSystem())
                {
                    vfs.loadingBehaviour = VFS.LoadingBehaviour.x3tc;

                    FileInfo sourceInfo = new FileInfo(vfsRoot);

                    if ((sourceInfo.Attributes & FileAttributes.Directory) == 0)
                    {
                        vfs.AppendCatalog(vfsRoot);
                    }
                    else
                    {
                        vfs.Create(vfsRoot);
                    }

                    foreach (VFS.FileInfo fileInfo in vfs.Find(vfsPath.Substring(0, vfsPath.LastIndexOf("."))))
                    {
                        if (realFileName == null)
                        {
                            realFileName = fileInfo.realName;
                        }
                        else
                        {
                            throw new Exception("VFS Entry Not Unique");
                        }
                    }
                }
            }

            return realFileName;
        }

        /// <summary>
        /// Returns a list containing all the matching files both modified/cached and not
        /// </summary>
        /// <param name="vfsLocation"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        internal IEnumerable<FileDetails> findAll(bool recurse)
        {
            return findAll("", "*", recurse);
        }

        /// <summary>
        /// Returns a list containing all the matching files both modified/cached and not
        /// </summary>
        /// <param name="vfsLocation"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        internal IEnumerable<FileDetails> findAll(string vfsLocation, bool recurse)
        {
            return findAll(vfsLocation, "*", recurse);
        }

        /// <summary>
        /// Returns a list containing all the matching files both modified/cached and not 
        /// </summary>
        /// <param name="vfsLocation"></param>
        /// <param name="pattern"></param>
        /// <param name="recurse"></param>
        /// <returns></returns>
        internal IEnumerable<FileDetails> findAll(string vfsLocation="", string pattern = "*", bool recurse = false)
        {
            List<FileDetails> rval=new List<FileDetails>();

            rval.AddRange(from fileName in changedFiles.Keys
                          where doesFileMatch(fileName, vfsLocation, pattern, recurse)
                          select new FileDetails(config, fileName));

            if (vfsRoot != null)
            {
                using (VFS.DirectoryFileSystem vfs = new VFS.DirectoryFileSystem())
                {
                    vfs.loadingBehaviour = VFS.LoadingBehaviour.x3tc;
                    vfs.AddFilter(vfsLocation + "\\*");

                    FileInfo sourceInfo = new FileInfo(vfsRoot);

                    if ((sourceInfo.Attributes & FileAttributes.Directory) == 0)
                    {
                        vfs.AppendCatalog(vfsRoot);
                    }
                    else
                    {
                        vfs.Create(vfsRoot);
                    }

                    foreach (VFS.FileInfo fileInfo in vfs.Find(vfsLocation + "\\*"))
                    {
                        if (fileInfo.isFile)
                        {
                            if (doesFileMatch(getVfsPath(fileInfo.realName), vfsLocation, pattern, false) && !isModified(fileInfo.realName))
                            {
                                rval.Add(new FileDetails(config, fileInfo));
                            }
                        }
                        else if (recurse)
                        {
                            rval.Add(new FileDetails(config, fileInfo));
                        }
                    }
                }
            }

            return rval;
        }

        internal void exportModifications(ZipFile archive)
        {
            foreach (KeyValuePair<String, MemoryStream> entry in changedFiles)
            {
                using (Stream tempStream = new MemoryStream())
                {
                    Stream persistentStream = entry.Value;

                    persistentStream.CopyTo(tempStream);

                    persistentStream.Seek(0, SeekOrigin.Begin);
                    tempStream.Seek(0, SeekOrigin.Begin);

                    archive.AddEntry(entry.Key, tempStream);
                }
            }
        }

        internal void importModifications(ZipFile archive)
        {
            SortedDictionary<String, String> catalogs = new SortedDictionary<String, String>();

            foreach (ZipEntry entry in archive.Entries)
            {
                if (!entry.IsDirectory)
                {
                    if (entry.FileName.EndsWith(".cat") || entry.FileName.EndsWith(".dat"))
                    {
                        String catalogName=entry.FileName.Substring(0, entry.FileName.Length-4);

                        if (catalogs.ContainsKey(catalogName))
                        {
                            String tempDir = catalogs[catalogName];
                            entry.Extract(tempDir);
                            using (VFS.DirectoryFileSystem vfs = new VFS.DirectoryFileSystem())
                            {
                                vfs.loadingBehaviour = VFS.LoadingBehaviour.x3tc;
                                vfs.AddFilter("*");
                                vfs.Create(tempDir);
                                vfs.AppendCatalog(tempDir + "\\" + catalogName + ".cat");

                                importModifications(vfs);
                            }
                            Directory.Delete(tempDir, true);
                            catalogs.Remove(catalogName);
                        }
                        else
                        {
                            String tempDir = "Changeling." + Guid.NewGuid().ToString();
                            System.IO.Directory.CreateDirectory(tempDir);
                            entry.Extract(tempDir);
                            catalogs.Add(catalogName, tempDir);
                        }
                    }
                    else
                    {
                        writeFile(entry.FileName, entry.OpenReader());
                    }
                }
            }

            foreach (String tempDir in catalogs.Values)
            {
                Directory.Delete(tempDir, true);
            }
            catalogs.Clear();
        }

        private void importModifications(VFS.DirectoryFileSystem vfs, string path="")
        {
            String pathPrefix = (!path.Equals("") ? path + "\\" : "");
            foreach (VFS.FileInfo fileInfo in vfs.Find(pathPrefix + "*"))
            {
                if (fileInfo.isFile)
                {
                    if (fileInfo.realName.EndsWith(".dat"))
                    {
                        // Ignore the .dat file
                    }
                    else if (!isModified(fileInfo.realName))
                    {
                        writeFile(fileInfo.realName, readFileDirect(fileInfo.realName));
                    }
                }
                else
                {
                    importModifications(vfs, pathPrefix + fileInfo.fileName);
                }
            }
        }
    }
}
