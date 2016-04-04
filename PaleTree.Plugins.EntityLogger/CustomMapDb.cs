using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Data;
using System.Linq;

namespace PaleTree.Plugins.EntityLogger
{
	/// <summary>
	/// Map database, indexed by map id.
	/// </summary>
	public class CustomMapDb : MapDb
	{
		private Dictionary<string, MapData> _nameIndex = new Dictionary<string, MapData>();

		public MapData Find(string localKey)
		{
			MapData result;
			_nameIndex.TryGetValue(localKey, out result);
			return result;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("mapId", "className", "engName", "localKey");

			var info = new MapData();

			info.Id = entry.ReadInt("mapId");
			info.ClassName = entry.ReadString("className");
			info.EngName = entry.ReadString("engName");
			info.LocalKey = entry.ReadString("localKey");

			this.Entries[info.Id] = info;
			_nameIndex[info.LocalKey] = info;
		}
	}

	internal static class JsonExtensions
	{
		internal static bool ReadBool(this JObject obj, string key, bool def = false) { return (bool)(obj[key] ?? def); }
		internal static byte ReadByte(this JObject obj, string key, byte def = 0) { return (byte)(obj[key] ?? def); }
		internal static sbyte ReadSByte(this JObject obj, string key, sbyte def = 0) { return (sbyte)(obj[key] ?? def); }
		internal static short ReadShort(this JObject obj, string key, short def = 0) { return (short)(obj[key] ?? def); }
		internal static ushort ReadUShort(this JObject obj, string key, ushort def = 0) { return (ushort)(obj[key] ?? def); }
		internal static int ReadInt(this JObject obj, string key, int def = 0) { return (int)(obj[key] ?? def); }
		internal static uint ReadUInt(this JObject obj, string key, uint def = 0) { return (uint)(obj[key] ?? def); }
		internal static float ReadFloat(this JObject obj, string key, float def = 0) { return (float)(obj[key] ?? def); }
		internal static double ReadDouble(this JObject obj, string key, double def = 0) { return (double)(obj[key] ?? def); }
		internal static string ReadString(this JObject obj, string key, string def = "") { return (string)(obj[key] ?? def); }

		internal static bool ContainsAnyKeys(this JObject obj, params string[] keys)
		{
			if (keys.Length == 1)
				return (obj[keys[0]] != null);

			return keys.Any(key => obj[key] != null);
		}

		/// <summary>
		/// Returns true if object contains all keys.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="keys"></param>
		/// <returns></returns>
		internal static bool ContainsKeys(this JObject obj, params string[] keys)
		{
			if (keys.Length == 1)
				return (obj[keys[0]] != null);

			return keys.All(key => obj[key] != null);
		}

		/// <summary>
		/// Returns true if object containes key.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		internal static bool ContainsKey(this JObject obj, string key)
		{
			return (obj[key] != null);
		}

		/// <summary>
		/// Throws exception if one of the keys is missing from the object.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="keys"></param>
		/// <exception cref="MandatoryValueException"></exception>
		internal static void AssertNotMissing(this JObject obj, params string[] keys)
		{
			foreach (var key in keys)
			{
				if (obj[key] == null)
					throw new MandatoryValueException(null, key, obj);
			}
		}
	}
}
