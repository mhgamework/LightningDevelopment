using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modules.ContextModel
{
    /// <summary>
    /// Warning: this may cause deadlocks!!!??
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ContextualValue<T>
    {
        private T value;
        public T Get()
        {
            lock (this)
            {
                return value;

            }
        }
        public void Set(T setValue)
        {
            lock (this)
            {
                var old = value;
                value = setValue;
                if (Changed != null)
                    Changed.Invoke(this, old);
            }

        }

        public event Action<ContextualValue<T>, T> Changed;
    }
}
