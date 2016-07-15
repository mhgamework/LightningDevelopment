using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class Boards : IQuickAction
    {


        public string Command
        {
            get { return "b"; }
        }

        public void Execute(string[] arguments)
        {
                Process cmd = new Process();
            cmd.StartInfo.FileName = GMail.ChromeAddress;
            cmd.StartInfo.Arguments = GetUrl(arguments);
            cmd.Start();
        }

        private string GetUrl(string[] arguments)
        {
            if (arguments.Length == 0) //
                return @"https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=3&quickFilter=240";
            if(arguments[0] == "up")
                return @"https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=91&quickFilter=368";
            if (arguments[0] == "do")
                return @"https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=3&quickFilter=240";
            if (arguments[0] == "doe")
                return @"https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=79";

            //Otherwise we go for downstream.
            return @"https://xenitsupport.jira.com/secure/RapidBoard.jspa?rapidView=3&quickFilter=240";

        }
    }
}
