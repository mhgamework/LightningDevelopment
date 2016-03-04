using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;

namespace Tools.Tools
{
    public class Exit : IQuickAction
    {
        public string Command
        {
            get { return "exit"; }
        }

        public void Execute(string[] arguments)
        {
            Environment.Exit(0);
        }
    }
}
