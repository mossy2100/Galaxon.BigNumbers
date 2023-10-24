# BigDecimal

This is a full-featured, .NET 7-compatible value type for working with high-precision decimal
numbers. It's well-tested, fast, and supports new interfaces for numeric types provided in .NET 7.

## Features

The type implements the following interfaces:

* [IFloatingPoint\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ifloatingpoint-1?view=net-7.0)
* [IPowerFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ipowerfunctions-1?view=net-7.0)
* [IRootFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.irootfunctions-1?view=net-7.0)
* [IExponentialFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.iexponentialfunctions-1?view=net-7.0)
* [ILogarithmicFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ilogarithmicfunctions-1?view=net-7.0)
* [ITrigonometricFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.itrigonometricfunctions-1?view=net-7.0)
* [IHyperbolicFunctions\<TSelf\>](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.ihyperbolicfunctions-1?view=net-7.0)
* [ICloneable](https://learn.microsoft.com/en-us/dotnet/api/system.icloneable?view=net-7.0)
* [IConvertible](https://learn.microsoft.com/en-us/dotnet/api/system.iconvertible?view=net-7.0)

And therefore includes:

* Arithmetic operators
* Cast operators to and from all C# built-in numeric types
* **Parse()** and **ToString()**
* **Clone()**, **Abs()**, **Round()**, **Truncate()**, **Floor()**, and **Ceiling()**
* Exponentiation, root, and logarithm functions
* Trigonometric and hyperbolic functions

It also includes some bonus methods like **ArithmeticGeometricMean()**, and a few handy LINQ
methods for **IEnumerable\<BigDecimal\>** like **Sum()** and **Average()**.

## Constants

In accordance with the **IFloatingPointConstants\<TSelf\>** interface, the type provides built-in
support for the constants e (E), π (Pi), and τ (Tau). Built-in constants for φ (Phi) and the natural
logarithm of 10 (Ln10) are also included. Constants are computed on the fly to the specified number
of significant figures (see below), and cached.

## Significant figures

BigDecimal values store a maximum number of significant figures, and the value is rounded off to
that many significant figures after every operation. The
default [rounding method](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding) is *
*ToEven**, for
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

Please let me know if you find any bugs, would like to see new features added, or can see ways the
type can be made more performant or otherwise improved.

Shaun Moss (<shaun@astromultimedia.com>)
