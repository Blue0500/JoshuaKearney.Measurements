namespace JoshuaKearney.Measurements {

    public interface ICubableMeasurement<out TResult>
        where TResult : Measurement<TResult> {

        TResult Cube();
    }

    public interface IDividableMeasurement<in TIn, out TResult>
        where TResult : Measurement<TResult>
        where TIn : Measurement<TIn> {

        TResult Divide(TIn second);
    }

    public interface IMultipliableMeasurement<in TIn, out TResult>
        where TResult : Measurement<TResult>
        where TIn : Measurement<TIn> {

        TResult Multiply(TIn second);
    }

    public interface ISquareableMeasurement<out TResult>
        where TResult : Measurement<TResult> {

        TResult Square();
    }
}