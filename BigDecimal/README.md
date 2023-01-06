# BigDecimal

This is a full-featured, .NET 7 compatible struct to represent high-precision decimal values.

It implements the following interfaces:
* IFloatingPoint<T>
* IConvertible
* ICloneable
* IPowerFunctions<T>
* IRootFunctions<T>
* IExponentialFunctions<T>
* ILogarithmicFunctions<T>
* ITrigonometricFunctions<T>
* IHyperbolicFunctions<T>

And therefore includes:
* cast operators to and from all C# built-in numeric types
* Parse() and ToString()
* all arithmetic operators
* Clone(), Abs(), Round(), Truncate(), Floor(), and Ceiling()
* exponentiation, root, and logarithm functions
* trigonometric functions including hyperbolic functions
* some additional features like LINQ methods and ArithmeticGeometricMean()

Before doing stuff with BigDecimal values, set the maximum number of significant figures you want
using the MaxSigFigs static property.
e.g.
```
BigDecimal.MaxSigFigs = 130;
```
The default is 100, and the minimum is 30.
