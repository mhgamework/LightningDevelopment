using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightningDevelopment;
using Modules.Core;

namespace Tools.Tools
{
    public class CodeMetrics : IQuickAction
    {
        public string Command
        {
            get { return "codemetrics"; }
        }

        public void Execute(string[] arguments)
        {
            var lineCount = 0;
            var fileCount = 0;
            foreach (var file in Directory.EnumerateFiles(Directory.GetParent(CorePlugin.WorkingDirectory.Get()).FullName, "*.cs", SearchOption.AllDirectories))
            {
                if (file.Contains("Deprecated")) continue;
                if (file.Contains("_Libraries")) continue;
                if (file.Contains("ColladaSchema")) continue;
                if (file.Contains("_Source\\bin")) continue;
                Console.WriteLine(file);
                fileCount++;

                //if (fileCount % 30 == 0)
                //    Console.ReadLine();

                lineCount += File.ReadAllLines(file).Length;
            }
            Console.WriteLine("Linecount: " + lineCount);
            Console.WriteLine("Filecount: " + fileCount);

            Console.ReadLine();
        }
    }
}
