using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using LightningDevelopment;
using Modules.Core;
using Modules.WinAPI;
using SHDocVw;

namespace Modules.WindowsExplorer
{
    /// <summary>
    /// Responsible fetching info from the active Windows Explorer
    /// </summary>
    public class ExplorerPlugin : IPlugin
    {


        public void Dispose()
        {
        }

        public void Initialize(LightningDevelopmentHandle handle)
        {

            var t = new Thread(new ThreadStart(delegate
                               {
                                   while (true)
                                   {
                                       try
                                       {
                                           fetchExplorerInfo();
                                       }
                                       catch (Exception ex)
                                       {
                                           File.AppendAllText("log.txt", ex.ToString());
                                       }
                                       Thread.Sleep(200);
                                   }
                               }));
            t.IsBackground = true;
            t.Name = "FetchExplorerInfo";
            t.Start();





        }

      

     

        private void fetchExplorerInfo()
        {
            var windows = new ShellWindows();
            var foreground = User32.GetForegroundWindow();
            foreach (InternetExplorer item in windows)
            {
                if (item.HWND != foreground.ToInt32()) continue;

                var workingDir = new Uri(item.LocationURL).LocalPath;
                if (Directory.Exists(workingDir)) CorePlugin.WorkingDirectory.Set(workingDir);
                var workingFile = item.Document.FocusedItem.Path;
                if (File.Exists(workingDir)) CorePlugin.WorkingFile.Set(workingDir);
            }


        }
    }
}
