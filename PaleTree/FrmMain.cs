using PaleTree.Plugins;
using PaleTree.Properties;
using PaleTree.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Be.Windows.Forms;
using System.Reflection;

namespace PaleTree
{
	public partial class FrmMain : Form
	{
		private PluginManager pluginManager;

		private IntPtr providerHWnd;

		private Queue<PalePacket> packetQueue;
		private System.Timers.Timer queueTimer;

		private HashSet<string> recvFilter, sendFilter;

		private StringWriter log;

		public FrmMain()
		{
			InitializeComponent();

			Trace.Listeners.Add(new TextWriterTraceListener(log = new StringWriter()));

			pluginManager = new PluginManager(this);
			packetQueue = new Queue<PalePacket>();

			queueTimer = new System.Timers.Timer();
			queueTimer.Interval = 250;
			queueTimer.Elapsed += OnQueueTimer;

			recvFilter = new HashSet<string>();
			sendFilter = new HashSet<string>();

			LblCurrentFileName.Text = "";
			LblPacketProvider.Text = "";
			
			CheckForFile();
		}

		private void CheckForFile()
		{
			if (File.Exists("Zemyna_Ops.txt"))
			{
				Shared.Op.FillDictionary();
			}
			else
			{
				var messageBoxResault = MessageBox.Show("Zemyna_Ops.txt is missing, do you want to continue anyway?", "Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (messageBoxResault == DialogResult.No)
				{
					this.Close();
				}
			}
		}

		private void FrmMain_Load(object sender, EventArgs e)
		{
			if (Settings.Default.X != -1 && Settings.Default.Y != -1)
				StartPosition = FormStartPosition.Manual;

			if (Settings.Default.X != -1) Left = Settings.Default.X;
			if (Settings.Default.Y != -1) Top = Settings.Default.Y;

			Width = Settings.Default.Width;
			Height = Settings.Default.Height;

			if (Settings.Default.Maximized) WindowState = FormWindowState.Maximized;

			UpdateFilters();

			LstPackets.ContextMenu = CtxPacketList;

			pluginManager.Load();
		}

		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				Settings.Default.X = Left;
				Settings.Default.Y = Top;
				Settings.Default.Width = Width;
				Settings.Default.Height = Height;
				Settings.Default.Maximized = (WindowState == FormWindowState.Maximized);
			}
			Settings.Default.Save();

			Disconnect();

			pluginManager.OnEnd();
		}

		/// <summary>
		/// Called when selecting a packet in the list,
		/// shows the packet in the textbox.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LstPackets_SelectedIndexChanged(object sender, EventArgs e)
		{
			PalePacket palePacket = null;

			if (LstPackets.SelectedItems.Count == 0)
			{
				TxtPacketInfo.Text = "";
				HexBox.ByteProvider = null;
			}
			else
			{
				palePacket = (PalePacket)LstPackets.SelectedItems[0].Tag;

				TxtPacketInfo.Text = palePacket.GetPacketInfo();
				HexBox.ByteProvider = new DynamicByteProvider(palePacket.Packet.GetBuffer());
			}

			pluginManager.OnSelected(palePacket);
		}

		/// <summary>
		/// Menu item ?>About, opens About dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnMenuAbout_Click(object sender, EventArgs e)
		{
			new FrmAbout().ShowDialog();
		}

		/// <summary>
		/// Menu item File>Exit, closes form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnMenuExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Open file button, opens a log file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOpen_Click(object sender, EventArgs e)
		{
			if (OpenLogDialog.ShowDialog() == DialogResult.Cancel)
				return;

			if (ClearListQuestion() == DialogResult.Cancel)
				return;

			var filePath = OpenLogDialog.FileName;

			if (!File.Exists(filePath))
				return;

			LblCurrentFileName.Text = Path.GetFileName(filePath);

			LoadFile(filePath);
		}

