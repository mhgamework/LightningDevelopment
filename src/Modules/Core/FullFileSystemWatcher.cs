using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Modules.Core
{
    public class FullFileSystemWatcher
    {
        public void Init()
        {

            foreach (var drive in Directory.GetLogicalDrives())
            {

                var watcher = new FileSystemWatcher();
                watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                if (!Directory.Exists(drive)) continue;
                watcher.Path = drive;
                watcher.EnableRaisingEvents = true;

            }





            var t = new Thread(processChangedFileQueueThread);
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
    }
}
