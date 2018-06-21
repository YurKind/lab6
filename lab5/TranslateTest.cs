using System.Drawing;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class TranslateTest
    {
        [TestMethod]
        public void CorrectCircleTranslate()
        {
            var shape = new Circle(1, 1, 1);
            shape.Transform(Transformation.Translate(new PointF(10, 10)));
            Assert.IsTrue(shape.Center.X == 11 && shape.Center.Y == 11);
        }

        [TestMethod]
        public void CorrectCompoundShapeTranslate()
        {
            var circle = new Circle(1, 1, 1);
            var line = new Line(1, 1, 1, 1);
            var shape = new CompoundShape
            {
                Shapes = { circle, line }
            };
            shape.Transform(Transformation.Translate(new PointF(10, 10)));
            Assert.IsTrue(circle.Center.X == 11 && circle.Center.Y == 11);
            Assert.IsTrue(line.StartPnt.X == 11 && line.StartPnt.Y == 11 && line.EndPnt.X == 11 && line.EndPnt.Y == 11);
        }

        [TestMethod]
        public void CorrectLineTranslate()
        {
            var shape = new Line(1, 1, 1, 1);
            shape.Transform(Transformation.Translate(new PointF(10, 10)));
            Assert.IsTrue(shape.StartPnt.X == 11 && shape.StartPnt.Y == 11 && shape.EndPnt.X == 11 && shape.EndPnt.Y == 11);
        }

        [TestMethod]
        public void CorrectEllipseTranslate()
        {
            var shape = new Ellipse(1, 1, 1, 1, 1);
            shape.Transform(Transformation.Translate(new PointF(10, 10)));
            Assert.IsTrue(shape.Center.X == 11 && shape.Center.Y == 11);
        }

        [TestMethod]
        public void CorrectPointTranslate()
        {
            var shape = new GraphicsEditor.Point(1, 1);
            shape.Transform(Transformation.Translate(new PointF(10, 10)));
            Assert.IsTrue(shape.Coordinates.X == 11 && shape.Coordinates.Y == 11);
        }
    }
}
