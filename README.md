# JoshuaKearney.Measurements #

## Basics ##

JoshuaKearney.Measurements is a dimensional analysis library that attempts to express physical quantities in C# neatly and efficiently. Before continuing, here's some vocab used below:

- Measurement - Broad, general category of measuring. Ex: Distance, Area, Force
- Unit - Used to express a measurement with a specific number. Ex: foot, meter, Newton, radian
- Label - Double that is paired with a type acting as a unit to create a "one-off" measurement.   
  Ex: 3 cats `Label<Cat> some = new Label<Cat>(3);`


This library is represented by a heiarchy of immutable classes used to represent measurements in an extremely type safe way. Not only are measurements of the same type type-safe, but derived units are also type safe during calculations. For example, this is a snippet of source code from the `Speed` class:

    public class Speed : Ratio<Speed, Distance, Time>, IDividableMeasurement<Time, Acceleration> {
    ...
    }
    
First, note that Speed actually inherits from a ratio of distance and time -- it had true type safety. This provides countless benifits, such as being able to use distance and time units together in many places where a speed unit is expected, like this:

    Speed s = new Speed(45, Distance.Units.Inch, Time.Units.Hour);
    Console.WriteLine(s.ToString(Distance.Units.Kilometer, Time.Units.Hour));
    
No sense in keeping track of almost endless `Speed` units, right? Also, `Speed` implements an interface that states that `Speed` can be divided by `Time`, which results in `Acceleration`. Even more type safety! When combined, these methods provide a fluent interface for converting between units, almost without realizing it.

    Pressure p = new Distance(45, Distance.Units.Inch)
      .Divide(new Time(6.2, Time.Units.Second))
      .Divide(new Time(.21, Time.Units.Hour))
      .Multiply(new Mass(700, Mass.Units.Pound))
      .Divide(new Area(.1, Area.Units.FootSquared));
      
That's pressure from scratch! Of course there are simpler ways of doing this, it's just an example. This can even be simplified further if you staticly import the units.

    using static JoshuaKearney.Measurements.Distance.Units;
    using static JoshuaKearney.Measurements.Time.Units;
    using static JoshuaKearney.Measurements.Mass.Units;
    using static JoshuaKearney.Measurements.Area.Units;
    
    ...
    
    Pressure p = new Distance(45, Inch)
      .Divide(new Time(6.2, Second))
      .Divide(new Time(.21, Hour))
      .Multiply(new Mass(700, Pound))
      .Divide(new Area(.1, FootSquared));
   
C# operators can also be used for arithmetic, but may be less reliable or more ambiguous for the compiler.    
   
---

## Units ##
Units are represented by a generic `Unit<T>` class, and supply conversions to and from the measurement's base unit. A base unit is what unit a measurement internally stores it's value in by default. For the default measurements, these are always SI base units such as kilogram, meter, radian, etc. To create a unit from scratch, use the constructor, like so:

    public static Unit<Distance> Foot { get; } = new Unit<Distance>(
        name: "foot",
        symbol: "ft",
        unitsPerDefault: 1d / .3048
    );
    
This is the definition for `Foot`. The name and symbol are self explanatory, and the third is the conversion factor. There are exactly .3048 meters in a foot, and to `1 /` flips that to approximately 3.28 ft/m. Always express units in `# of Your Units/1 Base Unit`. The reason it's written like this is that 1 / .3048 is irrational, or at least close to it, so for the optimal accuracy it's left in division form to fill the maximum amount of digits of precision in a double.

**But what if I want to add prefixes to my units?**  
There's a prefix class! Instead of creating a new `Unit<T>`, create a `PrefixableUnit<T>` and you can use the prefix class, like so:

    using static JoshuaKearney.Measurements.Distance.Units; // Optional
    
    ...
    
    Unit<Length> Centimeter = Prefix.Centi(Meter);
    
**Is there an easier way to create units?**  
Yes! You can create units based on the internal storage of a measurement and never have to consider conversion factors. Consider the definition of a [Pound Force](https://en.wikipedia.org/wiki/Pound_(force)): 

> The pound-force is equal to the gravitational force exerted on a mass of one avoirdupois pound on the surface of Earth

So, the easiest way to create this unit would be like so:

    Unit<Force> PoundForce = new Mass(1, Mass.Units.Pound).Multiply(Acceleration.Gravity).CreateUnit("pound-force", "lbf");
    
---
## On-the-fly measurements ##
Currently, there are 3 classes that let you create more types of measurements without creating any new classes.
  1. `Term<T1, T2>` - Used for representing multiplication of measurements, like Distance*Force
  2. `Ratio<TNumerator, TDenominator>` - Used for representing the division of measurements, like DigitalSize/Time
  3. `Label<T>` - Used for representing an amount of a non-unit thing. Ex: 45 apples (assuming there is an `Apple` class)
  

```
    using static JoshuaKearney.Measurements.DigitalSize.Units;
    using static JoshuaKearney.Measurements.Mass.Units;
    using static JoshuaKearney.Measurements.Time.Units;
    using static JoshuaKearney.Measurements.Volume.Units;
         
    ...
        
    public class Banana { }
        
    ...
        
    Term<Mass, Volume> arialDensity = new Term<Mass, Volume>(new Mass(45, Kilogram), new Volume(4, MeterCubed));
    Console.WriteLine(arialDensity);

    Ratio<DigitalSize, Time> downloadSpeed = new Ratio<DigitalSize, Time>(new DigitalSize(2.1, Prefix.Giga(Bit)), new Time(1, Second));
    Console.WriteLine(downloadSpeed);

    Label<Banana> bananas = new Label<Banana>(4, Label.Units.Dozen);
    Console.WriteLine(bananas);
```
---
  
