using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    internal class ExtendedMeasurementProvider<T> : MeasurementProvider<T> where T : Measurement<T> {
        private Func<double, Unit<T>, T> createMeasurement;

        private Func<IEnumerable<Unit<T>>> getParsableUnits;

        public ExtendedMeasurementProvider(Func<double, Unit<T>, T> createMeasurement, Func<IEnumerable<Unit<T>>> getParsableUnits) {
            this.createMeasurement = createMeasurement;
            this.getParsableUnits = getParsableUnits;
        }

        public override T CreateMeasurement(double value, Unit<T> unit) => this.createMeasurement(value, unit);

        protected override IEnumerable<Unit<T>> GetParsableUnits() => this.getParsableUnits();
    }

    internal class ExtendedComplexMeasurementProvider<T, TComp1, TComp2> : CompoundMeasurementProvider<T, TComp1, TComp2>
        where T : Measurement<T>
        where TComp1 : Measurement<TComp1>
        where TComp2 : Measurement<TComp2> {

        private Func<double, Unit<T>, T> createMeasurement;

        private Func<IEnumerable<Unit<T>>> getParsableUnits;

        public ExtendedComplexMeasurementProvider(MeasurementProvider<TComp1> comp1Provider, MeasurementProvider<TComp2> comp2Provider, Func<double, Unit<T>, T> createMeasurement, Func<IEnumerable<Unit<T>>> getParsableUnits) {
            this.createMeasurement = createMeasurement;
            this.getParsableUnits = getParsableUnits;
            this.Component1Provider = comp1Provider;
            this.Component2Provider = comp2Provider;
        }

        public override MeasurementProvider<TComp1> Component1Provider { get; }

        public override MeasurementProvider<TComp2> Component2Provider { get; }

        public override T CreateMeasurement(double value, Unit<T> unit) => this.createMeasurement(value, unit);

        protected override IEnumerable<Unit<T>> GetParsableUnits() => this.getParsableUnits();
    }
}
