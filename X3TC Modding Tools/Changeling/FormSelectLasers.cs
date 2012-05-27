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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Changeling.Lib.X3TC;

namespace Changeling
{
    public partial class FormSelectLasers : Form
    {
        private SpecLaserList selectableNodes;
        private ListView.ListViewItemCollection selectedNodes;
        private IConfig config;
        private String raceName;

        internal FormSelectLasers(IConfig config, SpecLaserList selectableNodes, String raceName, ListView.ListViewItemCollection selectedNodes)
        {
            this.selectableNodes = selectableNodes;
            this.selectedNodes = selectedNodes;
            this.config = config;
            this.raceName = raceName;

            InitializeComponent();

            foreach (ListViewItem critNode in selectedNodes)
            {
                CriteriaLaser crit = (CriteriaLaser)critNode.Tag;
                SpecLaser spec = selectableNodes[crit.laserIndex];

                ListViewItem item = new ListViewItem();

                item.Name = spec.generateName();
                item.Text = spec.generateLabel();
                item.ToolTipText = spec.generateTooltip();
                item.Tag = critNode;

                listViewSelected.Items.Add(item);
            }

            foreach (SpecLaser spec in selectableNodes)
            {
                if (!listViewSelected.Items.ContainsKey(spec.name))
                {
                    ListViewItem item = new ListViewItem();

                    item.Name = spec.generateName();
                    item.Text = spec.generateLabel();
                    item.ToolTipText = spec.generateTooltip();
                    item.SubItems.Add(spec.type);
                    item.SubItems.Add(spec.characteristics.ToString("X08"));
                    item.Tag = spec;

                    listViewAvailable.Items.Add(item);
                }
            }
        }

