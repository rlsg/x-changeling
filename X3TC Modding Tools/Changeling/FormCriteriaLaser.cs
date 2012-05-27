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
using Changeling.Lib;
using Changeling.Lib.X3TC;

namespace Changeling
{
    public partial class FormCriteriaLaser : Form
    {
        CriteriaLaser crit;

        internal FormCriteriaLaser(CriteriaLaser crit)
        {
            this.crit = crit;

            InitializeComponent();

            textBoxBias.Text = crit.bias.ToString();
            buttonOk.Enabled = false;
        }

        private void textBoxBias_TextChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = true;
            NumericI bias = 0;

            if (bias.TryParse(textBoxBias.Text))
            {
                if (bias != crit.bias)
                {
                    if (1 <= bias && bias <= 10000)
                    {
                        textBoxBias.BackColor = Color.Yellow;
                    }
                    else
                    {
                        buttonOk.Enabled = false;
                        textBoxBias.BackColor = Color.Orange;
                    }
                }
                else
                {
                    textBoxBias.BackColor = Color.FromName("Window");
                }
            }
            else
            {
                buttonOk.Enabled = false;
                textBoxBias.BackColor = Color.Red;
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (crit.bias.TryParse(textBoxBias.Text))
            {
                // Data OK
            }
            else
            {
                // INTERNAL ERROR - Data Invalid
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void FormCriteriaLaser_HelpRequested(object sender, HelpEventArgs hlpevent)
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
