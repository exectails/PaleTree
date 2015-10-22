using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreePale.Plugins.VariablePreview.Properties;

namespace TreePale.Plugins.VariablePreview
{
	public partial class FrmVariablePreview : Form
	{
		public FrmVariablePreview()
		{
			InitializeComponent();
		}

		private void FrmVariablePreview_Load(object sender, EventArgs e)
		{
			if (Settings.Default.X != -1 && Settings.Default.Y != -1)
			{
				StartPosition = FormStartPosition.Manual;
				Left = Settings.Default.X;
				Top = Settings.Default.Y;
			}

			LstValues.Columns[1].Width = ClientSize.Width - LstValues.Columns[0].Width;

			LstValues.BeginUpdate();

			LstValues.Items.Clear();

			ListViewItem lvi;

			lvi = new ListViewItem("Signed Byte"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Unsigned Byte"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Signed Short"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Unsigned Short"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Signed Int"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Unsigned Int"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Signed Long"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Unsigned Long"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Float"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("Double"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);
			lvi = new ListViewItem("String"); lvi.SubItems.Add(""); LstValues.Items.Add(lvi);

			LstValues.EndUpdate();
		}

		private void FrmVariablePreview_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				Settings.Default.X = Left;
				Settings.Default.Y = Top;
				Settings.Default.Save();
			}
		}

		public void UpdateValues(byte[] buffer, int start)
		{
			if (buffer == null)
			{
				Reset();
				return;
			}

			LstValues.BeginUpdate();

			LstValues.Items[0].SubItems[1].Text = (buffer.Length - start < 1 ? "" : ((sbyte)buffer[start]).ToString());
			LstValues.Items[1].SubItems[1].Text = (buffer.Length - start < 1 ? "" : buffer[start].ToString());
			LstValues.Items[2].SubItems[1].Text = (buffer.Length - start < 2 ? "" : BitConverter.ToInt16(buffer, start).ToString());
			LstValues.Items[3].SubItems[1].Text = (buffer.Length - start < 2 ? "" : BitConverter.ToUInt16(buffer, start).ToString());
			LstValues.Items[4].SubItems[1].Text = (buffer.Length - start < 4 ? "" : BitConverter.ToInt32(buffer, start).ToString());
			LstValues.Items[5].SubItems[1].Text = (buffer.Length - start < 4 ? "" : BitConverter.ToUInt32(buffer, start).ToString());
			LstValues.Items[6].SubItems[1].Text = (buffer.Length - start < 8 ? "" : BitConverter.ToInt64(buffer, start).ToString());
			LstValues.Items[7].SubItems[1].Text = (buffer.Length - start < 8 ? "" : BitConverter.ToUInt64(buffer, start).ToString());
			LstValues.Items[8].SubItems[1].Text = (buffer.Length - start < 4 ? "" : BitConverter.ToSingle(buffer, start).ToString());
			LstValues.Items[9].SubItems[1].Text = (buffer.Length - start < 8 ? "" : BitConverter.ToDouble(buffer, start).ToString());

			var len = -1;
			for (int i = start; i < buffer.Length; ++i)
			{
				if (buffer[i] == 0)
				{
					len = i - start;
					break;
				}
			}

			if (len == -1)
				LstValues.Items[10].SubItems[1].Text = "";
			else
				LstValues.Items[10].SubItems[1].Text = Encoding.UTF8.GetString(buffer, start, len);

			LstValues.EndUpdate();
		}

		private void Reset()
		{
			LstValues.BeginUpdate();

			foreach (ListViewItem item in LstValues.Items)
				item.SubItems[1].Text = "";

			LstValues.EndUpdate();
		}
	}
}
