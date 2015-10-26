using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaleTree.Plugins.VariablePreview.Properties;
using PaleTree.Shared;

namespace PaleTree.Plugins.VariablePreview
{
	public class Main : Plugin
	{
		private FrmVariablePreview form;
		private byte[] buffer;

		public override string Name
		{
			get { return "Variable Preview"; }
		}

		public Main(IPluginManager pluginManager)
			: base(pluginManager)
		{

		}

		public override void Initialize()
		{
			manager.AddToMenu(Name, OnClick);
			manager.AddToToolbar(Resources.timeline_marker, Name, OnClick);

			manager.Selected += OnSelected;
			manager.SelectedHex += OnSelectedHex;
		}

		private void OnClick(object sender, EventArgs e)
		{
			if (form == null || form.IsDisposed)
				manager.OpenCentered(form = new FrmVariablePreview());
			else
				form.Focus();
		}

		private void OnSelected(PalePacket palePacket)
		{
			if (palePacket != null)
				buffer = palePacket.Packet.GetBuffer();
			else
				buffer = null;

			form.UpdateValues(buffer, 0);
		}

		private void OnSelectedHex(PalePacket palePacket, int start)
		{
			if (form == null || form.IsDisposed)
				return;

			form.UpdateValues(buffer, start);
		}
	}
}
