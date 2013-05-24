using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tools
{
    public class Config
    {
        private static Config instance;

        static Config()
        {
            var s = new XmlSerializer(typeof(Config));

            instance = new Config();

            var file = "config.xml";
            if (File.Exists(file))
                using (var fs = File.OpenRead("config.xml"))
                    instance = (Config)s.Deserialize(fs);

            using (var fs = File.OpenWrite(file))
                s.Serialize(fs, instance);


        }

        private Config()
        {
            TheWizardsRoot = Directory.GetParent(Environment.CurrentDirectory).FullName;
            TortoiseProc = @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe";
            GitSh = @"C:\Program Files (x86)\Git\bin\sh.exe";
        }


        public string TheWizardsRoot { get; set; }

        public string TortoiseProc { get; set; }

        public string GitSh { get; set; }

        public static Config Get { get { return instance; } }
    }
}
