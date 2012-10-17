using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MHGameWork.TheWizards.DependencyInjection
{
    /// <summary>
    /// This attribute can be applied on a static method, together with the interface the method creates a binding for.
    /// When no binding is found for a specific interface, the method with the defaultbindingattribute for this type is called.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class DefaultBindingMethodAttribute : Attribute
    {
        public Type InterfaceType { get; private set; }

        public DefaultBindingMethodAttribute(Type interfaceType)
        {
            this.InterfaceType = interfaceType;
        }
    }
}
