using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class HexagonShape : Shapes
    {
        public HexagonShape()
        {
        }

        public HexagonShape(Color color, int x, int y, int edge)
        {
            X = x;
            Y = y;
            Edge = edge;
            _color = color;
        }

        public override void DrawIt(Graphics g)
        {
            SolidBrush brush = new SolidBrush(_color);


            int x_0 = X + (Edge / 2);
            int y_0 = Y + (Edge / 2);

            var shape = new PointF[6];

            int r = Edge / 2; //70 px radius 

            //Create 6 points
            for (int a = 0; a < 6; a++)
            {
                shape[a] = new PointF(
                    x_0 + r * (float)Math.Cos(a * 60 * Math.PI / 180f),
                    y_0 + r * (float)Math.Sin(a * 60 * Math.PI / 180f));
            }

            g.FillPolygon(brush, shape);

        }
    }
}
