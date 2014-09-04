using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace LightningDevelopment
{
    public class Configuration
    {
        private static Configuration instance;
        static Configuration()
        {
            instance = LoadConfigurationFile<Configuration>("config.xml");

        }

        public static T LoadConfigurationFile<T>(string file) where T : new()
        {
            var s = new XmlSerializer(typeof(T));

            var ret = new T();

            if (File.Exists(file))
                using (var fs = File.OpenRead(file))
                    ret = (T)s.Deserialize(fs);

            if (ret == null) throw new InvalidOperationException("Error while loading config file");

            using (var fs = File.OpenWrite(file))
                s.Serialize(fs, ret);
            return ret;
        }

        public Configuration()
        {
            ModulesDllPath = "Modules\\Modules.dll";
        }

        public string ModulesDllPath { get; set; }

        public static Configuration Get
        {
            get { return instance; }
        }
    }
}