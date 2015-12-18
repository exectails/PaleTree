using PaleTree.Plugins.Open010.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleTree.Plugins.Open010
{
	public partial class FrmOptions : Form
	{
		public FrmOptions()
		{
			InitializeComponent();
		}

		private void BtnSelectFolder010_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				TxtFolder010.Text = folderBrowserDialog.SelectedPath;
		}

		private void BtnSelectFolderTpl_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				TxtFolderTpl.Text = folderBrowserDialog.SelectedPath;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			Settings.Default.Folder010 = TxtFolder010.Text;
			Settings.Default.FolderTpl = TxtFolderTpl.Text;
			Settings.Default.Save();

			Close();
		}

		private void FrmOptions_Load(object sender, EventArgs e)
		{
			TxtFolder010.Text = Settings.Default.Folder010;
			TxtFolderTpl.Text = Settings.Default.FolderTpl;
		}
	}
}
