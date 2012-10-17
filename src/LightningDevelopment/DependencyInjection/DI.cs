using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MHGameWork.TheWizards.DependencyInjection;

namespace MHGameWork
{
    /// <summary>
    /// Static access point for accessing the dependency injection system
    /// </summary>
    public static class DI
    {
        public static DependencyBindings CurrentBindings = new DependencyBindings();


        /// <summary>
        /// This method binds an instance to give interface. The caller is passed to provide information about the object requesting the bind. (so always DI.Bind(this))
        /// </summary> 
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Bind<T>() where T : class
        {
            return CurrentBindings.GetBinding<T>();
        }
    }
}