using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JoshuaKearney.Measurements {

    public abstract class Measurement {
        protected double StandardUnits { get; set; } = 0;

        internal void SetStandardUnits(double value) {
            this.StandardUnits = value;
        }

        internal double ToStandardUnits() {
            return this.StandardUnits;
        }

        internal Measurement() {
        }
    }

    public abstract class Measurement<T> : Measurement, IEquatable<T>, IComparable<T>, IXmlSerializable where T : Measurement<T>, new() {

        public Measurement() {
        }

        public abstract UnitDefinitionCollection<T> UnitDefinitions { get; }

        public UnitDefinition<T> StandardUnitDefinition {
            get {
                return this.UnitDefinitions.StandardUnit;
            }
        }

        public static T FromUnit(double amount, UnitDefinition<T> unit) {
            if (unit == null) {
                throw new ArgumentNullException();
            }

            return unit.ToStandardUnits(amount);
        }

        public static T operator -(Measurement<T> mass, Measurement<T> mass2) {
            return mass.Subtract(mass2);
        }

        public static T operator *(Measurement<T> mass, double factor) {
            return new T().WithStandardUnits(mass.StandardUnits * factor);
        }

        public static T operator *(double factor, Measurement<T> mass) {
            return new T().WithStandardUnits(mass.StandardUnits * factor);
        }

        public static T operator /(Measurement<T> mass, double factor) {
            return new T().WithStandardUnits(mass.StandardUnits / factor);
        }

        public static T operator +(Measurement<T> mass, Measurement<T> mass2) {
            return mass.Add(mass2);
        }

        public T Add(Measurement<T> other) {
            return new T().WithStandardUnits(this.StandardUnits + other.StandardUnits);
        }

        public int CompareTo(T other) {
            return this.StandardUnits.CompareTo(other.StandardUnits);
        }

        public bool Equals(T other) {
            return this.StandardUnits == other.StandardUnits;
        }

        public override bool Equals(object obj) {
            T len = obj as T;

            if (len == null) {
                return false;
            }
            else {
                return this.Equals(len);
            }
        }

        public override int GetHashCode() {
            return this.StandardUnits.GetHashCode();
        }

        XmlSchema IXmlSerializable.GetSchema() {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader) {
            reader.MoveToContent();
            string symbol = reader.GetAttribute("units");
            double value = reader.ReadElementContentAsDouble();

            var list = this.UnitDefinitions.AllUnits.ToList();
            var def = list.Where(x => x.Symbol == symbol).FirstOrDefault();
            if (def != null) {
                this.StandardUnits = def.ToStandardUnits(value).StandardUnits;
            }
            else {
                throw new InvalidOperationException($"The unit '{symbol}' is not a recognized unit");
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer) {
            writer.WriteAttributeString("units", this.StandardUnitDefinition.Symbol);
            writer.WriteRaw(this.StandardUnits.ToString());
        }

        //public virtual bool WithParseResult(string toParse) {
        //    try {
        //        string[] split = toParse.Split(' ');
        //        double value = double.Parse(split[0]);
        //        string symbol = split[1];

        //        var def = this.UnitDefinitions.AllUnits.Where(x => x.Symbol == symbol).FirstOrDefault();
        //        if (def != null) {
        //            this.StandardUnits = def.ToStandardUnits(value).StandardUnits;
        //        }
        //        else {
        //            def = this.UnitDefinitions.AllUnits.Where(x => x.Symbol.ToLower() == symbol.ToLower()).FirstOrDefault();
        //            if (def != null) {
        //                this.StandardUnits = def.ToStandardUnits(value).StandardUnits;
        //            }
        //            else {
        //                return false;
        //            }
        //        }

        //        return true;
        //    }
        //    catch {
        //        return false;
        //    }
        //}

        internal T WithStandardUnits(double standardUnits) {
            this.StandardUnits = standardUnits;
            return (T)this;
        }

        public T Subtract(Measurement<T> other) {
            return new T().WithStandardUnits(this.StandardUnits - other.StandardUnits);
        }

        public override string ToString() {
            return this.ToString(MeasurementSystem.Metric);
        }

        public string ToString(MeasurementSystem system) {
            var units = this.UnitDefinitions.AllUnits
                .Where(x => x.MeasurementSystem == system)
                .Select(x => Tuple.Create(x, x.FromStandardUnits((T)this)))
                .OrderBy(x => x.Item2);

            var unit = units.FirstOrDefault(x => x.Item2 > 1) ?? units.First();

            return unit.Item2.ToString() + " " + unit.Item1.Symbol;
        }

        public string ToString(UnitDefinition<T> unit) {
            return unit.FromStandardUnits((T)this).ToString() + " " + unit.Symbol;
        }
    }
}