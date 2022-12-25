using System.Numerics;

namespace Galaxon.Numerics.Types;

/// <summary>
/// Trigonometric methods for BigDecimal.
/// </summary>
public partial struct BigDecimal : ITrigonometricFunctions<BigDecimal>
{
    /// <inheritdoc />
    public static BigDecimal Acos(BigDecimal x) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal AcosPi(BigDecimal x) =>
        Acos(x) / Pi;

    /// <inheritdoc />
    public static BigDecimal Asin(BigDecimal x) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal AsinPi(BigDecimal x) =>
        Asin(x) / Pi;

    /// <inheritdoc />
    public static BigDecimal Atan(BigDecimal x) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal AtanPi(BigDecimal x) =>
        Atan(x) / Pi;

    /// <inheritdoc />
    public static BigDecimal Cos(BigDecimal x) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static BigDecimal CosPi(BigDecimal x) =>
        Cos(x * Pi);

    /// <inheritdoc />
    public static BigDecimal Sin(BigDecimal x) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static (BigDecimal Sin, BigDecimal Cos) SinCos(BigDecimal x) =>
        (Sin(x), Cos(x));

    /// <inheritdoc />
    public static (BigDecimal SinPi, BigDecimal CosPi) SinCosPi(BigDecimal x) =>
        (SinPi(x), CosPi(x));

    /// <inheritdoc />
    public static BigDecimal SinPi(BigDecimal x) =>
        Sin(x * Pi);

    /// <inheritdoc />
    public static BigDecimal Tan(BigDecimal x) =>
        Sin(x) / Cos(x);

    /// <inheritdoc />
    public static BigDecimal TanPi(BigDecimal x) =>
        Tan(x * Pi);
}
