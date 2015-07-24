using LightningDevelopment;

namespace Modules.ModuleHotloading
{
    /// <summary>
    /// This plugin will try and detect if the user is working in a context where there are additional 
    /// lightningdevelopment plugins available, and load these plugins
    /// Currently this works by detecting a git root, and finding a dll with specific name in this root
    /// </summary>
    public class ModuleHotloadingPlugin :IPlugin
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(LightningDevelopmentHandle handle)
        {
            
        }
    }
}