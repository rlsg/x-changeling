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
using System.Globalization;
using System.IO;
using Changeling.Lib.X3TC;

namespace Changeling
{
    public partial class MainForm : Form
    {
        Configuration config=new Configuration();
        private string fileName="";
        private DateTime dragSelectTime = System.DateTime.Now;

        public enum Status
        {
            Ok,
            Dirty,
            LoadingSaving,
            Busy,
            Fault
        };

        internal Status setStatus(Status status)
        {
            Status oldStatus=Status.Ok;

            if (toolStripStatusIcon.Image != null)
            {
                if (toolStripStatusIcon.Image == imageListStatus.Images[0])
                {
                    oldStatus = Status.Dirty;
                }
                else if (toolStripStatusIcon.Image == imageListStatus.Images[1])
                {
                    oldStatus = Status.Busy;
                }
                else if (toolStripStatusIcon.Image == imageListStatus.Images[2])
                {
                    oldStatus = Status.LoadingSaving;
                }
                else
                {
                    oldStatus = Status.Fault;
                }
            }

            switch (status)
            {
            case Status.Ok:
                toolStripStatusIcon.Image = null;
                break;

            case Status.Dirty:
                toolStripStatusIcon.Image = imageListStatus.Images[0];
                break;

            case Status.Busy:
                toolStripStatusIcon.Image = imageListStatus.Images[1];
                break;

            case Status.LoadingSaving:
                toolStripStatusIcon.Image = imageListStatus.Images[2];
                break;

            default:
                toolStripStatusIcon.Image = imageListStatus.Images[3];
                break;
            }

            return oldStatus;
        }

        static internal void displayError(Exception err, bool fatal = false)
        {
            displayError(ActiveForm, err, fatal);
        }

