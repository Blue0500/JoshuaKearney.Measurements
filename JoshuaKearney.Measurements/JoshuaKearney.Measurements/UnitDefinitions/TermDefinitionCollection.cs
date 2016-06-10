using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class TermDefinitionCollection<T1, T2> : UnitDefinitionCollection<MeasurementTerm<T1, T2>>
            where T1 : Measurement<T1>, new()
            where T2 : Measurement<T2>, new() {
        public override IEnumerable<UnitDefinition<MeasurementTerm<T1, T2>>> AllUnits { get; }

        public TermDefinitionCollection() {
            this.AllUnits = new[] { this.StandardUnitDefinition };
        }

        public UnitDefinition<MeasurementTerm<T1, T2>> StandardUnitDefinition { get; } = new UnitDefinition<MeasurementTerm<T1, T2>>(
            new T1().StandardUnitDefinition.Symbol + "/" + new T2().StandardUnitDefinition.Symbol,
            ratio => {
                return new MeasurementTerm<T1, T2>().WithStandardUnits(ratio);
            },
            x => x.ToStandardUnits(),
            MeasurementSystem.Metric
        );

        public override UnitDefinition<MeasurementTerm<T1, T2>> StandardUnit {
            get {
                return this.StandardUnitDefinition;
            }
        }
    }
}