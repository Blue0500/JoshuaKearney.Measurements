using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class Energy : Term<Energy, Force, Distance>,
        IDividableMeasurement<Charge, ElectricPotential>,
        IDividableMeasurement<Time, Power> {

        public Energy() : base() {
        }

        public Energy(double amount, Unit<Energy> unit)
            : base(amount, unit) {
        }

        public Energy(double amount, Unit<Force> forceUnit, Unit<Distance> distanceUnit)
            : base(amount, forceUnit, distanceUnit) {
        }

        public Energy(Force f, Distance d)
            : base(f, d) {
        }

        public Energy(double amount, Unit<Pressure> pressureUnit, Unit<Volume> volumeUnit)
            : this(new Pressure(amount, pressureUnit), new Volume(1, volumeUnit)) {
        }

        public Energy(Pressure pressure, Volume volume)
            : this(pressure.Multiply(volume.Divide(new Distance(1, Distance.Units.Meter))), new Distance(1, Distance.Units.Meter)) {
        }

        public Energy(double amount, Unit<Power> powerUnit, Unit<Time> timeUnit)
            : this(amount, powerUnit.Multiply<Power, Time, Energy>(timeUnit)) {
        }

        public Energy(Power power, Time time)
            : base(power.DefaultUnits * time.DefaultUnits, power.MeasurementProvider.DefaultUnit.Multiply<Power, Time, Energy>(time.MeasurementProvider.DefaultUnit)) {
        }

        public Energy(double amount, Unit<Charge> chargeUnit, Unit<ElectricPotential> elecPotentialUnit) : this() { }

        public static IMeasurementProvider<Energy> Provider { get; } = new EnergyProvider();

        public override IMeasurementProvider<Energy> MeasurementProvider => Provider;

        protected override IMeasurementProvider<Force> Item1Provider => Force.Provider;

        protected override IMeasurementProvider<Distance> Item2Provider => Distance.Provider;

        public ElectricPotential Divide(Charge measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return new ElectricPotential(this, measurement2);
        }

        public Power Divide(Time measurement2) {
            Validate.NonNull(measurement2, nameof(measurement2));

            return new Power(this, measurement2);
        }

        public static class Units {
            public static PrefixableUnit<Energy> Calorie { get; } = new PrefixableUnit<Energy>("calorie", "cal", 1 / 1.184d);

            public static Unit<Energy> Joule { get; } = new Unit<Energy>("Joule", "J", 1);

            public static Unit<Energy> Kilocalorie { get; } = Prefix.Kilo(Calorie);
        }

        private class EnergyProvider : IMeasurementProvider<Energy> {
            public IEnumerable<Unit<Energy>> AllUnits => new[] { Units.Joule };

            public Unit<Energy> DefaultUnit => Units.Joule;

            public Energy CreateMeasurement(double value, Unit<Energy> unit) => new Energy(value, unit);
        }
    }
}