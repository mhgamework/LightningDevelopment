using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightningDevelopment
{
    /// <summary>
    /// A quick action that the user can type in the box to execute
    /// </summary>
    public interface IQuickAction
    {
        string Command { get; }
        void Execute();
    }
}
