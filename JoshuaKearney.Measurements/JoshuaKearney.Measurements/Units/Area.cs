using System;

namespace JoshuaKearney.Measurements {

    public sealed class Area : Measurement<Area>, IMeasurementTerm<Length, Length> {

        public Area() {
        }

        public static AreaDefinitionCollection Units { get; } = new AreaDefinitionCollection();

        public double Acres {
            get {
                return Area.Units.Acre.FromStandardUnits(this);
            }
        }

        public double CentimetersSquared {
            get {
                return Area.Units.CentimeterSquared.FromStandardUnits(this);
            }
        }

        public double FeetSquared {
            get {
                return Area.Units.FootSquared.FromStandardUnits(this);
            }
        }

        public double Hectares {
            get {
                return Area.Units.Hectare.FromStandardUnits(this);
            }
        }

        public double InchesSquared {
            get {
                return Area.Units.InchSquared.FromStandardUnits(this);
            }
        }

        public double KilometersSquared {
            get {
                return Area.Units.KilometerSquared.FromStandardUnits(this);
            }
        }

        public double MetersSquared {
            get {
                return Area.Units.MeterSquared.FromStandardUnits(this);
            }
        }

        public double MilesSquared {
            get {
                return Area.Units.MileSquared.FromStandardUnits(this);
            }
        }

        public double MillimetersSquared {
            get {
                return Area.Units.MillimeterSquared.FromStandardUnits(this);
            }
        }

        public double YardsSquared {
            get {
                return Area.Units.YardSquared.FromStandardUnits(this);
            }
        }

        protected override UnitDefinitionCollection<Area> UnitDefinitions {
            get {
                return Area.Units;
            }
        }

        public static Area FromAcres(double acres) {
            return Area.FromUnit(acres, Area.Units.Acre);
        }

        public static Area FromCentimetersSquared(double centimetersSquared) {
            return Area.FromUnit(centimetersSquared, Area.Units.CentimeterSquared);
        }

        public static Area FromFeetSquared(double feetSquared) {
            return Area.FromUnit(feetSquared, Area.Units.FootSquared);
        }

        public static Area FromHectares(double hectares) {
            return Area.FromUnit(hectares, Area.Units.Hectare);
        }

        public static Area FromInchesSquared(double inchesSquared) {
            return Area.FromUnit(inchesSquared, Area.Units.InchSquared);
        }

        public static Area FromKilometersSquared(double kilometerSquared) {
            return Area.FromUnit(kilometerSquared, Area.Units.KilometerSquared);
        }

        public static Area FromMetersSquared(double metersSquared) {
            return Area.FromUnit(metersSquared, Area.Units.MeterSquared);
        }

        public static Area FromMilesSquared(double milesSquared) {
            return Area.FromUnit(milesSquared, Area.Units.MileSquared);
        }

        public static Area FromMillimetersSquared(double millimetersSquared) {
            return Area.FromUnit(millimetersSquared, Area.Units.MillimeterSquared);
        }

        public static Area FromYardsSquared(double yardsSquared) {
            return Area.FromUnit(yardsSquared, Area.Units.YardSquared);
        }

        Measurement IMeasurementTerm<Length, Length>.ToMeasurement() {
            return this;
        }
    }
}