using System;
using System.Collections.Generic;

namespace LightningDevelopment
{
    public class CompositeActionsModule : IActionsModule
    {
        private List<IActionsModule> submodules = new List<IActionsModule>();

        public List<IActionsModule> Submodules
        {
            get { return submodules; }
        }

        public bool ContainsAction(string txt)
        {
            foreach (var module in Submodules)
            {
                if (module.ContainsAction(txt)) return true;
            }
            return false;
        }

        public void RunAction(string txt,string[] arguments )
        {
            foreach (var module in Submodules)
            {
                if (!module.ContainsAction(txt))
                    continue;
                module.RunAction(txt,arguments);
                return;

            }
            Console.WriteLine("Unable to run action, action not found: " + txt);
        }
    }
}