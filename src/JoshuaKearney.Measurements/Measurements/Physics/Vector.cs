//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JoshuaKearney.Measurements {

//    public class Vector : Vector<DoubleMeasurement> {

//        public Vector(double magnitude, double angle) : base(magnitude, angle) {
//        }
//    }

//    public class Vector<T> where T : Measurement<T> {
//        public T Magnitude { get; }

//        public Angle Angle { get; }

//        public Vector(T horizontalComp, T verticalComp) {
//            this.Magnitude = horizontalComp.MeasurementProvider.CreateMeasurementWithDefaultUnits(
//                Math.Sqrt(
//                    horizontalComp.DefaultUnits * horizontalComp.DefaultUnits +
//                    verticalComp.DefaultUnits + verticalComp.DefaultUnits
//                )
//            );
//            this.Angle = new Angle(Math.Atan2(horizontalComp.DefaultUnits, verticalComp.DefaultUnits), Angle.Units.Radian);
//        }

//        public Vector(T magnitude, Angle angle) {
//            this.Magnitude = magnitude;
//            this.Angle = angle;
//        }

//        public T VerticleComponent => this.Magnitude.Multiply(Math.Sin(this.Angle.ToDouble(Angle.Units.Radian)));

//        public T HorizontalComponent => this.Magnitude.Multiply(Math.Cos(this.Angle.ToDouble(Angle.Units.Radian)));

//        public Vector<T> Add(Vector<T> that) {
//            return new Vector<T>(this.HorizontalComponent + that.HorizontalComponent, this.VerticleComponent + that.VerticleComponent);
//        }

//        public override string ToString() {
//            return $"[{this.Magnitude.ToString()} {this.Angle.ToString(Angle.Units.Degree)}]";
//        }
//    }
//}