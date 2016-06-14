using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace JoshuaKearney.Measurements {

    public sealed class Length : Measurement<Length> {

        private Length(double meters) {
            this.StandardUnits = meters;
        }

        public Length() {
        }

        public double Centimeters {
            get {
                return Length.Units.Centimeter.FromStandardUnits(this);
            }
        }

        public double Feet {
            get {
                return Length.Units.Foot.FromStandardUnits(this);
            }
        }

        public double Inches {
            get {
                return Length.Units.Inch.FromStandardUnits(this);
            }
        }

        public double Kilometers {
            get {
                return Length.Units.Kilometer.FromStandardUnits(this);
            }
        }

        public double Meters {
            get {
                return this.StandardUnits;
            }
        }

        public double Miles {
            get {
                return Length.Units.Mile.FromStandardUnits(this);
            }
        }

        public double Millimeters {
            get {
                return Length.Units.Millimeter.FromStandardUnits(this);
            }
        }

        public double Yards {
            get {
                return Length.Units.Yard.FromStandardUnits(this);
            }
        }

        public override UnitDefinitionCollection<Length> UnitDefinitions { get; } = Length.Units;

        public static LengthDefinitionCollection Units { get; } = new LengthDefinitionCollection();

        public static Length FromCentimeters(double centimeters) {
            return Length.FromUnit(centimeters, Length.Units.Centimeter);
        }

        public static Length FromFeet(double feet) {
            return Length.FromUnit(feet, Length.Units.Foot);
        }

        public static Length FromInches(double inches) {
            return Length.FromUnit(inches, Length.Units.Inch);
        }

        public static Length FromKilometers(double kilometers) {
            return Length.FromUnit(kilometers, Length.Units.Kilometer);
        }

        public static Length FromMeters(double meters) {
            return Length.FromUnit(meters, Length.Units.Meter);
        }

        public static Length FromMiles(double miles) {
            return Length.FromUnit(miles, Length.Units.Mile);
        }

        public static Length FromMillimeters(double millimeters) {
            return Length.FromUnit(millimeters, Length.Units.Millimeter);
        }

        public static Length FromYards(double yards) {
            return Length.FromUnit(yards, Length.Units.Yard);
        }
    }
}