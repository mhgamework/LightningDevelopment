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
        public void SetBinding<T>(T instance) where T : class
        {
            bindings[typeof(T)] = instance;
        }

        public T GetBinding<T>() where T : class
        {
            if (!bindings.ContainsKey(typeof(T)))
                executeDefaultBindingMethod(typeof (T));
                

            if (!bindings.ContainsKey(typeof(T)))
            {
                Console.WriteLine("Interface: '{0}' is not bound to the DependencyBindings, and no default binding found.", typeof(T).FullName);
                return null;
            }

            return bindings[typeof(T)] as T;
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
