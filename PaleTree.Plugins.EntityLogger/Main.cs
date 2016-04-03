using PaleTree.Plugins.EntityLogger.Properties;
using PaleTree.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PaleTree.Plugins.EntityLogger
{
	public class Main : Plugin
	{
		private List<IEntity> entities;

		private FrmEntityLogger form;

		public override string Name
		{
			get { return "Entity Logger"; }
		}

		public Main(IPluginManager pluginManager)
			: base(pluginManager)
		{
			entities = new List<IEntity>();
		}

		public override void Initialize()
		{
			manager.AddToMenu(Name, OnClick);
			manager.AddToToolbar(Resources.bug, Name, OnClick);

			manager.Recv += OnRecv;
			manager.Clear += OnClear;
		}

		private void OnClick(object sender, EventArgs e)
		{
			if (form == null || form.IsDisposed)
			{
				form = new FrmEntityLogger(entities);
				manager.OpenCentered(form);
			}
			else
			{
				form.Focus();
			}
		}

		private void OnClear()
		{
			lock (entities)
				entities.Clear();

			if (form != null && !form.IsDisposed)
				form.ClearEntities();
		}

		private void OnRecv(PalePacket palePacket)
		{
			// ZC_ENTER_MONSTER
			if (palePacket.Op == Op.ZC_ENTER_MONSTER)
			{
				AddMonsterInfo(palePacket.Packet);
			}
		}

		private void AddMonsterInfo(Packet packet)
		{
			var size = packet.GetShort();
			var handle = packet.GetInt();
			var x = packet.GetFloat();
			var y = packet.GetFloat();
			var z = packet.GetFloat();
			var dx = packet.GetFloat();
			var dy = packet.GetFloat();
			var type = packet.GetByte();
			var b1 = packet.GetByte();
			var hp = packet.GetInt();
			var maxHp = packet.GetInt();
			var s1 = packet.GetShort();
			var i1 = packet.GetFloat();

			// [i11025 (2016-02-26)] ?
			var s2 = packet.GetShort();

			// MONSTER
			var monsterId = packet.GetInt();
			var monsterHp = packet.GetInt(); // monsterHp?
			var monsterMaxHp = packet.GetInt();
			var level = packet.GetInt();
			var sdr = packet.GetFloat();
			var b1_2 = packet.GetByte();
			var bin2 = packet.GetBin(3);

			var i4 = packet.GetInt();
			var namesSize = packet.GetInt();
			var propertiesSize = packet.GetByte();
			var b4 = packet.GetByte();

			var name = packet.GetString(packet.GetShort());
			var uniqueName = packet.GetString(packet.GetShort());
			var dialogName = packet.GetString(packet.GetShort());
			var warpName = packet.GetString(packet.GetShort());
			var str3 = packet.GetString(packet.GetShort());

			var properties = packet.GetBin(propertiesSize);

			var monster = new Monster(handle, monsterId, (NpcType)type);
			monster.Position = new Position(x, y, z);
			monster.Direction = new Direction(dx, dy);

			monster.Hp = monsterHp;
			monster.MaxHp = monsterMaxHp; 
			monster.Level = level;
			monster.SDR = sdr;

			monster.Name = name;
			monster.DialogName = dialogName;
			monster.WarpName = warpName;
			AddEntity(monster);			
		}		

		private void AddEntity(Monster entity)
		{
			if (CheckDuplicate(entity))
				return;

			lock (entities)
				entity.Entities = entities;
				entities.Add(entity);

			if (form != null && !form.IsDisposed)
				form.AddEntity(entity);
		}

		private bool CheckDuplicate(IEntity newEntity)
		{
			lock (entities)
				return entities.Any(entity => entity.Equals(newEntity));
		}		
	}
}
