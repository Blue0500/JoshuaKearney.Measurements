using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JoshuaKearney.Measurements.UnitTests {
    public class VolumeTests {
        [Fact]
        public void TestInitialization() {
            var a = Volume.Provider.Zero;
            a = new Volume();

            Assert.NotNull(a.MeasurementProvider);

            var units = Volume.Provider.ParsableUnits.ToList();
        }
    }
}
