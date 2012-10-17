using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;

namespace Tools.Tools
{
    public class Commit : IQuickAction
    {
        public string Command
        {
            get { return "commit"; }
        }

        public void Execute()
        {
            if (!Directory.Exists(Context.WorkingDir))
                return;
            TortoiseProc.Do(TortoiseProc.Command.Commit, Context.WorkingDir);
        }
    }
}
