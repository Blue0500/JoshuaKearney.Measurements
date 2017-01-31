using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    public class Vector2d<T> where T : IMeasurement<T> {
        public IMeasurement<T> Magnitude { get; }

        public Angle Angle { get; }

        public Vector2d(IMeasurement<T> horizontalComp, IMeasurement<T> verticalComp) {
            Validate.NonNull(horizontalComp, nameof(horizontalComp));
            Validate.NonNull(verticalComp, nameof(verticalComp));

            this.Magnitude = horizontalComp.MeasurementProvider.CreateMeasurement(
                Math.Sqrt(
                    Math.Pow(horizontalComp.ToDouble(horizontalComp.MeasurementProvider.DefaultUnit), 2) +
                    Math.Pow(verticalComp.ToDouble(horizontalComp.MeasurementProvider.DefaultUnit), 2)
                ),
                horizontalComp.MeasurementProvider.DefaultUnit
            );

            this.Angle = Angle.Atan2(verticalComp.ToDouble(verticalComp.MeasurementProvider.DefaultUnit), horizontalComp.ToDouble(verticalComp.MeasurementProvider.DefaultUnit));
        }

        public Vector2d(IMeasurement<T> magnitude, Angle angle) {
            Validate.NonNull(magnitude, nameof(magnitude));
            Validate.NonNull(angle, nameof(angle));

            this.Magnitude = magnitude;
            this.Angle = angle;
        }

        public T VerticleComponent => this.Magnitude.Multiply(Angle.Sin(this.Angle));

        public T HorizontalComponent => this.Magnitude.Multiply(Angle.Cos(this.Angle));

        public Vector2d<T> Add(Vector2d<T> that) {
            Validate.NonNull(that, nameof(that));

            return new Vector2d<T>(
                this.HorizontalComponent.Add(that.HorizontalComponent), 
                this.VerticleComponent.Add(that.VerticleComponent)
            );
        }

        public Vector2d<T> Subtract(Vector2d<T> that) {
            Validate.NonNull(that, nameof(that));

            return new Vector2d<T>(
                this.HorizontalComponent.Subtract(that.HorizontalComponent), 
                this.VerticleComponent.Subtract(that.VerticleComponent)
            );
        }

        public override string ToString() {
            return $"[{this.Magnitude.ToString()} {this.Angle.ToString(Angle.Units.Degree)}]";
        }
    }
}