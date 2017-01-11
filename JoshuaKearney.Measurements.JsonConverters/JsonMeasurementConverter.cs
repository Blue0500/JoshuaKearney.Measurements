using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.JsonConverters {
    public abstract class JsonMeasurementConverter<T> : JsonConverter where T : Measurement<T> {
        public static MeasurementParser<T> Parser { get; private set; }

        public JsonMeasurementConverter(Lazy<IMeasurementProvider<T>> provider) {
            if (Parser == null) {
                Parser = new MeasurementParser<T>(provider);
            }
        }

        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(T);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            T measurement = value as T;

            if (measurement == null) {
                throw new InvalidOperationException("The type to be serialized to json must be a " + typeof(T));
            }
            else {
                writer.WriteValue(measurement.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if (!this.CanConvert(objectType)) {
                throw new InvalidOperationException("The type to be deserialized from json must be a " + typeof(T));
            }

            string parse = reader.ReadAsString();
            return Parser.Parse(parse);
        }
    }
}
