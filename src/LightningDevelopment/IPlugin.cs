using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightningDevelopment
{
    public interface IPlugin : IDisposable
    {
        void Initialize(LightningDevelopmentHandle handle);

    }
}
