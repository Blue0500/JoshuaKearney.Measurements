using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {
    public class Vector2d<T> where T : IMeasurement<T> {
        public T Magnitude { get; }

        public Angle Angle { get; }

        public Vector2d(T horizontalComp, T verticalComp) {
            this.Magnitude = horizontalComp.MeasurementSupplier.CreateMeasurement(
                Math.Sqrt(
                    Math.Pow(horizontalComp.ToDouble(horizontalComp.MeasurementSupplier.DefaultUnit), 2) +
                    Math.Pow(verticalComp.ToDouble(horizontalComp.MeasurementSupplier.DefaultUnit), 2)
                ),
                horizontalComp.MeasurementSupplier.DefaultUnit
            );

            this.Angle = Angle.Atan2(
                verticalComp.ToDouble(verticalComp.MeasurementSupplier.DefaultUnit), 
                horizontalComp.ToDouble(verticalComp.MeasurementSupplier.DefaultUnit)
            );
        }

        public Vector2d(T magnitude, Angle angle) {
            this.Magnitude = magnitude;
            this.Angle = angle;
        }

        public T VerticleComponent => this.Magnitude.Multiply(Angle.Sin(this.Angle));

        public T HorizontalComponent => this.Magnitude.Multiply(Angle.Cos(this.Angle));

        public Vector2d<T> Add(Vector2d<T> that) {
            return new Vector2d<T>(
                this.HorizontalComponent.Add(that.HorizontalComponent), 
                this.VerticleComponent.Add(that.VerticleComponent)
            );
        }

        public Vector2d<T> Subtract(Vector2d<T> that) {
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