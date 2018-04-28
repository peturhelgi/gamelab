using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TheGreatEscape.GameLogic.Util
{
    // TODO: Find a better name for this class. "Debugger" is taken.
    public static class MyDebugger
    {
        public static bool IsActive;
        public static void WriteLine(object value)
        {
            if(IsActive)
            {
                Debug.WriteLine(value);
            }
        }
    }
}
