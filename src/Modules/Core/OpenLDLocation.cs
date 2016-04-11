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
    public class OpenLDLocation : IQuickAction
    {
        public string Command
        {
            get { return "ldloc"; }
        }

        public void Execute(string[] arguments)
        {

            Process.Start(@"C:\Users\Jasperhilven\Desktop\LightningDevelopment\src");

        }
    }
}
