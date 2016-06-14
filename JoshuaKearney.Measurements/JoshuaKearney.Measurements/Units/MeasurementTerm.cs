using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public sealed class MeasurementTerm<T1, T2> : Measurement<MeasurementTerm<T1, T2>>, IMeasurementTerm<T1, T2>
            where T1 : Measurement<T1>, new()
            where T2 : Measurement<T2>, new() {
        public override UnitDefinitionCollection<MeasurementTerm<T1, T2>> UnitDefinitions { get; } = MeasurementTerm<T1, T2>.Units;

        private static TermDefinitionCollection<T1, T2> Units { get; } = new TermDefinitionCollection<T1, T2>();

        private MeasurementTerm(double units) {
            this.StandardUnits = units;
        }

        public MeasurementTerm() {
        }

        public MeasurementTerm(T1 item1, T2 item2) {
            this.StandardUnits = item1.ToStandardUnits() * item2.ToStandardUnits();
        }

        Measurement IMeasurementTerm<T1, T2>.ToMeasurement() {
            return this;
        }
    }

    public static class MeasurementTerm {

        public static MeasurementTerm<T1, T2> FromUnits<T1, T2>(T1 item1, T2 item2)
                where T1 : Measurement<T1>, new()
                where T2 : Measurement<T2>, new() {
            return new MeasurementTerm<T1, T2>(item1, item2);
        }
    }
}