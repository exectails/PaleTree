using PaleTree.Plugins.EntityLogger.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace PaleTree.Plugins.EntityLogger
{
	public partial class FrmOptions : Form
	{
		public FrmOptions()
		{
			InitializeComponent();
		}

		private void BtnSelectFolderMelia_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
				TxtFolderMelia.Text = folderBrowserDialog.SelectedPath;
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists(Path.Combine(TxtFolderMelia.Text, "system")) && !string.IsNullOrWhiteSpace(TxtFolderMelia.Text))
			{
				MessageBox.Show("'system' not found in Melia folder.", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			Settings.Default.FolderMelia = TxtFolderMelia.Text;
			Settings.Default.Save();

			if(!string.IsNullOrWhiteSpace(TxtFolderMelia.Text))
			{
				DBUtils.LoadData(Settings.Default.FolderMelia, Main.Data, DBUtils.DataToLoad.All, true);
			}

			Close();
		}

		private void FrmOptions_Load(object sender, EventArgs e)
		{
			TxtFolderMelia.Text = Settings.Default.FolderMelia;
		}
	}
}
