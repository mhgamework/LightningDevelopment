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
            if (arguments.Length != 1)
                return;
            int ticketNumber;
            if (!Int32.TryParse(arguments[0], out ticketNumber))
                return;
            Process cmd = new Process();
            cmd.StartInfo.FileName = GMail.ChromeAddress;
            cmd.StartInfo.Arguments = "https://xenitsupport.jira.com/browse/XENFRED-" + ticketNumber;
            cmd.Start();
        }







    }
}
