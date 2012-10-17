using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.Git;

namespace Tools.Tools
{
    public class Bash : IQuickAction
    {
        public string Command
        {
            get { return "bash"; }
        }

        public void Execute()
        {
            var gitRoot = GitPlugin.GitRoot.Get();
            if (gitRoot == null) return;

            //"C:\Program Files (x86)\Git\Git Bash.lnk"


            //Process p;
            //p = Process.Start(new ProcessStartInfo
            //{
            //    FileName = @"cmd",
            //    Arguments = "/c \"\"C:\\Program Files (x86)\\Git\\bin\\sh.exe\" --login -i\"",
            //    WorkingDirectory = Config.TheWizardsRoot,
            //    CreateNoWindow = false,
            //    UseShellExecute = false,
            //    RedirectStandardOutput = true
            //});

            //Console.ReadLine();
            Process p;
            p = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = "/c \"\"" + Config.GitSh + "\" --login -i\"",
                WorkingDirectory = gitRoot,
                CreateNoWindow = false,
                UseShellExecute = false,
                RedirectStandardOutput = false
            });
        }
    }
}
