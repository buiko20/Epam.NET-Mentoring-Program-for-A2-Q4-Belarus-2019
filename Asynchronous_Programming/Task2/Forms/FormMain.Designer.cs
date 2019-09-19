namespace Task2.Forms
{
    partial class FormMain
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
            if (disposing)
            {
                _viewer?.Dispose();
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbUrl = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lvUrls = new System.Windows.Forms.ListView();
            this.chUrls = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemove = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 297);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Url:";
            // 
            // tbUrl
            // 
            this.tbUrl.Location = new System.Drawing.Point(38, 294);
            this.tbUrl.Name = "tbUrl";
            this.tbUrl.Size = new System.Drawing.Size(156, 20);
            this.tbUrl.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(38, 320);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(156, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // lvUrls
            // 
            this.lvUrls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chUrls,
            this.chStatus});
            this.lvUrls.Dock = System.Windows.Forms.DockStyle.Top;
            this.lvUrls.FullRowSelect = true;
            this.lvUrls.GridLines = true;
            this.lvUrls.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvUrls.HideSelection = false;
            this.lvUrls.Location = new System.Drawing.Point(0, 0);
            this.lvUrls.MultiSelect = false;
            this.lvUrls.Name = "lvUrls";
            this.lvUrls.Size = new System.Drawing.Size(605, 288);
            this.lvUrls.TabIndex = 3;
            this.lvUrls.UseCompatibleStateImageBehavior = false;
            this.lvUrls.View = System.Windows.Forms.View.Details;
            this.lvUrls.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.LvUrls_ItemSelectionChanged);
            // 
            // chUrls
            // 
            this.chUrls.Text = "Urls";
            this.chUrls.Width = 500;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Status";
            this.chStatus.Width = 100;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(200, 292);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(103, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 359);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.lvUrls);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbUrl);
            this.Controls.Add(this.label1);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Task 2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUrl;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListView lvUrls;
        private System.Windows.Forms.ColumnHeader chUrls;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ColumnHeader chStatus;
    }
}

