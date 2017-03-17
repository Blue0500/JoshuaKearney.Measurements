using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements {
    internal class ExtendedMeasurementProvider<T> : MeasurementProvider<T> where T : IMeasurement<T> {
        private Func<double, Unit<T>, T> createMeasurement;

        private Func<IEnumerable<Unit<T>>> getParsableUnits;

        private Func<IEnumerable<Operator>> getOperators;

        public ExtendedMeasurementProvider(Func<double, Unit<T>, T> createMeasurement, Func<IEnumerable<Unit<T>>> getParsableUnits, Func<IEnumerable<Operator>> getops) {
            Validate.NonNull(createMeasurement, nameof(createMeasurement));
            Validate.NonNull(getParsableUnits, nameof(getParsableUnits));
            Validate.NonNull(getops, nameof(getops));

            this.createMeasurement = createMeasurement;
            this.getParsableUnits = getParsableUnits;
            this.getOperators = getops;
        }

        public override T CreateMeasurement(double value, Unit<T> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.createMeasurement(value, unit);
        }

        public override IEnumerable<Unit<T>> ParsableUnits => this.getParsableUnits();

        public override IEnumerable<Operator> ParseOperators => this.getOperators();
    }

    internal class ExtendedComplexMeasurementProvider<T, TComp1, TComp2> : CompoundMeasurementProvider<T, TComp1, TComp2>
        where T : IMeasurement<T>
        where TComp1 : IMeasurement<TComp1>
        where TComp2 : IMeasurement<TComp2> {

        private Func<double, Unit<T>, T> createMeasurement;

        private Func<IEnumerable<Unit<T>>> getParsableUnits;

        private Func<IEnumerable<Operator>> getOperators;

        public ExtendedComplexMeasurementProvider(MeasurementProvider<TComp1> comp1Provider, MeasurementProvider<TComp2> comp2Provider, Func<double, Unit<T>, T> createMeasurement, Func<IEnumerable<Unit<T>>> getParsableUnits, Func<IEnumerable<Operator>> getops) {
            Validate.NonNull(comp1Provider, nameof(comp1Provider));
            Validate.NonNull(comp2Provider, nameof(comp2Provider));
            Validate.NonNull(createMeasurement, nameof(createMeasurement));
            Validate.NonNull(getParsableUnits, nameof(getParsableUnits));
            Validate.NonNull(getops, nameof(getops));

            this.createMeasurement = createMeasurement;
            this.getParsableUnits = getParsableUnits;
            this.Component1Provider = comp1Provider;
            this.Component2Provider = comp2Provider;
            this.getOperators = getops;
        }

        public override MeasurementProvider<TComp1> Component1Provider { get; }

        public override MeasurementProvider<TComp2> Component2Provider { get; }

        public override T CreateMeasurement(double value, Unit<T> unit) {
            Validate.NonNull(unit, nameof(unit));

            return this.createMeasurement(value, unit);
        }

        public override IEnumerable<Unit<T>> ParsableUnits => this.getParsableUnits();

        public override IEnumerable<Operator> ParseOperators => this.getOperators();
    }
}
