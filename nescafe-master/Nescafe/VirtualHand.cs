using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Nescafe
{
    class VirtualHand
    {
        private Point pos;
        private bool locked;

        public VirtualHand()
        {
            pos = new Point(-1, -1);
            locked = false;
        }

        public Controller.Button TouchButton(ButtonImg[] buttons)
        {
            if(pos.x == -1 && pos.y == -1)
            {
                return Controller.Button.Empty;
            }
            foreach (ButtonImg b in buttons)
            {
                if (Math.Pow(pos.x - b.center.x, 2) + Math.Pow(pos.y - b.center.y, 2) <= Math.Pow(b.radius, 2))
                {
                    return b.type;
                }
            }
            return Controller.Button.Empty;
        }

        public void UpdatePos(Point newPoint)
        {
            //TODO
            if (locked)
            {
                return;
            }
            else
            {
                this.pos = newPoint;
            }
        }



        public void LockPos()
        {
            this.locked = true;
        }
        public void UnlockPos()
        {
            this.locked = false;
        }
        public Point GetPos()
        {
            return this.pos;
        }
    }

}
