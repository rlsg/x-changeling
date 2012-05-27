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
    public partial class FormSelectRace : Form
    {
        private string raceName;
        public List<string> listRaceName = new List<string>();

        public FormSelectRace(string raceName)
        {
            this.raceName = raceName;

            InitializeComponent();

            foreach (String targetRaceName in Configuration.raceStrings)
            {
                if (targetRaceName != raceName)
                {
                    ListViewItem newItem = new ListViewItem();

                    newItem.Name = targetRaceName;
                    newItem.Text = targetRaceName;

                    listViewRaces.Items.Add(newItem);
                }
            }

            buttonOk.Enabled = false;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selItem in listViewRaces.SelectedItems)
            {
                listRaceName.Add(selItem.Name);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void listViewRaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = (listViewRaces.SelectedItems.Count != 0);
        }

        private void FormSelectRace_HelpRequested(object sender, HelpEventArgs hlpevent)
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
