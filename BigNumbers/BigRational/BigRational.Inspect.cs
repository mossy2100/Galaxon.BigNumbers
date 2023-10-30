using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    /// <inheritdoc/>
    /// <remarks>
    /// A BigRational value should always be kept in canonical form, which means:
    /// - reduced to the simplest possible ratio of integers
    /// - the denominator should be positive
    /// </remarks>
    public static bool IsCanonical(BigRational value) => true;

    /// <inheritdoc/>
    public static bool IsZero(BigRational value) => value.Numerator == 0;

    /// <inheritdoc/>
    public static bool IsNegative(BigRational value) => value.Numerator < 0;

    /// <inheritdoc/>
    public static bool IsPositive(BigRational value) => value.Numerator > 0;

    /// <inheritdoc/>
    public static bool IsInteger(BigRational value) => IsInteger(value);

    /// <inheritdoc/>
    public static bool IsOddInteger(BigRational value) =>
        IsInteger(value) && BigInteger.IsOddInteger(value.Numerator);

    /// <inheritdoc/>
    public static bool IsEvenInteger(BigRational value) =>
        IsInteger(value) && BigInteger.IsEvenInteger(value.Numerator);

    /// <inheritdoc/>
    public static bool IsRealNumber(BigRational value) => true;

    /// <inheritdoc/>
    public static bool IsImaginaryNumber(BigRational value) => false;

    /// <inheritdoc/>
    public static bool IsComplexNumber(BigRational value) => false;

    /// <inheritdoc/>
    public static bool IsFinite(BigRational value) => true;

    /// <inheritdoc/>
    public static bool IsInfinity(BigRational value) => false;

    /// <inheritdoc/>
    public static bool IsNegativeInfinity(BigRational value) => false;

    /// <inheritdoc/>
    public static bool IsPositiveInfinity(BigRational value) => false;

    /// <inheritdoc/>
    public static bool IsNaN(BigRational value) => false;

    /// <inheritdoc/>
    public static bool IsNormal(BigRational value) => true;

    /// <inheritdoc/>
    public static bool IsSubnormal(BigRational value) => false;
}
