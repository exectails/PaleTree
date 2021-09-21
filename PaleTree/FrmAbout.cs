using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PaleTree
{
	public partial class FrmAbout : Form
	{
		public FrmAbout()
		{
			InitializeComponent();
		}

		private void FrmAbout_Load(object sender, EventArgs e)
		{
		}

		private void BtnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void Link_Click(object sender, EventArgs e)
		{
			var url = (string)((Control)sender).Tag;
			Process.Start(url);
		}
	}
}
