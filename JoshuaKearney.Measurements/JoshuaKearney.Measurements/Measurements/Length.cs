using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class Length : Measurement<Length>,
        IMultipliableMeasurement<Length, Area>,
        IMultipliableMeasurement<Area, Volume>,
        ISquareableMeasurement<Area>,
        IDividableMeasurement<Time, Speed>,
        ICubableMeasurement<Volume> {
        public static IMeasurementProvider<Length> Provider { get; } = new LengthProvider();

        public override IMeasurementProvider<Length> MeasurementProvider => Provider;

        public Length() {
        }

        public Length(double amount, IUnit<Length> unit) : base(amount, unit) {
        }

        public static Area operator *(Length length, Length length2) {
            if (length == null || length2 == null) {
                return null;
            }

            return length.Multiply(length2);
        }

        public static Volume operator *(Length length, Area area) {
            if (length == null || area == null) {
                return null;
            }

            return length.Multiply(area);
        }

        public static Speed operator /(Length length, Time time) {
            if (length == null || time == null) {
                return null;
            }

            return length.Divide(time);
        }

        public Volume Cube() => this.Multiply(this.Square());

        public Area Multiply(Length length) {
            Validate.NonNull(length, nameof(length));
            return new Area(this, length);
        }

        public Volume Multiply(Area area) {
            Validate.NonNull(area, nameof(area));
            return new Volume(this, area);
        }

        public Area Square() => this.Multiply(this);

        public Speed Divide(Time time) {
            Validate.NonNull(time, nameof(time));
            return new Speed(this, time);
        }

        public static class Units {
            public static IPrefixableUnit<Length> Meter { get; } = MeasurementSystems.Metric.Meter;

            public static IUnit<Length> Centimeter { get; } = Prefix.Centi(MeasurementSystems.Metric.Meter);
            public static IUnit<Length> Foot { get; } = MeasurementSystems.EnglishLength.Foot;
            public static IUnit<Length> Inch { get; } = MeasurementSystems.EnglishLength.Inch;
            public static IUnit<Length> Kilometer { get; } = Prefix.Kilo(MeasurementSystems.Metric.Meter);
            public static IUnit<Length> Mile { get; } = MeasurementSystems.EnglishLength.Mile;
            public static IUnit<Length> Millimeter { get; } = Prefix.Milli(MeasurementSystems.Metric.Meter);
            public static IUnit<Length> Yard { get; } = MeasurementSystems.EnglishLength.Yard;
        }

        private class LengthProvider : IMeasurementProvider<Length> {
            public IUnit<Length> DefaultUnit => Units.Meter;

            public Length CreateMeasurement(double value, IUnit<Length> unit) => new Length(value, unit);
        }
    }
}