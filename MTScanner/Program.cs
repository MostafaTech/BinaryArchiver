using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MTScanner
{
    static class Program
    {
        ///<summary>
        ///Register the class as a control and set its CodeBase entry
        ///</summary>
        ///<param name="key">The registry key of the control</param>
        [ComRegisterFunction()]
        public static void RegisterClass(string key)
        {
            // runs with: regasm /codebase MyAssemblie.dll
        }

        ///<summary>
        ///Called to unregister the control
        ///</summary>
        ///<param name="key">The registry key</param>
        [ComUnregisterFunction()]
        public static void UnregisterClass(string key)
        {
            // runs with: regasm /u MyAssemblie.dll
        }
    }
}
