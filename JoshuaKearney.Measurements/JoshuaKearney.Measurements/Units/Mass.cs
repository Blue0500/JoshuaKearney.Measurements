using System;
using System.Collections.Generic;
using System.Linq;

namespace JoshuaKearney.Measurements {

    public sealed class Mass : Measurement<Mass> {

        private Mass(double kilograms) {
            this.StandardUnits = kilograms;
        }

        public Mass() {
        }

        public double Grams {
            get {
                return Mass.Units.Gram.FromStandardUnits(this);
            }
        }

        public double Ounces {
            get {
                return Mass.Units.Ounce.FromStandardUnits(this);
            }
        }

        public double MetricTons {
            get {
                return Mass.Units.MetricTon.FromStandardUnits(this);
            }
        }

        public double Kilograms {
            get {
                return this.StandardUnits;
            }
        }

        public double Tons {
            get {
                return Mass.Units.Ton.FromStandardUnits(this);
            }
        }

        public double Milligrams {
            get {
                return Mass.Units.Milligram.FromStandardUnits(this);
            }
        }

        public double Pounds {
            get {
                return Mass.Units.Pound.FromStandardUnits(this);
            }
        }

        protected override UnitDefinitionCollection<Mass> UnitDefinitions { get; } = Mass.Units;

        public static MassDefinitionCollection Units { get; } = new MassDefinitionCollection();

        public static Mass FromGrams(double grams) {
            return Mass.FromUnit(grams, Mass.Units.Gram);
        }

        public static Mass FromOnces(double ounces) {
            return Mass.FromUnit(ounces, Mass.Units.Ounce);
        }

        public static Mass FromMetricTons(double metricTons) {
            return Mass.FromUnit(metricTons, Mass.Units.MetricTon);
        }

        public static Mass FromKilograms(double kilograms) {
            return new Mass().WithStandardUnits(kilograms);
        }

        public static Mass FromTons(double tons) {
            return Mass.FromUnit(tons, Mass.Units.Ton);
        }

        public static Mass FromMilligrams(double milligrams) {
            return Mass.FromUnit(milligrams, Mass.Units.Milligram);
        }

        public static Mass FromPounds(double pounds) {
            return Mass.FromUnit(pounds, Mass.Units.Pound);
        }
    }
}