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
using System.Windows.Forms;
using System.Xml;

namespace Changeling.Lib
{
    /// <summary>
    /// 
    /// </summary>
    abstract class ConfigDatum : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        internal class NameComparer : IComparer<ConfigDatum>
        {
            public int Compare(ConfigDatum x, ConfigDatum y)
            {
                return x.generateName().CompareTo(y.generateName());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class LabelComparer : IComparer<ConfigDatum>
        {
            public int Compare(ConfigDatum x, ConfigDatum y)
            {
                return x.generateLabel().CompareTo(y.generateLabel());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrDef"></param>
        /// <returns></returns>
        protected static Boolean getAttr(XmlElement xmlNode, string attrName, Boolean attrDef)
        {
            Boolean attrVar = attrDef;

            if (xmlNode.Attributes[attrName] != null)
            {
                attrVar = Boolean.Parse(xmlNode.Attributes[attrName].Value);
            }

            return attrVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrDef"></param>
        /// <returns></returns>
        protected static NumericI getAttr(XmlNode specNode, String attrName, NumericI attrDef)
        {
            NumericI attrVar = attrDef;

            if (specNode.Attributes[attrName] != null)
            {
                attrVar = specNode.Attributes[attrName].Value;
            }

            return attrVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrDef"></param>
        /// <returns></returns>
        protected static NumericFP getAttr(XmlNode specNode, String attrName, NumericFP attrDef)
        {
            NumericFP attrVar = attrDef;

            if (specNode.Attributes[attrName] != null)
            {
                attrVar = specNode.Attributes[attrName].Value;
            }

            return attrVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrDef"></param>
        /// <returns></returns>
        protected static NumericD getAttr(XmlNode specNode, String attrName, NumericD attrDef)
        {
            NumericD attrVar = attrDef;

            if (specNode.Attributes[attrName] != null)
            {
                attrVar = specNode.Attributes[attrName].Value;
            }

            return attrVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrDef"></param>
        /// <returns></returns>
        protected static String getAttr(XmlNode specNode, String attrName, String attrDef)
        {
            String attrVar = attrDef;

            if (specNode.Attributes[attrName] != null)
            {
                attrVar = specNode.Attributes[attrName].Value;
            }

            return attrVar;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrDef"></param>
        protected static void setAttr(XmlElement xmlNode, String attrName, Boolean attrVal)
        {
            xmlNode.SetAttribute(attrName, attrVal.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrVal"></param>
        protected static void setAttr(XmlElement xmlNode, String attrName, Numeric attrVal)
        {
            xmlNode.SetAttribute(attrName, attrVal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrVal"></param>
        protected static void setAttr(XmlElement xmlNode, String attrName, String attrVal)
        {
            xmlNode.SetAttribute(attrName, attrVal);
        }

        private IConfig _config = null;
        /// <summary>
        /// 
        /// </summary>
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
        /// <returns></returns>
        public abstract bool isValid();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string generateName();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string generateLabel();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string generateTooltip();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subItems"></param>
        public virtual void generateSubItems(ListViewItem.ListViewSubItemCollection subItems)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        internal abstract XmlElement save(XmlDocument xmlDoc);

        /// <summary>
        /// Basic default constructor that initialises the read-only property that is used to link this datum with
        /// a 
        /// </summary>
        /// <param name="config"></param>
        protected ConfigDatum(IConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        protected ConfigDatum(ConfigDatum datum)
        {
            _config = datum._config;
        }

        /// <summary>
        /// Creates an appropriately initialised instance of a ListViewItem that can be used to view/edit this datum
        /// </summary>
        /// <returns>Instance of an item that can be inserted into a ListView and used to view/edit this datam</returns>
        internal ListViewItem newListViewItem()
        {
            ListViewItem item = new ListViewItem(generateName());

            item.Text = generateLabel();
            item.ToolTipText = generateTooltip();
            item.Tag = this;
            generateSubItems(item.SubItems);

            return item;
        }

        internal void refresh(ListViewItem item)
        {
            if (item.Tag == this)
            {
                item.ListView.BeginUpdate();
                item.Name = generateName();
                item.Text = generateLabel();
                item.ToolTipText = generateTooltip();

                while (item.SubItems.Count > 1)
                {
                    item.SubItems.RemoveAt(1);
                }
                generateSubItems(item.SubItems);
                item.ListView.EndUpdate();
            }
            else
            {
                throw new Exception("ConfigDatum instance mismatch");
            }
        }

        internal virtual void refresh(TreeNodeCollection nodeList)
        {
        }
    }
}
