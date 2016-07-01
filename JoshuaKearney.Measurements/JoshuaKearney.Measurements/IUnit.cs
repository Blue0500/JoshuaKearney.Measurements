using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public interface IPrefixableUnit : IUnit {
    }

    public interface IPrefixableUnit<T> : IUnit<T>, IPrefixableUnit where T : Measurement, new() {
    }

    public interface IUnit {
        Type AssociatedMeasurement { get; }
        string Name { get; }
        string Symbol { get; }
        double UnitsPerStored { get; }
    }

    public interface IUnit<T> : IUnit where T : Measurement, new() {
    }
}