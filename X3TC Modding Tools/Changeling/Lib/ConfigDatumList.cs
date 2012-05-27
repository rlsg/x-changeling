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
using System.Windows.Forms;

namespace Changeling.Lib
{
    /// <summary>
    /// 
    /// </summary>
    interface IConfigDatumList
    {
        void refresh(ListView listView);
        void refresh(TreeNodeCollection treeNodeCollection);
        void refresh(TreeNode treeNode);

        ConfigDatumSelection getAllDatums();
    }

    /// <summary>
    /// 
    /// </summary>
    class ConfigDatumSelection : SortedDictionary<Int32, ConfigDatum>, IDisposable
    {
        public IConfigDatumList selectedFrom = null;

        public void Dispose()
        {
            Clear();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class ConfigDatumList<T> : List<T>, IConfigDatumList, IDisposable where T : ConfigDatum
    {
        private String xmlName = null;
        private IConfig _config = null;
        protected IConfig config
        {
            get
            {
                return _config;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="xmlName"></param>
        protected ConfigDatumList(IConfig config, string xmlName)
        {
            _config = config;
            this.xmlName = xmlName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <returns></returns>
        protected abstract T create(XmlElement xmlNode);

        /// <summary>
        /// 
        /// </summary>
        public virtual new void Clear()
        {
            base.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        public virtual void generateColumns(ListView.ColumnHeaderCollection columns)
        {
            columns.Add("Item Name");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual String generateName()
        {
            return xmlName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string generateLabel()
        {
            return xmlName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual String generateTooltip()
        {
            return this.Count.ToString() + " items";
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRoot"></param>
        protected virtual void loadAttributes(XmlElement listRoot)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listRoot"></param>
        protected virtual void saveAttributes(XmlElement listRoot)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentNode"></param>
        public void load(XmlNode parentNode)
        {
            Clear();
            foreach (XmlNode listRoot in parentNode.SelectNodes("descendant::" + xmlName))
            {
                loadAttributes(listRoot as XmlElement);
                foreach (XmlElement xmlNode in listRoot.ChildNodes)
                {
                    T item = create(xmlNode);

                    if (item != null)
                    {
                        Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentRoot"></param>
        public void save(XmlNode parentRoot)
        {
            XmlDocument doc = parentRoot.OwnerDocument;
            XmlElement listRoot = doc.CreateElement(xmlName);
            saveAttributes(listRoot);
            foreach (T spec in this)
            {
                listRoot.AppendChild(spec.save(doc));
            }
            parentRoot.AppendChild(listRoot);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listView"></param>
        public void refresh(ListView listView)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            listView.BeginUpdate();
            try
            {
                if (listView.Tag != this)
                {
                    listView.Tag = this;
                    listView.Items.Clear();
                    listView.Columns.Clear();

                    generateColumns(listView.Columns);
                }

                List<ListViewItem> invalidItems = new List<ListViewItem>();
                foreach (ListViewItem item in listView.Items)
                {
                    if (typeof(T).IsInstanceOfType(item.Tag))
                    {
                        T datum = (T)item.Tag;

                        if (datum.isValid())
                        {
                            item.SubItems.Clear();
                            item.Name = datum.generateName();
                            item.Text = datum.generateLabel();
                            item.ToolTipText = datum.generateTooltip();
                            datum.generateSubItems(item.SubItems);
                        }
                        else
                        {
                            invalidItems.Add(item);
                        }
                    }
                }
                foreach (ListViewItem item in invalidItems)
                {
                    listView.Items.Remove(item);
                }
                invalidItems.Clear();

                foreach (T datum in this)
                {
                    if (!listView.Items.ContainsKey(datum.generateName()))
                    {
                        listView.Items.Insert(IndexOf(datum), datum.newListViewItem());
                    }
                }

                if (listView.Items.Count > 0)
                {
                    listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
                else
                {
                    listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
            catch (Exception)
            {
            }
            listView.EndUpdate();
            Cursor.Current = last;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeNodeCollection"></param>
        public void refresh(TreeNodeCollection treeNodeCollection)
        {
            String name=generateName();

            if (!treeNodeCollection.ContainsKey(name))
            {
                TreeNode newNode = new TreeNode();

                newNode.Name = name;
                refresh(newNode);

                treeNodeCollection.Add(newNode);
            }
            else
            {
                refresh(treeNodeCollection[name]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeNode"></param>
        public virtual void refresh(TreeNode treeNode)
        {
            treeNode.Tag = this;
            treeNode.Text = generateLabel();
            treeNode.ToolTipText = generateTooltip();

            foreach (T datum in this)
            {
                datum.refresh(treeNode.Nodes);
            }
        }

        /// <summary>
        /// This function is provided to support the selection of the entire datum list
        /// 
        /// </summary>
        /// <returns>Mapping from position in the datum list to datum instance for all members</returns>
        public ConfigDatumSelection getAllDatums()
        {
            ConfigDatumSelection selectedDatums = new ConfigDatumSelection();
            selectedDatums.selectedFrom = this;

            foreach (T datum in this)
            {
                selectedDatums.Add(IndexOf(datum), datum);
            }

            return selectedDatums;
        }
    }
}
