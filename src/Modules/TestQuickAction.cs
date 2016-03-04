using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using DocumentationHelper;
using LightningDevelopment;

namespace Modules.TWSourceToDokuWiki
{
    public class TestQuickAction : IQuickAction
    {
       

        public string Command
        {
            get { return "test"; }
        }

        public void Execute(string[] arguments)
        {

           

        }

    }

}
