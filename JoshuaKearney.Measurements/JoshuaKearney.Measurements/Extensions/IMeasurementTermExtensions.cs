using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public static class IMeasurementTermExtensions {

        public static IMeasurementTerm<T1, T2> SetUnits<T1, T2>(this IMeasurementTerm<T1, T2> term, double amount, UnitDefinition<T1> item1Unit, UnitDefinition<T2> item2Unit)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            term.ToMeasurement().SetStandardUnits(amount / item1Unit.FromStandardUnits(new T1().WithStandardUnits(1)) / item2Unit.FromStandardUnits(new T2().WithStandardUnits(1)));
            return term;
        }

        public static Area ToArea(this IMeasurementTerm<Length, Length> area) {
            Area a = area as Area;
            if (a != null) {
                return a;
            }
            else {
                return new Area().WithStandardUnits(area.ToMeasurement().ToStandardUnits());
            }
        }

        public static string ToString<T1, T2>(this IMeasurementTerm<T1, T2> term, UnitDefinition<T1> item1Unit, UnitDefinition<T2> item2Unit)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return term.ToUnits(item1Unit, item2Unit).ToString() + " " + item1Unit.Symbol + "*" + item2Unit.Symbol;
        }

        public static double ToUnits<T1, T2>(this IMeasurementTerm<T1, T2> term, UnitDefinition<T1> item1Unit, UnitDefinition<T2> item2Unit)
                                        where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return item2Unit.FromStandardUnits(new T2().WithStandardUnits(item1Unit.FromStandardUnits(new T1().WithStandardUnits(term.ToMeasurement().ToStandardUnits()))));
        }

        public static MeasurementTerm<T1, T2> ToUnitTerm<T1, T2>(this IMeasurementTerm<T1, T2> term)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return new MeasurementTerm<T1, T2>().WithStandardUnits(term.ToMeasurement().ToStandardUnits());
        }

        public static Volume ToVolume(this IMeasurementTerm<Area, Length> volume) {
            Volume v = volume as Volume;
            if (v != null) {
                return v;
            }
            else {
                return new Volume().WithStandardUnits(volume.ToMeasurement().ToStandardUnits());
            }
        }

        public static Volume ToVolume(this IMeasurementTerm<Length, Area> volume) {
            return new Volume().WithStandardUnits(volume.ToMeasurement().ToStandardUnits());
        }
    }
}