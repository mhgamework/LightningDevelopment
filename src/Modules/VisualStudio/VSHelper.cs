using System;
using EnvDTE80;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MHGameWork.TheWizards.VSIntegration
{
    public class VSHelper
    {

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved,
                                                 out IBindCtx ppbc);
        [DllImport("ole32.dll")]
        private static extern void GetRunningObjectTable(int reserved,
          out IRunningObjectTable prot);

        public static DTE2 GetByID(int ID)
        {
            //rot entry for visual studio running under current process.
            string rotEntry = String.Format("!VisualStudio.DTE.10.0:{0}", ID);
            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);
            IEnumMoniker enumMoniker;
            rot.EnumRunning(out enumMoniker);
            enumMoniker.Reset();
            IntPtr fetched = IntPtr.Zero;
            IMoniker[] moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                CreateBindCtx(0, out bindCtx);
                string displayName;
                moniker[0].GetDisplayName(bindCtx, null, out displayName);
                if (displayName.StartsWith("!VisualStudio.DTE.", StringComparison.OrdinalIgnoreCase) &&
                        displayName.EndsWith(ID.ToString()))
                {
                    object comObject;
                    rot.GetObject(moniker[0], out comObject);
                    return (EnvDTE80.DTE2)comObject;
                }
            }
            return null;
        }
    }
}