		/// <summary>
		/// Asks user about clearing list, clears if answer is yes.
		/// </summary>
		/// <returns>User's answer, Yes, No, or Cancel.</returns>
		private DialogResult ClearListQuestion()
		{
			if (LstPackets.Items.Count == 0)
				return DialogResult.No;

			var answer = MessageBox.Show("Remove current packet data?", Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if (answer == DialogResult.Yes)
				ClearList();

			return answer;
		}

		/// <summary>
		/// Loads log file and adds packets to list.
		/// </summary>
		/// <param name="path"></param>
		private void LoadFile(string path)
		{
			var newPackets = new List<PalePacket>();

			using (var sr = new StreamReader(path))
			{
				string line;
				for (int ln = 1; (line = sr.ReadLine()) != null; ++ln)
				{
					try
					{
						line = line.Trim();
						var recv = false;

						if (string.IsNullOrWhiteSpace(line) || (!line.StartsWith("Send ") && !(recv = line.StartsWith("Recv "))))
							continue;

						var tabIndex = line.IndexOf('\t');
						var split = line.Substring(0, tabIndex).Split(' ');

						PacketType type;
						DateTime date;
						string name;
						int length = -1;

						if (split.Length == 3)
						{
							type = (split[0] == "Send" ? PacketType.ClientServer : PacketType.ServerClient);
							date = DateTime.Parse(split[1]);
							name = split[2];
						}
						else if (split.Length == 4)
						{
							type = (split[0] == "Send" ? PacketType.ClientServer : PacketType.ServerClient);
							date = DateTime.Parse(split[1]);
							name = split[2];
							length = Convert.ToInt32(split[3]);
						}
						else
							continue;

						var packetStr = line.Substring(tabIndex + 1, line.Length - tabIndex - 1);
						var packetArr = HexTool.ToByteArray(packetStr);
						var packet = new Packet(packetArr, type);
						var palePacket = new PalePacket(name, length, packet, date, recv);

						newPackets.Insert(0, palePacket);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Error on line " + ln + ": " + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}

			LstPackets.BeginUpdate();

			foreach (var palePacket in newPackets)
				AddPacketToFormList(palePacket, false);

			LstPackets.EndUpdate();

			UpdateCount();

			foreach (var palePacket in newPackets)
			{
				if (palePacket.Received)
					pluginManager.OnRecv(palePacket);
				else
					pluginManager.OnSend(palePacket);
			}
		}

		/// <summary>
		/// Updates packet count status label.
		/// </summary>
		private void UpdateCount()
		{
			StatusStrip.InvokeIfRequired((MethodInvoker)delegate
			{
				LblPacketCount.Text = "Packets: " + LstPackets.Items.Count;
			});
		}

		/// <summary>
		/// Adds packet to list, scrolls down if scroll is true.
		/// </summary>
		/// <param name="palePacket"></param>
		/// <param name="scroll"></param>
		private void AddPacketToFormList(PalePacket palePacket, bool scroll)
		{
			var name = palePacket.OpName;

			var lvi = new ListViewItem((palePacket.Received ? "<" : ">") + palePacket.Op.ToString("X8"));
			lvi.UseItemStyleForSubItems = false;
			lvi.BackColor = palePacket.Received ? Color.FromArgb(0x0033bbff) : Color.FromArgb(0x00ff5522);
			lvi.ForeColor = Color.White;
			lvi.Tag = palePacket;

			lvi.SubItems.Add(name);
			lvi.SubItems.Add(palePacket.Time > DateTime.MinValue ? palePacket.Time.ToString("hh:mm:ss.fff") : "");

			LstPackets.InvokeIfRequired((MethodInvoker)delegate
			{
				LstPackets.Items.Add(lvi);

				if (scroll)
					LstPackets.Items[LstPackets.Items.Count - 1].EnsureVisible();
			});
		}

		/// <summary>
		/// Clear button, clears packet list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnClear_Click(object sender, EventArgs e)
		{
			ClearList();
		}

		/// <summary>
		/// Clears packet list.
		/// </summary>
		private void ClearList()
		{
			LstPackets.BeginUpdate();
			LstPackets.Items.Clear();
			LstPackets.EndUpdate();

			TxtPacketInfo.Text = "";
			HexBox.ByteProvider = null;

			pluginManager.OnClear();

			UpdateCount();
		}

		/// <summary>
		/// Save log file button, opens save dialog to save all logged packets.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnSave_Click(object sender, EventArgs e)
		{
			SaveLogDialog.FileName = DateTime.Now.ToString("yyyyMMdd_HHmmss");

			if (SaveLogDialog.ShowDialog() == DialogResult.Cancel)
				return;

			try
			{
				using (var stream = SaveLogDialog.OpenFile())
				using (var sw = new StreamWriter(stream))
				{
					for (int i = LstPackets.Items.Count - 1; i >= 0; --i)
					{
						var palePacket = (PalePacket)LstPackets.Items[i].Tag;

						var method = palePacket.Received ? "Recv" : "Send";
						var time = palePacket.Time.ToString("hh:mm:ss.fff");
						var packetStr = HexTool.ToString(palePacket.Packet.GetBuffer());
						var name = palePacket.OpName;
						var size = palePacket.OpSize;

						sw.WriteLine(method + " " + time + " " + name + " " + size + "\t" + packetStr);
					}

					LblCurrentFileName.Text = Path.GetFileName(SaveLogDialog.FileName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to save file (" + ex.Message + ").", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Enables dropping of files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None);
		}

		/// <summary>
		/// Handles file drop.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_DragDrop(object sender, DragEventArgs e)
		{
			if (ClearListQuestion() == DialogResult.Cancel)
				return;

			var fileNames = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (fileNames.Length == 0)
				return;

			LoadFile(fileNames[0]);
		}

		/// <summary>
		/// Settings button, opens settings dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnSettings_Click(object sender, EventArgs e)
		{
			var form = new FrmSettings(log.ToString());
			var result = form.ShowDialog();

			if (result == DialogResult.Cancel)
				return;

			UpdateFilters();
		}

		/// <summary>
		/// Clears filter lists and loads them from settings.
		/// </summary>
		private void UpdateFilters()
		{
			lock (recvFilter)
			{
				recvFilter.Clear();
				ReadOpList(Settings.Default.FilterRecv, ref recvFilter);
			}

			lock (sendFilter)
			{
				sendFilter.Clear();
				ReadOpList(Settings.Default.FilterSend, ref sendFilter);
			}
		}

		/// <summary>
		/// Reads ops from string (line by line) and adds them to hash set.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="set"></param>
		private void ReadOpList(string list, ref HashSet<string> set)
		{
			using (var sr = new StringReader(list))
			{
				var line = "";
				while ((line = sr.ReadLine()) != null)
				{
					line = line.Trim();
					if (string.IsNullOrWhiteSpace(line) || line.StartsWith("//"))
						continue;

					var field = typeof(Op).GetField(line, BindingFlags.Public | BindingFlags.Static);
					if (field == null)
					{
						Trace.TraceError("Filter: Unknown op '{0}'.", line);
						continue;
					}

					set.Add(line);
				}
			}
		}

		/// <summary>
		/// Connect button, sends connect message to provider window.
		/// </summary>
		private void BtnConnect_Click(object sender, EventArgs e)
		{
			if (providerHWnd == IntPtr.Zero)
			{
				if (!SelectPacketProvider(true))
					return;
			}

			Connect();
		}

		/// <summary>
		/// Opens packet provider selection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnConnectTo_Click(object sender, EventArgs e)
		{
			if (!SelectPacketProvider(false))
				return;

			Connect();
		}

		/// <summary>
		/// Connects to the provider window.
		/// </summary>
		private void Connect()
		{
			if (!WinApi.IsWindow(providerHWnd))
			{
				//MessageBox.Show("Failed to connect, please make sure the selected packet provider is still running.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				providerHWnd = IntPtr.Zero;
				BtnConnect_Click(null, null);
				return;
			}

			SendMessage(providerHWnd, Sign.Connect);

			BtnConnect.Enabled = false;
			BtnConnectTo.Enabled = false;
			BtnDisconnect.Enabled = true;

			queueTimer.Enabled = true;
		}

		/// <summary>
		/// Tries to find a valid packet provider, asks the user to select one
		/// if there are multiple windows.
		/// </summary>
		/// <param name="selectSingle">If true a single valid candidate will be selected without prompt.</param>
		/// <returns></returns>
		private bool SelectPacketProvider(bool selectSingle)
		{
			var windows = WinApi.FindAllWindows("mod_Tossa");
			FoundWindow window = null;

			if (windows.Count == 0)
			{
				MessageBox.Show("No packet provider found.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			else if (selectSingle && windows.Count == 1)
			{
				window = windows[0];
			}
			else
			{
				var form = new FrmProviderSelection(windows, LblPacketProvider.Text);
				if (form.ShowDialog() == DialogResult.Cancel)
					return false;

				window = FrmProviderSelection.Selection;
			}

			providerHWnd = window.HWnd;
			LblPacketProvider.Text = window.ClassName;

			return true;
		}

		/// <summary>
		/// Disonnect button, sends disconnect message to provider window.
		/// </summary>
		private void BtnDisconnect_Click(object sender, EventArgs e)
		{
			Disconnect();
		}

		/// <summary>
		/// Sends disconnect message to provider window.
		/// </summary>
		private void Disconnect()
		{
			if (providerHWnd != IntPtr.Zero)
				SendMessage(providerHWnd, Sign.Disconnect);

			this.InvokeIfRequired((MethodInvoker)delegate
			{
				BtnConnect.Enabled = true;
				BtnConnectTo.Enabled = true;
				BtnDisconnect.Enabled = false;
			});

			queueTimer.Enabled = false;
		}

		/// <summary>
		/// Sends message to provider window.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="op"></param>
		private void SendMessage(IntPtr hWnd, int op)
		{
			WinApi.COPYDATASTRUCT cds;
			cds.dwData = (IntPtr)op;
			cds.cbData = 0;
			cds.lpData = IntPtr.Zero;

			var cdsBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(cds));
			Marshal.StructureToPtr(cds, cdsBuffer, false);

			this.InvokeIfRequired((MethodInvoker)delegate
			{
				WinApi.SendMessage(hWnd, WinApi.WM_COPYDATA, this.Handle, cdsBuffer);
			});
		}

		/// <summary>
		/// Window message handler, handles incoming data from provider.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WinApi.WM_COPYDATA)
			{
				var cds = (WinApi.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(WinApi.COPYDATASTRUCT));

				// The op will *always* be there, who knows about the
				// other things.
				if (cds.cbData < 2)
					return;

				var recv = (int)cds.dwData == Sign.Recv;

				var data = new byte[cds.cbData];
				Marshal.Copy(cds.lpData, data, 0, cds.cbData);

				var type = (!recv ? PacketType.ClientServer : PacketType.ServerClient);
				var packet = new Packet(data, type);
				var name = Shared.Op.GetName(packet.Op);
				var length = Shared.Op.GetSize(packet.Op);
				var palePacket = new PalePacket(name, length, packet, DateTime.Now, recv);

				lock (packetQueue)
					packetQueue.Enqueue(palePacket);
			}
			base.WndProc(ref m);
		}

		/// <summary>
		/// Returns a thread-safe list of all current packets.
		/// </summary>
		/// <returns></returns>
		public IList<PalePacket> GetPacketList()
		{
			IList<PalePacket> result = null;

			LstPackets.InvokeIfRequired((MethodInvoker)delegate
			{
				result = LstPackets.Items.Cast<ListViewItem>().Select(a => (PalePacket)a.Tag).ToArray();
			});

			return result;
		}

		/// <summary>
		/// Fired regularly while being connected, handles queued packets.
		/// </summary>
		/// <param name="state"></param>
		private void OnQueueTimer(object state, EventArgs args)
		{
			if (!WinApi.IsWindow(providerHWnd))
				Disconnect();

			var count = packetQueue.Count;
			if (count == 0)
				return;

			queueTimer.Enabled = false;

			var newPackets = new List<PalePacket>();
			for (int i = 0; i < count; ++i)
			{
				PalePacket palePacket;
				lock (packetQueue)
					palePacket = packetQueue.Dequeue();

				if (palePacket == null)
					continue;

				newPackets.Add(palePacket);
			}

			LstPackets.InvokeIfRequired((MethodInvoker)delegate
			{
				LstPackets.BeginUpdate();
				foreach (var palePacket in newPackets)
				{
					bool isFilter;

					lock (recvFilter)
						isFilter = Settings.Default.FilterRecvInvertedEnabled ? !recvFilter.Contains(palePacket.OpName) : recvFilter.Contains(palePacket.OpName);

					if (Settings.Default.FilterRecvEnabled && isFilter)
						continue;

					lock (sendFilter)
						isFilter = Settings.Default.FilterSendInvertedEnabled ? !sendFilter.Contains(palePacket.OpName) : sendFilter.Contains(palePacket.OpName);

					if (Settings.Default.FilterSendEnabled && isFilter)
						continue;

					AddPacketToFormList(palePacket, true);

					if (palePacket.Received)
						pluginManager.OnRecv(palePacket);
					else
						pluginManager.OnSend(palePacket);
				}
				LstPackets.EndUpdate();
			});

			UpdateCount();

			queueTimer.Enabled = true;
		}

		/// <summary>
		/// Packet list context menu opening, disables invalid items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CtxPacketList_Popup(object sender, EventArgs e)
		{
			if (LstPackets.SelectedItems.Count == 0)
			{
				foreach (MenuItem item in CtxPacketList.MenuItems)
					item.Enabled = false;
			}
		}

		/// <summary>
		/// Packet list context menu closing, re-enables all items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CtxPacketList_Collapse(object sender, EventArgs e)
		{
			foreach (MenuItem item in CtxPacketList.MenuItems)
				item.Enabled = true;
		}

		/// <summary>
		/// Returns (first) currently selected packet or null.
		/// </summary>
		/// <returns></returns>
		public PalePacket GetSelectedPacket()
		{
			if (LstPackets.SelectedItems.Count == 0)
				return null;

			if (!LstPackets.InvokeRequired)
				return (PalePacket)LstPackets.SelectedItems[0].Tag;

			PalePacket result = null;
			LstPackets.Invoke((MethodInvoker)delegate
			{
				result = (PalePacket)LstPackets.SelectedItems[0].Tag;
			});

			return result;
		}

		/// <summary>
		/// Copies op code of selected packet.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnMenuPacketsCopyOp_Click(object sender, EventArgs e)
		{
			var selected = GetSelectedPacket();
			if (selected == null)
				return;

			Clipboard.SetText(selected.Op.ToString());
		}

		/// <summary>
		/// Copies selected packet's buffer as hex string.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnMenuPacketsCopyHex_Click(object sender, EventArgs e)
		{
			var selected = GetSelectedPacket();
			if (selected == null)
				return;

			var str = HexTool.ToString(selected.Packet.GetBuffer());

			Clipboard.SetText(str);
		}

		/// <summary>
		/// Adds the selected packet's op to the filter list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnMenuPacketsFilter_Click(object sender, EventArgs e)
		{
			var selected = GetSelectedPacket();
			if (selected == null)
				return;

			var result = MessageBox.Show("Remove packets with this op from list now?", Text, MessageBoxButtons.YesNoCancel);
			if (result == DialogResult.Cancel)
				return;

			var addition = Environment.NewLine + selected.OpName + Environment.NewLine;

			if (selected.Received)
				Settings.Default.FilterRecv += addition;
			else
				Settings.Default.FilterSend += addition;

			Settings.Default.Save();

			UpdateFilters();

			if (result == DialogResult.Yes)
				FilterPacketList(selected.Op, selected.Received);
		}

		/// <summary>
		/// Removes specific packets from the packet list.
		/// </summary>
		private void FilterPacketList(int op, bool received)
		{
			var toRemove = new List<int>();

			for (int i = 0; i < LstPackets.Items.Count; ++i)
			{
				var palePacket = (PalePacket)LstPackets.Items[i].Tag;
				if (palePacket.Op == op && (!received || (received && palePacket.Received)))
					toRemove.Add(i);
			}

			RemoveFromList(toRemove);
		}

		/// <summary>
		/// Removes packets at the given indexes from list.
		/// </summary>
		/// <param name="idxs"></param>
		private void RemoveFromList(IList<int> idxs)
		{
			LstPackets.BeginUpdate();

			for (int i = idxs.Count - 1; i >= 0; --i)
				LstPackets.Items.RemoveAt(idxs[i]);

			LstPackets.EndUpdate();

			UpdateCount();
		}

		/// <summary>
		/// Filters the packet list using the filter settings.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnMenuEditFilter_Click(object sender, EventArgs e)
		{
			var toRemove = new List<int>();

			for (int i = 0; i < LstPackets.Items.Count; ++i)
			{
				var palePacket = (PalePacket)LstPackets.Items[i].Tag;
				if (palePacket.Received && Settings.Default.FilterRecvEnabled)
				{
					lock (recvFilter)
					{
						if (recvFilter.Contains(palePacket.OpName))
							toRemove.Add(i);
					}
				}
				else if (!palePacket.Received && Settings.Default.FilterSendEnabled)
				{
					lock (sendFilter)
					{
						if (sendFilter.Contains(palePacket.OpName))
							toRemove.Add(i);
					}
				}
			}

			RemoveFromList(toRemove);
		}

		/// <summary>
		/// Removes selected packets from list on pressing Delete.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LstPackets_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete)
				return;

			var toRemove = new List<int>();
			toRemove.AddRange(LstPackets.SelectedIndices.Cast<int>());

			RemoveFromList(toRemove);
		}

		/// <summary>
		/// Fired when form is shown, calls Ready event for plugins.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FrmMain_Shown(object sender, EventArgs e)
		{
			pluginManager.OnReady();
		}

		/// <summary>
		/// Fired when the selected byte in the hex control changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void HexBox_SelectionStartChanged(object sender, EventArgs e)
		{
			PalePacket palePacket = null;
			int start = -1;

			if (LstPackets.SelectedItems.Count != 0)
			{
				palePacket = (PalePacket)LstPackets.SelectedItems[0].Tag;
				start = (int)HexBox.SelectionStart;
			}

			pluginManager.OnSelectedHex(palePacket, start);
		}
	}
}
