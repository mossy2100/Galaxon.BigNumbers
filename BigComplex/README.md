# BigComplex

This is a full-featured, .NET 7-compatible value type for working with high-precision complex
numbers. It's well-tested, fast, and supports new interfaces for numeric types provided in .NET 7.

The purpose of the type is to enable working with complex numbers without being encumbered by the
limitations of the double type (which the built-in Complex type relies on), especially in terms of
range, precision, and accuracy.

The struct contains two BigDecimal types; therefore, this package (Galaxon.BigComplex)
depends on Galaxon.BigDecimal, which in turn depends on Galaxon.Numerics and Galaxon.Core.

To understand the capabilities of BigComplex, it's worth understanding BigDecimal.

A wide range of methods and operators are provided, including those required by .NET's built-in
number-related interfaces, and System.Complex, plus more.

If you have any ideas for improving the type, please let me know: <shaun@astromultimedia.com>.

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
* Trigonometric and hyperbolic functions

It also includes some bonus methods like **ArithmeticGeometricMean()**, and a few handy LINQ
methods for **IEnumerable\<BigDecimal\>** like **Sum()** and **Average()**.

## Constants

In accordance with the **IFloatingPointConstants\<TSelf\>** interface, the type provides built-in
support for the constants e (E), π (Pi), and τ (Tau). Bonus built-in constants for φ (Phi) and the
natural logarithm of 10 (Ln10) are also included. Constants are computed on the fly to the specified
number of significant figures (see below), and cached.

## Significant figures

Setting the number of significant figures on a BigComplex will set the number of significant figures
on the underlying BigDecimal values.

BigDecimal values store a maximum number of significant figures, and the value is rounded off to
that many significant figures after every operation. The
default [rounding method](https://learn.microsoft.com/en-us/dotnet/api/system.midpointrounding) is *
*ToEven**, for
consistency with other floating point types.

The default is 100, and the minimum is 30. The reason for this minimum is so that any decimal value
can be converted to a BigDecimal using an implicit cast, without loss of information. Presumably if
you want to use BigDecimal instead of decimal you want at least 30 significant figures anyway.

If you want to work with a different number of significant figures (smaller values will result in
greater speed, larger values will result in greater precision), set the maximum number of
significant figures you want using the **MaxSigFigs** public static property, before calling
methods or doing operations.

For example:

```
BigComplex.MaxSigFigs = 200;
BigComplex s = BigComplex.Sqrt(2);
```

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
- [Galaxon.BigDecimal](https://github.com/mossy2100/Galaxon.BigDecimal)

## Feedback

Please let me know if you find any bugs, would like to see new features added, or can see ways the
type can be made more performant or otherwise improved.

Shaun Moss (<shaun@astromultimedia.com>)
