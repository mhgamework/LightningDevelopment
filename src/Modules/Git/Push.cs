﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.Git;

namespace Tools.Tools
{
    public class Push : IQuickAction
    {
        public string Command
        {
            get { return "push"; }
        }

        public void Execute(string[] arguments)
        {
            var gitRoot = GitPlugin.GitRoot.Get();
            if (gitRoot == null) return;

            TortoiseProc.Do(TortoiseProc.Command.Push, gitRoot);
            //TortoiseProc.Do(TortoiseProc.Command.Fetch, gitRoot, new TortoiseParameter("remote", "origin"));

        
        }
    }
}

