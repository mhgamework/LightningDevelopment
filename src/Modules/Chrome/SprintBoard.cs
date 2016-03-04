using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class SprintBoard : IQuickAction
    {


        public string Command
        {
            get { return "sp"; }
        }

        public void Execute(string[] arguments)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            cmd.StartInfo.Arguments = @"https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=20&quickFilter=272";
            cmd.Start();
        }
    }
}
