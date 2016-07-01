namespace JoshuaKearney.Measurements {

    public interface ICubableMeasurement<out TResult>
        where TResult : Measurement, new() {

        TResult Cube();
    }

    public interface IDividableMeasurement<in TIn, out TResult>
        where TResult : Measurement, new()
        where TIn : Measurement, new() {

        TResult Divide(TIn second);
    }

    public interface IMultipliableMeasurement<in TIn, out TResult>
                where TResult : Measurement, new()
        where TIn : Measurement, new() {

        TResult Multiply(TIn second);
    }

    public interface ISquareableMeasurement<out TResult>
        where TResult : Measurement, new() {

        TResult Square();
    }
}