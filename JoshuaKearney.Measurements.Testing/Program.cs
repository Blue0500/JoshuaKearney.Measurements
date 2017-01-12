using JoshuaKearney.Measurements.JsonConverters;
using JoshuaKearney.Measurements.NewParser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static JoshuaKearney.Measurements.Distance.Units;
using static JoshuaKearney.Measurements.Mass.Units;
using static JoshuaKearney.Measurements.Volume.Units;

namespace JoshuaKearney.Measurements.Testing {
    public class Program {
        public static void Main(string[] args) {
            List<ParsingOperator> ops = new List<ParsingOperator>() {
                ParsingOperator.CreateMultiplication<Distance, Distance, Area>((x, y) => x.Multiply(y))
            };

            MeasurementParser<Area> parse = new MeasurementParser<Area>(Area.Provider, ops);

            Area result;
            Console.WriteLine(parse.TryParse("3m * 4ft", out result));
            Console.WriteLine(result);

            Console.Read();
        }
    }
}
