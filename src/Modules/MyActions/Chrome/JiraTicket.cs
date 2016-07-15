using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class JiraTicket : IQuickAction
    {


        public string Command
        {
            get { return "x"; }
        }

        public void Execute(string[] arguments)
        {

            if (arguments.Length != 2)
                return;
            var project = GetProject(arguments[0]);
            int ticketNumber;
            if (!Int32.TryParse(arguments[1], out ticketNumber))
                return;
            Process cmd = new Process();
            cmd.StartInfo.FileName = GMail.ChromeAddress;
            cmd.StartInfo.Arguments = "https://xenitsupport.jira.com/browse/"+ project + "-" + ticketNumber;
            cmd.Start();
        }

        public string GetProject(string shortcut)
        {
            if (shortcut == "fc")
                return "XENFIN";
            if (shortcut == "fr")
                return "XENFRED";
            if (shortcut == "et")
                return "ETHIASMIGR";
            return "XENFIN";

        }







    }
}
