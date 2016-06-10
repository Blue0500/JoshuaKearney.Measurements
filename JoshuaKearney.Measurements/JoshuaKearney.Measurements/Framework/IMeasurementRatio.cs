using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public interface IMeasurementRatio<TNumerator, TDenominator>
            where TNumerator : Measurement<TNumerator>, new()
            where TDenominator : Measurement<TDenominator>, new() {

        Measurement ToMeasurement();
    }
}