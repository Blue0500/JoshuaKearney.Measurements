using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class MeasurementInfo {

        public MeasurementInfo(Func<double, Measurement> instanceSupplier, IUnit storedUnit, Lazy<IEnumerable<IUnit>> uniqueUnits) {
            Validate.NonNull(instanceSupplier, nameof(instanceSupplier));
            Validate.NonNull(storedUnit, nameof(uniqueUnits));

            this.CreateInstance = instanceSupplier;
            this.StoredUnitDefinition = storedUnit;
            this.UniqueUnits = uniqueUnits;
        }

        public Func<double, Measurement> CreateInstance { get; }
        public IUnit StoredUnitDefinition { get; }
        public Lazy<IEnumerable<IUnit>> UniqueUnits { get; }
    }
}