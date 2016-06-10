using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public interface IMeasurementTerm<T1, T2>
            where T1 : Measurement<T1>, new()
            where T2 : Measurement<T2>, new() {

        Measurement ToMeasurement();
    }
}