//using System;
//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements {

//    public sealed class Molarity : Ratio<Molarity, ChemicalAmount, Volume>,
//        IMultipliableMeasurement<Volume, ChemicalAmount> {

//        public Molarity() {
//        }

//        public Molarity(double amount, Unit<Molarity> unit) : base(amount, unit) {
//        }

//        public Molarity(double amount, Unit<ChemicalAmount> chemAmountUnit, Unit<Volume> volumeUnit)
//            : base(amount, chemAmountUnit, volumeUnit) {
//        }

//        public Molarity(ChemicalAmount amount, Volume volume) : base(amount, volume) {
//        }

//        public static IMeasurementProvider<Molarity> Provider { get; } = new MolarityProvider();

//        public override IMeasurementProvider<Molarity> MeasurementProvider => Provider;

//        protected override IMeasurementProvider<Volume> DenominatorProvider => Volume.Provider;

//        protected override IMeasurementProvider<ChemicalAmount> NumeratorProvider => ChemicalAmount.Provider;

//        public static class Units {

//            public static Unit<Molarity> MolePerMeterCubed { get; } = new ChemicalAmount(1, ChemicalAmount.Units.Mole)
//                .Divide(new Volume(1, Volume.Units.MeterCubed))
//                .CreateUnit("mole per meter cubed", "mol/cm^3");

//            public static Unit<Molarity> MolePerMilliliter { get; } = new ChemicalAmount(1, ChemicalAmount.Units.Mole)
//                .Divide(new Volume(1, Volume.Units.Milliliter))
//                .CreateUnit("mole per milliliter", "mol/mL");

//            public static Unit<Molarity> Molar => MolePerMilliliter;
//        }

//        private class MolarityProvider : IMeasurementProvider<Molarity> {
//            public IEnumerable<Unit<Molarity>> AllUnits => new[] { Units.MolePerMeterCubed, Units.MolePerMilliliter };

//            public Unit<Molarity> DefaultUnit => Units.MolePerMeterCubed;

//            public Molarity CreateMeasurement(double value, Unit<Molarity> unit) {
//                return new Molarity(value, unit);
//            }
//        }
//    }
//}