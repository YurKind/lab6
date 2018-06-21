using System;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class DrawTest
    {
        private Picture picture;

        [TestInitialize]
        public void TestInit() => picture = new Picture();

        [TestMethod]
        public void DrawPoint()
        {
            picture.Add(new Point(1, 1));
            Assert.IsTrue(picture.shapes[0] is Point);
        }

        [TestMethod]
        public void DrawLine()
        {
            picture.Add(new Line(1, 1, 1, 1));
            Assert.IsTrue(picture.shapes[0] is Line);
        }

        [TestMethod]
        public void DrawEllipse()
        {
            picture.Add(new Ellipse(1, 1, 1, 1, 90));
            Assert.IsTrue(picture.shapes[0] is Ellipse);
        }

        [TestMethod]
        public void DrawCircle()
        {
            picture.Add(new Circle(1, 1, 1));
            Assert.IsTrue(picture.shapes[0] is Circle);
        }

        [TestMethod]
        public void DrawCompoundShape()
        {
            picture.Add(new CompoundShape {
                Shapes = { new Line(1, 1, 2, 2), new Circle(1, 1, 1) }
            });
            Assert.IsTrue(picture.shapes[0] is CompoundShape);
        }

        [TestMethod]
        public void DrawCoupleShapes()
        {
            picture.Add(new Circle(1, 1, 1));
            picture.Add(new Ellipse(1, 1, 1, 1, 90));
            picture.Add(new Line(1, 1, 1, 1));
            Assert.AreEqual(3, picture.shapes.Count);
        }

        [TestMethod]
        public void DontDrawCircleWithIncorrectRadius()
        {
            Assert.ThrowsException<ArgumentException>(() => picture.Add(new Circle(1, 1, -1)));
        }

        [TestMethod]
        public void IncorrectEllipseWidth()
        {
            Assert.ThrowsException<ArgumentException>(() => picture.Add(new Ellipse(1, 1, -1, 1, 1)));
        }
    }
}