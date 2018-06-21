using System;
using System.Drawing;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class RotateTest
    {
        private float angle = 90;
        private PointF rotatePoint = new PointF(1, 1);

        private PointF RotateFormula(PointF soursePoint, PointF rotatePoint, float angle)
        {
            var radians = angle * (float)Math.PI / 180.0f;
            return new PointF
            {
                X = rotatePoint.X + (soursePoint.X - rotatePoint.X) * (float)Math.Cos(radians) - (soursePoint.Y - rotatePoint.Y) * (float)Math.Sin(radians),
                Y = rotatePoint.Y + (soursePoint.Y - rotatePoint.Y) * (float)Math.Cos(radians) + (soursePoint.X - rotatePoint.X) * (float)Math.Sin(radians)
            };
        }

        bool NearEqual(PointF point1, PointF point2)
        {
            const float digit = 0.01f;
            return Math.Abs(point1.X - Math.Abs(point2.X)) <= digit && Math.Abs(Math.Abs(point1.Y) - Math.Abs(point2.Y)) <= digit;
        }

        [TestMethod]
        public void CorrectLineRotate()
        {
            Line shape = new Line(1, 0, 2, 0);
            shape.Transform(Transformation.Rotate(rotatePoint, angle));
            Assert.IsTrue(NearEqual(new PointF(2, 1), shape.StartPnt));
            Assert.IsTrue(NearEqual(new PointF(2, 2), shape.EndPnt));
        }

        [TestMethod]
        public void CorrectCircleRotate()
        {
            Circle shape = new Circle(1, 1, 1);
            shape.Transform(Transformation.Rotate(rotatePoint, angle));
            Assert.IsTrue(NearEqual(new PointF(1, 1), shape.Center));
        }

        [TestMethod]
        public void CorrectEllipseRotate()
        {
            Ellipse shape = new Ellipse(1, 1, 1, 1, 1);
            shape.Transform(Transformation.Rotate(rotatePoint, angle));
            Assert.IsTrue(NearEqual(new PointF(1, 1), shape.Center));
            Assert.AreEqual(91, shape.RotateAngle);
        }

        [TestMethod]
        public void CorrectCompoundShapeRotate()
        {
            var circle = new Circle(1, 1, 1);
            var line = new Line(1, 0, 2, 0);
            var shape = new CompoundShape
            {
                Shapes = { circle, line }
            };
            shape.Transform(Transformation.Rotate(rotatePoint, angle));
            Assert.IsTrue(NearEqual(new PointF(2, 1), line.StartPnt));
            Assert.IsTrue(NearEqual(new PointF(2, 2), line.EndPnt));
            Assert.IsTrue(NearEqual(new PointF(1, 1), circle.Center));
        }
    }
}