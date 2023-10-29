using System.Globalization;
using System.Numerics;

namespace Galaxon.BigNumbers;

/// <summary>
/// Encapsulates a rational.
/// <see href="https://en.wikipedia.org/wiki/Rational_number"/>
/// <see href="https://introcs.cs.princeton.edu/java/92symbolic/BigRational.java.html"/>
/// <see href="https://github.com/danm-de/BigRationals"/>
/// </summary>
public partial struct BigRational :
    INumberBase<BigRational>,
    IComparisonOperators<BigRational, BigRational, bool>
{
    #region Constructors

    /// <summary>
    /// Construct a BigRational from two integers, the numerator and denominator.
    /// The fraction is automatically reduced to its simplest form.
    /// </summary>
    /// <param name="num">The numerator.</param>
    /// <param name="den">The denominator.</param>
    /// <exception cref="ArgumentOutOfRangeException">If the denominator is 0.</exception>
    public BigRational(BigInteger num, BigInteger den)
    {
        // Reduce.
        (num, den) = Reduce(num, den);

        // Assign parts.
        Numerator = num;
        Denominator = den;
    }

    /// <summary>
    /// Construct a BigRational from a single integer, taken to be the numerator.
    /// </summary>
    /// <param name="num">The numerator.</param>
    public BigRational(BigInteger num) : this(num, 1)
    {
    }

    /// <summary>
    /// Construct a zero BigRational.
    /// </summary>
    public BigRational() : this(0, 1)
    {
    }

    /// <summary>
    /// Construct a BigRational from a tuple of 2 BigInteger values.
    /// </summary>
    /// <param name="rational">The tuple.</param>
    public BigRational((BigInteger, BigInteger) rational) : this(rational.Item1, rational.Item2)
    {
    }

    /// <summary>
    /// Construct a BigRational from an array of 2 BigInteger values.
    /// </summary>
    /// <param name="rational">The array.</param>
    /// <exception cref="ArgumentException">If the array does not contain exactly 2
    /// values.</exception>
    public BigRational(BigInteger[] rational)
    {
        // Guard.
        if (rational.Length != 2)
        {
            throw new ArgumentException("The array must contain exactly two elements.");
        }

        // Reduce.
        var (num, den) = Reduce(rational[0], rational[1]);

        // Assign parts.
        Numerator = num;
        Denominator = den;
    }

    #endregion Constructors

    #region Properties

    public BigInteger Numerator { get; set; }

    public BigInteger Denominator { get; set; }

    /// <inheritdoc/>
    public static int Radix { get; }

    public static BigRational Zero => new (0, 1);

    /// <inheritdoc/>
    public static bool IsCanonical(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsComplexNumber(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsEvenInteger(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsFinite(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsImaginaryNumber(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsInfinity(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsInteger(BigRational value)
    {
        return value.Denominator == 1;
    }

    /// <inheritdoc/>
    public static bool IsNaN(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsNegative(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsNegativeInfinity(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsNormal(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsOddInteger(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsPositive(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsPositiveInfinity(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsRealNumber(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsSubnormal(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool IsZero(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigRational result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigRational result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigRational result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryConvertToChecked<TOther>(BigRational value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryConvertToSaturating<TOther>(BigRational value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryConvertToTruncating<TOther>(BigRational value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    public static BigRational One => new (1, 1);

    #endregion Properties

    /// <inheritdoc/>
    public static BigRational Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static BigRational AdditiveIdentity { get; }

    /// <inheritdoc/>
    public static BigRational MultiplicativeIdentity { get; }
}