        static internal void displayError(IWin32Window wnd, Exception err, bool fatal=false)
        {
            MainForm mainForm=(MainForm)Application.OpenForms["MainForm"];

            if (mainForm != null)
            {
                mainForm.setStatus(Status.Fault);
            }

            String message =
                "Please report the following information to the author:-\n" +
                "\n    Object: " + err.Source +
                "\n    Method: " + err.TargetSite +
                "\n    Message: " + err.Message +
                "\n\nStack Trace\n-----------\n" + err.StackTrace;

            if (fatal)
            {
                MessageBox.Show(wnd,
                    "The application needs to restart because of an unrecoverable error.\n\n" +
                    message, "FATAL ERROR REPORT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                System.Windows.Forms.Application.Restart();
            }
            else
            {
                MessageBox.Show(wnd, message, "RECOVERABLE ERROR REPORT", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        public MainForm()
        {
            InitializeComponent();

            switch (CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
            {
            case "cs":
                czechToolStripMenuItem.Select();
                break;

            case "de":
                germanToolStripMenuItem.Select();
                break;

            case "fr":
                frenchToolStripMenuItem.Select();
                break;

            case "it":
                italianToolStripMenuItem.Select();
                break;

            case "pl":
                polishToolStripMenuItem.Select();
                break;

            case "ru":
                russianToolStripMenuItem.Select();
                break;

            case "es":
                spanishToolStripMenuItem.Select();
                break;

            default:
                englishToolStripMenuItem.Select();
                break;
            }

            TreeNode raceRoot = configTreeView.Nodes["Races"];
            foreach (string race in Configuration.raceStrings)
            {
                TreeNode lasersNode = new System.Windows.Forms.TreeNode("Lasers");
                TreeNode missilesNode = new System.Windows.Forms.TreeNode("Missiles");
                TreeNode raceNode = new System.Windows.Forms.TreeNode(race, new System.Windows.Forms.TreeNode[] {
                    lasersNode,
                    missilesNode});

                lasersNode.Name = "Lasers";
                lasersNode.Text = "Lasers";
                lasersNode.Tag = race;
                lasersNode.ContextMenuStrip = contextMenuStripCriteriaList;
                missilesNode.Name = "Missiles";
                missilesNode.Text = "Missiles";
                missilesNode.Tag = race;
                missilesNode.ContextMenuStrip = contextMenuStripCriteriaList;
                raceNode.Name = race;
                raceNode.Text = race;
                raceNode.ContextMenuStrip = contextMenuStripRace;

                raceRoot.Nodes.Add(raceNode);
            }

            refreshView();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        private void configListView_ItemActivate(object sender, EventArgs e)
        {
            if (configListView.SelectedItems.Count > 1)
            {
                System.Media.SystemSounds.Beep.Play();
            }
            else
            {
                ListViewItem item = getContextMenuItem(sender);

                if (item != null)
                {
                    openItem(item.Tag);
                }
            }
        }

        private void configTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Button)
            {
            case MouseButtons.Left:
            case MouseButtons.None:
                if (e.Node == null)
                {
                    // Do Nothing
                }
                else
                {
                    openItem(e.Node.Tag);
                }
                break;
            }
        }

        private void refreshView()
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                config.refresh(configTreeView);
                if (configTreeView.SelectedNode != null)
                {
                    TreeNode node = configTreeView.SelectedNode;
                    configTreeView.SelectedNode = null;
                    configTreeView.SelectedNode = node;
                }
                else
                {
                    configListView.Tag = null;
                }
                config.refresh(configListView);
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }

        private bool openItem(object data)
        {
            bool operatedOn = true;

            try
            {
                if (data == null)
                {
                    // Do Nothing
                    operatedOn = false;
                }
                else if (data is CriteriaLaser)
                {
                    Status prev = setStatus(Status.Busy);
                    FormCriteriaLaser dlg = new FormCriteriaLaser((CriteriaLaser)data);

                    switch (dlg.ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        configTreeView.BeginUpdate();
                        config.markDirty();
                        config.refresh(configTreeView.Nodes);
                        config.refresh(configListView);
                        configTreeView.EndUpdate();
                        setStatus(Status.Dirty);
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
                else if (data is CriteriaMissile)
                {
                    CriteriaMissile crit = (CriteriaMissile)data;
                }
                else if (data is SpecLaser)
                {
                    Status prev = setStatus(Status.Busy);
                    FormSpecLaser dlg = new FormSpecLaser((SpecLaser)data);

                    switch (dlg.ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        configTreeView.BeginUpdate();
                        config.markDirty();
                        config.refresh(configTreeView.Nodes);
                        config.refresh(configListView);
                        configTreeView.EndUpdate();
                        setStatus(Status.Dirty);
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
                else if (data is SpecMissile)
                {
                    Status prev = setStatus(Status.Busy);
                    FormSpecMissile dlg = new FormSpecMissile((SpecMissile)data);

                    switch (dlg.ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        configTreeView.BeginUpdate();
                        config.markDirty();
                        config.refresh(configTreeView.Nodes);
                        config.refresh(configListView);
                        configTreeView.EndUpdate();
                        setStatus(Status.Dirty);
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
                else if (data is Changeling.Lib.FileDetails)
                {
                    Changeling.Lib.FileDetails details = (Changeling.Lib.FileDetails)data;
                    XmlEditorForm dlg = new XmlEditorForm(details.open());

                    dlg.ShowDialog(this);

                    details.save(dlg.save());

                    configTreeView.BeginUpdate();
                    config.markDirty();
                    config.refresh(configTreeView.Nodes);
                    config.refresh(configListView);
                    configTreeView.EndUpdate();
                    setStatus(Status.Dirty);
                }
                else
                {
                    operatedOn = false;
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }

            return operatedOn;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.LoadingSaving);
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Load Session";
            dlg.Filter = "Session File (*.ses)|*.ses|XML Files (*.xml)|*.xml";
            dlg.DefaultExt = ".ses";
            dlg.FileName = (!fileName.EndsWith(".ses") ? "config.ses" : fileName);
            dlg.AddExtension = true;

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    toolStripStatusFileName.Text = "Loading: " + dlg.FileName;
                    Refresh();
                    Cursor last = Cursor.Current;
                    Cursor.Current=Cursors.WaitCursor;
                    fileName = "";
                    try
                    {
                        if (config.load(dlg.FileName))
                        {
                            fileName = dlg.FileName;
                            setStatus(Status.Ok);
                        }
                        else
                        {
                            System.Media.SystemSounds.Beep.Play();
                            setStatus(prev);
                        }
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    toolStripStatusFileName.Text = fileName;
                    refreshView();
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName.Equals(""))
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
            else if (config.isDirty())
            {
                setStatus(Status.LoadingSaving);
                toolStripStatusFileName.Text = "Saving: " + fileName;
                Refresh();
                Cursor last = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    config.save(fileName);
                    setStatus(Status.Ok);
                }
                catch (Exception err)
                {
                    displayError(err);
                }
                toolStripStatusFileName.Text = fileName;
                Cursor.Current = last;
            }
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.LoadingSaving);
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "Save Session";
            dlg.Filter = "Session File (*.ses)|*.ses|XML Files (*.xml)|*.xml";
            dlg.DefaultExt = ".ses";
            dlg.FileName = (!fileName.EndsWith(".ses") ? "config.ses" : fileName);
            dlg.AddExtension = true;

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    toolStripStatusFileName.Text = "Saving: " + dlg.FileName;
                    Refresh();
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.save(dlg.FileName);
                        fileName = dlg.FileName;
                        setStatus(Status.Ok);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    toolStripStatusFileName.Text = fileName;
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importEESConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Import EES Configuration";
            dlg.Filter = "XML Files (*.xml)|" + Configuration.fileNameEES + "*.xml";
            dlg.DefaultExt = ".xml";
            dlg.FileName = Configuration.fileNameEES + ".xml";

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.importEES(dlg.FileName);
                        refreshView();
                        setStatus(Status.Dirty);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void exportEESConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "Export EES Configuration";
            dlg.Filter = "XML Files (*.xml)|" + Configuration.fileNameEES + "*.xml";
            dlg.DefaultExt = ".xml";
            dlg.FileName = Configuration.fileNameEES + ".xml";

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.exportEES(dlg.FileName);
                        setStatus(prev);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void importMARSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Import MARS Configuration";
            dlg.Filter = "XML Files (*.xml)|" + Configuration.fileNameMARS + "*.xml";
            dlg.DefaultExt = ".xml";
            dlg.FileName = config.getLocalisedFileName(Configuration.fileNameMARS) + ".xml";

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.importMARS(dlg.FileName);
                        refreshView();
                        setStatus(Status.Dirty);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void exportMARSConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            SaveFileDialog dlg = new SaveFileDialog();

            dlg.Title = "Export MARS Configuration";
            dlg.Filter = "XML Files (*.xml)|"+Configuration.fileNameMARS+"*.xml";
            dlg.DefaultExt = ".xml";
            dlg.FileName = config.getLocalisedFileName(Configuration.fileNameMARS) + ".xml";

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.exportMARS(dlg.FileName);
                        setStatus(prev);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void importGameDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.importGameData(dlg.SelectedPath);
                        refreshView();
                        setStatus(Status.Dirty);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void exportGameDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            FolderBrowserDialog dlg = new FolderBrowserDialog();

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.exportGameData(dlg.SelectedPath);
                        setStatus(prev);
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void importModZipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Status prev = setStatus(Status.Busy);
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Title = "Import Modification";
            dlg.Filter = "ZIP Files (*.zip)|*.zip";
            dlg.DefaultExt = ".zip";

            switch (dlg.ShowDialog(this))
            {
            case System.Windows.Forms.DialogResult.OK:
                {
                    Cursor last = Cursor.Current;
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        config.importMod(dlg.FileName);
                        if (config.isDirty())
                        {
                            refreshView();
                            setStatus(Status.Dirty);
                        }
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                    Cursor.Current = last;
                }
                break;

            default:
                setStatus(prev);
                break;
            }
        }

        private void exportModZipToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox dlg=new AboutBox();

            dlg.ShowDialog(this);
        }

        private void configTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            switch (e.Button)
            {
            case System.Windows.Forms.MouseButtons.Left:
                if (e.Item is TreeNode)
                {
                    TreeNode node = e.Item as TreeNode;

                    if (node.Tag is IConfigDatumList)
                    {
                        dragSelectTime = System.DateTime.Now;

                        ConfigDatumSelection selectedDatums = (node.Tag as IConfigDatumList).getAllDatums();

                        DoDragDrop(selectedDatums, DragDropEffects.Link | DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll);
                    }
                }
                break;
            }
        }

        private void configListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            switch (e.Button)
            {
            case System.Windows.Forms.MouseButtons.Left:
                if (e.Item is ListViewItem)
                {
                    ListViewItem node = e.Item as ListViewItem;

                    if (node.Tag is ConfigDatum)
                    {
                        dragSelectTime = System.DateTime.Now;
                        ConfigDatumSelection selectedDatums = new ConfigDatumSelection();
                        selectedDatums.selectedFrom = configTreeView.SelectedNode.Tag as IConfigDatumList;
                        foreach (ListViewItem item in configListView.SelectedItems)
                        {
                            selectedDatums.Add(item.Index, item.Tag as ConfigDatum);
                        }

                        DoDragDrop(selectedDatums, DragDropEffects.Link | DragDropEffects.Move | DragDropEffects.Copy | DragDropEffects.Scroll);
                    }
                }
                break;
            }
        }

        private void configTreeView_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = configTreeView.PointToClient(new Point(e.X, e.Y));

            if (targetPoint.Y < (configTreeView.ItemHeight / 2))
            {
                if (configTreeView.TopNode.PrevVisibleNode != null)
                {
                    configTreeView.TopNode = configTreeView.TopNode.PrevVisibleNode;
                }
            }
            else if (targetPoint.Y >= ((configTreeView.VisibleCount - 1) * configTreeView.ItemHeight))
            {
                if (configTreeView.TopNode.NextVisibleNode != null)
                {
                    configTreeView.TopNode = configTreeView.TopNode.NextVisibleNode;
                }
            }

            // Select the node at the mouse position.
            TreeNode targetNode = configTreeView.GetNodeAt(targetPoint);

            if (targetNode == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else 
            {
                if (configTreeView.SelectedNode != targetNode)
                {
                    dragSelectTime = System.DateTime.Now;
                    configTreeView.SelectedNode = targetNode;
                    ConfigDatumSelection subjectDatums = (ConfigDatumSelection)e.Data.GetData(typeof(ConfigDatumSelection));

                    if (subjectDatums == null)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else if (subjectDatums.Count == 0)
                    {
                        e.Effect = DragDropEffects.None;
                    }
                    else if (targetNode.Tag is IConfigDatumList)
                    {
                        IConfigDatumList targetDatumList = targetNode.Tag as IConfigDatumList;

                        if (subjectDatums.selectedFrom is SpecLaserList &&
                            targetDatumList is CriteriaLaserList)
                        {
                            e.Effect = DragDropEffects.Link; 
                        }
                        else if (subjectDatums.selectedFrom is SpecMissileList &&
                                 targetDatumList is CriteriaMissileList)
                        {
                            e.Effect = DragDropEffects.Link;
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
                else if (System.DateTime.Now.Subtract(dragSelectTime).TotalSeconds >= 1.0)
                {
                    dragSelectTime = System.DateTime.Now.AddSeconds(10);
                    if (targetNode.IsExpanded)
                    {
                        targetNode.Collapse();
                    }
                    else
                    {
                        targetNode.Expand();
                    }
                }
            }
        }

        private void configListView_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = configListView.PointToClient(new Point(e.X, e.Y));
            ListViewItem targetItem = configListView.GetItemAt(targetPoint.X, targetPoint.Y);

            if (targetItem != null)
            {
                if (targetPoint.Y < (configListView.Font.GetHeight() / 2))
                {
                    configListView.EnsureVisible(targetItem.Index - 1);
                }
                else if (targetPoint.Y >= (configListView.Height - (configListView.Font.GetHeight() / 2)))
                {
                    if (targetItem.Index != (configListView.Items.Count - 1))
                    {
                        configListView.EnsureVisible(targetItem.Index + 1);
                    }
                }
            }

            if (e.Data == null)
            {
                e.Effect = DragDropEffects.None;
            }
            else if (typeof(ConfigDatumSelection).IsInstanceOfType(e.Data.GetData(typeof(ConfigDatumSelection))))
            {
                ConfigDatumSelection selectedDatums = (ConfigDatumSelection)e.Data.GetData(typeof(ConfigDatumSelection));

                if (selectedDatums == null)
                {
                    e.Effect = DragDropEffects.None;
                }
                else if (selectedDatums.selectedFrom == configTreeView.SelectedNode.Tag)
                {
                    e.Effect = DragDropEffects.Move;
                }
                else if (configTreeView.SelectedNode.Tag is CriteriaLaserList)
                {
                    if (selectedDatums.selectedFrom is SpecLaserList)
                    {
                        e.Effect = DragDropEffects.Link;
                    }
                    else if (selectedDatums.selectedFrom is CriteriaLaserList)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else if (configTreeView.SelectedNode.Tag is CriteriaMissileList)
                {
                    if (selectedDatums.selectedFrom is SpecMissileList)
                    {
                        e.Effect = DragDropEffects.Link;
                    }
                    else if (selectedDatums.selectedFrom is CriteriaMissileList)
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void configTreeView_DragDrop(object sender, DragEventArgs e)
        {
            IConfigDatumList targetList = configTreeView.SelectedNode.Tag as IConfigDatumList;
            ConfigDatumSelection selectedDatums = (ConfigDatumSelection)e.Data.GetData(typeof(ConfigDatumSelection));
            int targetIdx = configListView.Items.Count;

            executeDragDrop(selectedDatums, targetList, targetIdx);
        }

        private void configListView_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = configListView.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.
            ListViewItem targetItem = configListView.GetItemAt(targetPoint.X, targetPoint.Y);
            IConfigDatumList targetList = configTreeView.SelectedNode.Tag as IConfigDatumList;

            ConfigDatumSelection selectedDatums = (ConfigDatumSelection)e.Data.GetData(typeof(ConfigDatumSelection));

            int targetIdx=configListView.Items.Count;
            if (targetItem!=null)
            {
                targetIdx=targetItem.Index;

                if (selectedDatums.ContainsValue(targetItem.Tag as ConfigDatum))
                {
                    targetList = null;
                }
            }

            executeDragDrop(selectedDatums, targetList, targetIdx);
        }

        private void executeDragDrop(ConfigDatumSelection selectedDatums, IConfigDatumList targetList, int targetIdx)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            if (targetList != null && selectedDatums != null)
            {
                if (selectedDatums.selectedFrom == targetList)
                {
                    try
                    {
                        if (selectedDatums.selectedFrom is SpecLaserList)
                        {
                            setStatus(Status.Busy);
                            try
                            {
                                Int32 offset = 0;
                                foreach (KeyValuePair<Int32, ConfigDatum> datum in selectedDatums)
                                {
                                    SpecLaser spec = datum.Value as SpecLaser;
                                    Int32 specIdx = datum.Key < targetIdx ? datum.Key - offset : datum.Key;

                                    config.move(spec, specIdx, targetIdx);

                                    ListViewItem specItem = configListView.Items[specIdx];
                                    specItem.Remove();
                                    configListView.Items.Insert(targetIdx, specItem);

                                    if (datum.Key < targetIdx)
                                    {
                                        offset++;
                                    }
                                    else
                                    {
                                        targetIdx++;
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                MainForm.displayError(this, err, true);
                            }

                            configTreeView.BeginUpdate();
                            foreach (TreeNode raceNode in configTreeView.Nodes["Races"].Nodes)
                            {
                                config.refresh(raceNode.Nodes["Lasers"]);
                            }
                            configTreeView.EndUpdate();
                            setStatus(Status.Dirty);
                        }
                        else if (selectedDatums.selectedFrom is SpecMissileList)
                        {
                            setStatus(Status.Busy);
                            try
                            {
                                Int32 offset = 0;
                                foreach (KeyValuePair<Int32, ConfigDatum> datum in selectedDatums)
                                {
                                    SpecMissile spec = datum.Value as SpecMissile;
                                    Int32 specIdx = datum.Key < targetIdx ? datum.Key - offset : datum.Key;

                                    config.move(spec, specIdx, targetIdx);

                                    ListViewItem specItem = configListView.Items[specIdx];
                                    specItem.Remove();
                                    configListView.Items.Insert(targetIdx, specItem);

                                    if (datum.Key < targetIdx)
                                    {
                                        offset++;
                                    }
                                    else
                                    {
                                        targetIdx++;
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                MainForm.displayError(this, err, true);
                            }

                            configTreeView.BeginUpdate();
                            foreach (TreeNode raceNode in configTreeView.Nodes["Races"].Nodes)
                            {
                                config.refresh(raceNode.Nodes["Missiles"]);
                            }
                            configTreeView.EndUpdate();
                            setStatus(Status.Dirty);
                        }
                        else if (selectedDatums.selectedFrom is CriteriaLaserList)
                        {
                            setStatus(Status.Busy);
                            try
                            {
                                Int32 offset = 0;
                                foreach (KeyValuePair<Int32, ConfigDatum> datum in selectedDatums)
                                {
                                    CriteriaLaser crit = datum.Value as CriteriaLaser;
                                    Int32 critIdx = datum.Key < targetIdx ? datum.Key - offset : datum.Key;

                                    config.move(crit, configTreeView.SelectedNode.Parent.Name, critIdx, targetIdx);

                                    ListViewItem specItem = configListView.Items[critIdx];
                                    specItem.Remove();
                                    configListView.Items.Insert(targetIdx, specItem);

                                    if (datum.Key < targetIdx)
                                    {
                                        offset++;
                                    }
                                    else
                                    {
                                        targetIdx++;
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                MainForm.displayError(this, err, true);
                            }
                            setStatus(Status.Dirty);
                        }
                        else if (selectedDatums.selectedFrom is CriteriaMissileList)
                        {
                            setStatus(Status.Busy);
                            try
                            {
                                Int32 offset = 0;
                                foreach (KeyValuePair<Int32, ConfigDatum> datum in selectedDatums)
                                {
                                    CriteriaMissile crit = datum.Value as CriteriaMissile;
                                    Int32 critIdx = datum.Key < targetIdx ? datum.Key - offset : datum.Key;

                                    config.move(crit, configTreeView.SelectedNode.Parent.Name, critIdx, targetIdx);

                                    ListViewItem specItem = configListView.Items[critIdx];
                                    specItem.Remove();
                                    configListView.Items.Insert(targetIdx, specItem);

                                    if (datum.Key < targetIdx)
                                    {
                                        offset++;
                                    }
                                    else
                                    {
                                        targetIdx++;
                                    }
                                }
                            }
                            catch (Exception err)
                            {
                                MainForm.displayError(this, err, true);
                            }
                            setStatus(Status.Dirty);
                        }
                        else
                        {
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                    catch (Exception err)
                    {
                        displayError(err);
                    }
                }
                else
                {
                    if (selectedDatums.selectedFrom is SpecLaserList)
                    {
                        if (targetList is CriteriaLaserList)
                        {
                            Status prev = setStatus(Status.Busy);
                            foreach (KeyValuePair<Int32, ConfigDatum> datum in selectedDatums)
                            {
                                try
                                {
                                    ListViewItem critItem = config.add(datum.Value as SpecLaser, datum.Key, configTreeView.SelectedNode.Parent.Name, targetIdx);

                                    if (critItem != null)
                                    {
                                        configListView.Items.Insert(targetIdx, critItem);

                                        targetIdx++;
                                        setStatus(Status.Dirty);
                                    }
                                }
                                catch (Exception err)
                                {
                                    MainForm.displayError(this, err);
                                }
                            }
                            refreshView();
                        }
                        else
                        {
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                    else if (selectedDatums.selectedFrom is SpecMissileList)
                    {
                        if (targetList is CriteriaMissileList)
                        {
                            Status prev = setStatus(Status.Busy);
                            foreach (KeyValuePair<Int32, ConfigDatum> datum in selectedDatums)
                            {
                                try
                                {
                                    ListViewItem critItem = config.add(datum.Value as SpecMissile, datum.Key, configTreeView.SelectedNode.Parent.Name, targetIdx);

                                    if (critItem != null)
                                    {
                                        configListView.Items.Insert(targetIdx, critItem);

                                        targetIdx++;
                                        prev = Status.Dirty;
                                    }
                                }
                                catch (Exception err)
                                {
                                    MainForm.displayError(this, err);
                                }
                            }
                            refreshView();
                        }
                        else
                        {
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                    else
                    {
                        System.Media.SystemSounds.Beep.Play();
                    }
                }
            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
            }
            Cursor.Current = last;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (config.isDirty())
            {
                switch (MessageBox.Show(this, "Do you wish to save your session changes?", "Session changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3))
                {
                case System.Windows.Forms.DialogResult.Yes:
                    saveToolStripMenuItem_Click(sender, e);
                    e.Cancel = config.isDirty();
                    break;

                case System.Windows.Forms.DialogResult.No:
                    switch (MessageBox.Show(this, "Are you sure?", "Changes will be lost", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2))
                    {
                    case System.Windows.Forms.DialogResult.Yes:
                        break;

                    default:
                        e.Cancel = true;
                        break;
                    }
                    break;

                default:
                    e.Cancel = true;
                    break;
                }
            }
        }

        private void configTreeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            TreeNode node = configTreeView.SelectedNode;

            if (node != null)
            {
                switch (e.KeyChar)
                {
                case '\x0D':    // CR
                    if (openItem(node.Tag))
                    {
                        e.Handled = true;
                    }
                    break;
                }
            }
        }

        private void configTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
        }

        private void configTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            configListView.ContextMenuStrip = null;
            TreeNode subjectNode = e.Node;

            if (subjectNode != null)
            {
                object data = subjectNode.Tag;

                if (data is IConfigDatumList)
                {
                    IConfigDatumList list = (IConfigDatumList)data;

                    list.refresh(configListView);

                    if (list is CriteriaLaserList ||
                        list is CriteriaMissileList)
                    {
                        configListView.ContextMenuStrip = contextMenuStripCriteria;
                    }
                    else if (list is SpecLaserList ||
                             list is SpecMissileList)
                    {
                        configListView.ContextMenuStrip = contextMenuStripSpec;
                    }
                    else if (list is Changeling.Lib.FileDetailsList)
                    {
                        configListView.ContextMenuStrip = contextMenuStripFileDetails;
                    }
                }
                else
                {
                    // Not a list - So ensure List View is clear
                    configListView.Tag = null;
                    configListView.Items.Clear();
                    configListView.Columns.Clear();
                    System.GC.Collect();
                }
            }
        }

        private void configTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode subjectNode = e.Node;

            if (subjectNode != null)
            {
                object data = subjectNode.Tag;

                configTreeView.SelectedNode = subjectNode;

                if (data == null)
                {
                    // Do Nothing - Static Items
                }
                else if (typeof(CriteriaLaser).IsInstanceOfType(data) ||
                         typeof(CriteriaMissile).IsInstanceOfType(data))
                {
                    if (subjectNode.ContextMenuStrip == null)
                    {
                        subjectNode.ContextMenuStrip = contextMenuStripCriteria;
                    }
                }
                else if (typeof(SpecLaser).IsInstanceOfType(data) ||
                         typeof(SpecMissile).IsInstanceOfType(data))
                {
                    if (subjectNode.ContextMenuStrip == null)
                    {
                        subjectNode.ContextMenuStrip = contextMenuStripSpec;
                    }
                }
            }
        }

        private TreeNode getContextMenuNode(object sender)
        {
            TreeNode subject = configTreeView.SelectedNode;

            if (typeof(TreeNode).IsInstanceOfType(sender))
            {
                if (subject != sender)
                {
                    subject = (TreeNode)sender;
                    configTreeView.SelectedNode = subject;
                }
            }

            return subject;
        }

        private ListViewItem getContextMenuItem(object sender)
        {
            ListViewItem subject = configListView.FocusedItem;

            if (typeof(ListViewItem).IsInstanceOfType(sender))
            {
                if (subject != sender)
                {
                    subject = (ListViewItem)sender;
                    configListView.FocusedItem = subject;
                }
            }

            return subject;
        }

        private void contextMenuStripSpec_Opening(object sender, CancelEventArgs e)
        {
            if (configTreeView.SelectedNode != null)
            {
                addSpecMenuItem.Enabled = (configListView.Items.Count < Configuration.maxWeaponIdx);
                deleteSpecMenuItem.Enabled = (configListView.SelectedItems.Count > 0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void addSpecMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem targetItem = getContextMenuItem(sender);

                object context = null;
                int newItemIndex = -1;

                if (targetItem != null)
                {
                    context=targetItem.Tag;
                    newItemIndex = targetItem.Index;
                }
                else if (configTreeView.SelectedNode != null)
                {
                    context=configTreeView.SelectedNode.Tag;
                    newItemIndex = configListView.Items.Count;
                }


                if (context is SpecLaser || context is SpecLaserList)
                {
                    Status prev = setStatus(Status.Busy);
                    SpecLaser spec = new SpecLaser(config);

                    switch (new FormSpecLaser(spec).ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        ListViewItem node = config.add(spec, newItemIndex);
                        if (node != null)
                        {
                            configListView.Items.Insert(newItemIndex, node);

                            configTreeView.BeginUpdate();
                            foreach (TreeNode raceNode in configTreeView.Nodes["Races"].Nodes)
                            {
                                config.refresh(raceNode.Nodes["Lasers"]);
                            }
                            configTreeView.EndUpdate();
                            setStatus(Status.Dirty);
                        }
                        else
                        {
                            setStatus(prev);
                            System.Media.SystemSounds.Beep.Play();
                        }
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
                else if (context is SpecMissile || context is SpecMissileList)
                {
                    Status prev = setStatus(Status.Busy);
                    SpecMissile spec = new SpecMissile(config);

                    switch (new FormSpecMissile(spec).ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        ListViewItem node = config.add(spec, newItemIndex);
                        if (node != null)
                        {
                            configListView.Items.Insert(newItemIndex, node);

                            configTreeView.BeginUpdate();
                            foreach (TreeNode raceNode in configTreeView.Nodes["Races"].Nodes)
                            {
                                config.refresh(raceNode.Nodes["Missiles"]);
                            }
                            configTreeView.EndUpdate();
                            setStatus(Status.Dirty);
                        }
                        else
                        {
                            setStatus(prev);
                            System.Media.SystemSounds.Beep.Play();
                        }
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
                else
                {
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
        }

        private void deleteSpecMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                while (configListView.SelectedItems.Count>0)
                {
                    ListViewItem subjectItem = configListView.SelectedItems[0];
                    Int32 subjectIdx = subjectItem.Index;
                    object data = subjectItem.Tag;

                    if (data is SpecLaser)
                    {
                        setStatus(Status.Busy);
                        config.delete((SpecLaser)data, subjectIdx);
                        subjectItem.Remove();

                        configTreeView.BeginUpdate();
                        foreach (TreeNode raceNode in configTreeView.Nodes["Races"].Nodes)
                        {
                            config.refresh(raceNode.Nodes["Lasers"]);
                        }
                        configTreeView.EndUpdate();
                        setStatus(Status.Dirty);
                    }
                    else if (data is SpecMissile)
                    {
                        setStatus(Status.Busy);
                        config.delete((SpecMissile)data, subjectIdx);
                        subjectItem.Remove();

                        configTreeView.BeginUpdate();
                        foreach (TreeNode raceNode in configTreeView.Nodes["Races"].Nodes)
                        {
                            config.refresh(raceNode.Nodes["Missiles"]);
                        }
                        configTreeView.EndUpdate();
                        setStatus(Status.Dirty);
                    }
                    else
                    {
                        subjectItem.Selected=false;
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }

        private void contextMenuStripRace_Opening(object sender, CancelEventArgs e)
        {
            TreeNode subjectNode = getContextMenuNode(sender);

            if (subjectNode != null)
            {
                // Do Nothing - Default Settings Ok
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void cloneRacePrioritiesMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode subjectNode = getContextMenuNode(sender);
                ListViewItem subjectItem = getContextMenuItem(sender);

                if (subjectNode != null)
                {
                    Status prev=setStatus(Status.Busy);
                    String raceName = subjectNode.Name;
                    FormSelectRace dlg = new FormSelectRace(raceName);

                    switch (dlg.ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        {
                            Cursor last = Cursor.Current;
                            Cursor.Current = Cursors.WaitCursor;
                            foreach (String dstRaceName in dlg.listRaceName)
                            {
                                config.cloneCriteria(raceName, dstRaceName);
                            }
                            refreshView();
                            setStatus(Status.Dirty);
                            Cursor.Current = last;
                        }
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
        }

        private void clearRacePrioritiesMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            TreeNode raceNode = getContextMenuNode(sender);

            if (raceNode != null)
            {
                Status prev = setStatus(Status.Busy);
                String raceName = raceNode.Name;

                foreach (TreeNode subjectNode in raceNode.Nodes)
                {
                    object data = subjectNode.Tag;
                    if (data is CriteriaLaserList)
                    {
                        CriteriaLaserList list = data as CriteriaLaserList;

                        while (list.Count > 0)
                        {
                            int idx=list.Count-1;
                            config.delete(list[idx], raceName, idx);
                        }
                    }
                    else if (data is CriteriaMissileList)
                    {
                        CriteriaMissileList list = data as CriteriaMissileList;

                        while (list.Count > 0)
                        {
                            int idx = list.Count - 1;
                            config.delete(list[idx], raceName, idx);
                        }
                    }
                }

                if (config.isDirty())
                {
                    setStatus(Status.Dirty);
                    refreshView();
                }
                else
                {
                    setStatus(prev);
                }
            }
            Cursor.Current = last;
        }

        private void contextMenuStripCriteria_Opening(object sender, CancelEventArgs e)
        {
            if (configTreeView.SelectedNode != null)
            {
                deleteCriteriaMenuItem.Enabled = (configListView.SelectedItems.Count > 0);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void deleteCriteriaMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                String raceName = configTreeView.SelectedNode.Parent.Name;

                while (configListView.SelectedItems.Count > 0)
                {
                    ListViewItem subjectItem = configListView.SelectedItems[0];
                    Int32 subjectIdx = subjectItem.Index;
                    object data = subjectItem.Tag;

                    if (data is CriteriaLaser)
                    {
                        setStatus(Status.Busy);
                        config.delete((CriteriaLaser)data, raceName, subjectIdx);
                        subjectItem.Remove();
                        setStatus(Status.Dirty);
                    }
                    else if (data is CriteriaMissile)
                    {
                        setStatus(Status.Busy);
                        config.delete((CriteriaMissile)data, raceName, subjectIdx);
                        subjectItem.Remove();
                        setStatus(Status.Dirty);
                    }
                    else
                    {
                        subjectItem.Selected = false;
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }

        private void contextMenuStripCriteriaList_Opening(object sender, CancelEventArgs e)
        {
            TreeNode subjectNode = getContextMenuNode(sender);

            if (subjectNode != null)
            {
                // Do Nothing - Default Settings Ok
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void selectCriteriaMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode subjectNode = getContextMenuNode(sender);

                if (subjectNode != null ? subjectNode.Parent != null : false)
                {
                    String raceName = subjectNode.Parent.Name;

                    Status prev=setStatus(Status.Busy);
                    switch (subjectNode.Name)
                    {
                    case "Lasers":
                        new FormSelectLasers(config, configTreeView.Nodes["TLaser"].Tag as SpecLaserList, raceName, configListView.Items).ShowDialog();
                        break;

                    case "Missiles":
                        new FormSelectMissiles(config, configTreeView.Nodes["TMissiles"].Tag as SpecMissileList, raceName, configListView.Items).ShowDialog();
                        break;
                    }
                    if (config.isDirty())
                    {
                        setStatus(Status.Dirty);
                        refreshView();
                    }
                    else
                    {
                        setStatus(prev);
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
        }

        private void cloneCriteriaMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode subjectNode = getContextMenuNode(sender);

                if (subjectNode != null ? subjectNode.Parent != null : false)
                {
                    String raceName = subjectNode.Parent.Name;
                    FormSelectRace dlg = new FormSelectRace(raceName);

                    Status prev = setStatus(Status.Busy);
                    switch (dlg.ShowDialog(this))
                    {
                    case System.Windows.Forms.DialogResult.OK:
                        {
                            Cursor last = Cursor.Current;
                            Cursor.Current = Cursors.WaitCursor;
                            switch (subjectNode.Name)
                            {
                            case "Lasers":
                                foreach (String dstRaceName in dlg.listRaceName)
                                {
                                    config.cloneCriteriaLasers(raceName, dstRaceName);
                                }
                                refreshView();
                                setStatus(Status.Dirty);
                                break;

                            case "Missiles":
                                foreach (String dstRaceName in dlg.listRaceName)
                                {
                                    config.cloneCriteriaMissiles(raceName, dstRaceName);
                                }
                                refreshView();
                                setStatus(Status.Dirty);
                                break;

                            default:
                                setStatus(prev);
                                System.Media.SystemSounds.Beep.Play();
                                break;
                            }
                            Cursor.Current = last;
                        }
                        break;

                    default:
                        setStatus(prev);
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
        }

        private void clearCriteriaMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode subjectNode = getContextMenuNode(sender);

                if (subjectNode != null ? subjectNode.Parent != null : false)
                {
                    String raceName = subjectNode.Parent.Name;
                    Status prev = setStatus(Status.Busy);                

                    while (configListView.Items.Count > 0)
                    {
                        ListViewItem node = configListView.Items[configListView.Items.Count - 1];
                        object data = node.Tag;

                        if (typeof(CriteriaLaser).IsInstanceOfType(data))
                        {
                            config.delete((CriteriaLaser)data, raceName, node.Index);
                        }
                        else if (typeof(CriteriaMissile).IsInstanceOfType(data))
                        {
                            config.delete((CriteriaMissile)data, raceName, node.Index);
                        }

                        node.Remove();
                    }

                    if (config.isDirty())
                    {
                        setStatus(Status.Dirty);
                        refreshView();
                    }
                    else
                    {
                        setStatus(prev);
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }

        private void contextMenuStripSpecList_Opening(object sender, CancelEventArgs e)
        {
            TreeNode subjectNode = getContextMenuNode(sender);

            if (subjectNode != null)
            {
                appendSpecMenuItem.Enabled = (subjectNode.Nodes.Count < Configuration.maxWeaponIdx);
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void appendSpecMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode subjectNode = getContextMenuNode(sender);

                if (subjectNode != null)
                {
                    switch (subjectNode.Name)
                    {
                    case "TLaser":
                        {
                            SpecLaser spec = new SpecLaser(config);

                            switch (new FormSpecLaser(spec).ShowDialog(this))
                            {
                            case System.Windows.Forms.DialogResult.OK:
                                toolStripStatusIcon.Image = imageListStatus.Images[0];
                                ListViewItem node = config.add(spec, configListView.Items.Count);
                                if (node != null)
                                {
                                    configListView.Items.Insert(configListView.Items.Count, node);
                                }
                                break;
                            }
                        }
                        break;

                    case "TMissiles":
                        {
                            SpecMissile spec = new SpecMissile(config);

                            switch (new FormSpecMissile(spec).ShowDialog(this))
                            {
                            case System.Windows.Forms.DialogResult.OK:
                                toolStripStatusIcon.Image = imageListStatus.Images[0];
                                ListViewItem node = config.add(spec, subjectNode.Nodes.Count);
                                if (node != null)
                                {
                                    configListView.Items.Insert(configListView.Items.Count, node);
                                }
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
        }

        private void clearSpecListMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode subjectNode = configTreeView.SelectedNode;

                if (subjectNode != null)
                {
                    toolStripStatusIcon.Image = imageListStatus.Images[0];
                    while (configListView.Items.Count > 0)
                    {
                        ListViewItem node = configListView.Items[configListView.Items.Count-1];
                        object data = node.Tag;

                        if (typeof(SpecLaser).IsInstanceOfType(data))
                        {
                            config.delete((SpecLaser)data, node.Index);
                        }
                        else if (typeof(SpecMissile).IsInstanceOfType(data))
                        {
                            config.delete((SpecMissile)data, node.Index);
                        }

                        node.Remove();
                    }

                    configTreeView.BeginUpdate();
                    config.refresh(configTreeView.Nodes["Races"]);
                    configTreeView.EndUpdate();
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                TreeNode rootNode = getContextMenuNode(sender);

                if (rootNode != null ? rootNode.Name.Equals("Races") : false)
                {
                    Status prev = setStatus(Status.Busy);

                    toolStripStatusIcon.Image = imageListStatus.Images[0];
                    foreach (TreeNode raceNode in rootNode.Nodes)
                    {
                        String raceName = raceNode.Name;

                        foreach (TreeNode subjectNode in raceNode.Nodes)
                        {
                            object data = subjectNode.Tag;
                            if (data is CriteriaLaserList)
                            {
                                CriteriaLaserList list = data as CriteriaLaserList;

                                while (list.Count > 0)
                                {
                                    int idx = list.Count - 1;
                                    config.delete(list[idx], raceName, idx);
                                }
                            }
                            else if (data is CriteriaMissileList)
                            {
                                CriteriaMissileList list = data as CriteriaMissileList;

                                while (list.Count > 0)
                                {
                                    int idx = list.Count - 1;
                                    config.delete(list[idx], raceName, idx);
                                }
                            }
                        }
                    }

                    if (config.isDirty())
                    {
                        setStatus(Status.Dirty);
                        refreshView();
                    }
                    else
                    {
                        setStatus(prev);
                    }
                }
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }

        private void languageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                if (sender == item)
                {
                    if (!item.Checked)
                    {
                        item.Checked = true;
                        config.setLocale(int.Parse((String)item.Tag));
                        refreshView();
                    }
                }
                else
                {
                    item.Checked = false;
                }
            }
            Cursor.Current = last;
        }

        private void contextMenuStripFileDetails_Opening(object sender, CancelEventArgs e)
        {
            if (configTreeView.SelectedNode != null)
            {
                revertFileDetailsToolStripMenuItem.Enabled=false;
                foreach (ListViewItem item in configListView.SelectedItems)
                {
                    if (item.Tag is Changeling.Lib.FileDetails)
                    {
                        Changeling.Lib.FileDetails file = item.Tag as Changeling.Lib.FileDetails;

                        revertFileDetailsToolStripMenuItem.Enabled |= file.isModified;
                    }
                }
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void newFileDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Changeling.Lib.FileDetailsList fileDetailsList = configListView.Tag as Changeling.Lib.FileDetailsList;

            if (fileDetailsList != null)
            {
            }
        }

        private void revertFileDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor last = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                Status endStatus=setStatus(Status.Busy);

                Changeling.Lib.FileDetailsList fileDetailsList = configListView.Tag as Changeling.Lib.FileDetailsList;
                while (configListView.SelectedItems.Count > 0)
                {
                    ListViewItem subjectItem = configListView.SelectedItems[0];
                    Int32 subjectIdx = subjectItem.Index;
                    object data = subjectItem.Tag;

                    if (data is Changeling.Lib.FileDetails)
                    {
                        Changeling.Lib.FileDetails fileDetails = (Changeling.Lib.FileDetails)data;

                        if (fileDetails.isModified)
                        {
                            fileDetails.revert();
                            if (fileDetails.fullFilePath == null)
                            {
                                fileDetailsList.Remove(fileDetails);
                                subjectItem.Remove();
                            }
                            else
                            {
                                fileDetails.refresh(subjectItem);
                            }
                            endStatus = Status.Dirty;
                        }
                        else
                        {
                            subjectItem.Selected = false;
                        }
                    }
                    else
                    {
                        subjectItem.Selected = false;
                    }
                }
                setStatus(endStatus);
            }
            catch (Exception err)
            {
                displayError(err);
            }
            Cursor.Current = last;
        }
    }
}
