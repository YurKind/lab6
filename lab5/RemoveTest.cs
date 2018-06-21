using System;
using System.Collections.Generic;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class RemoveTest
    {
        private Picture picture;

        [TestInitialize]
        public void TestInit() => picture = new Picture();

        [TestMethod]
        public void SmokeTest()
        {
            new Point(0, 0);
        }

        [TestMethod]
        public void CheckEmpty()
        {
            Assert.AreEqual(0, picture.shapes.Count);
        }

        [TestMethod]
        public void RemoveOneElement()
        {
            Line line = new Line(1, 1, 1, 1);
            picture.Add(line);
            picture.Remove(line);
            Assert.AreEqual(0, picture.shapes.Count);
        }

        [TestMethod]
        public void RemoveNotExisting()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => picture.RemoveAt(1));
        }

        [TestMethod]
        public void CorrectRemove()
        {
            picture.Add(new Line(1, 1, 1, 1));
            picture.Add(new Circle(1, 1, 1));
            picture.RemoveAt(1);
            Assert.IsTrue(picture.shapes[0] is Line);
        }

        [TestMethod]
        public void RemoveInRandomOrder()
        {
            picture.Add(new Line(1, 1, 1, 1));
            picture.Add(new Circle(1, 1, 1));
            picture.RemoveAt(1);
            picture.RemoveAt(0);
            Assert.AreEqual(0, picture.shapes.Count);
        }
    }
}
