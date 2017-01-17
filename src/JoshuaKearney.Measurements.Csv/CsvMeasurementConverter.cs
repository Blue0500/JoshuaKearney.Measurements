using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using JoshuaKearney.Measurements.Parser;

namespace JoshuaKearney.Measurements.CsvConverters {
    public abstract class CsvMeasurementConverter<T> : DefaultTypeConverter where T : Measurement<T> {
        public MeasurementParser<T> Parser { get; }

        public CsvMeasurementConverter(MeasurementProvider<T> provider) : this(provider, Enumerable.Empty<Operator>()) { }

        public CsvMeasurementConverter(MeasurementProvider<T> provider, IEnumerable<Operator> operators) {
            Validate.NonNull(provider, nameof(provider));
            Validate.NonNull(operators, nameof(operators));

            Parser = new MeasurementParser<T>(provider, operators);
        }

        public override bool CanConvertTo(Type type) {
            Validate.NonNull(type, nameof(type));

            return type == typeof(string);
        }

        public override string ConvertToString(TypeConverterOptions options, object text) {
            Validate.NonNull(options, nameof(options));
            Validate.NonNull(text, nameof(text));

            T measurement = text as T;

            if (measurement == null) {
                throw new ArgumentException($"Cannot convert the type '{text.GetType()}' to a string");
            }

            return measurement.ToString();
        }

        public override bool CanConvertFrom(Type type) {
            Validate.NonNull(type, nameof(type));

            return type == typeof(string);
        }

        public override object ConvertFromString(TypeConverterOptions options, string text) {
            Validate.NonNull(options, nameof(options));
            Validate.NonNull(text, nameof(text));

            return Parser.Parse(text);
        }
    }
}
