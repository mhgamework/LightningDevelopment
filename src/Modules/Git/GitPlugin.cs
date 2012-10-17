using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.ContextModel;
using Modules.Core;

namespace Modules.Git
{
    public class GitPlugin : IPlugin
    {
        public static ContextualValue<string> GitRoot = new ContextualValue<string>();


        public void Dispose()
        {
        }

        public void Initialize(LightningDevelopmentHandle handle)
        {
            CorePlugin.WorkingDirectory.Changed += updateGitBasedir;
            // TODO: detect and log git repository locations
        }

        private void updateGitBasedir(ContextualValue<string> arg1, string arg2)
        {
            var dir = new DirectoryInfo(arg1.Get());
            while (dir.Parent != dir)
            {
                if (Directory.Exists(dir.FullName + "\\.git"))
                {
                    if (!dir.Exists) return;
                    GitRoot.Set(dir.FullName);
                }
                dir = dir.Parent;
            }
        }
    }
}
