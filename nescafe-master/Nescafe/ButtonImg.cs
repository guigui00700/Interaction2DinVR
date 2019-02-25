using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Nescafe
{
    class ButtonImg
    {
        public Point center;
        public int radius;
        public Color color;
        public Controller.Button type;

        public ButtonImg(Controller.Button type, Point center, int radius, Color color)
        {
            this.center = center;
            this.radius = radius;
            this.color = color;
            this.type = type;
        }
    }

    enum ButtonType
    {
        Start,
        Select,
        A,
        B,
        Up,
        Down,
        Right,
        Left,
        Empty
    }
}
