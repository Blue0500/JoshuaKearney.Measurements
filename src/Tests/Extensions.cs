using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Testing {

    public static class Extensions {

        public static string SerializeToString<T>(this XmlSerializer serial, T obj) {
            using (StringWriter writer = new StringWriter()) {
                serial.Serialize(writer, obj);

                writer.Flush();

                return writer.ToString();
            }
        }

        public static T DeserializeFromString<T>(this XmlSerializer serial, string toSerialize) {
            using (StringWriter writer = new StringWriter()) {
                writer.Flush();

                return (T)serial.Deserialize(new StringReader(toSerialize));
            }
        }
    }
}