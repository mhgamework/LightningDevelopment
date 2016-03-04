using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class PullRequests : IQuickAction
    {


        public string Command
        {
            get { return "pu"; }
        }

        public void Execute(string[] arguments)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            cmd.StartInfo.Arguments = "https://bitbucket.org/xenit/fred/pull-requests/";
            cmd.Start();
        }







    }
}
