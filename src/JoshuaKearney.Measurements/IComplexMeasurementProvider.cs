using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public interface IComplexMeasurementProvider<T1, T2>
        where T1 : Measurement<T1>
        where T2 : Measurement<T2> {
        Lazy<IMeasurementProvider<T1>> Component1Provider { get; }

        Lazy<IMeasurementProvider<T2>> Component2Provider { get; }
    }
}