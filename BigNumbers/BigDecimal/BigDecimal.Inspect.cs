using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    /// <inheritdoc/>
    /// <summary>
    /// Checks if the value is in its canonical state, which for BigDecimal means the value should
    /// be zero or the significand should be a number not divisible by 10 (i.e. represented by the
    /// smallest possible BigInteger).
    /// If the significand is 0, then the exponent should also be 0.
    /// </summary>
    public static bool IsCanonical(BigDecimal value) =>
        value == Zero || value.Significand % 10 != 0;

    /// <inheritdoc/>
    public static bool IsZero(BigDecimal value) => value.Significand == 0;

    /// <inheritdoc/>
    public static bool IsNegative(BigDecimal value) => value.Significand < 0;

    /// <inheritdoc/>
    public static bool IsPositive(BigDecimal value) => value.Significand > 0;

    /// <inheritdoc/>
    public static bool IsInteger(BigDecimal value) => value.Exponent >= 0;

    /// <inheritdoc/>
    public static bool IsOddInteger(BigDecimal value) =>
        // If the exponent is less than 0, the value will not be an integer.
        // If the exponent is greater than 0, then the value will be even, because all positive
        // powers of 10 are even.
        // Therefore, the exponent must be 0.
        value.Exponent == 0 && BigInteger.IsOddInteger(value.Significand);

    /// <inheritdoc/>
    public static bool IsEvenInteger(BigDecimal value) =>
        // If the exponent is less than 0, the value will not be an integer.
        // If the exponent is greater than 0, then the value will be even, because all positive
        // powers of 10 are even.
        // If the exponent is 0, then the number is even only if the significand is even.
        value.Exponent > 0 || BigInteger.IsEvenInteger(value.Significand);

    /// <inheritdoc/>
    public static bool IsRealNumber(BigDecimal value) => true;

    /// <inheritdoc/>
    public static bool IsImaginaryNumber(BigDecimal value) => false;

    /// <inheritdoc/>
    public static bool IsComplexNumber(BigDecimal value) => false;

    /// <inheritdoc/>
    public static bool IsFinite(BigDecimal value) => true;

    /// <inheritdoc/>
    public static bool IsInfinity(BigDecimal value) => false;

    /// <inheritdoc/>
    public static bool IsNegativeInfinity(BigDecimal value) => false;

    /// <inheritdoc/>
    public static bool IsPositiveInfinity(BigDecimal value) => false;

    /// <inheritdoc/>
    public static bool IsNaN(BigDecimal value) => false;

    /// <inheritdoc/>
    public static bool IsNormal(BigDecimal value) => true;

    /// <inheritdoc/>
    public static bool IsSubnormal(BigDecimal value) => false;
}
