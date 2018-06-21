using System.Drawing;
using DrawablesUI;

namespace GraphicsEditor
{
    public class Point : IShape
    {
        public PointF Coordinates { get; private set; }
        public string UID { get; set; }

        public Point(float x, float y, string uid)
        {
            var point = new PointF
            {
                X = x,
                Y = y
            };

            UID = uid;
            Coordinates = point;
        }

        public void Draw(IDrawer drawer)
        {
            drawer.DrawPoint(Coordinates);
        }

        public void Transform(Transformation trans)
        {
            Coordinates = trans[Coordinates];
        }

        public override string ToString()
        {
            return $"Точка({Coordinates.X}, {Coordinates.Y})";
        }

        public string ToSvg()
        {
            return $"<circle cx=\"{Coordinates.X}\" cy=\"{Coordinates.Y}\" r=\"1\" stroke-width=\"2\"/>";
        }

        public IShape Clone()
        {
            return (Point) MemberwiseClone();
        }
    }
}