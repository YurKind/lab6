using System;
using System.Drawing;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class SVDTest
    {
        [TestMethod]
        public void Scale()
        {
            var trans = Transformation.Scale(new PointF(10, 10), 2);

            Assert.AreEqual(0, trans.SingularValueDecomposition.FirstAngle);
            Assert.AreEqual(0, trans.SingularValueDecomposition.SecondAngle);
            Assert.AreEqual(2, trans.SingularValueDecomposition.Scale[0]);
            Assert.AreEqual(2, trans.SingularValueDecomposition.Scale[1]);
        }

        [TestMethod]
        public void Rotate()
        {
            var trans = Transformation.Rotate(30);

            Assert.AreEqual(30.0000019, Math.Round(trans.SingularValueDecomposition.FirstAngle, 7));
            Assert.AreEqual(0, trans.SingularValueDecomposition.SecondAngle);
            Assert.AreEqual(1, trans.SingularValueDecomposition.Scale[0]);
            Assert.AreEqual(1, trans.SingularValueDecomposition.Scale[1]);
        }

        [TestMethod]
        public void Translate()
        {
            var trans = Transformation.Translate(new PointF(10, 10));

            Assert.AreEqual(0, trans.SingularValueDecomposition.FirstAngle);
            Assert.AreEqual(0, trans.SingularValueDecomposition.SecondAngle);
            Assert.AreEqual(1, trans.SingularValueDecomposition.Scale[0]);
            Assert.AreEqual(1, trans.SingularValueDecomposition.Scale[1]);
        }

        [TestMethod]
        public void DoubleRotate()
        {
            var trans = Transformation.Rotate(30) * Transformation.Rotate(45);

            Assert.AreEqual(75, trans.SingularValueDecomposition.FirstAngle);
            Assert.AreEqual(0, trans.SingularValueDecomposition.SecondAngle);
            Assert.AreEqual(0.99999994, Math.Round(trans.SingularValueDecomposition.Scale[0], 8));
            Assert.AreEqual(0.99999994, Math.Round(trans.SingularValueDecomposition.Scale[1], 8));
        }

        [TestMethod]
        public void ScaleAndRotate()
        {
            var trans = Transformation.Rotate(30) * Transformation.Scale(new PointF(10, 10), 4);

            Assert.AreEqual(30.0000019, Math.Round(trans.SingularValueDecomposition.FirstAngle, 7));
            Assert.AreEqual(0, trans.SingularValueDecomposition.SecondAngle);
            Assert.AreEqual(4, trans.SingularValueDecomposition.Scale[0]);
            Assert.AreEqual(4, trans.SingularValueDecomposition.Scale[1]);
        }
    }
}