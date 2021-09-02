// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using System;
using System.Data;

namespace PaleTree.Shared
{
	public static class Op
	{
		public static DataTable OpsTable = new DataTable();

		public static void FillDataTable()
		{

			OpsTable.Columns.Add("name");
			OpsTable.Columns.Add("op");
			OpsTable.Columns.Add("size");

			string[] lines = System.IO.File.ReadAllLines("Zemyna_Ops.txt");

			foreach (string line in lines)
			{
				var cols = line.Split(' ');

				DataRow dr = OpsTable.NewRow();
				for (int index = 0; index < 3; index++)
				{
					dr[index] = cols[index];
				}
				OpsTable.Rows.Add(dr);
			}
		}

		public static int GetSize(int op)
		{
			foreach (DataRow row in OpsTable.Rows)
			{
                		if (row["op"].ToString() == "0x" + op.ToString("X"))
                		{
					return Int32.Parse(row["size"].ToString().Trim('(', ')'));
				}
			}
			return -1;
		}

		public static string GetName(int op)
		{
           		 foreach (DataRow row in OpsTable.Rows)
			{
				if (row["op"].ToString() == "0x" + op.ToString("X"))
				{
					return row["name"].ToString().Trim(':');
				}
			}
			return "?";
		}
	}
}
