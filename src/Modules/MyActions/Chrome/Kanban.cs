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
            var url = (arguments.Count() > 0 && arguments[0].Equals("i"))
                ? "https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=3&view=planning.nodetail&quickFilter=18&quickFilter=240"
                : "https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=3&quickFilter=240";
            Process cmd = new Process();
            cmd.StartInfo.FileName = GMail.ChromeAddress;
            cmd.StartInfo.Arguments = url;
            cmd.Start();
        }







    }
}
