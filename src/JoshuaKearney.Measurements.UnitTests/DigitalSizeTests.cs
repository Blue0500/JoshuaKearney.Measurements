using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JoshuaKearney.Measurements.UnitTests {
    public class DigitalSizeTests {
        [Fact]
        public void TestInitialization() {
            var a = DigitalSize.Provider.Zero;

            Assert.NotNull(a.MeasurementProvider);

            var units = DigitalSize.Provider.ParsableUnits.ToList();
        }
    }
}