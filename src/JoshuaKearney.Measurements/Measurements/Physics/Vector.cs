using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements {

    public class Vector2d : Vector2d<DoubleMeasurement> {
        public Vector2d(double xComp, double yComp) : base(xComp, yComp) { } 

        public Vector2d(double magnitude, Angle angle) : base(magnitude, angle) {
        }
    }

    public class Vector2d<T> where T : Measurement<T> {
        public T Magnitude { get; }

        public Angle Angle { get; }

        public Vector2d(T horizontalComp, T verticalComp) {
            this.Magnitude = horizontalComp.MeasurementProvider.CreateMeasurementWithDefaultUnits(
                Math.Sqrt(
                    horizontalComp.DefaultUnits * horizontalComp.DefaultUnits +
                    verticalComp.DefaultUnits + verticalComp.DefaultUnits
                )
            );
            this.Angle = new Angle(Math.Atan2(horizontalComp.DefaultUnits, verticalComp.DefaultUnits), Angle.Units.Radian);
        }

        public Vector2d(T magnitude, Angle angle) {
            this.Magnitude = magnitude;
            this.Angle = angle;
        }

        public T VerticleComponent => this.Magnitude.Multiply(Math.Sin(this.Angle.ToDouble(Angle.Units.Radian)));

        public T HorizontalComponent => this.Magnitude.Multiply(Math.Cos(this.Angle.ToDouble(Angle.Units.Radian)));

        public Vector2d<T> Add(Vector2d<T> that) {
            return new Vector2d<T>(
                this.HorizontalComponent + that.HorizontalComponent, 
                this.VerticleComponent + that.VerticleComponent
            );
        }

        public Vector2d<T> Subtract(Vector2d<T> that) {
            return new Vector2d<T>(
                this.HorizontalComponent - that.HorizontalComponent, 
                this.VerticleComponent - that.VerticleComponent
            );
        }

        public override string ToString() {
            return $"[{this.Magnitude.ToString()} {this.Angle.ToString(Angle.Units.Degree)}]";
        }
    }
}