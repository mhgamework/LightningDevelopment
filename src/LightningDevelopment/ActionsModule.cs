using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MHGameWork;

namespace LightningDevelopment
{
    public class ActionsModule : IActionsModule
    {
        private Dictionary<string, IQuickAction> actions = new Dictionary<string, IQuickAction>();
        private LightningDevelopmentHandle handle;

        public ActionsModule(LightningDevelopmentHandle lightningDevelopmentHandle)
        {
            handle = lightningDevelopmentHandle;
        }


        public static ActionsModule CreateFromProject(string projectFile, string outputFile, LightningDevelopmentHandle lightningDevelopmentHandle)
        {
            var engine = new Microsoft.Build.Evaluation.Project(projectFile);
            engine.Build();

            return CreateFromDll(outputFile, lightningDevelopmentHandle);
        }
        public static ActionsModule CreateFromDll(string dll, LightningDevelopmentHandle lightningDevelopmentHandle)
        {
            var ret = new ActionsModule(lightningDevelopmentHandle);
            ret.loadAssembly(Assembly.LoadFrom(dll));
            return ret;
        }

        private void loadAssembly(Assembly modulesAssembly)
        {
            var actionTypes = listQuickActions(modulesAssembly);

            foreach (var pluginType in listPlugins(modulesAssembly))
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);
                plugin.Initialize(handle);
                DI.CurrentBindings.SetBinding(pluginType, plugin);
            }

            foreach (var type in actionTypes)
            {
                var obj = (IQuickAction)Activator.CreateInstance(type);
                actions.Add(obj.Command, obj);
            }
        }

        private IEnumerable<Type> listQuickActions(Assembly ass)
        {
            return ass.GetTypes().Where(t => t.GetInterfaces().Count(i => i == typeof(IQuickAction)) != 0);
        }
        private IEnumerable<Type> listPlugins(Assembly ass)
        {
            return ass.GetTypes().Where(t => t.GetInterfaces().Count(i => i == typeof(IPlugin)) != 0);
        }

        public bool ContainsAction(string txt)
        {
            return actions.ContainsKey(txt);
        }

        public void RunAction(string txt)
        {
            actions[txt].Execute();
        }
    }
}