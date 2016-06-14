using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class MeasurementRatio<TNumerator, TDenominator> : Measurement<MeasurementRatio<TNumerator, TDenominator>>, IMeasurementRatio<TNumerator, TDenominator>
            where TNumerator : Measurement<TNumerator>, new()
            where TDenominator : Measurement<TDenominator>, new() {
        private static TNumerator numRef = new TNumerator();
        private static TDenominator denomRef = new TDenominator();

        public override UnitDefinitionCollection<MeasurementRatio<TNumerator, TDenominator>> UnitDefinitions { get; } = MeasurementRatio<TNumerator, TDenominator>.Units;

        private static RatioDefinitionCollection<TNumerator, TDenominator> Units { get; } = new RatioDefinitionCollection<TNumerator, TDenominator>();

        public MeasurementRatio() {
        }

        public MeasurementRatio(TNumerator num, TDenominator denom) {
            this.WithStandardUnits(num.ToStandardUnits() / denom.ToStandardUnits());
        }

        public override string ToString() {
            return $"{this.ToStandardUnits()} {numRef.StandardUnitDefinition.Symbol}/{denomRef.StandardUnitDefinition.Symbol}";
        }

        Measurement IMeasurementRatio<TNumerator, TDenominator>.ToMeasurement() {
            return this;
        }
    }

    public static class MeasurementRatio {

        public static MeasurementRatio<T1, T2> FromUnits<T1, T2>(T1 item1, T2 item2)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return new MeasurementRatio<T1, T2>(item1, item2);
        }
    }
}