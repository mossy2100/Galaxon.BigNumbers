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
    public static bool IsCanonical(BigRational value)
    {
        return true;
    }

    /// <inheritdoc/>
    public static bool IsZero(BigRational value)
    {
        return value.Numerator == 0;
    }

    /// <inheritdoc/>
    public static bool IsNegative(BigRational value)
    {
        return value.Numerator < 0;
    }

    /// <inheritdoc/>
    public static bool IsPositive(BigRational value)
    {
        return value.Numerator > 0;
    }

    /// <inheritdoc/>
    public static bool IsInteger(BigRational value)
    {
        return IsInteger(value);
    }

    /// <inheritdoc/>
    public static bool IsOddInteger(BigRational value)
    {
        return IsInteger(value) && BigInteger.IsOddInteger(value.Numerator);
    }

    /// <inheritdoc/>
    public static bool IsEvenInteger(BigRational value)
    {
        return IsInteger(value) && BigInteger.IsEvenInteger(value.Numerator);
    }

    /// <inheritdoc/>
    public static bool IsRealNumber(BigRational value)
    {
        return true;
    }

    /// <inheritdoc/>
    public static bool IsImaginaryNumber(BigRational value)
    {
        return false;
    }

    /// <inheritdoc/>
    public static bool IsComplexNumber(BigRational value)
    {
        return false;
    }

    /// <inheritdoc/>
    public static bool IsFinite(BigRational value)
    {
        return true;
    }

    /// <inheritdoc/>
    public static bool IsInfinity(BigRational value)
    {
        return false;
    }

    /// <inheritdoc/>
    public static bool IsNegativeInfinity(BigRational value)
    {
        return false;
    }

    /// <inheritdoc/>
    public static bool IsPositiveInfinity(BigRational value)
    {
        return false;
    }

    /// <inheritdoc/>
    public static bool IsNaN(BigRational value)
    {
        return false;
    }

    /// <inheritdoc/>
    public static bool IsNormal(BigRational value)
    {
        return true;
    }

    /// <inheritdoc/>
    public static bool IsSubnormal(BigRational value)
    {
        return false;
    }
}
