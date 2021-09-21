namespace PaleTree
{
	partial class FrmAbout
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
			this.PnlHeader = new System.Windows.Forms.Panel();
			this.ImgIcon = new System.Windows.Forms.PictureBox();
			this.LblVersion = new System.Windows.Forms.Label();
			this.LblSubTitle = new System.Windows.Forms.Label();
			this.LblTitle = new System.Windows.Forms.Label();
			this.GrpLicense = new System.Windows.Forms.GroupBox();
			this.TxtLicense = new System.Windows.Forms.TextBox();
			this.ImgPatreon = new System.Windows.Forms.PictureBox();
			this.ImgGitHub = new System.Windows.Forms.PictureBox();
			this.BtnClose = new System.Windows.Forms.Button();
			this.PnlHeader.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ImgIcon)).BeginInit();
			this.GrpLicense.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ImgPatreon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ImgGitHub)).BeginInit();
			this.SuspendLayout();
			// 
			// PnlHeader
			// 
			this.PnlHeader.BackColor = System.Drawing.Color.White;
			this.PnlHeader.Controls.Add(this.ImgIcon);
			this.PnlHeader.Controls.Add(this.LblVersion);
			this.PnlHeader.Controls.Add(this.LblSubTitle);
			this.PnlHeader.Controls.Add(this.LblTitle);
			this.PnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
			this.PnlHeader.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PnlHeader.Location = new System.Drawing.Point(0, 0);
			this.PnlHeader.Name = "PnlHeader";
			this.PnlHeader.Size = new System.Drawing.Size(394, 81);
			this.PnlHeader.TabIndex = 24;
			// 
			// ImgIcon
			// 
			this.ImgIcon.Image = ((System.Drawing.Image)(resources.GetObject("ImgIcon.Image")));
			this.ImgIcon.Location = new System.Drawing.Point(25, 24);
			this.ImgIcon.Name = "ImgIcon";
			this.ImgIcon.Size = new System.Drawing.Size(32, 32);
			this.ImgIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.ImgIcon.TabIndex = 4;
			this.ImgIcon.TabStop = false;
			// 
			// LblVersion
			// 
			this.LblVersion.AutoSize = true;
			this.LblVersion.ForeColor = System.Drawing.Color.Gray;
			this.LblVersion.Location = new System.Drawing.Point(148, 28);
			this.LblVersion.Name = "LblVersion";
			this.LblVersion.Size = new System.Drawing.Size(37, 13);
			this.LblVersion.TabIndex = 3;
			this.LblVersion.Text = "v1.2.0";
			// 
			// LblSubTitle
			// 
			this.LblSubTitle.AutoSize = true;
			this.LblSubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.LblSubTitle.Location = new System.Drawing.Point(65, 43);
			this.LblSubTitle.Name = "LblSubTitle";
			this.LblSubTitle.Size = new System.Drawing.Size(100, 13);
			this.LblSubTitle.TabIndex = 2;
			this.LblSubTitle.Text = "ToS Packet Logger";
			// 
			// LblTitle
			// 
			this.LblTitle.AutoSize = true;
			this.LblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.LblTitle.Location = new System.Drawing.Point(63, 20);
			this.LblTitle.Name = "LblTitle";
			this.LblTitle.Size = new System.Drawing.Size(87, 24);
			this.LblTitle.TabIndex = 2;
			this.LblTitle.Text = "PaleTree";
			// 
			// GrpLicense
			// 
			this.GrpLicense.Controls.Add(this.TxtLicense);
			this.GrpLicense.Location = new System.Drawing.Point(12, 87);
			this.GrpLicense.Name = "GrpLicense";
			this.GrpLicense.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
			this.GrpLicense.Size = new System.Drawing.Size(370, 192);
			this.GrpLicense.TabIndex = 28;
			this.GrpLicense.TabStop = false;
			this.GrpLicense.Text = "License";
			// 
			// TxtLicense
			// 
			this.TxtLicense.BackColor = System.Drawing.SystemColors.Control;
			this.TxtLicense.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TxtLicense.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TxtLicense.Location = new System.Drawing.Point(12, 21);
			this.TxtLicense.Multiline = true;
			this.TxtLicense.Name = "TxtLicense";
			this.TxtLicense.ReadOnly = true;
			this.TxtLicense.Size = new System.Drawing.Size(346, 163);
			this.TxtLicense.TabIndex = 0;
			this.TxtLicense.Text = resources.GetString("TxtLicense.Text");
			// 
			// ImgPatreon
			// 
			this.ImgPatreon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ImgPatreon.Image = ((System.Drawing.Image)(resources.GetObject("ImgPatreon.Image")));
			this.ImgPatreon.Location = new System.Drawing.Point(12, 285);
			this.ImgPatreon.Name = "ImgPatreon";
			this.ImgPatreon.Size = new System.Drawing.Size(189, 32);
			this.ImgPatreon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.ImgPatreon.TabIndex = 27;
			this.ImgPatreon.TabStop = false;
			this.ImgPatreon.Tag = "https://www.patreon.com/exectails";
			this.ImgPatreon.Click += new System.EventHandler(this.Link_Click);
			// 
			// ImgGitHub
			// 
			this.ImgGitHub.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ImgGitHub.Image = ((System.Drawing.Image)(resources.GetObject("ImgGitHub.Image")));
			this.ImgGitHub.Location = new System.Drawing.Point(207, 285);
			this.ImgGitHub.Name = "ImgGitHub";
			this.ImgGitHub.Size = new System.Drawing.Size(32, 32);
			this.ImgGitHub.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.ImgGitHub.TabIndex = 26;
			this.ImgGitHub.TabStop = false;
			this.ImgGitHub.Tag = "https://github.com/exectails";
			this.ImgGitHub.Click += new System.EventHandler(this.Link_Click);
			// 
			// BtnClose
			// 
			this.BtnClose.Location = new System.Drawing.Point(307, 294);
			this.BtnClose.Name = "BtnClose";
			this.BtnClose.Size = new System.Drawing.Size(75, 23);
			this.BtnClose.TabIndex = 25;
			this.BtnClose.Text = "OK";
			this.BtnClose.UseVisualStyleBackColor = true;
			this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
			// 
			// FrmAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(394, 329);
			this.Controls.Add(this.PnlHeader);
			this.Controls.Add(this.GrpLicense);
			this.Controls.Add(this.ImgPatreon);
			this.Controls.Add(this.ImgGitHub);
			this.Controls.Add(this.BtnClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.Load += new System.EventHandler(this.FrmAbout_Load);
			this.PnlHeader.ResumeLayout(false);
			this.PnlHeader.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ImgIcon)).EndInit();
			this.GrpLicense.ResumeLayout(false);
			this.GrpLicense.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ImgPatreon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ImgGitHub)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel PnlHeader;
		private System.Windows.Forms.PictureBox ImgIcon;
		private System.Windows.Forms.Label LblVersion;
		private System.Windows.Forms.Label LblSubTitle;
		private System.Windows.Forms.Label LblTitle;
		private System.Windows.Forms.GroupBox GrpLicense;
		private System.Windows.Forms.TextBox TxtLicense;
		private System.Windows.Forms.PictureBox ImgPatreon;
		private System.Windows.Forms.PictureBox ImgGitHub;
		private System.Windows.Forms.Button BtnClose;
	}
}