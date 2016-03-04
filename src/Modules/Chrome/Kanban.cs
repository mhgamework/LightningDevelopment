using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class Kanban : IQuickAction
    {


        public string Command
        {
            get { return "ka"; }
        }

        public void Execute(string[] arguments)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            cmd.StartInfo.Arguments = "https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=3&quickFilter=240";
            cmd.Start();
        }







    }
}
