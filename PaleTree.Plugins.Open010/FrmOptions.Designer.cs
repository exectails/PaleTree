namespace PaleTree.Plugins.Open010
{
	partial class FrmOptions
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOptions));
			this.TxtFolder010 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.TxtFolderTpl = new System.Windows.Forms.TextBox();
			this.BtnSave = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.BtnSelectFolder010 = new System.Windows.Forms.Button();
			this.BtnSelectFolderTpl = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TxtFolder010
			// 
			this.TxtFolder010.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtFolder010.Location = new System.Drawing.Point(12, 25);
			this.TxtFolder010.Name = "TxtFolder010";
			this.TxtFolder010.Size = new System.Drawing.Size(306, 20);
			this.TxtFolder010.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Path to 010 Editor folder";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.Gray;
			this.label2.Location = new System.Drawing.Point(167, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(184, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Example: C:\\Program Files\\010 Editor";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.Gray;
			this.label3.Location = new System.Drawing.Point(221, 57);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(130, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Example: C:\\Melia\\doc\\bt";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 57);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(113, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Path to template folder";
			// 
			// TxtFolderTpl
			// 
			this.TxtFolderTpl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtFolderTpl.Location = new System.Drawing.Point(12, 73);
			this.TxtFolderTpl.Name = "TxtFolderTpl";
			this.TxtFolderTpl.Size = new System.Drawing.Size(306, 20);
			this.TxtFolderTpl.TabIndex = 6;
			// 
			// BtnSave
			// 
			this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnSave.Location = new System.Drawing.Point(276, 108);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(75, 23);
			this.BtnSave.TabIndex = 7;
			this.BtnSave.Text = "Save";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// BtnSelectFolder010
			// 
			this.BtnSelectFolder010.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnSelectFolder010.Location = new System.Drawing.Point(324, 25);
			this.BtnSelectFolder010.Name = "BtnSelectFolder010";
			this.BtnSelectFolder010.Size = new System.Drawing.Size(27, 19);
			this.BtnSelectFolder010.TabIndex = 8;
			this.BtnSelectFolder010.Text = "...";
			this.BtnSelectFolder010.UseVisualStyleBackColor = true;
			this.BtnSelectFolder010.Click += new System.EventHandler(this.BtnSelectFolder010_Click);
			// 
			// BtnSelectFolderTpl
			// 
			this.BtnSelectFolderTpl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnSelectFolderTpl.Location = new System.Drawing.Point(324, 73);
			this.BtnSelectFolderTpl.Name = "BtnSelectFolderTpl";
			this.BtnSelectFolderTpl.Size = new System.Drawing.Size(27, 19);
			this.BtnSelectFolderTpl.TabIndex = 9;
			this.BtnSelectFolderTpl.Text = "...";
			this.BtnSelectFolderTpl.UseVisualStyleBackColor = true;
			this.BtnSelectFolderTpl.Click += new System.EventHandler(this.BtnSelectFolderTpl_Click);
			// 
			// FrmOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(363, 143);
			this.Controls.Add(this.BtnSelectFolderTpl);
			this.Controls.Add(this.BtnSelectFolder010);
			this.Controls.Add(this.BtnSave);
			this.Controls.Add(this.TxtFolderTpl);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.TxtFolder010);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmOptions";
			this.Text = "010 Options";
			this.Load += new System.EventHandler(this.FrmOptions_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TxtFolder010;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TxtFolderTpl;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Button BtnSelectFolder010;
		private System.Windows.Forms.Button BtnSelectFolderTpl;
	}
}