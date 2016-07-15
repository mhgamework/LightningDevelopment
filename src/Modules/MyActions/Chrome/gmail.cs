using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class GMail : IQuickAction
    {
        public static string ChromeAddress = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";

        public string Command
        {
            get { return "ma"; }
        }

        public void Execute(string[] arguments)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = ChromeAddress;
            cmd.StartInfo.Arguments = "https://mail.google.com/mail/u/0/#inbox";
            cmd.Start();
        }







    }
}
