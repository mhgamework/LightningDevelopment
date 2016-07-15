using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using System.Diagnostics;

namespace Modules.Chrome
{
    class ClientLinks: IQuickAction
    {


        public string Command
        {
            get { return "l"; }
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
                return @"http://192.168.99.1:8080/alfresco/s/searchapp/index.html?all=*&pageSize=15&view=1&page=1";
            if (arguments[0] == "fl")
                return @"http://192.168.99.1:8080/alfresco/s/searchapp/index.html?all=*&pageSize=15&view=1&page=1";
            if (arguments[0] == "pl")
                return @"http://192.168.99.1:8080/alfresco/s/searchapp/index-non-life.html?all=*&pageSize=15&view=1&page=1";
            if (arguments[0] == "rl")
                return @"http://192.168.99.1:8080/alfresco/s/searchapp/index-rezidor.html?all=*&pageSize=15&view=1&page=1";
            if (arguments[0] == "el")
                return @"http://192.168.99.1:8080/alfresco/s/searchapp/index-ethias.html?all=*&pageSize=15&view=1&page=1";
            if (arguments[0] == "ps")
                return @"https://nonlife.dev.xenit.eu/alfresco/service/searchapp/?all=**&pageSize=15&view=1&page=1";
            if (arguments[0] == "rs")
                return @"https://rezidor.dev.xenit.eu/alfresco/s/searchapp/?all=**&pageSize=15&view=1&page=1";
            if (arguments[0] == "es")
                return @"https://nonlife.dev.xenit.eu/alfresco/service/searchapp/?all=**&pageSize=15&view=1&page=1";
            return @"http://192.168.99.1:8080/alfresco/s/searchapp/index.html?all=*&pageSize=15&view=1&page=1";
        }
    }
}
