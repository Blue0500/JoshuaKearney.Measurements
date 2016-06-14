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
            this.AllUnits = this.GetAllDefinitions();
        }

        private IEnumerable<UnitDefinition<MeasurementTerm<T1, T2>>> GetAllDefinitions() {
            yield return this.StandardUnitDefinition;

            var all = new T1().UnitDefinitions.AllUnits.ToList();

            foreach (var unit in new T1().UnitDefinitions.AllUnits) {
                foreach (var unit2 in new T2().UnitDefinitions.AllUnits) {
                    string symbol = unit.Symbol + "*" + unit2.Symbol;
                    yield return new UnitDefinition<MeasurementTerm<T1, T2>>() {
                        Symbol = unit.Symbol + "*" + unit2.Symbol,
                        FromStandardUnits = ratio => ratio.ToUnits(unit, unit2),
                        ToStandardUnits = x => {
                            var ret = new MeasurementTerm<T1, T2>();
                            ret.SetUnits(x, unit, unit2);
                            return ret;
                        },
                        MeasurementSystem = (unit.MeasurementSystem == unit2.MeasurementSystem) ? unit.MeasurementSystem : MeasurementSystem.Metric
                    };
                }
            }
        }

        public UnitDefinition<MeasurementTerm<T1, T2>> StandardUnitDefinition { get; } = new UnitDefinition<MeasurementTerm<T1, T2>>(
            new T1().StandardUnitDefinition.Symbol + "*" + new T2().StandardUnitDefinition.Symbol,
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