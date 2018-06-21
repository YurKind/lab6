using System;
using System.Drawing;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class ScaleTest
    {
        [TestMethod]
        public void CorrectLineScale()
        {
            var shape = new Line(1, 2, 3, 4);
            shape.Transform(Transformation.Scale(new PointF(10, 10), 2f));
            Assert.IsTrue(shape.StartPnt.X == -8 && shape.StartPnt.Y == -6);
            Assert.IsTrue(shape.EndPnt.X == -4 && shape.EndPnt.Y == -2);
        }

        [TestMethod]
        public void CorrectCircleScale()
        {
            var shape = new Circle(1, 1, 1);
            shape.Transform(Transformation.Scale(new PointF(10, 10), 2f));
            Assert.AreEqual(2, shape.Radius);
            Assert.IsTrue(shape.Center.X == -8 && shape.Center.Y == -8);
        }

        [TestMethod]
        public void CorrectEllipseScale()
        {
            var shape = new Ellipse(2, 2, 2, 2, 0);
            shape.Transform(Transformation.Scale(new PointF(10, 10), 3f));
            Assert.IsTrue(shape.Size.Height == 6 && shape.Size.Width == 6);
            Assert.IsTrue(shape.Center.X == -14 && shape.Center.Y == -14);
        }

        [TestMethod]
        public void CorrectCompoundShapeScale()
        {
            var circle = new Circle(1, 1, 1);
            var ellipse = new Ellipse(2, 2, 2, 2, 0);
            var shape = new CompoundShape
            {
                Shapes = { circle, ellipse }
            };
            shape.Transform(Transformation.Scale(new PointF(10, 10), 2f));
            Assert.AreEqual(2, circle.Radius);
            Assert.IsTrue(circle.Center.X == -8 && circle.Center.Y == -8);
            Assert.IsTrue(ellipse.Size.Height == 4 && ellipse.Size.Width == 4);
            Assert.IsTrue(ellipse.Center.X == -6 && ellipse.Center.Y == -6);
        }
    }
}
