using Melia.Shared.Data;
using PaleTree.Plugins.EntityLogger.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaleTree.Plugins.EntityLogger
{
	public class DBUtils
	{
		public static bool IsMeliaFolderEnable()
		{
			return !string.IsNullOrWhiteSpace(Settings.Default.FolderMelia);
		}

		/// <summary>
		/// Loads db, first from system, then from user.
		/// Logs problems as warnings.
		/// </summary>
		public static void LoadDb(IDatabase db, string meliaPath, string path, bool reload, bool log = true)
		{
			var systemPath = Path.Combine(meliaPath, "system");
			systemPath = Path.Combine(systemPath, path).Replace('\\', '/');
			var userPath = Path.Combine(meliaPath, "user");
			userPath = Path.Combine(systemPath, path).Replace('\\', '/');

			if (!File.Exists(systemPath))
				throw new FileNotFoundException("Data file '" + systemPath + "' couldn't be found.", systemPath);

			db.Load(new string[] { systemPath, userPath }, null, reload);
		}

		public static void LoadData(string meliaPath, MeliaData data, DataToLoad toLoad, bool reload)
		{
			if ((toLoad & DataToLoad.Items) != 0)
			{
				LoadDb(data.ItemDb, meliaPath, "db/items.txt", reload);
			}

			if ((toLoad & DataToLoad.Jobs) != 0)
			{
				LoadDb(data.JobDb, meliaPath, "db/jobs.txt", reload);
			}

			if ((toLoad & DataToLoad.Maps) != 0)
			{
				LoadDb(data.MapDb, meliaPath, "db/maps.txt", reload);
			}

			if ((toLoad & DataToLoad.Monsters) != 0)
			{
				LoadDb(data.MonsterDb, meliaPath, "db/monsters.txt", reload);
			}

			if ((toLoad & DataToLoad.Barracks) != 0)
			{
				LoadDb(data.BarrackDb, meliaPath, "db/barracks.txt", reload);
			}
		}

		[Flags]
		public enum DataToLoad
		{
			Items = 0x01,
			Maps = 0x02,
			Jobs = 0x04,
			Barracks = 0x10,
			Monsters = 0x20,

			All = 0x7FFFFFFF,
		}
	}
}
