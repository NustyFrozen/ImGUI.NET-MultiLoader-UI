using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Design_imGUINET
{
    static class Imports
    {
        [DllImport("user32.dll")]
       public static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);
    }
}
