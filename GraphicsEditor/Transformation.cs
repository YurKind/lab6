using System;
using System.Drawing;

namespace GraphicsEditor
{
    public class Transformation
    {
        private float[,] matrix;

        public Transformation()
        {
            matrix = new float[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
        }
        
        public static Transformation Rotate(float angle)
        {
            var rotate = new Transformation();

            var radians = angle * (float)Math.PI / 180.0f;

            rotate.matrix[0, 0] = (float)Math.Cos(radians);
            rotate.matrix[0, 1] = (float)-Math.Sin(radians);
            rotate.matrix[1, 0] = (float)Math.Sin(radians);
            rotate.matrix[1, 1] = (float)Math.Cos(radians);

            return rotate;
        }
        
        public static Transformation Rotate(PointF point, float angle)
        {
            var rotate = new Transformation();

            rotate *= Translate(new PointF(-point.X, -point.Y));
            rotate *= Rotate(angle);
            rotate *= Translate(point);

            return rotate;
        }

        public static Transformation Translate(PointF point)
        {
            var translate = new Transformation();

            translate.matrix[0, 2] = point.X;
            translate.matrix[1, 2] = point.Y;

            return translate;
        }

        public static Transformation Scale(float scaleFactor)
        {
            var scale = new Transformation();

            scale.matrix[0, 0] = scaleFactor;
            scale.matrix[1, 1] = scaleFactor;

            return scale;
        }

        public static Transformation Scale(PointF point, float scaleFactor)
        {
            var scale = new Transformation();

            scale *= Translate(new PointF(-point.X, -point.Y));
            scale *= Scale(scaleFactor);
            scale *= Translate(point);

            return scale;
        }

        public static Transformation FromMatrix(float[,] matrix)
        {
            return new Transformation {matrix = matrix};
        }

        public static Transformation operator *(Transformation a, Transformation b)
        {
            var result = new Transformation();

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    result.matrix[i, j] = b.matrix[i, 0] * a.matrix[0, j]
                                          + b.matrix[i, 1] * a.matrix[1, j]
                                          + b.matrix[i, 2] * a.matrix[2, j];
                }
            }
            return result;
        }
        
        public PointF this[PointF point]
        {
            get
            {
                var pointImage = new PointF
                {
                    X = matrix[0, 0] * point.X + matrix[0, 1] * point.Y + matrix[0, 2],
                    Y = matrix[1, 0] * point.X + matrix[1, 1] * point.Y + matrix[1, 2]
                };


                return pointImage;
            }
        }
        
        public SingularValueDecompositoin SingularValueDecomposition => new SingularValueDecompositoin(matrix);

        public class SingularValueDecompositoin
        {
            public SingularValueDecompositoin(float[,] matrix)
            {
                var tolerance = Math.Pow(10, -7);
                Scale = new float[2];
                var a = (float) 0.5 * (
                              (float) Math.Pow(matrix[0, 0], 2) +
                              (float) Math.Pow(matrix[0, 1], 2) +
                              (float) Math.Pow(matrix[1, 0], 2) +
                              (float) Math.Pow(matrix[1, 1], 2));
                var d = matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
                if (Math.Abs(a - d) < tolerance)
                {
                    Scale[0] = (float) Math.Sqrt(a);
                    Scale[1] = Scale[0];
                    FirstAngle = (float) (Math.Acos(matrix[0, 0] / Scale[1]) / Math.PI * 180);
                    SecondAngle = 0;
                }
                else if (Math.Abs(a - (-d)) < tolerance)
                {
                    Scale[0] = (float) Math.Sqrt(a);
                    Scale[1] = -Scale[0];
                    FirstAngle = -(float) (Math.Acos(matrix[0, 0] / Scale[1]) * 180 / Math.PI);
                    SecondAngle = 0;
                }
                else
                {
                    Scale[0] = (float) Math.Sqrt((a + d) / 2) + (float) Math.Sqrt((a - d) / 2);
                    Scale[1] = (float) Math.Sqrt((a + d) / 2) - (float) Math.Sqrt((a - d) / 2);
                    float gammaPlusAlpha =
                        (float) (Math.Acos((matrix[0, 0] + matrix[1, 1]) / (Scale[1] + Scale[2])) / Math.PI * 180);
                    float gammaMinusAlpha =
                        (float) (Math.Acos((matrix[0, 0] - matrix[1, 1]) / (Scale[1] - Scale[2])) / Math.PI * 180);
                    SecondAngle = (gammaPlusAlpha + gammaMinusAlpha) / 2;
                    FirstAngle = gammaPlusAlpha - SecondAngle;
                }
            }

            public float FirstAngle { get; }
            public float[] Scale { get; }
            public float SecondAngle { get; }
        }
    }
}