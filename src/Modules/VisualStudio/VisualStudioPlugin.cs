using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using LightningDevelopment;
using Modules;
using Modules.Core;
using Modules.WinAPI;
using SHDocVw;
using Process = System.Diagnostics.Process;
using Thread = System.Threading.Thread;

namespace MHGameWork.TheWizards.VSIntegration
{
    /// <summary>
    /// Responsible for tracking the active VS selected item
    /// </summary>
    public class VisualStudioPlugin : IPlugin
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(LightningDevelopmentHandle handle)
        {
            var t = new Thread(new ThreadStart(delegate
            {
                while (true)
                {
                    try
                    {
                        fetchVSState();
                    }
                    catch (Exception ex)
                    {
                        File.AppendAllText("log.txt", ex.ToString());
                    }
                    Thread.Sleep(200);
                }
            }));
            t.IsBackground = true;
            t.Name = "FetchVSState";
            t.Start();
        }

        private void fetchVSState()
        {
            Process process;
            if (!isDevenvForeground(out process)) return;

            var dte = VSHelper.GetByID(process.Id);

            if (solutionExplorerFocused(dte))
            {
                var selectedPathGuess = guessSelectedPathFromSolutionExplorer(dte);
                if (selectedPathGuess != null)
                {
                    var workingDir = Path.GetDirectoryName(selectedPathGuess);
                    CorePlugin.WorkingDirectory.Set(workingDir);
                }
            }
            else if (dte.ActiveDocument != null)
            {
                var workingDir = Path.GetDirectoryName(dte.ActiveDocument.FullName);
                CorePlugin.WorkingDirectory.Set(workingDir);
            }
            Console.WriteLine(CorePlugin.WorkingDirectory.Get());

            guessSelectedPathFromSolutionExplorer(dte);
        }

        private bool solutionExplorerFocused(DTE2 dte)
        {
            return dte.ActiveWindow.Caption == "Solution Explorer";
        }

        private static string guessSelectedPathFromSolutionExplorer(DTE2 dte)
        {
            UIHierarchyItem firstSelectedObject = null;

            foreach (UIHierarchyItem selectedItem in dte.ToolWindows.SolutionExplorer.SelectedItems)
            {
                firstSelectedObject = selectedItem;
                break;
            }
            if (firstSelectedObject == null) return null;

            string selectedPathGuess = "";

            for (int i = 0; i < 100; i++)
            {
                Solution sol;
                Project proj;
                ProjectItem item;
                if ((sol = firstSelectedObject.Object as Solution) != null)
                {
                    return sol.FullName;
                }
                if ((proj = firstSelectedObject.Object as Project) != null)
                {
                    return proj.FullName;
                }
                if ((item = firstSelectedObject.Object as ProjectItem) != null)
                {
                    return item.FileNames[0];
                }

                firstSelectedObject = firstSelectedObject.Collection.Parent;


            }
            // Silently abort
            Console.WriteLine("Seem to have infinite parents for solution explorer item");

            return "";
        }

        private static bool isDevenvForeground(out Process process)
        {
            process = getForegroundProcess();
            if (process.ProcessName != "devenv") return false;
            return true;
        }

        private static Process getForegroundProcess()
        {
            var foreground = User32.GetForegroundWindow();
            uint procId;
            User32.GetWindowThreadProcessId(foreground, out procId);

            var process = Process.GetProcessById((int)procId);
            return process;
        }
    }
}