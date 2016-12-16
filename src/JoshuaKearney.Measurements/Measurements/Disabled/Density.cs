//using System;
//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements {

//    public static partial class MeasurementExtensions {

//        public static Density ToDensity(this Ratio<Mass, Volume> density) {
//            Validate.NonNull(density, nameof(density));

//            return new Density(density.ToDouble(density.MeasurementSupplier.DefaultUnit), density.MeasurementSupplier.DefaultUnit.Cast<Ratio<Mass, Volume>, Density>());
//        }
//    }

//    public sealed class Density : Ratio<Density, Mass, Volume>,
//        IMultipliableMeasurement<Volume, Mass> {

//        public Density() {
//        }

//        public Density(Mass mass, Volume volume) : base(mass, volume) {
//        }

//        public Density(double amount, Unit<Density> unit) : base(amount, unit) {
//        }

//        public Density(double amount, Unit<Mass> massDef, Unit<Volume> volumeDef) : base(amount, massDef, volumeDef) {
//        }

//        public static IMeasurementSupplier<Density> Provider { get; } = new DensityProvider();

//        public override IMeasurementSupplier<Density> MeasurementSupplier => Provider;

//        protected override IMeasurementSupplier<Volume> DenominatorProvider => Volume.Provider;

//        protected override IMeasurementSupplier<Mass> NumeratorProvider => Mass.Provider;

//        public static class Units {
//            public static Unit<Density> GramPerCentimeterCubed { get; } = Mass.Units.Gram.Divide<Mass, Volume, Density>(Volume.Units.CentimeterCubed);

//            public static Unit<Density> KilogramPerLiter { get; } = Mass.Units.Kilogram.Divide<Mass, Volume, Density>(Volume.Units.Liter);

//            public static Unit<Density> KilogramPerMeterCubed { get; } = Mass.Units.Kilogram.Divide<Mass, Volume, Density>(Volume.Units.MeterCubed);

//            public static Unit<Density> MetricTonPerMeterCubed { get; } = Mass.Units.ShortTon.Divide<Mass, Volume, Density>(Volume.Units.MeterCubed);

//            public static Unit<Density> OuncePerInchCubed { get; } = Mass.Units.Ounce.Divide<Mass, Volume, Density>(Volume.Units.InchCubed);

//            public static Unit<Density> PoundPerFootCubed { get; } = Mass.Units.Pound.Divide<Mass, Volume, Density>(Volume.Units.FootCubed);
//        }

//        private class DensityProvider : IMeasurementSupplier<Density>, IComplexMeasurementSupplier<Mass, Volume> {
//            public IEnumerable<Unit<Density>> AllUnits => new Unit<Density>[] { };

//            public IMeasurementSupplier<Mass> Component1Provider => Mass.Provider;

//            public IMeasurementSupplier<Volume> Component2Provider => Volume.Provider;

//            public Unit<Density> DefaultUnit => Units.KilogramPerMeterCubed;

//            public Density CreateMeasurement(double value, Unit<Density> unit) {
//                return new Density(value, unit);
//            }
//        }
//    }
//}