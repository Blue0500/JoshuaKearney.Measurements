using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace JoshuaKearney.Measurements.CsvConverters {
    public abstract class CsvMeasurementConverter<T> : DefaultTypeConverter where T : Measurement<T> {
        public static MeasurementParser<T> Parser { get; private set; } = null;

        public CsvMeasurementConverter(MeasurementProvider<T> provider) {
            if (Parser == null) {
                Parser = new MeasurementParser<T>(provider);
            }
        }

        public override bool CanConvertFrom(Type type) {
            return type == typeof(string);
        }

        public override object ConvertFromString(TypeConverterOptions options, string text) {
            return Parser.Parse(text);
        }
    }
}
