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
                                       catch (Exception)
                                       {

                                       }
                                       Thread.Sleep(200);
                                   }
                               }));
            t.IsBackground = true;
            t.Name = "FetchExplorerInfo";
            t.Start();


            foreach (var drive in Directory.GetLogicalDrives())
            {

                var watcher = new FileSystemWatcher();
                watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                if (!Directory.Exists(drive)) continue;
                watcher.Path = drive;
                watcher.EnableRaisingEvents = true;

            }





            t = new Thread(processChangedFileQueueThread);
            t.IsBackground = true;
            t.Start();



        }

        private Queue<string> changedFileQueue = new Queue<string>();
        private object changedFileLock = new object();

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            lock (changedFileLock)
            {
                changedFileQueue.Enqueue(e.FullPath);
                Monitor.Pulse(changedFileLock);
            }

        }

        private void processChangedFileQueueThread()
        {
            while (true)
            {
                string path;
                lock (changedFileLock)
                {
                    while (changedFileQueue.Count == 0) Monitor.Wait(changedFileLock);
                    path = changedFileQueue.Dequeue();
                }
                processFileChange(path);
            }

        }

        /// <summary>
        /// Meant to watch for user changes to files and then detect the active working project
        /// </summary>
        /// <param name="path"></param>
        private void processFileChange(string path)
        {
            if (path.StartsWith("C:\\Windows"))
                return;

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
