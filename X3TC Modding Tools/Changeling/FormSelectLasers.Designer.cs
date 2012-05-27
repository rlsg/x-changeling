namespace Changeling
{
    partial class FormSelectLasers
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
            this.splitContainerLists = new System.Windows.Forms.SplitContainer();
            this.listViewAvailable = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCharacteristics = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewSelected = new System.Windows.Forms.ListView();
            this.columnSelected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLists)).BeginInit();
            this.splitContainerLists.Panel1.SuspendLayout();
            this.splitContainerLists.Panel2.SuspendLayout();
            this.splitContainerLists.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerLists
            // 
            this.splitContainerLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLists.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLists.Name = "splitContainerLists";
            // 
            // splitContainerLists.Panel1
            // 
            this.splitContainerLists.Panel1.Controls.Add(this.listViewAvailable);
            // 
            // splitContainerLists.Panel2
            // 
            this.splitContainerLists.Panel2.Controls.Add(this.listViewSelected);
            this.splitContainerLists.Size = new System.Drawing.Size(521, 419);
            this.splitContainerLists.SplitterDistance = 363;
            this.splitContainerLists.TabIndex = 2;
            this.splitContainerLists.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainerLists_SplitterMoved);
            // 
            // listViewAvailable
            // 
            this.listViewAvailable.AllowDrop = true;
            this.listViewAvailable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnMount,
            this.columnCharacteristics});
            this.listViewAvailable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewAvailable.FullRowSelect = true;
            this.listViewAvailable.Location = new System.Drawing.Point(0, 0);
            this.listViewAvailable.Name = "listViewAvailable";
            this.listViewAvailable.ShowItemToolTips = true;
            this.listViewAvailable.Size = new System.Drawing.Size(363, 419);
            this.listViewAvailable.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewAvailable.TabIndex = 0;
            this.listViewAvailable.UseCompatibleStateImageBehavior = false;
            this.listViewAvailable.View = System.Windows.Forms.View.Details;
            this.listViewAvailable.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewAvailable_ItemDrag);
            this.listViewAvailable.SelectedIndexChanged += new System.EventHandler(this.listViewAvailable_SelectedIndexChanged);
            this.listViewAvailable.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewAvailable_DragDrop);
            this.listViewAvailable.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewAvailable_DragOver);
            this.listViewAvailable.DragOver += new System.Windows.Forms.DragEventHandler(this.listViewAvailable_DragOver);
            this.listViewAvailable.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSelectLasers_HelpRequested);
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 120;
            // 
            // columnMount
            // 
            this.columnMount.Text = "Mount Type";
            this.columnMount.Width = 120;
            // 
            // columnCharacteristics
            // 
            this.columnCharacteristics.Text = "Characteristics";
            this.columnCharacteristics.Width = 90;
            // 
            // listViewSelected
            // 
            this.listViewSelected.AllowDrop = true;
            this.listViewSelected.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnSelected});
            this.listViewSelected.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewSelected.Location = new System.Drawing.Point(0, 0);
            this.listViewSelected.Name = "listViewSelected";
            this.listViewSelected.ShowItemToolTips = true;
            this.listViewSelected.Size = new System.Drawing.Size(154, 419);
            this.listViewSelected.TabIndex = 1;
            this.listViewSelected.UseCompatibleStateImageBehavior = false;
            this.listViewSelected.View = System.Windows.Forms.View.Details;
            this.listViewSelected.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewSelected_ItemDrag);
            this.listViewSelected.SelectedIndexChanged += new System.EventHandler(this.listViewSelected_SelectedIndexChanged);
            this.listViewSelected.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewSelected_DragDrop);
            this.listViewSelected.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewSelected_DragOver);
            this.listViewSelected.DragOver += new System.Windows.Forms.DragEventHandler(this.listViewSelected_DragOver);
            this.listViewSelected.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSelectLasers_HelpRequested);
            // 
            // columnSelected
            // 
            this.columnSelected.Text = "Name";
            this.columnSelected.Width = 120;
            // 
            // FormSelectLasers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 419);
            this.Controls.Add(this.splitContainerLists);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectLasers";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Select Lasers";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.FormSelectLasers_HelpRequested);
            this.splitContainerLists.Panel1.ResumeLayout(false);
            this.splitContainerLists.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLists)).EndInit();
            this.splitContainerLists.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerLists;
        private System.Windows.Forms.ListView listViewAvailable;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnMount;
        private System.Windows.Forms.ColumnHeader columnCharacteristics;
        private System.Windows.Forms.ListView listViewSelected;
        private System.Windows.Forms.ColumnHeader columnSelected;


    }
}