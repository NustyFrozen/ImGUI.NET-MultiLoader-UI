using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design_imGUINET
{
    internal class Monitor
    {
        static float delay = 0;
        public static void monitorWithDelay(string message)
        {
            if(delay >= 1) delay = 0;
            if(delay == 0)
            {
                Console.WriteLine(message);
            }
            delay += 0.001f;
        }
    }
}
