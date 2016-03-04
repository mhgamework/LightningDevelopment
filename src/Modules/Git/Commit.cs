using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.Git;

namespace Tools.Tools
{
    public class Commit : IQuickAction
    {
        public string Command
        {
            get { return "commit"; }
        }

        public void Execute(string[] arguments)
        {
            var gitRoot = GitPlugin.GitRoot.Get();
            if (gitRoot == null) return;

            TortoiseProc.Do(TortoiseProc.Command.Commit, gitRoot);
        }
    }
}
