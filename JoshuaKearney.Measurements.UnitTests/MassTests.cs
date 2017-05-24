using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JoshuaKearney.Measurements.UnitTests {
    public class MassTests {
        [Fact]
        public void TestInitialization() {
            var a = Mass.Provider.Zero;
            a = new Mass();

            Assert.NotNull(a.MeasurementProvider);

            var units = Mass.Provider.ParsableUnits.ToList();
        }
    }
}
