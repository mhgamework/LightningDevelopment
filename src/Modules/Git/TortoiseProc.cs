using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class TortoiseProc
    {
        public static void Do(Command cmd, string path,params TortoiseParameter[] parameters)
        {
            var add = "";
            foreach (var p in parameters)
            {
                if (add != "") add += " ";
                add += "/" + p.Name + ":" + p.Value;
            }
            CSRunner.RunExecutable(Config.Get.TortoiseProc, "/command:" + cmd.ToString().ToLower() + " /path:\"" + path + "\" " + add ,false);
        }
        public enum Command
        {
            None,
            Commit,
            Log,
            Fetch,
            Push
        }
    }

    public struct TortoiseParameter
    {
        public string Name;
        public string Value;
        public TortoiseParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
