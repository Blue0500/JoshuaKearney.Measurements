using JoshuaKearney.Measurements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Testing {

    public class Program {

        public static void Main(string[] args) {
            XmlSerializer serial = new XmlSerializer(typeof(MeasurementTerm<Mass, Area>));
            MeasurementTerm<Mass, Area> some = new MeasurementTerm<Mass, Area>(Mass.FromGrams(5), Area.FromCentimetersSquared(1));

            //File.WriteAllText("some.xml", serial.SerializeToString(some));

            // some = serial.DeserializeFromString<MeasurementTerm<Mass, Area>>(File.ReadAllText("some.xml"));

            Console.WriteLine(serial.SerializeToString<MeasurementTerm<Mass, Area>>(some));

            Console.Read();
        }
    }
}