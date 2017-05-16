using System;
using System.Collections.Generic;
using System.Text;

namespace JoshuaKearney.Measurements {
    public interface IMultipliableMeasurement<TSelf, TIn, TResult> : IMeasurement<TSelf> 
        where TIn : IMeasurement<TIn>
        where TSelf : IMeasurement<TSelf> 
        where TResult : IMeasurement<TResult> {

        TResult Multiply(IMeasurement<TIn> other);
    }

    public interface IDivisableMeasurement<TSelf, TIn, TResult> : IMeasurement<TSelf>
        where TIn : IMeasurement<TIn>
        where TSelf : IMeasurement<TSelf>
        where TResult : IMeasurement<TResult> {

        TResult Divide(IMeasurement<TIn> other);
    }
}