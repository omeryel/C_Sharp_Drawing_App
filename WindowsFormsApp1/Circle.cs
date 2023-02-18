using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace WindowsFormsApp1
{
    public class CircleShape : Shapes
    {
        public CircleShape()
        {
        }

        public CircleShape(Color color, int x, int y, int edge)
        {
            X = x;
            Y = y;
            Edge = edge;
            _color = color;
        }

        public override void DrawIt(Graphics g)
        {
            SolidBrush brush = new SolidBrush(_color);
            Rectangle rec = new Rectangle(X, Y, Edge, Edge);
            g.FillEllipse(brush, rec);

        }
    }
}
