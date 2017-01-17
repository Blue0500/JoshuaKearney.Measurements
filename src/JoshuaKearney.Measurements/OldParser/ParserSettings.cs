using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.OldParser {
    public class ParserSettings {
        private Dictionary<Type, IEnumerable<object>> allUnits = new Dictionary<Type, IEnumerable<object>>();

        public void AddUnits<T>(params T[] units) where T : Measurement<T> {
            Validate.NonNull(units, nameof(units));
            Validate.NonEmpty(units, nameof(units));

            Type t = typeof(T);

            if (!allUnits.ContainsKey(t)) {
                allUnits[t] = units;
            }
            else {
                allUnits[t] = allUnits[t].Concat(units);
            }
        }
    }
}