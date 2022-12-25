using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Types;

public partial struct BigDecimal : IFloatingPoint<BigDecimal>, ICloneable
{
    #region Constructors

    public BigDecimal(BigInteger significand, int exponent = 0, bool roundSigFigs = false)
    {
        // Round off to the maximum number of significant figures if requested.
        if (roundSigFigs)
        {
            (significand, exponent) = RoundSigFigs(significand, exponent, MaxSigFigs);
        }

        // Trim trailing 0s on the significand.
        (significand, exponent) = MakeCanonical(significand, exponent);

        // Set properties.
        Significand = significand;
        Exponent = exponent;
    }

    public BigDecimal() : this(0)
    {
    }

    #endregion Constructors

    #region Instance properties

    /// <summary>
    /// The part of a number in scientific notation or in floating-point representation, consisting
    /// of its significant digits.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Significand">Wikipedia: Significand</see>
    public BigInteger Significand { get; set; }

    /// <summary>The power of 10.</summary>
    public int Exponent { get; set; }

    /// <summary>
    /// The sign of the value. Same as for BigInteger.
    ///   -1 for negative
    ///   0 for zero
    ///   1 for positive
    /// <see cref="BigInteger.Sign" />
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger.sign?view=net-7.0" />
    /// </summary>
    public int Sign => Significand.Sign;

    /// <summary>
    /// Get the number of significant figures.
    /// </summary>
    public int NumSigFigs => Significand.NumDigits();

    #endregion Instance properties

    #region Static properties

