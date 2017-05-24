using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JoshuaKearney.Measurements.UnitTests {
    public class DistanceTests {
        [Fact]
        public void TestInitialization() {
            Distance a = Distance.Provider.Zero;
            a = new Distance();

            Assert.NotNull(a.MeasurementProvider);

            var units = Distance.Provider.ParsableUnits.ToList();
        }
    }
}
