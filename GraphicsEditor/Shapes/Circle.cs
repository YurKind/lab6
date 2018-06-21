using System;
using System.Drawing;
using DrawablesUI;

namespace GraphicsEditor
{
    public class Circle : IShape
    {
        public float Radius { get; private set; }
        public PointF Center { get; private set; }
        public SizeF Size { get; private set; }
        public float RotateAngle { get; private set; }
        public string UID { get; set; }


        public Circle(float x, float y, float radius, string uid)
        {
            ParameterParser.CheckRadius(radius);
            var center = new PointF
            {
                X = x,
                Y = y
            };

            var size = new SizeF
            {
                Height = radius * 2,
                Width = radius * 2
            };

            Center = center;
            Size = size;
            RotateAngle = 0;
            Radius = radius;
            UID = uid;
        }

        public override string ToString()
        {
            return $"Круг(Точка({Center.X}, {Center.Y}), Радиус={Radius})";
        }

        public void Draw(IDrawer drawer)
        {
            drawer.DrawEllipseArc(Center, Size);
        }

        public void Transform(Transformation trans)
        {
            Transformation.SingularValueDecompositoin svd = trans.SingularValueDecomposition;

            if (svd.Scale[0] == svd.Scale[1])
            {
                Center = trans[Center];
                Size = new SizeF
                {
                    Height = svd.Scale[0] * Size.Height,
                    Width = svd.Scale[1] * Size.Width
                };
                Radius = Size.Height / 2;
            }
            else
            {
                throw new NotImplementedException("Коэффициенты масштабирования круга по осям не равны");
            }
        }

        public string ToSvg()
        {
            return $"<circle cx=\"{Center.X}\" cy=\"{Center.Y}\" r=\"{Radius}\" " +
                   "stroke=\"black\" stroke-width=\"1\" fill=\"none\" />";
        }

        public IShape Clone()
        {
            return (Circle) MemberwiseClone();
        }
    }
}