    /// <summary>
    /// This value determines the maximum number of significant figures to store for a BigDecimal
    /// value.
    ///
    /// After any calculation, the result will be rounded to this many significant figures.
    /// This not only helps control memory usage by controlling the scale of the significand, but
    /// also determines when to halt numerical methods, e.g. for calculating a square root or
    /// logarithm.
    ///
    /// If the value is modified, only new objects and calculations are affected by it.
    /// If you want to reduce the number of significant figures in an existing value, use
    /// RoundSigFigs().
    ///
    /// I've made the default 101 to support the constants (Pi, E, etc.) that are provided to 100
    /// decimal places, i.e. 101 significant figures.
    /// </summary>
    private static int s_maxSigFigs = 101;
    public static int MaxSigFigs {
        get => s_maxSigFigs;

        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxSigFigs),
                    "Must be at least 1.");
            }
            s_maxSigFigs = value;
        }
    }

    /// <inheritdoc />
    public static BigDecimal Zero { get; } = new (0);

    /// <inheritdoc />
    public static BigDecimal One { get; } = new (1);

    /// <inheritdoc />
    public static BigDecimal NegativeOne { get; } = new (-1);

    /// <summary>
    /// Useful in division and square root operations.
    /// </summary>
    public static BigDecimal Two { get; } = new (2);

    /// <summary>
    /// Useful in various calculations.
    /// </summary>
    public static BigDecimal Ten { get; } = new (10);

    /// <inheritdoc />
    public static int Radix => 10;

    /// <inheritdoc />
    public static BigDecimal AdditiveIdentity => Zero;

    /// <inheritdoc />
    public static BigDecimal MultiplicativeIdentity => One;

    /// <summary>
    /// Maximum number of significant figures in a double value.
    /// </summary>
    private const int _DoubleMaxSigFigs = 17;

    #endregion Static properties

    #region Constants

    /// <inheritdoc />
    /// <remarks>
    /// Euler's number (e) to 100 decimal places.
    /// If you need more for testing or whatever, you can get up to 10,000 decimal places here:
    /// <see href="https://www.math.utah.edu/~pa/math/e" />
    /// To calculate e to a larger number of significant figures, set MaxSigFigs and call Exp(1).
    /// </remarks>
    public static BigDecimal E { get; } = Parse("2."
        + "7182818284 5904523536 0287471352 6624977572 4709369995 "
        + "9574966967 6277240766 3035354759 4571382178 5251664274", null);

    /// <inheritdoc />
    /// <remarks>
    /// The circle constant (π) to 100 decimal places.
    /// If you need more, you can get up to 10,000 decimal places here:
    /// <see href="https://www.math.utah.edu/~pa/math/pi" />
    /// </remarks>
    public static BigDecimal Pi { get; } = Parse("3."
        + "1415926535 8979323846 2643383279 5028841971 6939937510 "
        + "5820974944 5923078164 0628620899 8628034825 3421170680", null);

    /// <inheritdoc />
    /// <remarks>
    /// The other circle constant (τ = 2π) to 100 decimal places.
    /// If you need more, you can get up to 10,000 decimal places here:
    /// <see href="https://tauday.com/tau-digits" />
    /// </remarks>
    public static BigDecimal Tau { get; } = Parse("6."
        + "2831853071 7958647692 5286766559 0057683943 3879875021 "
        + "1641949889 1846156328 1257241799 7256069650 6842341360", null);

    /// <summary>
    /// The golden ratio (φ) to 100 decimal places.
    /// </summary>
    public static BigDecimal Phi { get; } = Parse("1."
        + "6180339887 4989484820 4586834365 6381177203 0917980576 "
        + "2862135448 6227052604 6281890244 9707207204 1893911375", null);

    /// <summary>
    /// The square root of 2 to 100 decimal places.
    /// </summary>
    public static BigDecimal Sqrt2 { get; } = Parse("1."
        + "4142135623 7309504880 1688724209 6980785696 7187537694 "
        + "8073176679 7379907324 7846210703 8850387534 3276415727", null);

    /// <summary>
    /// The square root of 10 to 100 decimal places.
    /// </summary>
    public static BigDecimal Sqrt10 { get; } = Parse("3."
        + "1622776601 6837933199 8893544432 7185337195 5513932521 "
        + "6826857504 8527925944 3863923822 1344248108 3793002952", null);

    /// <summary>
    /// The natural logarithm of 2 to 100 decimal places.
    /// </summary>
    public static BigDecimal Ln2 { get; } = Parse("0."
        + "6931471805 5994530941 7232121458 1765680755 0013436025 "
        + "5254120680 0094933936 2196969471 5605863326 9964186875", null);

    /// <summary>
    /// The natural logarithm of 10 to 100 decimal places.
    /// </summary>
    public static BigDecimal Ln10 { get; } = Parse("2."
        + "3025850929 9404568401 7991454684 3642076011 0148862877 "
        + "2976033327 9009675726 0967735248 0235997205 0895982983", null);

    #endregion Constants

    #region Methods related to data transfer

    /// <inheritdoc />
    public int GetSignificandByteCount() =>
        Significand.GetByteCount();

    /// <inheritdoc />
    public int GetSignificandBitLength() =>
        GetSignificandByteCount() * 8;

    /// <inheritdoc />
    public int GetExponentByteCount() =>
        4;

    /// <inheritdoc />
    public int GetExponentShortestBitLength() =>
        32;

    /// <inheritdoc />
    public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Significand, destination, out bytesWritten, true);

    /// <inheritdoc />
    public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Significand, destination, out bytesWritten, false);

    /// <inheritdoc />
    public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteInt(Exponent, destination, out bytesWritten, true);

    /// <inheritdoc />
    public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteInt(Exponent, destination, out bytesWritten, false);

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteBigInteger" />
    /// <see cref="TryWriteInt" />
    /// </summary>
    private static bool TryWrite(byte[] bytes, Span<byte> destination, out int bytesWritten)
    {
        try
        {
            bytes.CopyTo(destination);
            bytesWritten = bytes.Length;
            return true;
        }
        catch
        {
            bytesWritten = 0;
            return false;
        }
    }

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteSignificandBigEndian" />
    /// <see cref="TryWriteSignificandLittleEndian" />
    /// </summary>
    private static bool TryWriteBigInteger(BigInteger bi, Span<byte> destination,
        out int bytesWritten,
        bool isBigEndian)
    {
        byte[] bytes = bi.ToByteArray(false, isBigEndian);
        return TryWrite(bytes, destination, out bytesWritten);
    }

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteExponentBigEndian" />
    /// <see cref="TryWriteExponentLittleEndian" />
    /// </summary>
    private static bool TryWriteInt(int i, Span<byte> destination, out int bytesWritten,
        bool isBigEndian)
    {
        // Get the bytes.
        byte[] bytes = BitConverter.GetBytes(i);

        // Check if the requested endianness matches the architecture. If not, reverse the array.
        if (BitConverter.IsLittleEndian && isBigEndian
            || !BitConverter.IsLittleEndian && !isBigEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }

        return TryWrite(bytes, destination, out bytesWritten);
    }

    #endregion Methods related to data transfer

    #region Inspection methods

    /// <summary>
    /// Checks if the value is in its canonical state.
    /// In this case, the value should not be evenly divisible by 10. In canonical form, a
    /// multiple of 10 should be shortened and the exponent increased.
    /// </summary>
    public static bool IsCanonical(BigDecimal value) =>
        value.Significand % 10 != 0;

    /// <summary>
    /// Check if the value is a complex number.
    /// </summary>
    public static bool IsComplexNumber(BigDecimal value) =>
        false;

    /// <summary>
    /// The value will be an integer if in canonical form and the exponent is >= 0.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsInteger(BigDecimal value) =>
        value.MakeCanonical().Exponent >= 0;

    /// <inheritdoc />
    public static bool IsOddInteger(BigDecimal value)
    {
        if (!IsInteger(value))
        {
            return false;
        }

        // If the exponent is > 0 then it's a multiple of 10, and therefore even.
        if (value.Exponent > 0)
        {
            return false;
        }

        // If the exponent is 0, check if it's odd.
        return BigInteger.IsOddInteger(value.Significand);
    }

    /// <inheritdoc />
    public static bool IsEvenInteger(BigDecimal value)
    {
        if (!IsInteger(value))
        {
            return false;
        }

        // If the exponent is > 0 then it's a multiple of 10, and therefore even.
        if (value.Exponent > 0)
        {
            return true;
        }

        // If the exponent is 0, check if it's even.
        return BigInteger.IsEvenInteger(value.Significand);
    }

    /// <inheritdoc />
    public static bool IsZero(BigDecimal value) =>
        value.Significand == 0;

    /// <inheritdoc />
    public static bool IsNegative(BigDecimal value) =>
        value.Significand < 0;

    /// <inheritdoc />
    public static bool IsPositive(BigDecimal value) =>
        value.Significand > 0;

    /// <inheritdoc />
    public static bool IsFinite(BigDecimal value) =>
        true;

    /// <inheritdoc />
    public static bool IsInfinity(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsNegativeInfinity(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsPositiveInfinity(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsRealNumber(BigDecimal value) =>
        true;

    /// <inheritdoc />
    public static bool IsImaginaryNumber(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsNormal(BigDecimal value) =>
        true;

    /// <inheritdoc />
    public static bool IsSubnormal(BigDecimal value) =>
        false;

    /// <inheritdoc />
    public static bool IsNaN(BigDecimal value) =>
        false;

    #endregion Inspection methods

    #region Comparison methods

    /// <inheritdoc />
    public int CompareTo(BigDecimal other)
    {
        if (Sign < other.Sign)
        {
            return -1;
        }
        if (Sign > other.Sign)
        {
            return 1;
        }
        (BigDecimal x, BigDecimal y) = Align(this, other);
        if (x.Significand < y.Significand)
        {
            return -1;
        }
        if (x.Significand > y.Significand)
        {
            return 1;
        }
        return 0;
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is not BigDecimal other)
        {
            throw new ArgumentInvalidException(nameof(obj), "Must be a BigDecimal.");
        }
        return CompareTo(other);
    }

    /// <inheritdoc />
    public bool Equals(BigDecimal other) =>
        CompareTo(other) == 0;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is BigDecimal bd)
        {
            return Equals(bd);
        }
        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(Significand, Exponent);

    /// <inheritdoc />
    public static BigDecimal MaxMagnitude(BigDecimal x, BigDecimal y) =>
        x > y ? x : y;

    /// <inheritdoc />
    public static BigDecimal MaxMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MaxMagnitude(x, y);

    /// <inheritdoc />
    public static BigDecimal MinMagnitude(BigDecimal x, BigDecimal y) =>
        x < y ? x : y;

    /// <inheritdoc />
    public static BigDecimal MinMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MinMagnitude(x, y);

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc />
    public static bool operator ==(BigDecimal x, BigDecimal y) =>
        x.Equals(y);

    /// <inheritdoc />
    public static bool operator !=(BigDecimal x, BigDecimal y) =>
        !x.Equals(y);

    /// <inheritdoc />
    public static bool operator <(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) < 0;

    /// <inheritdoc />
    public static bool operator <=(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) <= 0;

    /// <inheritdoc />
    public static bool operator >(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) > 0;

    /// <inheritdoc />
    public static bool operator >=(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) >= 0;

    #endregion Comparison operators
}
