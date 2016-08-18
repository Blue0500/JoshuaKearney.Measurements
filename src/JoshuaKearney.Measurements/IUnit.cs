namespace JoshuaKearney.Measurements {
    //public interface IPrefixableUnit : IUnit {
    //}

    public interface IPrefixableUnit<T> : IUnit<T> where T : Measurement<T> {
    }

    //public interface IUnit {
    //    //Type AssociatedMeasurement { get; }
    //    string Name { get; }

    //    string Symbol { get; }
    //    double UnitsPerDefault { get; }
    //}

    public interface IUnit<T> where T : Measurement<T> {
        string Name { get; }
        string Symbol { get; }
        double UnitsPerDefault { get; }
    }
}