using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class RatioDefinitionCollection<T1, T2> : UnitDefinitionCollection<MeasurementRatio<T1, T2>>
            where T1 : Measurement<T1>, new()
            where T2 : Measurement<T2>, new() {
        public override IEnumerable<UnitDefinition<MeasurementRatio<T1, T2>>> AllUnits { get; }

        public RatioDefinitionCollection() {
            this.AllUnits = new[] { this.StandardUnitDefinition };
        }

        public UnitDefinition<MeasurementRatio<T1, T2>> StandardUnitDefinition { get; } = new UnitDefinition<MeasurementRatio<T1, T2>>(
            new T1().StandardUnitDefinition.Symbol + "/" + new T2().StandardUnitDefinition.Symbol,
            ratio => new MeasurementRatio<T1, T2>().WithStandardUnits(ratio),
            x => x.ToStandardUnits(),
            MeasurementSystem.Metric
        );

        public override UnitDefinition<MeasurementRatio<T1, T2>> StandardUnit {
            get {
                return this.StandardUnitDefinition;
            }
        }
    }
}