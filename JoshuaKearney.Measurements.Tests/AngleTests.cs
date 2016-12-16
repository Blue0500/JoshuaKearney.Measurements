using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoshuaKearney.Measurements;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Volume.Units;
using static JoshuaKearney.Measurements.Area.Units;
using System.Collections.Generic;
using System.Collections;

namespace JoshuaKearney.Measurements.Tests {
    class A : IEnumerable<A>, IEnumerable<string> {
        public IEnumerator<A> GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }

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
