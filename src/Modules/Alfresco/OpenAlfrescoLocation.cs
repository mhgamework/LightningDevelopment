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
    public class OpenAlfrescoLocation : IQuickAction
    {
            public string Command
            {
                get { return "alfloc"; }
            }

            public void Execute(string[] arguments)
            {

                Process.Start(@"C:\data\Alfresco\alfresco-enterprise-5.0.2.1");

            }
    }
}