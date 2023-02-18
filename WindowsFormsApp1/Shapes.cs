using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public abstract class Shapes
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Edge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Color _color { get; set; }

        public bool IsSelected { get; set; }




        public abstract void DrawIt(Graphics g);
    }
}
