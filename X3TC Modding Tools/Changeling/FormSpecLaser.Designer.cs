namespace Changeling
{
    partial class FormSpecLaser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxWareSize = new System.Windows.Forms.TextBox();
            this.textBoxWareClass = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.checkBox00 = new System.Windows.Forms.CheckBox();
            this.checkBox01 = new System.Windows.Forms.CheckBox();
            this.checkBox02 = new System.Windows.Forms.CheckBox();
            this.checkBox03 = new System.Windows.Forms.CheckBox();
            this.checkBox04 = new System.Windows.Forms.CheckBox();
            this.checkBox05 = new System.Windows.Forms.CheckBox();
            this.checkBox06 = new System.Windows.Forms.CheckBox();
            this.checkBox07 = new System.Windows.Forms.CheckBox();
            this.checkBox08 = new System.Windows.Forms.CheckBox();
            this.checkBox09 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.checkBox13 = new System.Windows.Forms.CheckBox();
            this.checkBox14 = new System.Windows.Forms.CheckBox();
            this.checkBox15 = new System.Windows.Forms.CheckBox();
            this.checkBox23 = new System.Windows.Forms.CheckBox();
            this.checkBox22 = new System.Windows.Forms.CheckBox();
            this.checkBox21 = new System.Windows.Forms.CheckBox();
            this.checkBox20 = new System.Windows.Forms.CheckBox();
            this.checkBox19 = new System.Windows.Forms.CheckBox();
            this.checkBox18 = new System.Windows.Forms.CheckBox();
            this.checkBox17 = new System.Windows.Forms.CheckBox();
            this.checkBox16 = new System.Windows.Forms.CheckBox();
            this.checkBox24 = new System.Windows.Forms.CheckBox();
            this.checkBox25 = new System.Windows.Forms.CheckBox();
            this.checkBox26 = new System.Windows.Forms.CheckBox();
            this.checkBox27 = new System.Windows.Forms.CheckBox();
            this.checkBox28 = new System.Windows.Forms.CheckBox();
            this.checkBox29 = new System.Windows.Forms.CheckBox();
            this.checkBox30 = new System.Windows.Forms.CheckBox();
            this.checkBox31 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDefaultBias = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxLabelID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxLabel = new System.Windows.Forms.TextBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxAoEClass = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxMinShipSize = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxMaxShipSize = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageEES = new System.Windows.Forms.TabPage();
            this.tabPageMARS = new System.Windows.Forms.TabPage();
            this.comboBoxMount = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageEES.SuspendLayout();
            this.tabPageMARS.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(90, 158);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(196, 20);
            this.textBoxName.TabIndex = 7;
            this.textBoxName.Tag = "Internal unique identifier used by the game";
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            this.textBoxName.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Internal Name";
            this.label1.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Mount";
            this.label2.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Ware Size";
            this.label3.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 240);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Ware Class";
            this.label4.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxWareSize
            // 
            this.textBoxWareSize.Location = new System.Drawing.Point(90, 210);
            this.textBoxWareSize.Name = "textBoxWareSize";
            this.textBoxWareSize.Size = new System.Drawing.Size(196, 20);
            this.textBoxWareSize.TabIndex = 11;
            this.textBoxWareSize.TextChanged += new System.EventHandler(this.textBoxWareSize_TextChanged);
            this.textBoxWareSize.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxWareClass
            // 
            this.textBoxWareClass.Location = new System.Drawing.Point(90, 237);
            this.textBoxWareClass.Name = "textBoxWareClass";
            this.textBoxWareClass.Size = new System.Drawing.Size(196, 20);
            this.textBoxWareClass.TabIndex = 13;
            this.textBoxWareClass.TextChanged += new System.EventHandler(this.textBoxWareClass_TextChanged);
            this.textBoxWareClass.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // buttonCancel
            // 
            this.buttonCancel.CausesValidation = false;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(215, 3);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 56;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.CausesValidation = false;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(134, 2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 55;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // checkBox00
            // 
            this.checkBox00.AutoSize = true;
            this.checkBox00.Location = new System.Drawing.Point(90, 34);
            this.checkBox00.Name = "checkBox00";
            this.checkBox00.Size = new System.Drawing.Size(15, 14);
            this.checkBox00.TabIndex = 17;
            this.checkBox00.UseVisualStyleBackColor = true;
            this.checkBox00.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox00.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox01
            // 
            this.checkBox01.AutoSize = true;
            this.checkBox01.Location = new System.Drawing.Point(111, 34);
            this.checkBox01.Name = "checkBox01";
            this.checkBox01.Size = new System.Drawing.Size(15, 14);
            this.checkBox01.TabIndex = 18;
            this.checkBox01.UseVisualStyleBackColor = true;
            this.checkBox01.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox01.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox02
            // 
            this.checkBox02.AutoSize = true;
            this.checkBox02.Location = new System.Drawing.Point(132, 34);
            this.checkBox02.Name = "checkBox02";
            this.checkBox02.Size = new System.Drawing.Size(15, 14);
            this.checkBox02.TabIndex = 19;
            this.checkBox02.UseVisualStyleBackColor = true;
            this.checkBox02.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox02.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox03
            // 
            this.checkBox03.AutoSize = true;
            this.checkBox03.Location = new System.Drawing.Point(153, 34);
            this.checkBox03.Name = "checkBox03";
            this.checkBox03.Size = new System.Drawing.Size(15, 14);
            this.checkBox03.TabIndex = 20;
            this.checkBox03.UseVisualStyleBackColor = true;
            this.checkBox03.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox03.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox04
            // 
            this.checkBox04.AutoSize = true;
            this.checkBox04.Location = new System.Drawing.Point(174, 34);
            this.checkBox04.Name = "checkBox04";
            this.checkBox04.Size = new System.Drawing.Size(15, 14);
            this.checkBox04.TabIndex = 21;
            this.checkBox04.UseVisualStyleBackColor = true;
            this.checkBox04.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox04.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox05
            // 
            this.checkBox05.AutoSize = true;
            this.checkBox05.Location = new System.Drawing.Point(195, 34);
            this.checkBox05.Name = "checkBox05";
            this.checkBox05.Size = new System.Drawing.Size(15, 14);
            this.checkBox05.TabIndex = 22;
            this.checkBox05.UseVisualStyleBackColor = true;
            this.checkBox05.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox05.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox06
            // 
            this.checkBox06.AutoSize = true;
            this.checkBox06.Location = new System.Drawing.Point(216, 34);
            this.checkBox06.Name = "checkBox06";
            this.checkBox06.Size = new System.Drawing.Size(15, 14);
            this.checkBox06.TabIndex = 23;
            this.checkBox06.UseVisualStyleBackColor = true;
            this.checkBox06.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox06.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox07
            // 
            this.checkBox07.AutoSize = true;
            this.checkBox07.Location = new System.Drawing.Point(237, 34);
            this.checkBox07.Name = "checkBox07";
            this.checkBox07.Size = new System.Drawing.Size(15, 14);
            this.checkBox07.TabIndex = 24;
            this.checkBox07.UseVisualStyleBackColor = true;
            this.checkBox07.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox07.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox08
            // 
            this.checkBox08.AutoSize = true;
            this.checkBox08.Location = new System.Drawing.Point(90, 54);
            this.checkBox08.Name = "checkBox08";
            this.checkBox08.Size = new System.Drawing.Size(15, 14);
            this.checkBox08.TabIndex = 25;
            this.checkBox08.UseVisualStyleBackColor = true;
            this.checkBox08.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox08.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox09
            // 
            this.checkBox09.AutoSize = true;
            this.checkBox09.Location = new System.Drawing.Point(111, 54);
            this.checkBox09.Name = "checkBox09";
            this.checkBox09.Size = new System.Drawing.Size(15, 14);
            this.checkBox09.TabIndex = 26;
            this.checkBox09.UseVisualStyleBackColor = true;
            this.checkBox09.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox09.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(132, 54);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(15, 14);
            this.checkBox10.TabIndex = 27;
            this.checkBox10.UseVisualStyleBackColor = true;
            this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox10.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox11
            // 
            this.checkBox11.AutoSize = true;
            this.checkBox11.Location = new System.Drawing.Point(153, 54);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(15, 14);
            this.checkBox11.TabIndex = 28;
            this.checkBox11.UseVisualStyleBackColor = true;
            this.checkBox11.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox11.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox12
            // 
            this.checkBox12.AutoSize = true;
            this.checkBox12.Location = new System.Drawing.Point(174, 54);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(15, 14);
            this.checkBox12.TabIndex = 29;
            this.checkBox12.UseVisualStyleBackColor = true;
            this.checkBox12.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox12.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox13
            // 
            this.checkBox13.AutoSize = true;
            this.checkBox13.Location = new System.Drawing.Point(195, 54);
            this.checkBox13.Name = "checkBox13";
            this.checkBox13.Size = new System.Drawing.Size(15, 14);
            this.checkBox13.TabIndex = 30;
            this.checkBox13.UseVisualStyleBackColor = true;
            this.checkBox13.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox13.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox14
            // 
            this.checkBox14.AutoSize = true;
            this.checkBox14.Location = new System.Drawing.Point(216, 54);
            this.checkBox14.Name = "checkBox14";
            this.checkBox14.Size = new System.Drawing.Size(15, 14);
            this.checkBox14.TabIndex = 31;
            this.checkBox14.UseVisualStyleBackColor = true;
            this.checkBox14.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox14.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox15
            // 
            this.checkBox15.AutoSize = true;
            this.checkBox15.Location = new System.Drawing.Point(237, 54);
            this.checkBox15.Name = "checkBox15";
            this.checkBox15.Size = new System.Drawing.Size(15, 14);
            this.checkBox15.TabIndex = 32;
            this.checkBox15.UseVisualStyleBackColor = true;
            this.checkBox15.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox15.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox23
            // 
            this.checkBox23.AutoSize = true;
            this.checkBox23.Location = new System.Drawing.Point(237, 74);
            this.checkBox23.Name = "checkBox23";
            this.checkBox23.Size = new System.Drawing.Size(15, 14);
            this.checkBox23.TabIndex = 40;
            this.checkBox23.UseVisualStyleBackColor = true;
            this.checkBox23.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox23.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox22
            // 
            this.checkBox22.AutoSize = true;
            this.checkBox22.Location = new System.Drawing.Point(216, 74);
            this.checkBox22.Name = "checkBox22";
            this.checkBox22.Size = new System.Drawing.Size(15, 14);
            this.checkBox22.TabIndex = 39;
            this.checkBox22.UseVisualStyleBackColor = true;
            this.checkBox22.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox22.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox21
            // 
            this.checkBox21.AutoSize = true;
            this.checkBox21.Location = new System.Drawing.Point(195, 74);
            this.checkBox21.Name = "checkBox21";
            this.checkBox21.Size = new System.Drawing.Size(15, 14);
            this.checkBox21.TabIndex = 38;
            this.checkBox21.UseVisualStyleBackColor = true;
            this.checkBox21.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox21.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox20
            // 
            this.checkBox20.AutoSize = true;
            this.checkBox20.Location = new System.Drawing.Point(174, 74);
            this.checkBox20.Name = "checkBox20";
            this.checkBox20.Size = new System.Drawing.Size(15, 14);
            this.checkBox20.TabIndex = 37;
            this.checkBox20.UseVisualStyleBackColor = true;
            this.checkBox20.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox20.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox19
            // 
            this.checkBox19.AutoSize = true;
            this.checkBox19.Location = new System.Drawing.Point(153, 74);
            this.checkBox19.Name = "checkBox19";
            this.checkBox19.Size = new System.Drawing.Size(15, 14);
            this.checkBox19.TabIndex = 36;
            this.checkBox19.UseVisualStyleBackColor = true;
            this.checkBox19.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox19.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox18
            // 
            this.checkBox18.AutoSize = true;
            this.checkBox18.Location = new System.Drawing.Point(132, 74);
            this.checkBox18.Name = "checkBox18";
            this.checkBox18.Size = new System.Drawing.Size(15, 14);
            this.checkBox18.TabIndex = 35;
            this.checkBox18.UseVisualStyleBackColor = true;
            this.checkBox18.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox18.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox17
            // 
            this.checkBox17.AutoSize = true;
            this.checkBox17.Location = new System.Drawing.Point(111, 74);
            this.checkBox17.Name = "checkBox17";
            this.checkBox17.Size = new System.Drawing.Size(15, 14);
            this.checkBox17.TabIndex = 34;
            this.checkBox17.UseVisualStyleBackColor = true;
            this.checkBox17.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox17.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox16
            // 
            this.checkBox16.AutoSize = true;
            this.checkBox16.Location = new System.Drawing.Point(90, 74);
            this.checkBox16.Name = "checkBox16";
            this.checkBox16.Size = new System.Drawing.Size(15, 14);
            this.checkBox16.TabIndex = 33;
            this.checkBox16.UseVisualStyleBackColor = true;
            this.checkBox16.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox16.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox24
            // 
            this.checkBox24.AutoSize = true;
            this.checkBox24.Location = new System.Drawing.Point(90, 94);
            this.checkBox24.Name = "checkBox24";
            this.checkBox24.Size = new System.Drawing.Size(15, 14);
            this.checkBox24.TabIndex = 41;
            this.checkBox24.UseVisualStyleBackColor = true;
            this.checkBox24.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox24.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox25
            // 
            this.checkBox25.AutoSize = true;
            this.checkBox25.Location = new System.Drawing.Point(111, 94);
            this.checkBox25.Name = "checkBox25";
            this.checkBox25.Size = new System.Drawing.Size(15, 14);
            this.checkBox25.TabIndex = 42;
            this.checkBox25.UseVisualStyleBackColor = true;
            this.checkBox25.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox25.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox26
            // 
            this.checkBox26.AutoSize = true;
            this.checkBox26.Location = new System.Drawing.Point(132, 94);
            this.checkBox26.Name = "checkBox26";
            this.checkBox26.Size = new System.Drawing.Size(15, 14);
            this.checkBox26.TabIndex = 43;
            this.checkBox26.UseVisualStyleBackColor = true;
            this.checkBox26.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox26.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox27
            // 
            this.checkBox27.AutoSize = true;
            this.checkBox27.Location = new System.Drawing.Point(153, 94);
            this.checkBox27.Name = "checkBox27";
            this.checkBox27.Size = new System.Drawing.Size(15, 14);
            this.checkBox27.TabIndex = 44;
            this.checkBox27.UseVisualStyleBackColor = true;
            this.checkBox27.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox27.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox28
            // 
            this.checkBox28.AutoSize = true;
            this.checkBox28.Location = new System.Drawing.Point(174, 94);
            this.checkBox28.Name = "checkBox28";
            this.checkBox28.Size = new System.Drawing.Size(15, 14);
            this.checkBox28.TabIndex = 45;
            this.checkBox28.UseVisualStyleBackColor = true;
            this.checkBox28.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox28.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox29
            // 
            this.checkBox29.AutoSize = true;
            this.checkBox29.Location = new System.Drawing.Point(195, 94);
            this.checkBox29.Name = "checkBox29";
            this.checkBox29.Size = new System.Drawing.Size(15, 14);
            this.checkBox29.TabIndex = 46;
            this.checkBox29.UseVisualStyleBackColor = true;
            this.checkBox29.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox29.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox30
            // 
            this.checkBox30.AutoSize = true;
            this.checkBox30.Location = new System.Drawing.Point(216, 94);
            this.checkBox30.Name = "checkBox30";
            this.checkBox30.Size = new System.Drawing.Size(15, 14);
            this.checkBox30.TabIndex = 47;
            this.checkBox30.UseVisualStyleBackColor = true;
            this.checkBox30.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox30.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // checkBox31
            // 
            this.checkBox31.AutoSize = true;
            this.checkBox31.Location = new System.Drawing.Point(237, 94);
            this.checkBox31.Name = "checkBox31";
            this.checkBox31.Size = new System.Drawing.Size(15, 14);
            this.checkBox31.TabIndex = 48;
            this.checkBox31.UseVisualStyleBackColor = true;
            this.checkBox31.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBox31.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Characteristics";
            this.label5.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxDefaultBias
            // 
            this.textBoxDefaultBias.Location = new System.Drawing.Point(90, 6);
            this.textBoxDefaultBias.Name = "textBoxDefaultBias";
            this.textBoxDefaultBias.Size = new System.Drawing.Size(196, 20);
            this.textBoxDefaultBias.TabIndex = 15;
            this.textBoxDefaultBias.Tag = "Default Laser Bias for EES";
            this.textBoxDefaultBias.TextChanged += new System.EventHandler(this.textBoxDefaultBias_TextChanged);
            this.textBoxDefaultBias.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Default Bias";
            this.label6.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxLabelID
            // 
            this.textBoxLabelID.Location = new System.Drawing.Point(90, 6);
            this.textBoxLabelID.Name = "textBoxLabelID";
            this.textBoxLabelID.Size = new System.Drawing.Size(196, 20);
            this.textBoxLabelID.TabIndex = 1;
            this.textBoxLabelID.TextChanged += new System.EventHandler(this.textBoxLabelID_TextChanged);
            this.textBoxLabelID.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Name ID";
            this.label7.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Name";
            this.label8.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxLabel
            // 
            this.textBoxLabel.Location = new System.Drawing.Point(90, 32);
            this.textBoxLabel.Name = "textBoxLabel";
            this.textBoxLabel.ReadOnly = true;
            this.textBoxLabel.Size = new System.Drawing.Size(196, 20);
            this.textBoxLabel.TabIndex = 3;
            this.textBoxLabel.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(90, 58);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.Size = new System.Drawing.Size(196, 94);
            this.textBoxDescription.TabIndex = 5;
            this.textBoxDescription.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 61);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Description";
            this.label9.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxAoEClass
            // 
            this.textBoxAoEClass.Location = new System.Drawing.Point(90, 6);
            this.textBoxAoEClass.Name = "textBoxAoEClass";
            this.textBoxAoEClass.Size = new System.Drawing.Size(196, 20);
            this.textBoxAoEClass.TabIndex = 50;
            this.textBoxAoEClass.Tag = "MARS AoE Laser Class (0-4)\\n  0=Not AOE\\n  1=PSG-Type\\n  2=PBG-Type\\n  3=CFAA-Typ" +
                "e/Other\\n  4=FAA-Type";
            this.textBoxAoEClass.TextChanged += new System.EventHandler(this.textBoxAoEClass_TextChanged);
            this.textBoxAoEClass.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "AoE Class";
            this.label10.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxMinShipSize
            // 
            this.textBoxMinShipSize.Location = new System.Drawing.Point(90, 32);
            this.textBoxMinShipSize.Name = "textBoxMinShipSize";
            this.textBoxMinShipSize.Size = new System.Drawing.Size(196, 20);
            this.textBoxMinShipSize.TabIndex = 52;
            this.textBoxMinShipSize.Tag = "MARS Minimum Target Ship Size (m)";
            this.textBoxMinShipSize.TextChanged += new System.EventHandler(this.textBoxMinShipSize_TextChanged);
            this.textBoxMinShipSize.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 51;
            this.label11.Text = "Min Ship Size";
            this.label11.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // textBoxMaxShipSize
            // 
            this.textBoxMaxShipSize.Location = new System.Drawing.Point(90, 58);
            this.textBoxMaxShipSize.Name = "textBoxMaxShipSize";
            this.textBoxMaxShipSize.Size = new System.Drawing.Size(196, 20);
            this.textBoxMaxShipSize.TabIndex = 54;
            this.textBoxMaxShipSize.Tag = "MARS Maximum Target Ship Size (m)";
            this.textBoxMaxShipSize.TextChanged += new System.EventHandler(this.textBoxMaxShipSize_TextChanged);
            this.textBoxMaxShipSize.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 13);
            this.label12.TabIndex = 53;
            this.label12.Text = "Max Ship Size";
            this.label12.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabControl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.buttonOk);
            this.splitContainer.Panel2.Controls.Add(this.buttonCancel);
            this.splitContainer.Size = new System.Drawing.Size(304, 459);
            this.splitContainer.SplitterDistance = 421;
            this.splitContainer.TabIndex = 57;
            this.splitContainer.TabStop = false;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageGeneral);
            this.tabControl.Controls.Add(this.tabPageEES);
            this.tabControl.Controls.Add(this.tabPageMARS);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(304, 421);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageGeneral.Controls.Add(this.comboBoxMount);
            this.tabPageGeneral.Controls.Add(this.textBoxLabelID);
            this.tabPageGeneral.Controls.Add(this.textBoxName);
            this.tabPageGeneral.Controls.Add(this.label1);
            this.tabPageGeneral.Controls.Add(this.label2);
            this.tabPageGeneral.Controls.Add(this.label3);
            this.tabPageGeneral.Controls.Add(this.label4);
            this.tabPageGeneral.Controls.Add(this.label9);
            this.tabPageGeneral.Controls.Add(this.textBoxWareSize);
            this.tabPageGeneral.Controls.Add(this.textBoxDescription);
            this.tabPageGeneral.Controls.Add(this.textBoxWareClass);
            this.tabPageGeneral.Controls.Add(this.textBoxLabel);
            this.tabPageGeneral.Controls.Add(this.label7);
            this.tabPageGeneral.Controls.Add(this.label8);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(296, 395);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            // 
            // tabPageEES
            // 
            this.tabPageEES.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageEES.Controls.Add(this.textBoxDefaultBias);
            this.tabPageEES.Controls.Add(this.checkBox00);
            this.tabPageEES.Controls.Add(this.checkBox01);
            this.tabPageEES.Controls.Add(this.checkBox02);
            this.tabPageEES.Controls.Add(this.checkBox03);
            this.tabPageEES.Controls.Add(this.checkBox04);
            this.tabPageEES.Controls.Add(this.checkBox05);
            this.tabPageEES.Controls.Add(this.checkBox06);
            this.tabPageEES.Controls.Add(this.label6);
            this.tabPageEES.Controls.Add(this.checkBox07);
            this.tabPageEES.Controls.Add(this.label5);
            this.tabPageEES.Controls.Add(this.checkBox08);
            this.tabPageEES.Controls.Add(this.checkBox31);
            this.tabPageEES.Controls.Add(this.checkBox09);
            this.tabPageEES.Controls.Add(this.checkBox30);
            this.tabPageEES.Controls.Add(this.checkBox10);
            this.tabPageEES.Controls.Add(this.checkBox29);
            this.tabPageEES.Controls.Add(this.checkBox11);
            this.tabPageEES.Controls.Add(this.checkBox28);
            this.tabPageEES.Controls.Add(this.checkBox12);
            this.tabPageEES.Controls.Add(this.checkBox27);
            this.tabPageEES.Controls.Add(this.checkBox13);
            this.tabPageEES.Controls.Add(this.checkBox26);
            this.tabPageEES.Controls.Add(this.checkBox14);
            this.tabPageEES.Controls.Add(this.checkBox25);
            this.tabPageEES.Controls.Add(this.checkBox15);
            this.tabPageEES.Controls.Add(this.checkBox24);
            this.tabPageEES.Controls.Add(this.checkBox16);
            this.tabPageEES.Controls.Add(this.checkBox23);
            this.tabPageEES.Controls.Add(this.checkBox17);
            this.tabPageEES.Controls.Add(this.checkBox22);
            this.tabPageEES.Controls.Add(this.checkBox18);
            this.tabPageEES.Controls.Add(this.checkBox21);
            this.tabPageEES.Controls.Add(this.checkBox19);
            this.tabPageEES.Controls.Add(this.checkBox20);
            this.tabPageEES.Location = new System.Drawing.Point(4, 22);
            this.tabPageEES.Name = "tabPageEES";
            this.tabPageEES.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEES.Size = new System.Drawing.Size(296, 395);
            this.tabPageEES.TabIndex = 1;
            this.tabPageEES.Text = "EES";
            // 
            // tabPageMARS
            // 
            this.tabPageMARS.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageMARS.Controls.Add(this.textBoxAoEClass);
            this.tabPageMARS.Controls.Add(this.textBoxMaxShipSize);
            this.tabPageMARS.Controls.Add(this.label10);
            this.tabPageMARS.Controls.Add(this.label12);
            this.tabPageMARS.Controls.Add(this.label11);
            this.tabPageMARS.Controls.Add(this.textBoxMinShipSize);
            this.tabPageMARS.Location = new System.Drawing.Point(4, 22);
            this.tabPageMARS.Name = "tabPageMARS";
            this.tabPageMARS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMARS.Size = new System.Drawing.Size(296, 395);
            this.tabPageMARS.TabIndex = 2;
            this.tabPageMARS.Text = "MARS";
            // 
            // comboBoxMount
            // 
            this.comboBoxMount.FormattingEnabled = true;
            this.comboBoxMount.Location = new System.Drawing.Point(90, 184);
            this.comboBoxMount.Name = "comboBoxMount";
            this.comboBoxMount.Size = new System.Drawing.Size(196, 21);
            this.comboBoxMount.TabIndex = 14;
            this.comboBoxMount.TextUpdate += new System.EventHandler(this.comboBoxMount_TextChanged);
            this.comboBoxMount.TextChanged += new System.EventHandler(this.comboBoxMount_TextChanged);
            // 
            // FormSpecLaser
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(304, 459);
            this.Controls.Add(this.splitContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSpecLaser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Laser Specification";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSpecLaser_HelpRequested);
            this.Validating += new System.ComponentModel.CancelEventHandler(this.FormLaserSpec_Validating);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageEES.ResumeLayout(false);
            this.tabPageEES.PerformLayout();
            this.tabPageMARS.ResumeLayout(false);
            this.tabPageMARS.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxWareSize;
        private System.Windows.Forms.TextBox textBoxWareClass;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.CheckBox checkBox00;
        private System.Windows.Forms.CheckBox checkBox01;
        private System.Windows.Forms.CheckBox checkBox02;
        private System.Windows.Forms.CheckBox checkBox03;
        private System.Windows.Forms.CheckBox checkBox04;
        private System.Windows.Forms.CheckBox checkBox05;
        private System.Windows.Forms.CheckBox checkBox06;
        private System.Windows.Forms.CheckBox checkBox07;
        private System.Windows.Forms.CheckBox checkBox08;
        private System.Windows.Forms.CheckBox checkBox09;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.CheckBox checkBox11;
        private System.Windows.Forms.CheckBox checkBox12;
        private System.Windows.Forms.CheckBox checkBox13;
        private System.Windows.Forms.CheckBox checkBox14;
        private System.Windows.Forms.CheckBox checkBox15;
        private System.Windows.Forms.CheckBox checkBox16;
        private System.Windows.Forms.CheckBox checkBox17;
        private System.Windows.Forms.CheckBox checkBox18;
        private System.Windows.Forms.CheckBox checkBox19;
        private System.Windows.Forms.CheckBox checkBox20;
        private System.Windows.Forms.CheckBox checkBox21;
        private System.Windows.Forms.CheckBox checkBox22;
        private System.Windows.Forms.CheckBox checkBox23;
        private System.Windows.Forms.CheckBox checkBox24;
        private System.Windows.Forms.CheckBox checkBox25;
        private System.Windows.Forms.CheckBox checkBox26;
        private System.Windows.Forms.CheckBox checkBox27;
        private System.Windows.Forms.CheckBox checkBox28;
        private System.Windows.Forms.CheckBox checkBox29;
        private System.Windows.Forms.CheckBox checkBox30;
        private System.Windows.Forms.CheckBox checkBox31;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDefaultBias;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxLabelID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxLabel;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxAoEClass;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxMinShipSize;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxMaxShipSize;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageEES;
        private System.Windows.Forms.TabPage tabPageMARS;
        private System.Windows.Forms.ComboBox comboBoxMount;

    }
}