using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics;

public partial struct BigDecimal : IFloatingPoint<BigDecimal>
{
    #region Constructors

    /// <summary>
    /// Main constructor.
    /// </summary>
    /// <param name="significand">The significand or mantissa.</param>
    /// <param name="exponent">The exponent.</param>
    /// <param name="roundSigFigs">
    /// If the value should be rounded off to the current value of MaxSigFigs.
    /// </param>
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

    /// <summary>
    /// Default constructor.
    /// </summary>
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
    /// The sign of the value. The same convention is used as for BigInteger except sbyte is used
    /// instead of an int.
    /// -1 for negative
    /// 0 for zero
    /// 1 for positive
    /// </summary>
    /// <see cref="BigInteger.Sign" />
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.biginteger.sign?view=net-7.0" />
    public sbyte Sign => (sbyte)Significand.Sign;

    /// <summary>
    /// Get the number of significant figures.
    /// </summary>
    public int NumSigFigs => Significand.NumDigits();

    #endregion Instance properties

    #region Static properties

    /// <summary>
    /// This property determines the maximum number of significant figures to keep in a BigDecimal
    /// value.
    /// After any calculation, the result will be rounded to this many significant figures.
    /// This not only helps control memory usage by controlling the size of the significand, but
    /// also determines when to halt numerical methods, e.g. for calculating a square root or
    /// logarithm.
    /// If this property is modified, only new objects and calculations are affected by it.
    /// If you want to reduce the number of significant figures in an existing value, use
    /// RoundSigFigs().
    /// </summary>
    public static int MaxSigFigs
    {
        get => s_maxSigFigs;

        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxSigFigs), "Must be at least 1.");
            }
            s_maxSigFigs = value;
        }
    }

    /// <summary>
    /// Private backing field for MaxSigFigs.
    /// </summary>
    private static int s_maxSigFigs = 100;

    /// <inheritdoc />
    public static BigDecimal Zero { get; } = new (0);

    /// <inheritdoc />
    public static BigDecimal One { get; } = new (1);

    /// <inheritdoc />
    public static BigDecimal NegativeOne { get; } = new (-1);

    /// <inheritdoc />
    public static int Radix => 10;

    /// <inheritdoc />
    public static BigDecimal AdditiveIdentity => Zero;

    /// <inheritdoc />
    public static BigDecimal MultiplicativeIdentity => One;

    /// <summary>Precision supported by the Half type.</summary>
    /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation" />
    public const int HalfPrecision = 5;

    /// <summary>Precision supported by the float type.</summary>
    /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation" />
    public const int FloatPrecision = 9;

    /// <summary>Precision supported by the double type.</summary>
    /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation" />
    public const int DoublePrecision = 17;

    /// <summary>Precision supported by the decimal type.</summary>
    public const int DecimalPrecision = 28;

    #endregion Static properties

    #region Inspection methods

    /// <summary>
    /// Checks if the value is in its canonical state.
    /// In this case, the value should not be evenly divisible by 10. In canonical form, a
    /// multiple of 10 should be shortened and the exponent increased.
    /// </summary>
    public static bool IsCanonical(BigDecimal value) =>
        value == Zero || value.Significand % 10 != 0;

    /// <summary>
    /// Check if the value is a complex number.
    /// </summary>
    public static bool IsComplexNumber(BigDecimal value) => false;

    /// <summary>
    /// The value will be an integer if in canonical form and the exponent is >= 0.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsInteger(BigDecimal value) => value.MakeCanonical().Exponent >= 0;

    /// <inheritdoc />
    public static bool IsOddInteger(BigDecimal value) =>
        IsInteger(value) && value.Exponent == 0 && BigInteger.IsOddInteger(value.Significand);

    /// <inheritdoc />
    public static bool IsEvenInteger(BigDecimal value) =>
        IsInteger(value) && (value.Exponent > 0 || BigInteger.IsEvenInteger(value.Significand));

    /// <inheritdoc />
    public static bool IsZero(BigDecimal value) => value.Significand == 0;

    /// <inheritdoc />
    public static bool IsNegative(BigDecimal value) => value.Significand < 0;

    /// <inheritdoc />
    public static bool IsPositive(BigDecimal value) => value.Significand > 0;

    /// <inheritdoc />
    public static bool IsFinite(BigDecimal value) => true;

    /// <inheritdoc />
    public static bool IsInfinity(BigDecimal value) => false;

    /// <inheritdoc />
    public static bool IsNegativeInfinity(BigDecimal value) => false;

    /// <inheritdoc />
    public static bool IsPositiveInfinity(BigDecimal value) => false;

    /// <inheritdoc />
    public static bool IsRealNumber(BigDecimal value) => true;

    /// <inheritdoc />
    public static bool IsImaginaryNumber(BigDecimal value) => false;

    /// <inheritdoc />
    public static bool IsNormal(BigDecimal value) => value != 0;

    /// <inheritdoc />
    public static bool IsSubnormal(BigDecimal value) => false;

    /// <inheritdoc />
    public static bool IsNaN(BigDecimal value) => false;

    #endregion Inspection methods

    #region Methods related to data transfer

    /// <inheritdoc />
    public int GetSignificandByteCount() => Significand.GetByteCount();

    /// <inheritdoc />
    public int GetSignificandBitLength() => GetSignificandByteCount() * 8;

    /// <inheritdoc />
    public int GetExponentByteCount() => 4;

    /// <inheritdoc />
    public int GetExponentShortestBitLength() => 32;

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
        var bytes = bi.ToByteArray(false, isBigEndian);
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
        var bytes = BitConverter.GetBytes(i);

        // Check if the requested endianness matches the architecture. If not, reverse the array.
        if (BitConverter.IsLittleEndian && isBigEndian
            || !BitConverter.IsLittleEndian && !isBigEndian)
        {
            bytes = bytes.Reverse().ToArray();
        }

        return TryWrite(bytes, destination, out bytesWritten);
    }

    #endregion Methods related to data transfer
}
