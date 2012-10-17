using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MHGameWork.TheWizards.DependencyInjection
{
    /// <summary>
    /// Responsible for holding dependency bindings for interfaces
    /// </summary>
    public class DependencyBindings
    {
        private Dictionary<Type, object> bindings = new Dictionary<Type, object>();

        /// <summary>
        /// T should be an interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SetBinding(Type t, object instance) 
        {
            bindings[t] = instance;
        }

        public object GetBinding(Type t) 
        {
            if (!bindings.ContainsKey(t))
                executeDefaultBindingMethod(t);
                

            if (!bindings.ContainsKey(t))
            {
                Console.WriteLine("Interface: '{0}' is not bound to the DependencyBindings, and no default binding found.", t.FullName);
                return null;
            }

            return bindings[t];
        }

        private void executeDefaultBindingMethod(Type interfaceType)
        {

            var methods = interfaceType.Assembly.GetTypes().SelectMany(t =>
                                                         t.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                             .Where(m => isBindingMethodForType(m, interfaceType)));

            if (methods.Count() == 0)
                return;

            if (methods.Count() > 1)
            {
                Console.WriteLine(
                    "Multiple default binding methods found for interface '{0}', can't use default binding!",
                    interfaceType);

                return;
            }


            methods.First().Invoke(null, null);

        }

        private bool isBindingMethodForType(MethodInfo m, Type interfaceType)
        {
            return m.GetCustomAttributes(typeof(DefaultBindingMethodAttribute), false).Where(att => ((DefaultBindingMethodAttribute)att).InterfaceType == interfaceType).Count() > 0;
        }

    }
}
