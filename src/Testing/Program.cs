using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JoshuaKearney.Measurements.Parser;
using JoshuaKearney.Measurements;

namespace Testing {
    class Program {
        static void Main(string[] args) {
            Temperature temp = Temperature.FromCelcius(560);

            Console.WriteLine(temp.ToFahrenheit());
            Console.Read();
        }
    }
}
