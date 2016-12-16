//using System;
//using System.Collections.Generic;

//namespace JoshuaKearney.Measurements {

//    public class Force : Term<Force, Mass, Acceleration>,
//        IMultipliableMeasurement<Distance, Energy>,
//        IDividableMeasurement<Acceleration, Mass>,
//        IDividableMeasurement<Area, Pressure>,
//        IDividableMeasurement<Mass, Acceleration> {

//        public Force() {
//        }

//        public Force(double amount, Unit<Force> unit) : base(amount, unit) {
//        }

//        public Force(Mass mass, Acceleration acceleration) : base(mass, acceleration) {
//        }

//        public Force(double amount, Unit<Mass> massDef, Unit<Acceleration> accelDef) : base(amount, massDef, accelDef) {
//        }

//        public static IMeasurementSupplier<Force> Provider { get; } = new ForceProvider();

//        public override IMeasurementSupplier<Force> MeasurementSupplier => Provider;

//        public static class Units {
//            private static Unit<Force> newton = new Unit<Force>("newton", "N", 1);

//            public static Unit<Force> PoundForce { get; } = new Mass(1, Mass.Units.Pound).Multiply(Acceleration.Gravity).CreateUnit("pound-force", "lbf");

//            public static Unit<Force> Newton => newton;
//        }

//        protected override IMeasurementSupplier<Mass> Item1Provider => Mass.Provider;

//        protected override IMeasurementSupplier<Acceleration> Item2Provider => Acceleration.Provider;

//        public static Acceleration operator /(Force force, Mass mass) {
//            if (force == null || mass == null) {
//                return null;
//            }
//            else {
//                return force.Divide(mass);
//            }
//        }

//        public Acceleration Divide(Mass measurement2) {
//            Validate.NonNull(measurement2, nameof(measurement2));

//            return this.DivideToSecond(measurement2);
//        }

//        public Pressure Divide(Area measurement2) {
//            Validate.NonNull(measurement2, nameof(measurement2));

//            return new Pressure(this, measurement2);
//        }

//        public Energy Multiply(Distance measurement2) {
//            Validate.NonNull(measurement2, nameof(measurement2));

//            return new Energy(this, measurement2);
//        }

//        private class ForceProvider : IMeasurementSupplier<Force>, IComplexMeasurementSupplier<Mass, Acceleration> {
//            public IEnumerable<Unit<Force>> AllUnits { get; } = new[] { Units.Newton, Units.PoundForce };

//            public IMeasurementSupplier<Mass> Component1Provider => Mass.Provider;

//            public IMeasurementSupplier<Acceleration> Component2Provider => Acceleration.Provider;

//            public Unit<Force> DefaultUnit => Units.Newton;

//            public Force CreateMeasurement(double value, Unit<Force> unit) => new Force(value, unit);
//        }
//    }
//}