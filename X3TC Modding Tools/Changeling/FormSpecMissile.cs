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
    public partial class FormSpecMissile : Form
    {
        SpecMissile spec;
        Int32 changedCharacteristics=0;

        internal FormSpecMissile(SpecMissile spec)
        {
            this.spec = spec;

            InitializeComponent();

            textBoxLabelID.Text = spec.displayNameId.ToString();
            textBoxLabel.Text = spec.getText(spec.displayNameId);
            textBoxDescription.Text = spec.getText(spec.displayNameId + 1);
            textBoxName.Text = spec.name;
            comboBoxMount.Items.AddRange(SpecMissile.typeStrings);
            comboBoxMount.Text = spec.type;
            textBoxWareClass.Text = spec.wareClass.ToString();
            textBoxWareSize.Text = spec.wareSize.ToString();
            changedCharacteristics = spec.characteristics;
            checkBox00.Checked = ((spec.characteristics & (1 << 00)) != 0);
            checkBox01.Checked = ((spec.characteristics & (1 << 01)) != 0);
            checkBox02.Checked = ((spec.characteristics & (1 << 02)) != 0);
            checkBox03.Checked = ((spec.characteristics & (1 << 03)) != 0);
            checkBox04.Checked = ((spec.characteristics & (1 << 04)) != 0);
            checkBox05.Checked = ((spec.characteristics & (1 << 05)) != 0);
            checkBox06.Checked = ((spec.characteristics & (1 << 06)) != 0);
            checkBox07.Checked = ((spec.characteristics & (1 << 07)) != 0);
            checkBox08.Checked = ((spec.characteristics & (1 << 08)) != 0);
            checkBox09.Checked = ((spec.characteristics & (1 << 09)) != 0);
            checkBox10.Checked = ((spec.characteristics & (1 << 10)) != 0);
            checkBox11.Checked = ((spec.characteristics & (1 << 11)) != 0);
            checkBox12.Checked = ((spec.characteristics & (1 << 12)) != 0);
            checkBox13.Checked = ((spec.characteristics & (1 << 13)) != 0);
            checkBox14.Checked = ((spec.characteristics & (1 << 14)) != 0);
            checkBox15.Checked = ((spec.characteristics & (1 << 15)) != 0);
            checkBox16.Checked = ((spec.characteristics & (1 << 16)) != 0);
            checkBox17.Checked = ((spec.characteristics & (1 << 17)) != 0);
            checkBox18.Checked = ((spec.characteristics & (1 << 18)) != 0);
            checkBox19.Checked = ((spec.characteristics & (1 << 19)) != 0);
            checkBox20.Checked = ((spec.characteristics & (1 << 20)) != 0);
            checkBox21.Checked = ((spec.characteristics & (1 << 21)) != 0);
            checkBox22.Checked = ((spec.characteristics & (1 << 22)) != 0);
            checkBox23.Checked = ((spec.characteristics & (1 << 23)) != 0);
            checkBox24.Checked = ((spec.characteristics & (1 << 24)) != 0);
            checkBox25.Checked = ((spec.characteristics & (1 << 25)) != 0);
            checkBox26.Checked = ((spec.characteristics & (1 << 26)) != 0);
            checkBox27.Checked = ((spec.characteristics & (1 << 27)) != 0);
            checkBox28.Checked = ((spec.characteristics & (1 << 28)) != 0);
            checkBox29.Checked = ((spec.characteristics & (1 << 29)) != 0);
            checkBox30.Checked = ((spec.characteristics & (1 << 30)) != 0);
            checkBox31.Checked = ((spec.characteristics & (1 << 31)) != 0);
            buttonOk.Enabled = false;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = true;
            if (spec.name != textBoxName.Text)
            {
                textBoxName.BackColor = Color.Yellow;
            }
            else
            {
                textBoxName.BackColor = Color.FromName("Window");
            }
        }

        private void comboBoxMount_TextChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = true;
            if (!spec.type.Equals(comboBoxMount.Text))
            {
                if (SpecMissile.typeStrings.Contains(comboBoxMount.Text))
                {
                    comboBoxMount.BackColor = Color.Yellow;
                }
                else
                {
                    buttonOk.Enabled = false;
                    comboBoxMount.BackColor = Color.Red;
                }
            }
            else if (!SpecMissile.typeStrings.Contains(comboBoxMount.Text))
            {
                comboBoxMount.BackColor = Color.Pink;
            }
            else
            {
                comboBoxMount.BackColor = Color.FromName("Window");
            }
        }

        private void textBoxWareSize_TextChanged(object sender, EventArgs e)
        {
            Int32 wareSize;

            buttonOk.Enabled = true;
            if (Int32.TryParse(textBoxWareSize.Text, out wareSize))
            {
                if (wareSize != spec.wareSize)
                {
                    textBoxWareSize.BackColor = Color.Yellow;
                }
                else
                {
                    textBoxWareSize.BackColor = Color.FromName("Window");
                }
            }
            else
            {
                textBoxWareSize.BackColor = Color.Red;
                buttonOk.Enabled = false;
            }
        }

        private void textBoxWareClass_TextChanged(object sender, EventArgs e)
        {
            Int32 wareClass;

            buttonOk.Enabled = true;
            if (Int32.TryParse(textBoxWareClass.Text, out wareClass))
            {
                if (wareClass != spec.wareClass)
                {
                    textBoxWareClass.BackColor = Color.Yellow;
                }
                else
                {
                    textBoxWareClass.BackColor = Color.FromName("Window");
                }
                textBoxWareClass.BackColor = Color.FromName("Window");
            }
            else
            {
                textBoxWareClass.BackColor = Color.Red;
                buttonOk.Enabled = false;
            }
        }

        private void FormLaserSpec_Validating(object sender, CancelEventArgs e)
        {
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            spec.name = textBoxName.Text;
            spec.type = comboBoxMount.Text;
            spec.characteristics ^= changedCharacteristics;
            if (spec.wareClass.TryParse(textBoxWareClass.Text) &&
                spec.wareSize.TryParse(textBoxWareSize.Text) &&
                spec.displayNameId.TryParse(textBoxLabelID.Text))
            {
                // Data Ok
            }
            else
            {
                // Data Error - SHOULD NOT HAPPEN
                System.Media.SystemSounds.Beep.Play();
            }
        }

        private void FormMissileSpec_Load(object sender, EventArgs e)
        {

        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = true;
            if (typeof(CheckBox).IsInstanceOfType(sender))
            {
                CheckBox thisBox = (CheckBox)sender;

                Int32 bit = thisBox.TabIndex - checkBox00.TabIndex;

                if (0 <= bit && bit <= 31)
                {
                    Int32 valueMask = 1 << bit;

                    changedCharacteristics ^= valueMask;

                    if ((changedCharacteristics & valueMask) != 0)
                    {
                        thisBox.BackColor = Color.Yellow;
                    }
                    else
                    {
                        thisBox.BackColor = Color.FromName("Window");
                    }
                }
            }
        }

        private void textBoxLabelID_TextChanged(object sender, EventArgs e)
        {
            Int32 labelID=-1;

            buttonOk.Enabled = true;
            if (Int32.TryParse(textBoxLabelID.Text, out labelID))
            {
                if (labelID != spec.displayNameId)
                {
                    textBoxLabel.Text = spec.getText(labelID);
                    textBoxDescription.Text = spec.getText(labelID + 1);

                    if (textBoxLabel.Text!=null && textBoxDescription!=null)
                    {
                        textBoxLabelID.BackColor = Color.Yellow;
                    }
                    else
                    {
                        textBoxLabelID.BackColor = Color.Orange;
                        buttonOk.Enabled = false;
                    }
                }
                else
                {
                    textBoxLabel.Text = spec.getText(labelID);
                    textBoxDescription.Text = spec.getText(labelID + 1);
                    textBoxLabelID.BackColor = Color.FromName("Window");
                }
            }
            else
            {
                textBoxLabelID.BackColor = Color.Red;
                buttonOk.Enabled = false;
            }
        }

        private void FormSpecMissile_HelpRequested(object sender, HelpEventArgs hlpevent)
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
