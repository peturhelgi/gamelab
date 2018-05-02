// from here: https://social.msdn.microsoft.com/Forums/vstudio/en-US/3b1b39be-3f0f-4f9f-8ee8-beaa4fce4981/sharing-one-random-object-across-entire-application?forum=csharpgeneral

using System;

namespace TheGreatEscape.Libs {
    class SingleRandom : Random
    {
        static SingleRandom _Instance;
        public static SingleRandom Instance
        {
            get
            {
                if (_Instance == null) _Instance = new SingleRandom();
                return _Instance;
            }
        }

        private SingleRandom() { }
    }
}
