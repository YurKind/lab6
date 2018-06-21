using System;
using System.Collections.Generic;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class GroupTest
    {
        private Picture picture;

        [TestInitialize]
        public void TestInit() => picture = new Picture();

        [TestMethod]
        public void GroupTwoShapes()
        {
            picture.Add(new Circle(1, 1, 1));
            picture.Add(new Line(1, 1, 2, 2));
            picture.Group(new string[] { "0", "1" });
            Assert.IsTrue(picture.shapes.Count == 1 && picture.shapes[0] is CompoundShape);
        }

        [TestMethod]
        public void GroupCoupleShapes()
        {
            picture.Add(new Circle(1, 1, 1));
            picture.Add(new Line(1, 1, 1, 1));
            picture.Add(new Ellipse(1, 1, 1, 1, 1));
            picture.Group(new string[] { "2", "0", "1" });
            Assert.IsTrue(picture.shapes.Count == 1 && picture.shapes[0] is CompoundShape);
        }

        [TestMethod]
        public void NoArguments()
        {
            Assert.ThrowsException<ArgumentException>(() => picture.Group(new string[] {  }));
        }

        [TestMethod]
        public void GroupOneShape()
        {
            picture.Add(new Circle(1, 1, 1));
            Assert.ThrowsException<ArgumentException>(() => picture.Group(new string[] { "0" }));
        }
        
        [TestMethod]
        public void NothingToGroup()
        {
            Assert.ThrowsException<ArgumentException>(() => picture.Group(new string[] { "0", "1" }));
        }

        [TestMethod]
        public void GroupTwoCompoundShapes()
        {
            picture.Add(new CompoundShape
            {
                Shapes = {new Circle(1, 1, 1), new Line(1, 1, 1, 1)}
            });
            picture.Add(new CompoundShape
            {
                Shapes = { new Ellipse(1, 1, 1, 1, 1), new Point(1, 1) }
            });
            picture.Group(new string[] { "0", "1" });
            Assert.IsTrue(picture.shapes.Count == 1 && picture.shapes[0] is CompoundShape);
        }

        [TestMethod]
        public void GroupInRandomOrder()
        {
            picture.Add(new Circle(1, 1, 1));
            picture.Add(new Circle(2, 2, 2));
            picture.Group(new string[] { "1", "0" });
            Assert.IsTrue(picture.shapes.Count == 1 && picture.shapes[0] is CompoundShape);
        }

        [TestMethod]
        public void GroupOneShapeTwoTimes()
        {
            picture.Add(new Circle(1, 1, 1));
            Assert.ThrowsException<ArgumentException>(() => picture.Group(new string[] { "0", "0" }));
        }
    }
}