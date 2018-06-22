using System.Drawing;
using DrawablesUI;

namespace GraphicsEditor
{
    public class Line : IShape
    {
        public PointF StartPnt { get; private set; }
        public PointF EndPnt { get; private set; }
        public string UID { get; set; }

        public Line(float xStart, float yStart, float xEnd, float yEnd, string uid)
        {
            var startPnt = new PointF
            {
                X = xStart,
                Y = yStart
            };

            var endPnt = new PointF
            {
                X = xEnd,
                Y = yEnd
            };

            UID = uid;
            StartPnt = startPnt;
            EndPnt = endPnt;
        }

        public void Draw(IDrawer drawer)
        {
            drawer.DrawLine(StartPnt, EndPnt);
        }

        public void Transform(Transformation trans)
        {
            StartPnt = trans[StartPnt];
            EndPnt = trans[EndPnt];
        }

        public override string ToString()
        {
            return $"Линия(Точка({StartPnt.X}, {StartPnt.Y}), Точка({EndPnt.X}, {EndPnt.Y}))";
        }
        
        public string ToSvg()
        {
            return $"<line x1=\"{StartPnt.X}\" y1=\"{StartPnt.Y}\" x2=\"{EndPnt.X}\" y2=\"{EndPnt.Y}\" " +
                   "style=\"stroke:black; stroke-width:1\" />";
        }
        
        public IShape Clone()
        {
            return (Line) MemberwiseClone();
        }
    }
}