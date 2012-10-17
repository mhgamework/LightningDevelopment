using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.ContextModel;

namespace Modules.Core
{
    public class CorePlugin : IPlugin
    {
        public static ContextualValue<string> WorkingDirectory = new ContextualValue<string>();
        public static ContextualValue<string> WorkingFile = new ContextualValue<string>();

        private FullFileSystemWatcher watcher = new FullFileSystemWatcher();

        public void Dispose()
        {
            
        }

        public void Initialize(LightningDevelopmentHandle handle)
        {
            watcher.Init();
        }


        //TODO: add extension method to listen to file changes
    }
}
