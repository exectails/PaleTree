namespace TreePale.Plugins.VariablePreview
{
	partial class FrmVariablePreview
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Unsinged Short");
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVariablePreview));
			this.LstValues = new System.Windows.Forms.ListView();
			this.ColType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ColValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// LstValues
			// 
			this.LstValues.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.LstValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColType,
            this.ColValue});
			this.LstValues.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LstValues.FullRowSelect = true;
			this.LstValues.GridLines = true;
			this.LstValues.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.LstValues.Location = new System.Drawing.Point(0, 0);
			this.LstValues.Name = "LstValues";
			this.LstValues.Size = new System.Drawing.Size(262, 253);
			this.LstValues.TabIndex = 0;
			this.LstValues.UseCompatibleStateImageBehavior = false;
			this.LstValues.View = System.Windows.Forms.View.Details;
			// 
			// ColType
			// 
			this.ColType.Text = "Type";
			this.ColType.Width = 100;
			// 
			// ColValue
			// 
			this.ColValue.Text = "Value";
			this.ColValue.Width = 150;
			// 
			// FrmVariablePreview
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 253);
			this.Controls.Add(this.LstValues);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "FrmVariablePreview";
			this.Text = "Variable Preview";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmVariablePreview_FormClosing);
			this.Load += new System.EventHandler(this.FrmVariablePreview_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView LstValues;
		private System.Windows.Forms.ColumnHeader ColType;
		private System.Windows.Forms.ColumnHeader ColValue;
	}
}