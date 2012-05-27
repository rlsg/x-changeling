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

namespace Changeling.Lib.X3TC
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

    abstract class ConfigDatumList<T> : Changeling.Lib.ConfigDatumList<T>, IConfigDatumList where T : ConfigDatum
    {
        public new IConfig config
        {
            get
            {
                return base.config as IConfig;
            }
        }

        protected ConfigDatumList(IConfig config, string xmlName) :
            base(config, xmlName)
        {
        }

        /// <summary>
        /// This function is provided to support the selection of the entire datum list
        /// 
        /// </summary>
        /// <returns>Mapping from position in the datum list to datum instance for all members</returns>
        public new ConfigDatumSelection getAllDatums()
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
