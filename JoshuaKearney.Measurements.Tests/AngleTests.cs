using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoshuaKearney.Measurements;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Volume.Units;
using static JoshuaKearney.Measurements.Area.Units;

namespace JoshuaKearney.Measurements.Tests {
    [TestClass]
    public class AngleTests {
        [TestMethod]
        public void TestInitialization() {
            Angle distance = new Angle();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullConstructor() {
            new Angle(4, null);
        }
    }
}
