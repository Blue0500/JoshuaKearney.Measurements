using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Pressure : RatioBase<Pressure, Force, Area> {

        public Pressure() {
        }

        public Pressure(Force f, Area a) : base(f, a) {
        }

        public Pressure(double amount, Unit<Pressure> unit) : base(amount, unit) {
        }

        public Pressure(double amount, Unit<Force> forceUnit, Unit<Area> areaUnit) : base(amount, forceUnit, areaUnit) {
        }

        public static IMeasurementProvider<Pressure> Provider { get; } = new PressureProvider();

        public override IMeasurementProvider<Pressure> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Area> DenominatorProvider => Area.Provider;

        protected override IMeasurementProvider<Force> NumeratorProvider => Force.Provider;

        public static class Units {
            private static PrefixableUnit<Pressure> pascal = new PrefixableUnit<Pressure>("Pascal", "Pa", 1);

            public static PrefixableUnit<Pressure> Pascal => pascal;

            //public static Unit<Pressure> MillimeterOfMercury { get; } = Pascal.Multiply(133.322387415).CreateUnit("millimeter of mercury", "mmHg");

            //public static Unit<Pressure> Atmosphere { get; } = Pascal.Multiply(101325).CreateUnit("atmosphere", "atm");

            //public static Unit<Pressure> Bar { get; } = Pascal.Multiply(100000).CreateUnit("bar", "bar");

            //public static Unit<Pressure> Kilopascal { get; } = Prefix.Kilo(Pascal);

            //public static Unit<Pressure> PoundsPerSquareInch { get; } =
            //    new Force(1, Force.Units.PoundForce)
            //    .Divide(new Area(1, Area.Units.InchSquared))
            //    .CreateUnit("Pounds per Square Inch", "lb/in²");

            //  public static Unit<Pressure> Torr { get; } = Atmosphere.Divide(760).CreateUnit("torr", "torr");
        }

        private class PressureProvider : IMeasurementProvider<Pressure>, IComplexMeasurementProvider<Force, Area> {
            public IEnumerable<Unit<Pressure>> BaseUnits { get; } = new[] { Units.Pascal };//, Units.Bar, Units.Atmosphere, Units.Torr, Units.PoundsPerSquareInch };

            public IMeasurementProvider<Force> Component1Provider => Force.Provider;

            public IMeasurementProvider<Area> Component2Provider => Area.Provider;

            public Unit<Pressure> DefaultUnit => Units.Pascal;

            public Pressure CreateMeasurement(double value, Unit<Pressure> unit) {
                return new Pressure(value, unit);
            }
        }
    }
}