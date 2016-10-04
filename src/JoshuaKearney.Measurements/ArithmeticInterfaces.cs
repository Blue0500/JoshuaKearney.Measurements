namespace JoshuaKearney.Measurements {

    /// <summary>
    /// Represents a measurement that can be cubed into another type of measurement
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ICubableMeasurement<out TResult>
        where TResult : Measurement<TResult> {

        /// <summary>
        /// Returns a measurement that represents the cube of this instance
        /// </summary>
        /// <returns></returns>
        TResult Cube();
    }

    /// <summary>
    /// Represents a measurement that can be divided by another type of measurement, resulting in a third type of measurement
    /// </summary>
    /// <typeparam name="TIn">The type of the divisor.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IDividableMeasurement<in TIn, out TResult>
        where TResult : Measurement<TResult>
        where TIn : Measurement<TIn> {

        /// <summary>
        /// Returns a measurement that represents the division of this instance by another measurement
        /// </summary>
        /// <param name="measurement2">The measurement2.</param>
        /// <returns></returns>
        TResult Divide(TIn measurement2);
    }

    /// <summary>
    /// Represents a measurement that can be multiplied with another type of measurement, resulting in a third type of measurement
    /// </summary>
    /// <typeparam name="TIn">The type of the 2nd term.</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface IMultipliableMeasurement<in TIn, out TResult>
        where TResult : Measurement<TResult>
        where TIn : Measurement<TIn> {

        /// <summary>
        /// Returns a measurement that represents the multiplication of this instance by another measurement
        /// </summary>
        /// <param name="measurement2">The measurement2.</param>
        /// <returns></returns>
        TResult Multiply(TIn measurement2);
    }

    /// <summary>
    /// Represents a measurement that can be squared into another type of measurement
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    public interface ISquareableMeasurement<out TResult>
        where TResult : Measurement<TResult> {

        /// <summary>
        /// Returns a measurement that represents the cube of this instance
        /// </summary>
        /// <returns></returns>
        TResult Square();
    }
}