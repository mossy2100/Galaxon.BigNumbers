# Galaxon.BigNumbers

This solution contains 3 related types:

- **BigRational**: A ratio of two BigIntegers.
- **BigDecimal**: A decimal number with configurable precision and very large range of exponents.
- **BigComplex**: A complex number type based on two BigDecimals.

My goal is to make these available as separate NuGet packages, hopefully by 2024. 

## Dependencies

The types are organised so there's no circular dependencies.

- All depend on the built-in real number types, including [System.Numerics.BigInteger](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger?view=net-7.0).
- BigDecimal depends on BigRational.
- BigComplex depends on BigDecimal and [System.Numerics.Complex](https://learn.microsoft.com/en-us/dotnet/api/system.numerics.complex?view=net-7.0).

## Feedback

Please let me know if you find any bugs, would like to see new features added, or can see ways the
type can be made more performant or otherwise improved.

(I'm considering implementing BigVector, BigMatrix, BigQuaternion, and/or BigOctonion, based on the
BigDecimal type — what do you think? Would they be used?)

Shaun Moss <[shaun@astromultimedia.com](mailto:shaun@astromultimedia.com)>