        private void listViewAvailable_DragOver(object sender, DragEventArgs e)
        {

            if (e.Data == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else if (typeof(ListViewItem).IsInstanceOfType(e.Data.GetData(typeof(ListViewItem))))
            {
                ListViewItem subjectItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                if (subjectItem.ListView == listViewAvailable)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else if (subjectItem.ListView == listViewSelected)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listViewAvailable_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                // Do Nothing
            }
            else if (typeof(ListViewItem).IsInstanceOfType(e.Data.GetData(typeof(ListViewItem))))
            {
                ListViewItem subjectItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                if (subjectItem.ListView == listViewSelected)
                {
                    List<ListViewItem> deletedItems=new List<ListViewItem>();
                    foreach (ListViewItem critItem in listViewSelected.SelectedItems)
                    {
                        deletedItems.Add(critItem);
                    }
                    foreach (ListViewItem critItem in deletedItems)
                    {
                        TreeNode critNode = (TreeNode)critItem.Tag;
                        CriteriaLaser crit = (CriteriaLaser)critNode.Tag;
                        SpecLaser spec = selectableNodes[crit.laserIndex];

                        try
                        {
                            config.delete(crit, raceName, critItem.Index);

                            critItem.Remove();
                            critNode.Remove();

                            ListViewItem item = new ListViewItem();

                            item.Name = spec.generateName();
                            item.Text = spec.generateLabel();
                            item.ToolTipText = spec.generateTooltip();
                            item.SubItems.Add(spec.type);
                            item.SubItems.Add(spec.characteristics.ToString("X08"));
                            item.Tag = spec;

                            listViewAvailable.Items.Add(item);
                        }
                        catch (Exception err)
                        {
                            MainForm.displayError(this, err);
                        }
                    }
                    deletedItems.Clear();
                }
                else
                {
                    // Do Nothing
                }
            }
            else
            {
                // Do Nothing
            }
        }

        private void listViewSelected_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = listViewSelected.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetItem = listViewSelected.GetItemAt(targetPoint.X, targetPoint.Y);

            if (targetItem != null)
            {
                if (targetPoint.Y < (listViewSelected.Font.GetHeight() / 2))
                {
                    listViewSelected.EnsureVisible(targetItem.Index - 1);
                }
                else if (targetPoint.Y >= (listViewSelected.Height - (listViewSelected.Font.GetHeight() / 2)))
                {
                    if (targetItem.Index != (listViewSelected.Items.Count - 1))
                    {
                        listViewSelected.EnsureVisible(targetItem.Index + 1);
                    }
                }
            }

            if (e.Data == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else if (typeof(ListViewItem).IsInstanceOfType(e.Data.GetData(typeof(ListViewItem))))
            {
                ListViewItem subjectItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                if (subjectItem.ListView == listViewAvailable)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else if (subjectItem.ListView == listViewSelected)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listViewSelected_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = listViewSelected.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetItem = listViewSelected.GetItemAt(targetPoint.X, targetPoint.Y);

            int targetIdx = listViewSelected.Items.Count;

            if (targetItem != null)
            {
                targetIdx = targetItem.Index;
            }

            if (e.Data == null)
            {
                // Do Nothing
            }
            else if (typeof(ListViewItem).IsInstanceOfType(e.Data.GetData(typeof(ListViewItem))))
            {
                ListViewItem subjectItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                if (subjectItem.ListView == listViewAvailable)
                {
                    List<ListViewItem> addedItems=new List<ListViewItem>();
                    foreach (ListViewItem specItem in listViewAvailable.SelectedItems)
                    {
                        SpecLaser spec = (SpecLaser)specItem.Tag;

                        try
                        {
                            ListViewItem critNode = config.add(spec, selectableNodes.IndexOf(spec), raceName, targetIdx);

                            if (critNode != null)
                            {
                                ListViewItem critItem = new ListViewItem();

                                critItem.Name = spec.generateName();
                                critItem.Text = spec.generateLabel();
                                critItem.ToolTipText = spec.generateTooltip();
                                critItem.Tag = critNode;

                                listViewSelected.Items.Insert(targetIdx, critItem);
                                selectedNodes.Insert(targetIdx, critNode);
                                addedItems.Add(specItem);

                                targetIdx++;
                            }
                        }
                        catch (Exception err)
                        {
                            MainForm.displayError(this, err);
                        }
                    }
                    foreach (ListViewItem specItem in addedItems)
                    {
                        specItem.Remove();
                    }
                    addedItems.Clear();
                }
                else if (subjectItem.ListView == listViewSelected)
                {
                    SortedDictionary<int, ListViewItem> movedItems=new SortedDictionary<int,ListViewItem>();
                    foreach(ListViewItem critItem in listViewSelected.SelectedItems)
                    {
                        movedItems.Add(critItem.Index, critItem);
                    }
                    try
                    {
                        foreach (ListViewItem critItem in movedItems.Values)
                        {
                            ListViewItem critNode = (ListViewItem)critItem.Tag;
                            CriteriaLaser crit = (CriteriaLaser)critNode.Tag;
                            int critIdx = critItem.Index;

                            config.delete(crit, raceName, critIdx);
                            critItem.Remove();
                            critNode.Remove();

                            if (critIdx < targetIdx)
                            {
                                targetIdx--;
                            }
                        }

                        foreach (ListViewItem critItem in movedItems.Values)
                        {
                            ListViewItem critNode = (ListViewItem)critItem.Tag;
                            CriteriaLaser crit = (CriteriaLaser)critNode.Tag;
                            int critIdx = critItem.Index;

                            config.insert(crit, raceName, targetIdx);

                            listViewSelected.Items.Insert(targetIdx, critItem);
                            selectedNodes.Insert(targetIdx, critNode);

                            targetIdx++;
                        }
                    }
                    catch (Exception err)
                    {
                        MainForm.displayError(this, err, true);
                    }
                    movedItems.Clear();
                }
                else
                {
                    // Do Nothing
                }
            }
            else
            {
                // Do Nothing
            }
        }

        private void listViewAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainerLists_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void listViewSelected_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewAvailable_ItemDrag(object sender, ItemDragEventArgs e)
        {
            switch (e.Button)
            {
            case System.Windows.Forms.MouseButtons.Left:
                if (typeof(ListViewItem).IsInstanceOfType(e.Item))
                {
                    ListViewItem item = (ListViewItem)e.Item;

                    if (item.Tag != null)
                    {
                        DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Scroll);
                    }
                }
                break;
            }
        }

        private void listViewSelected_ItemDrag(object sender, ItemDragEventArgs e)
        {
            switch (e.Button)
            {
            case System.Windows.Forms.MouseButtons.Left:
                if (typeof(ListViewItem).IsInstanceOfType(e.Item))
                {
                    ListViewItem item = (ListViewItem)e.Item;

                    if (item.Tag != null)
                    {
                        DoDragDrop(e.Item, DragDropEffects.Move | DragDropEffects.Scroll);
                    }
                }
                break;
            }
        }

        private void FormSelectLasers_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Control target = sender as Control;

            if (target != null)
            {
                String helpText = target.Tag as String;

                if (helpText != null)
                {
                    hlpevent.Handled = true;
                    MessageBox.Show(this, helpText.Replace("\\n", "\n").Replace("\\t", "\t"), "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
