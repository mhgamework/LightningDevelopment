using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightningDevelopment
{
    public class LightningDevelopmentHandle
    {
        public List<IActionsModule> ProvidedModules { get; private set; }
        private List<LightningDevelopmentHandle> subhandles = new List<LightningDevelopmentHandle>(); 

        public LightningDevelopmentHandle()
        {
            ProvidedModules = new List<IActionsModule>();
        }

        public IActionsModule LoadActionsModule()
        {
            //TODO: forward provided handles from submodules or simplify design
            throw new NotImplementedException();
            /*var ret = ActionsModule.CreateFromDll(Configuration.Get.ModulesDllPath, this);
            actionsModule.Submodules.Add(ret);
            return ret;*/
        }

        public void UnloadActionsModule(IActionsModule mod)
        {
            throw new NotImplementedException();
            //actionsModule.Submodules.Remove(mod);
        }

    }
}
