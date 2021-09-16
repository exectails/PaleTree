// Copyright (c) Aura development team - Licensed under GNU GPL
// For more information, see license file in the main folder

using System;
using System.Collections.Generic;
using System.IO;

namespace PaleTree.Shared
{
    public static class Op
    {
        static readonly Dictionary<int, int> OPSize = new Dictionary<int, int>();
        static readonly Dictionary<int, string> OPName = new Dictionary<int, string>();

        public static void FillDictionary()
        {
            using (StreamReader reader = new StreamReader("Zemyna_Ops.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    OPSize.Add(Convert.ToInt32(parts[1], 16), Int32.Parse(parts[2].ToString().Trim('(', ')')));
                    OPName.Add(Convert.ToInt32(parts[1], 16), parts[0].ToString().Trim(':'));
                }
            }
        }

        public static int GetSize(int op)
        {
            if (!OPSize.TryGetValue(op, out var size))
                return -1;
            return size;
        }

        public static string GetName(int op)
        {
            if (!OPName.TryGetValue(op, out var name))
                return "?";
            return name;
        }
    }
}
