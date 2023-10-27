using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

/// <summary>
/// Encapsulates a rational.
/// <see href="https://en.wikipedia.org/wiki/Rational_number" />
/// <see href="https://introcs.cs.princeton.edu/java/92symbolic/BigRational.java.html" />
/// <see href="https://github.com/danm-de/BigRationals" />
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
    /// <exception cref="ArgumentInvalidException">If the denominator is 0.</exception>
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

    /// <inheritdoc />
    public static int Radix { get; }

    public static BigRational Zero => new (0, 1);

    /// <inheritdoc />
    public static BigRational Abs(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsCanonical(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsComplexNumber(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsEvenInteger(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsFinite(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsImaginaryNumber(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsInfinity(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsInteger(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNaN(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNegative(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNegativeInfinity(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNormal(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsOddInteger(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsPositive(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsPositiveInfinity(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsRealNumber(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsSubnormal(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsZero(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational MaxMagnitude(BigRational x, BigRational y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational MaxMagnitudeNumber(BigRational x, BigRational y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational MinMagnitude(BigRational x, BigRational y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational MinMagnitudeNumber(BigRational x, BigRational y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigRational result) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigRational result) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigRational result) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertToChecked<TOther>(BigRational value, out TOther result) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertToSaturating<TOther>(BigRational value, out TOther result) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertToTruncating<TOther>(BigRational value, out TOther result) where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    public static BigRational One => new (1, 1);

    #endregion Properties

    #region Equality methods

    public bool Equals(BigRational br2)
    {
        // See if the numerators and denominators are equal.
        return Numerator == br2.Numerator && Denominator == br2.Denominator;
    }

    /// <inheritdoc />
    public static BigRational operator +(BigRational value)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        return obj is BigRational br2 && Equals(br2);
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }

    #endregion Equality methods

    #region String methods

    /// <summary>
    /// Format the rational as a string.
    /// </summary>
    /// <todo>
    /// Update to support standard format strings for integers, namely D, N, R, with the optional U
    /// code, same as for BigNumbers. Remove "A", keep "M" for mixed.
    /// </todo>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentFormatException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        // Default to the Unicode version.
        if (string.IsNullOrEmpty(format))
        {
            format = "U";
        }

        // If the denominator is 1 then just return the numerator as a string.
        if (Denominator == 1)
        {
            return Numerator.ToString();
        }

        // Get the parts we need.
        var sign = this < 0 ? "-" : "";
        var num = BigInteger.Abs(Numerator);
        var den = BigInteger.Abs(Denominator);

        switch (format.ToUpper())
        {
            // ASCII.
            case "A":
                return $"{sign}{num}/{den}";

            // Unicode.
            case "U":
                return $"{sign}{num.ToSuperscript()}/{den.ToSubscript()}";

            // Mixed.
            case "M":
                // Format improper fractions (or rationals that have a numerator with a larger
                // magnitude than the denominator) with a quotient and remainder, e.g 2³/₄
                if (num > den)
                {
                    var quot = num / den;
                    var rem = num % den;
                    return $"{sign}{quot}{rem.ToSuperscript()}/{den.ToSubscript()}";
                }
                return $"{sign}{num.ToSuperscript()}/{den.ToSubscript()}";

            default:
                throw new ArgumentFormatException(nameof(format),
                    "The provided format string is not supported.");
        }
    }

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Format the rational as a string.
    /// </summary>
    public string ToString(string format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Format the rational as a string.
    /// The is the default override version, which uses Unicode characters for a nicer format.
    /// </summary>
    public override string ToString()
    {
        return ToString("U", CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static BigRational Parse(string s, IFormatProvider? provider = null)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Just support ASCII for now.
        // I may support the superscript/subscript version later (as generated by ToString()), but
        // it's probably unnecessary.
        var match = Regex.Match(s, @"^(?<num>-?\d+)/(?<den>-?\d+)$");
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s),
                "Incorrect format. The correct format is int/int, e.g. 22/7 or -3/4.");
        }

        var num = BigInteger.Parse(match.Groups["num"].Value);
        var den = BigInteger.Parse(match.Groups["den"].Value);
        return new BigRational(num, den);
    }

    /// <summary>
    /// Try to parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, IFormatProvider? provider, out BigRational result)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Try to parse the provided string.
        try
        {
            result = Parse(s, provider);
        }
        catch (Exception)
        {
            result = default(BigRational);
            return false;
        }

        // All good.
        return true;
    }

    /// <summary>
    /// Try to parse a string into a rational.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, out BigRational result)
    {
        return TryParse(s, null, out result);
    }

    #endregion String methods

    #region Other methods

    /// <summary>
    /// Clone a rational.
    /// </summary>
    public BigRational Clone()
    {
        return (BigRational)MemberwiseClone();
    }

    /// <summary>
    /// Reduce a rational given as a numerator and denominator.
    /// I've made this version, which doesn't receive or return a BigRational object, so it can be
    /// called from the constructor.
    /// </summary>
    private static (BigInteger, BigInteger) Reduce(BigInteger num, BigInteger den)
    {
        // Guard.
        if (den == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(den),
                "The denominator of a rational number cannot be 0.");
        }

        // Optimizations.
        if (num == 0) return (0, 1);
        if (num == den) return (1, 1);

        // Make the denominator positive.
        if (den < 0)
        {
            num = -num;
            den = -den;
        }

        // Check for simple, irreducible fractions.
        if (num == 1) return (1, den);
        if (den == 1) return (num, 1);

        // Get the greatest common divisor.
        var gcd = XBigInteger.GreatestCommonDivisor(num, den);

        // If we found one greater than 1, divide.
        if (gcd > 1)
        {
            num /= gcd;
            den /= gcd;
        }

        return (num, den);
    }

    /// <summary>
    /// Set both values. This is useful for when you don't want to update both parts of the rational
    /// but don't need a new object. Small efficiency gain.
    /// </summary>
    /// <param name="num">New value for the numerator.</param>
    /// <param name="den">New value for the denominator.</param>
    public void Set(BigInteger num, BigInteger den)
    {
        Numerator = num;
        Denominator = den;
    }

    #endregion Miscellaneous methods

    #region Arithmetic methods

    /// <summary>
    /// Find the reciprocal.
    /// Slightly faster than using 1/br.
    /// </summary>
    public static BigRational Reciprocal(BigRational br)
    {
        return new BigRational(br.Denominator, br.Numerator);
    }

    /// <summary>
    /// Exponentiation (integer exponent).
    /// This version leverages BigInteger.Pow().
    /// </summary>
    public static BigRational Pow(BigRational br, int exp)
    {
        // Optimizations.
        switch (exp)
        {
            case 0:
                return One;

            case 1:
                return br.Clone();

            case -1:
                return Reciprocal(br);
        }

        // Raise the numerator and denominator to the power of Abs(exp).
        var sign = int.Sign(exp);
        exp = int.Abs(exp);
        var num = BigInteger.Pow(br.Numerator, exp);
        var den = BigInteger.Pow(br.Denominator, exp);

        // If the sign is negative, invert the rational.
        return sign < 0 ? new BigRational(den, num) : new BigRational(num, den);
    }

    /// <summary>
    /// Exponentiation (double exponent).
    /// </summary>
    public static BigRational Pow(BigRational br, double exp)
    {
        return Math.Pow((double)br, exp);
    }

    /// <summary>
    /// Exponentiation (rational exponent).
    /// </summary>
    public static BigRational Pow(BigRational br, BigRational exp)
    {
        return Pow(br, (double)exp);
    }

    /// <summary>
    /// Find the square root of a rational as a rational.
    /// </summary>
    public static BigRational Sqrt(BigRational br)
    {
        return Math.Sqrt((double)br);
    }

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <summary>
    /// Unary negation operator.
    /// </summary>
    public static BigRational operator -(BigRational br)
    {
        return new BigRational(-br.Numerator, br.Denominator);
    }

    /// <summary>
    /// Addition operator.
    /// </summary>
    public static BigRational operator +(BigRational br, BigRational br2)
    {
        var num = br.Numerator * br2.Denominator + br2.Numerator * br.Denominator;
        var den = br.Denominator * br2.Denominator;
        return new BigRational(num, den);
    }

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    public static BigRational operator -(BigRational br, BigRational br2)
    {
        return br + -br2;
    }

    /// <summary>
    /// Multiply a rational by a rational.
    /// </summary>
    public static BigRational operator *(BigRational br, BigRational br2)
    {
        return new BigRational(br.Numerator * br2.Numerator, br.Denominator * br2.Denominator);
    }

    /// <summary>
    /// Divide a rational by a rational.
    /// </summary>
    public static BigRational operator /(BigRational br, BigRational br2)
    {
        return new BigRational(br.Numerator * br2.Denominator, br.Denominator * br2.Numerator);
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    public static BigRational operator ^(BigRational br, BigRational br2)
    {
        return Pow(br, br2);
    }

    #endregion Arithmetic operators

    #region Comparison operators

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(BigRational br, BigRational br2)
    {
        return br.Equals(br2);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(BigRational br, BigRational br2)
    {
        return !(br == br2);
    }

    /// <summary>
    /// Less than operator.
    /// </summary>
    public static bool operator <(BigRational br, BigRational br2)
    {
        return (BigDecimal)br < (BigDecimal)br2;
    }

    /// <summary>
    /// Greater than operator.
    /// </summary>
    public static bool operator >(BigRational br, BigRational br2)
    {
        return (BigDecimal)br > (BigDecimal)br2;
    }

    /// <summary>
    /// Less than or equal to operator.
    /// </summary>
    public static bool operator <=(BigRational br, BigRational br2)
    {
        return br == br2 || br < br2;
    }

    /// <summary>
    /// Greater than or equal to operator.
    /// </summary>
    public static bool operator >=(BigRational br, BigRational br2)
    {
        return br == br2 || br > br2;
    }

    #endregion Comparison operators

    /// <inheritdoc />
    public static BigRational Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational AdditiveIdentity { get; }

    /// <inheritdoc />
    public static BigRational operator --(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational operator ++(BigRational value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigRational MultiplicativeIdentity { get; }
}
