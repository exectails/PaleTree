using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaleTree.Shared;
using PaleTree.Plugins.Open010.Properties;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.IO.Compression;

namespace PaleTree.Plugins.Open010
{
	public class Main : Plugin
	{
		private Form form;

		public override string Name
		{
			get { return "Open010"; }
		}

		public Main(IPluginManager pluginManager)
			: base(pluginManager)
		{

		}

		public override void Initialize()
		{
			manager.AddToMenu(Name + " options", OnClickMenu);
			manager.AddToToolbar(Resources.application_go, "Open packet in 010", OnClickButton);
		}

		private void OnClickMenu(object sender, EventArgs e)
		{
			if (form == null || form.IsDisposed)
				manager.OpenCentered(form = new FrmOptions());
			else
				form.Focus();
		}

		private void OnClickButton(object sender, EventArgs e)
		{
			var palePacket = manager.GetSelectedPacket();
			if (palePacket == null)
			{
				MessageBox.Show("No packet selected.", Name);
				return;
			}

			if (string.IsNullOrWhiteSpace(Settings.Default.Folder010) || string.IsNullOrWhiteSpace(Settings.Default.FolderTpl))
			{
				MessageBox.Show("Please configure the plugin at 'Plugins > Open010 options'.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var path010 = Path.Combine(Settings.Default.Folder010, "010Editor.exe");
			if (!File.Exists(path010))
			{
				MessageBox.Show("'010Editor.exe' not found in 010 folder.", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!Directory.Exists(Settings.Default.FolderTpl))
			{
				if (MessageBox.Show("Template folder not found, do you want to open 010 regardless?", Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					return;
			}

			// Get buffer
			var buffer = palePacket.Packet.GetBuffer();
			for (int i = 0; i < buffer.Length - 3; ++i)
			{
				var val = BitConverter.ToUInt16(buffer, i);
				if (val == 0xFA8D)
				{
					var len = BitConverter.ToUInt16(buffer, i + 2);
					if (i + 4 + len <= buffer.Length)
					{
						var uncompress = MessageBox.Show("Seemingly compressed data found, uncompress for 010?", Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
						if (uncompress == DialogResult.Yes)
						{
							using (var msOut = new MemoryStream())
							{
								msOut.Write(buffer, 0, i);
								msOut.Write(new byte[2], 0, 2);

								using (var msIn = new MemoryStream(buffer, i + 4, len))
								using (var ds = new DeflateStream(msIn, CompressionMode.Decompress))
								{
									ds.CopyTo(msOut);

									//Console.WriteLine(BitConverter.ToString(msOut.ToArray()).Replace("-", " "));
								}

								msOut.Write(buffer, i + 4 + len, buffer.Length - (i + 4 + len));
								buffer = msOut.ToArray();
							}
						}
					}
				}
			}

			// Create temp file for the buffer and open it in 010
			var tmpPath = Path.GetTempFileName();
			File.WriteAllBytes(tmpPath, buffer);

			// Open file with template
			var pathtpl = Path.Combine(Settings.Default.FolderTpl, palePacket.OpName + ".bt");
			var select = palePacket.OpSize;

			// Originally I had 010 open the file first, and then made it
			// run the template on it in another process start, but I don't
			// remember why I did it that way, and it now seems to be
			// working fine to do it all in one, and it's faster, so I'll
			// go with that.

			if (File.Exists(pathtpl))
			{
				Process.Start(path010, string.Format("\"{0}\" \"-template:{1}\" -select::{2}", tmpPath, pathtpl, select));
			}
			else
			{
				var result = MessageBox.Show("No template found for '" + palePacket.OpName + "', do you still want to open the packet in 010?", Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					Process.Start(path010, string.Format("\"{0}\" -select::{2}", tmpPath, pathtpl, select));
				}
			}
		}
	}
}
