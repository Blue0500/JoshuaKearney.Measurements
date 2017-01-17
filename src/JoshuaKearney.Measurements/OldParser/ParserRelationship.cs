using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoshuaKearney.Measurements.OldParser {
    internal interface IBinaryRelationship {
        object Apply(object item1, object item2);
    }

    internal interface IPostUrnaryRelationship {
        object Apply(object item);
    }

    internal enum RelationshipType {
        Multiplicative, Divivitive, Quadratic, Cubic
    }

    public abstract class MeasurementRelationship {      

        internal abstract RelationshipType RelationshipType { get; }

        public static MeasurementRelationship CreateMultiplicative<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> func)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryRelationship<TIn1, TIn2, TResult>(func, RelationshipType.Multiplicative);
        }

        public static MeasurementRelationship CreateFractional<TIn1, TIn2, TResult>(Func<TIn1, TIn2, TResult> func)
            where TIn1 : Measurement<TIn1>
            where TIn2 : Measurement<TIn2>
            where TResult : Measurement<TResult> {

            return new BinaryRelationship<TIn1, TIn2, TResult>(func, RelationshipType.Divivitive);
        }

        public static MeasurementRelationship CreateQuadratic<TIn1, TResult>(Func<TIn1, TResult> func)
            where TIn1 : Measurement<TIn1>
            where TResult : Measurement<TResult> {

            return new PostUrnaryRelationship<TIn1, TResult>(func, RelationshipType.Quadratic);
        }

        public static MeasurementRelationship CreateCubic<TIn1, TResult>(Func<TIn1, TResult> func)
            where TIn1 : Measurement<TIn1>
            where TResult : Measurement<TResult> {

            return new PostUrnaryRelationship<TIn1, TResult>(func, RelationshipType.Cubic);
        }

        private class BinaryRelationship<TIn1, TIn2, TOut> : MeasurementRelationship, IBinaryRelationship
                where TIn1 : Measurement<TIn1>
                where TIn2 : Measurement<TIn2>
                where TOut : Measurement<TOut> {
            private readonly Func<TIn1, TIn2, TOut> applier;

            internal override RelationshipType RelationshipType { get; }

            public BinaryRelationship(Func<TIn1, TIn2, TOut> func, RelationshipType type) {
                this.RelationshipType = type;
                this.applier = func;
            }

            public object Apply(object item1, object item2) => applier((TIn1)item1, (TIn2)item2);
        }

        private class PostUrnaryRelationship<TIn1, TOut> : MeasurementRelationship, IPostUrnaryRelationship
                where TIn1 : Measurement<TIn1>
                where TOut : Measurement<TOut> {
            private readonly Func<TIn1, TOut> applier;

            internal override RelationshipType RelationshipType { get; }

            public PostUrnaryRelationship(Func<TIn1, TOut> func, RelationshipType type) {
                this.RelationshipType = type;
                this.applier = func;
            }

            public object Apply(object item1) => applier((TIn1)item1);
        }
    }
}