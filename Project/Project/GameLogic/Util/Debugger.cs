using System.Diagnostics;

namespace TheGreatEscape.GameLogic.Util
{
    // TODO: Find a better name for this class. "Debugger" is taken.
    public static class MyDebugger
    {
        public static bool IsActive;
        public static void WriteLine(object value, bool forcePrint = false)
        {
            if(forcePrint || IsActive)
            {
                Debug.WriteLine(value);
            }
        }
    }
}
