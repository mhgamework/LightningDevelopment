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
            if (arguments.Length == 0 || arguments[0].Equals("fr"))
                cmd.StartInfo.Arguments = "https://bitbucket.org/xenit/fred/pull-requests/";
            if (arguments.Length > 0 && arguments[0].Equals("frn"))
                cmd.StartInfo.Arguments = "https://bitbucket.org/xenit/fred/pull-requests/new/";
            if (arguments.Length > 0 && arguments[0].Equals("fc"))
                cmd.StartInfo.Arguments = "https://bitbucket.org/xenit/finder-core/pull-requests/";
            if (arguments.Length > 0 && arguments[0].Equals("fcn"))
                cmd.StartInfo.Arguments = "https://bitbucket.org/xenit/finder-core/pull-requests/new/";

            cmd.Start();
        }







    }
}
