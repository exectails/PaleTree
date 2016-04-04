using Melia.Shared.Const;
using Melia.Shared.Data.Database;
using Melia.Shared.Util;
using PaleTree.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PaleTree.Plugins.EntityLogger
{
	public class Monster : IEntity
	{
		/// <summary>
		/// Index in world collection?
		/// </summary>
		public int Handle { get; private set; }

		//private Map _map = Map.Limbo;
		/// <summary>
		/// The map the monster is currently on.
		/// </summary>
		//public Map Map { get { return _map; } set { _map = value ?? Map.Limbo; } }

		/// <summary>
		/// Monster ID in database.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// What kind of NPC the monster is.
		/// </summary>
		public NpcType NpcType { get; set; }

		/// <summary>
		/// Monster's name, leave empty for default.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Name of dialog function to call on trigger,
		/// leave empty for no dialog hotkey display.
		/// </summary>
		public string DialogName { get; set; }

		/// <summary>
		/// Warp identifier?
		/// </summary>
		/// <remarks>
		/// Purpose unknown, doesn't seem to affect anything.
		/// Examples: WS_KLAPEDA_HIGHLANDER, WS_SIAULST1_KLAPEDA
		/// </remarks>
		public string WarpName { get; set; }

		/// <summary>
		/// Returns true if WarpName is not empty.
		/// </summary>
		public bool IsWarp { get { return !string.IsNullOrWhiteSpace(this.WarpName); } }

		/// <summary>
		/// Location to warp to.
		/// </summary>
		public Location WarpLocation { get; set; }

		/// <summary>
		/// Level.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Monster's position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Monster's direction.
		/// </summary>
		public Direction Direction { get; set; }

		/// <summary>
		/// AoE Defense Ratio
		/// </summary>
		public float SDR { get; set; }

		/// <summary>
		/// Health points.
		/// </summary>
		public int Hp
		{
			get { return _hp; }
			set { _hp = Math2.Clamp(0, this.MaxHp, value); }
		}
		private int _hp;

		/// <summary>
		/// Maximum health points.
		/// </summary>
		public int MaxHp { get; set; }

		/// <summary>
		/// At this time the monster will be removed from the map.
		/// </summary>
		public DateTime DisappearTime { get; set; }

		public List<IEntity> Entities { get; set; }

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		public Monster(int handle, int id, NpcType type)
		{
			this.Handle = handle;

			this.Id = id;
			this.NpcType = type;
			this.Level = 1;
			this.SDR = 1;
			this.Hp = this.MaxHp = 100;
			this.DisappearTime = DateTime.MaxValue;
		}

		public string GetInfo()
		{
			var sb = new StringBuilder();

			sb.AppendLine("Monster id: {0:X16}", this.Id);
			sb.AppendLine("Name: {0}", this.Name);
			if(this.IsWarp)			
				sb.AppendLine("WarpName: {0}", this.WarpName);
			sb.AppendLine();

			sb.AppendLine("Hp: {0:0.00} / {1:0.00}", this.Hp, this.MaxHp);
			sb.AppendLine();

			sb.AppendLine("Position: X:{0}, Y:{1}, Z:{2}", this.Position.X, this.Position.Y, this.Position.Z);
			sb.AppendLine("Direction: {0}", this.Direction.ToDegree());
			sb.AppendLine();
			
			return sb.ToString();
		}

		public string GetScript()
		{
			var sb = new StringBuilder();

			if (this.IsWarp)
			{
				Monster pairWarp = new Monster(0, 0, NpcType.NPC);
				pairWarp.Name = "NO_WARP_FOUND";
				for (int i = Entities.Count - 1; i >= 0; --i)
				{
					Monster other = (Monster)Entities[i];
					if (other.IsWarp && !this.Equals(other) ? this.IsPairWarp(other) : false)
					{
						pairWarp = other;
						break;
					}
				}

				if(DBUtils.IsMeliaFolderEnable()) 
				{
					Main.Data.MapDb.Find("12");
				}

				sb.Append("addWarp(");
				sb.Append("\"{0}\", {1}, ", this.WarpName, this.Direction.ToDegree());
				sb.Append("\"{0}\", {1}, {2}, {3}, ", GetMapClassName(pairWarp.Name), this.Position.X, this.Position.Y, this.Position.Z);
				sb.Append("\"{0}\", {1}, {2}, {3}", GetMapClassName(this.Name), pairWarp.Position.X + 50, pairWarp.Position.Y + 50, pairWarp.Position.Z + 50);
				sb.Append(")");
				sb.AppendLine();
			}

			return sb.ToString();
		}

		private string GetMapClassName(string localKey)
		{
			var dollarIdx = localKey.IndexOf("$");
			var lastDollarIdx = localKey.LastIndexOf("$");

			if (dollarIdx > -1 && lastDollarIdx > -1)
			{
				var extractedlocalKey = localKey.Substring(dollarIdx + 1, lastDollarIdx - dollarIdx - 1);

				if (Main.Data.MapDb is CustomMapDb)
				{
					var mapDb = (CustomMapDb) Main.Data.MapDb;
					MapData mapData = mapDb.Find(extractedlocalKey);
					if (mapData != null)
					{
						return mapData.ClassName;
					}
				}
			}
			return localKey;
		}

		public override bool Equals(object obj)
		{
			var other = obj as Monster;
			if (other == null)
				return false;

			return
				(this.Id == other.Id) &&
				(this.WarpName.Equals(other.WarpName));
		}

		public bool IsPairWarp(Monster other) {
			if (this.WarpName.Equals(other.WarpName))
				return false;

			var otherWarpNames = new List<string>(other.WarpName.Split('_'));
			var thisWarpNames = new List<string>(this.WarpName.Split('_'));
			return !otherWarpNames.Except(thisWarpNames).Any() && !thisWarpNames.Except(otherWarpNames).Any();
		}
	}
}
