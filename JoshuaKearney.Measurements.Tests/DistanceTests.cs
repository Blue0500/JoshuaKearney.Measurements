using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoshuaKearney.Measurements;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Volume.Units;
using static JoshuaKearney.Measurements.Area.Units;

namespace JoshuaKearney.Measurements.Tests {
    [TestClass]
    public class DistanceTests {
        [TestMethod]
        public void TestInitialization() {
            Distance distance = new Distance();
        }

        [TestMethod]
        public void TestMathAccruacy() {
            Assert.AreEqual(35, Math.Round(Meter.Cube().ToDouble(FootCubed)));
            Assert.AreEqual(254, Math.Round(Centimeter.Multiply(Inch).ToDouble(MillimeterSquared)));
            Assert.AreEqual(84951, Math.Round(Yard.Multiply(FootSquared).ToDouble(CentimeterCubed)));
            Assert.AreEqual(11, Math.Round(Meter.Square().ToDouble(FootSquared)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullConstructor() {
            new Distance(4, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullCube() {
            ((Distance)null).Cube();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullMultiplyDistance() {
            ((Distance)null).Multiply((Distance)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullMultiplyArea() {
            ((Distance)null).Multiply((Area)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullSquare() {
            ((Distance)null).Square();
        }
    }
}
