using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JoshuaKearney.Measurements {

    public static class IUnitRatioExtensions {

        public static IMeasurementRatio<TNumerator, TDenominator> SetUnits<TNumerator, TDenominator>(
            this IMeasurementRatio<TNumerator, TDenominator> term, double amount, UnitDefinition<TNumerator> numUnit, UnitDefinition<TDenominator> denomUnit
        )
               where TNumerator : Measurement<TNumerator>, new()
               where TDenominator : Measurement<TDenominator>, new() {
            term.ToMeasurement().SetStandardUnits(numUnit.ToStandardUnits(amount).ToStandardUnits() / denomUnit.ToStandardUnits(1).ToStandardUnits());
            return term;
        }

        public static Density ToDensity(this IMeasurementRatio<Mass, Volume> density) {
            Density d = density as Density;
            if (d != null) {
                return d;
            }
            else {
                return new Density().WithStandardUnits(density.ToMeasurement().ToStandardUnits());
            }
        }

        public static string ToString<T1, T2>(this IMeasurementRatio<T1, T2> ratio, UnitDefinition<T1> numUnit, UnitDefinition<T2> denomUnits)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return ratio.ToUnits(numUnit, denomUnits).ToString() + " " + numUnit.Symbol + "/" + denomUnits.Symbol;
        }

        public static MeasurementRatio<T1, T2> ToUnitRatio<T1, T2>(this IMeasurementRatio<T1, T2> term)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return new MeasurementRatio<T1, T2>().WithStandardUnits(term.ToMeasurement().ToStandardUnits());
        }

        public static double ToUnits<TNumerator, TDenominator>(this IMeasurementRatio<TNumerator, TDenominator> ratio, UnitDefinition<TNumerator> numUnit, UnitDefinition<TDenominator> denomUnit)
                where TNumerator : Measurement<TNumerator>, new()
                where TDenominator : Measurement<TDenominator>, new() {
            return numUnit.FromStandardUnits(new TNumerator().WithStandardUnits(ratio.ToMeasurement().ToStandardUnits())) / denomUnit.FromStandardUnits(new TDenominator().WithStandardUnits(1));
        }
    }
}