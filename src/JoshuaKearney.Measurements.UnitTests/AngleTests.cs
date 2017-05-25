using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JoshuaKearney.Measurements.UnitTests {
    public class AngleTests {
        [Fact]
        public void TestInitialization() {
            Angle a = Angle.Provider.Zero;

            Assert.NotNull(a.MeasurementProvider);

            var units = Angle.Provider.ParsableUnits.ToList();
        }

        [Theory]
        [InlineData(4, 5, 9)]
        [InlineData(20, 100, 120)]
        public void TestAddition(double x, double y, double res) {
            Angle n = new Angle(x, Angle.Units.Radian).Add(new Angle(y, Angle.Units.Radian));
            Assert.Equal(res, Math.Round(n.ToDouble(Angle.Units.Radian), 3));

            n = new Angle(x, Angle.Units.Radian).Subtract(new Angle(-y, Angle.Units.Radian));
            Assert.Equal(res, Math.Round(n.ToDouble(Angle.Units.Radian), 3));
        }
    }
}
