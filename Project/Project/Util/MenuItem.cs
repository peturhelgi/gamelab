using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Util
{
    public class MenuItem
    {
        public string LinkType; // classname
        public string LinkId;   // e.g. "screen", "menu"
        public string Link;     // the name to the json file 
        public Image Image;     // Image displayed as the menuItem
    }
}
