using System;
using System.Collections.Generic;
using GraphicsEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lab5
{
    [TestClass]
    public class UngroupTest
    {
        private Picture picture;

        [TestInitialize]
        public void TestInit() => picture = new Picture();

        [TestMethod]
        public void CorrectUngrouping()
        {
            picture.Add(new CompoundShape
            {
                Shapes = { new Circle(1, 1, 1), new Line(1, 1, 1, 1) }
            });
            picture.Ungroup(new string[] { "0" });
            Assert.IsFalse(picture.shapes.Exists(shape => shape is CompoundShape));
            Assert.IsTrue(picture.shapes.Exists(shape => shape is Circle));
            Assert.IsTrue(picture.shapes.Exists(shape => shape is Line));
            Assert.AreEqual(2, picture.shapes.Count);
        }

        [TestMethod]
        public void NothingToUngroup()
        {
            Assert.ThrowsException<ArgumentException>(() => picture.Ungroup(new string[] { "0" }));
        }

        [TestMethod]
        public void UngroupUncompaundShape()
        {
            picture.Add(new Circle(1, 1, 1));
            Assert.ThrowsException<ArgumentException>(() => picture.Ungroup(new string[] { "0" }));
        }
    }
}