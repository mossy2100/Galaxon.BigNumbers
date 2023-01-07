# BigDecimal

This is a full-featured, .NET 7-compatible value type for working with high-precision decimal
numbers. It's well-tested, fast, and supports new interfaces for numeric types provided in .NET 7.

## Features

The type implements the following interfaces:
* [IFloatingPoint\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpoint-1)
* [IPowerFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ipowerfunctions-1)
* [IRootFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.irootfunctions-1)
* [IExponentialFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iexponentialfunctions-1)
* [ILogarithmicFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ilogarithmicfunctions-1)
* [ITrigonometricFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.itrigonometricfunctions-1)
* [IHyperbolicFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ihyperbolicfunctions-1)
* [ICloneable](https://learn.microsoft.com/en-us/dotnet/api/system.icloneable)
* [IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible)

And therefore includes:
* Arithmetic operators
* Cast operators to and from all C# built-in numeric types
* **Parse()** and **ToString()**
* **Clone()**, **Abs()**, **Round()**, **Truncate()**, **Floor()**, and **Ceiling()**
* Exponentiation, root, and logarithm functions
* Trigonometric and hyperbolic functions (including bonus ones for cotangent, secant, and cosecant)

It also includes some bonus methods like **ArithmeticGeometricMean()**, and a few handy LINQ
methods for **IEnumerable\<BigDecimal\>** like **Sum()** and **Average()**.

## Constants

In accordance with the **IFloatingPointConstants\<TSelf\>** interface, the type provides built-in
support for the constants e (E), π (Pi), and τ (Tau). Built-in constants for φ (Phi) and the natural
logarithm of 10 (Ln10) are also included. Constants are computed on the fly to the specified number
of significant figures (see below), and cached.

## Significant figures

BigDecimal values store a maximum number of significant figures, and the value is rounded off to
that many significant figures after every operation. The default [rounding method](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding) is **ToEven**, for
consistency with other floating point types.   

Before doing stuff with BigDecimal values, set the maximum number of significant figures you want
using the **MaxSigFigs** public static property.

For example:
```
BigDecimal.MaxSigFigs = 200;
BigDecimal s = BigDecimal.Sqrt(2);
```
The default is 100, and the minimum is 30. The reason for this minimum is so that any decimal value
can be converted to a BigDecimal using an implicit cast, without loss of information. Presumably if
you want to use BigDecimal instead of decimal you want at least 30 significant figures anyway.

## Techniques

Exponentiation, root, logarithm, and trigonometric functions, and computation of constants are
achieved using numerical methods such as:
- [Taylor series](https://en.wikipedia.org/wiki/Taylor_series)
- [Newton's method](https://en.wikipedia.org/wiki/Newton%27s_method)
- [Mercator series](https://en.wikipedia.org/wiki/Mercator_series)
- [Chudnovsky algorithm](https://en.wikipedia.org/wiki/Chudnovsky_algorithm)

## Dependencies

This package depends on:
- [Galaxon.Core](https://github.com/mossy2100/Galaxon.Core)
- [Galaxon.Numerics](https://github.com/mossy2100/Galaxon.Numerics)

(TODO: Separate out BigRational from Galaxon.Numerics and change the dependency to just that.)

## Feedback

Please let me know if you find any bugs, if you would like so see any new features added to the
type, or if you can see ways it can be made more efficient or otherwise improved.

Shaun Moss (<shaun@astromultimedia.com>)
