﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JoshuaKearney.Measurements {

//    public sealed class Power : Ratio<Power, Energy, Time>,
//        IMultipliableMeasurement<Time, Energy> {
//        public IMeasurementProvider<Power> Provider { get; } = new PowerProvider();

//        public override IMeasurementProvider<Power> MeasurementProvider => Provider;

//        protected override IMeasurementProvider<Energy> NumeratorProvider => Energy.Provider;

//        protected override IMeasurementProvider<Time> DenominatorProvider => Time.Provider;

//        public Power() {
//        }

//        public Power(double amount, Unit<Power> unit) : base(amount, unit) {
//        }

//        public Power(Energy energy, Time time) : base(energy, time) {
//        }

//        public Power(double amount, Unit<Energy> energyUnit, Unit<Time> timeUnit) : base(amount, energyUnit, timeUnit) {
//        }

//        public static class Units {
//            public static Unit<Power> Watt { get; } = new Unit<Power>("Watt", "W", 1);
//        }

//        private class PowerProvider : IMeasurementProvider<Power> {
//            public IEnumerable<Unit<Power>> AllUnits => new[] { Units.Watt };

//            public Unit<Power> DefaultUnit => Units.Watt;

//            public Power CreateMeasurement(double value, Unit<Power> unit) => new Power(value, unit);
//        }
//    }
//}