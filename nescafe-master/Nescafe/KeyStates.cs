using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nescafe
{
    class KeyStates
    {
        public bool A;
        public bool B;
        public bool Selete;
        public bool Start;
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        public KeyStates()
        {
            A = false;
            B = false;
            Start = false;
            Selete = false;
            Up = false;
            Down = false;
            Left = false;
            Right = false;
        }

        public int Length()
        {
            return 8;
        }
    }
}
