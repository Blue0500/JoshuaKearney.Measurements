using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    internal static class Validate {

        public static void NonNull<T>(T obj, string paramName) {
            if (obj == null) {
                throw new ArgumentNullException(paramName, $"Argument '{paramName}' cannot be null. ");
            }
        }
    }
}