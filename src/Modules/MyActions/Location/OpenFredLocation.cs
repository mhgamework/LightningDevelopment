using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.Git;
namespace Modules.Alfresco
{
    public class OpenFredLocation : IQuickAction
    {
            public string Command
            {
                get { return "fredloc"; }
            }

            public void Execute(string[] arguments)
            {

                Process.Start(@"C:\data\projects\fred");

            }
    }
}