using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public abstract class UnitDefinitionCollection<T> where T : Measurement<T>, new() {
        public abstract IEnumerable<UnitDefinition<T>> AllUnits { get; }
        public abstract UnitDefinition<T> StandardUnit { get; }
    }
}