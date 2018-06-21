using System;
using System.Drawing;
using DrawablesUI;

namespace GraphicsEditor
{
    public class Ellipse : IShape
    {
        public PointF Center { get; private set; }
        public SizeF Size { get; private set; }
        public float RotateAngle { get; private set; }
        public string UID { get; set; }

        public Ellipse(float x, float y, float width, float height, float rotateAngle, string uid)
        {
            ParameterParser.CheckWidthAndHeight(width, height);
            Center = new PointF
            {
                X = x,
                Y = y
            };

            Size = new SizeF
            {
                Height = height,
                Width = width
            };

            RotateAngle = rotateAngle;
            UID = uid;
        }

        public void Draw(IDrawer drawer)
        {
            drawer.DrawEllipseArc(Center, Size, rotate: RotateAngle);
        }

        public void Transform(Transformation trans)
        {
            Transformation.SingularValueDecompositoin svd = trans.SingularValueDecomposition;
            var tolerance = Math.Pow(10, -7) * 5;
            if (Math.Abs(svd.Scale[0] - svd.Scale[1]) < tolerance)
            {
                Center = trans[Center];
                Size = new SizeF
                {
                    Height = svd.Scale[0] * Size.Height,
                    Width = svd.Scale[1] * Size.Width
                };
                RotateAngle += svd.FirstAngle;
            }
            else
            {
                throw new NotImplementedException("Коэффициенты масштабирования элипса по осям не равны");
            }
        }

        public override string ToString()
        {
            return $"Эллипс(Точка({Center.X}, {Center.Y}), Ширина={Size.Width}, Высота={Size.Height}, " +
                   $"Угол поворота={RotateAngle})";
        }

        public string ToSvg()
        {
            return $"<ellipse cx=\"{Center.X}\" cy=\"{Center.Y}\" rx=\"{Size.Width / 2}\" ry=\"{Size.Height / 2}\"\n " +
                   "style=\"fill:none; stroke:black; stroke-width:1\" " +
                   $"transform=\"rotate({RotateAngle} {Center.X} {Center.Y})\" />";
        }

        public IShape Clone()
        {
            return (Ellipse) MemberwiseClone();
        }
    }
}