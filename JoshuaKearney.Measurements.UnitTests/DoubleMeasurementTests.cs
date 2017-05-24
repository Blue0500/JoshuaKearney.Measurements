using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JoshuaKearney.Measurements.UnitTests {
    public class DoubleMeasurementTests {
        [Fact]
        public void TestInitialization() {
            var a = DoubleMeasurement.Provider.Zero;
            a = new DoubleMeasurement();
            a = new DoubleMeasurement(45);

            Assert.NotNull(a.MeasurementProvider);

            var units = DoubleMeasurement.Provider.ParsableUnits.ToList();
        }

        [Fact]
        public static void TestMeasurementMultiplication() {
            Random rand = new Random();

            for (int i = 1; i < 1000; i++) {
                DoubleMeasurement j = rand.NextDouble() * 1000;

                foreach (var unit in Distance.Provider.ParsableUnits) {
                    double d = j.Multiply(new Distance(i, unit)).Divide(new Distance(i, unit));
                    Assert.Equal(Math.Round(d, 5), Math.Round(j.ToDouble(), 5));
                }
            }
        }

        [Fact]
        public static void TestDoubleMultiplication() {
            Random rand = new Random();

            for (int i = 1; i < 1000; i++) {
                double j = rand.NextDouble() * i * 1000;

                Assert.Equal(j * i, new DoubleMeasurement(j).Multiply(i).ToDouble());
                Assert.Equal(j / i, new DoubleMeasurement(j).Divide(i).ToDouble());
            }
        }

        [Fact]
        public static void TestReciprocal() {
            Random rand = new Random();

            for (int i = 1; i < 1000; i++) {
                double j = rand.NextDouble() * i * 1000;
                Assert.Equal(1d / j, new DoubleMeasurement(j).Reciprocal().ToDouble());
            }
        }

        [Fact]
        public static void TestImplicitOperator() {
            Random rand = new Random();

            for (int i = 1; i < 1000; i++) {
                double j = rand.NextDouble() * 1000 * i;
                DoubleMeasurement meas = j;
                double newJ = meas;

                Assert.Equal(j, newJ);
            }
        }
    }
}