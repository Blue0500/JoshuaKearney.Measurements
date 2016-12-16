//using System;
//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements {

//    public sealed class ChemicalAmount : Measurement<ChemicalAmount>,
//        IDividableMeasurement<Volume, Molarity> {

//        private static Lazy<Ratio<DoubleMeasurement, ChemicalAmount>> avagadroConstant = new Lazy<Ratio<DoubleMeasurement, ChemicalAmount>>(() => new Ratio<DoubleMeasurement, ChemicalAmount>(
//            6.02214085774e23,
//            new ChemicalAmount(1, Units.Mole)
//        ));

//        public static Ratio<DoubleMeasurement, ChemicalAmount> AvagadroConstant => avagadroConstant.Value;

//        public static IMeasurementSupplier<ChemicalAmount> Provider { get; } = new ChemicalSubstanceProvider();

//        public override IMeasurementSupplier<ChemicalAmount> MeasurementSupplier => Provider;

//        public ChemicalAmount() {
//        }

//        public ChemicalAmount(double amount, Unit<ChemicalAmount> unit) : base(amount, unit) {
//        }

//        public static class Units {
//            public static Unit<ChemicalAmount> Mole { get; } = new Unit<ChemicalAmount>("Mole", "mol", 1);

//            public static Unit<ChemicalAmount> RepresentativeParticle { get; } = new Unit<ChemicalAmount>("Rep. Particle", "rep. part.", 6.02214085774e23);
//        }

//        private class ChemicalSubstanceProvider : IMeasurementSupplier<ChemicalAmount> {
//            public IEnumerable<Unit<ChemicalAmount>> AllUnits => new[] { Units.Mole, Units.RepresentativeParticle };

//            public Unit<ChemicalAmount> DefaultUnit => Units.Mole;

//            public ChemicalAmount CreateMeasurement(double value, Unit<ChemicalAmount> unit) {
//                return new ChemicalAmount(value, unit);
//            }
//        }

//        public Molarity Divide(Volume measurement2) {
//            return new Molarity(this, measurement2);
//        }
//    }
//}