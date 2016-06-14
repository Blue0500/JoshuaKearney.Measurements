using System;
using System.Collections.Generic;

namespace JoshuaKearney.Measurements {

    public sealed class Volume : Measurement<Volume>, IMeasurementTerm<Area, Length> {

        public Volume() {
        }

        public static Volume FromInchesCubed(double inchesCubed) {
            return Volume.FromUnit(inchesCubed, Volume.Units.InchCubed);
        }

        public static Volume FromFeetCubed(double feetCubed) {
            return Volume.FromUnit(feetCubed, Volume.Units.FootCubed);
        }

        public static Volume FromYardsCubed(double yardsCubed) {
            return Volume.FromUnit(yardsCubed, Volume.Units.YardCubed);
        }

        public static Volume FromMilesCubed(double milesCubed) {
            return Volume.FromUnit(milesCubed, Volume.Units.MileCubed);
        }

        public static Volume FromMillimetersCubed(double millimetersCubed) {
            return Volume.FromUnit(millimetersCubed, Volume.Units.MillimeterCubed);
        }

        public static Volume FromKilometersCubed(double kilometersCubed) {
            return Volume.FromUnit(kilometersCubed, Volume.Units.KilometerCubed);
        }

        public static Volume FromCentimetersCubed(double centimetersCubed) {
            return Volume.FromUnit(centimetersCubed, Volume.Units.CentimeterCubed);
        }

        public static Volume FromMetersCubed(double metersCubed) {
            return Volume.FromUnit(metersCubed, Volume.Units.MeterCubed);
        }

        public static Volume FromLiters(double liters) {
            return Volume.FromUnit(liters, Volume.Units.Liter);
        }

        public static Volume FromMilliliters(double milliliters) {
            return Volume.FromUnit(milliliters, Volume.Units.Milliliter);
        }

        public static Volume FromGallons(double gallons) {
            return Volume.FromUnit(gallons, Volume.Units.Gallon);
        }

        public static Volume FromQuarts(double quarts) {
            return Volume.FromUnit(quarts, Volume.Units.Quart);
        }

        public static Volume FromPints(double pints) {
            return Volume.FromUnit(pints, Volume.Units.Pint);
        }

        public static Volume FromCups(double cups) {
            return Volume.FromUnit(cups, Volume.Units.Cup);
        }

        public static Volume FromFluidOunces(double fluidOunces) {
            return Volume.FromUnit(fluidOunces, Volume.Units.FluidOunces);
        }

        public static Volume FromTableSpoons(double tablespoons) {
            return Volume.FromUnit(tablespoons, Volume.Units.Tablespoon);
        }

        public static Volume FromTeaspoons(double teaspoons) {
            return Volume.FromUnit(teaspoons, Volume.Units.Teaspoon);
        }

        Measurement IMeasurementTerm<Area, Length>.ToMeasurement() {
            return this;
        }

        public double InchesCubed {
            get {
                return Volume.Units.InchCubed.FromStandardUnits(this);
            }
        }

        public double FeetCubed {
            get {
                return Volume.Units.FootCubed.FromStandardUnits(this);
            }
        }

        public double YardsCubed {
            get {
                return Volume.Units.YardCubed.FromStandardUnits(this);
            }
        }

        public double KilometersCubed {
            get {
                return Volume.Units.KilometerCubed.FromStandardUnits(this);
            }
        }

        public double MetersCubed {
            get {
                return Volume.Units.MeterCubed.FromStandardUnits(this);
            }
        }

        public double CentimetersCubed {
            get {
                return Volume.Units.CentimeterCubed.FromStandardUnits(this);
            }
        }

        public double MillimetersCubed {
            get {
                return Volume.Units.MillimeterCubed.FromStandardUnits(this);
            }
        }

        public double MilesCubed {
            get {
                return Volume.Units.MileCubed.FromStandardUnits(this);
            }
        }

        public double Liters {
            get {
                return Volume.Units.Liter.FromStandardUnits(this);
            }
        }

        public double Milliliters {
            get {
                return Volume.Units.Milliliter.FromStandardUnits(this);
            }
        }

        public double Gallons {
            get {
                return Volume.Units.Gallon.FromStandardUnits(this);
            }
        }

        public double Quarts {
            get {
                return Volume.Units.Quart.FromStandardUnits(this);
            }
        }

        public double Pints {
            get {
                return Volume.Units.Pint.FromStandardUnits(this);
            }
        }

        public double Cups {
            get {
                return Volume.Units.Cup.FromStandardUnits(this);
            }
        }

        public double FluidOunces {
            get {
                return Volume.Units.FluidOunces.FromStandardUnits(this);
            }
        }

        public double Tablespoons {
            get {
                return Volume.Units.Tablespoon.FromStandardUnits(this);
            }
        }

        public double Teaspoons {
            get {
                return Volume.Units.Teaspoon.FromStandardUnits(this);
            }
        }

        public static VolumeDefinitionCollection Units { get; } = new VolumeDefinitionCollection();

        public override UnitDefinitionCollection<Volume> UnitDefinitions { get; } = Volume.Units;
    }
}