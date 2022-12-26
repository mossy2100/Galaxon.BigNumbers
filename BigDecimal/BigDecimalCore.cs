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
    /// </summary>
    private static int s_maxSigFigs = 100;
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

    /// <summary>
    /// Cached value for e.
    /// </summary>
    private static BigDecimal _e;

    /// <inheritdoc />
    public static BigDecimal E
    {
        get
        {
            if (_e.NumSigFigs <= MaxSigFigs)
            {
                return RoundSigFigsMax(_e);
            }
            _e = ComputeE();
            return _e;
        }
    }

    /// <summary>
    /// Cached value for π.
    /// </summary>
    private static BigDecimal _pi;

    /// <inheritdoc />
    public static BigDecimal Pi
    {
        get
        {
            if (_pi.NumSigFigs <= MaxSigFigs)
            {
                return RoundSigFigsMax(_pi);
            }
            _pi = ComputePi();
            return _pi;
        }
    }

    /// <summary>
    /// Cached value for τ.
    /// </summary>
    private static BigDecimal _tau;

    /// <inheritdoc />
    public static BigDecimal Tau
    {
        get
        {
            if (_tau.NumSigFigs <= MaxSigFigs)
            {
                return RoundSigFigsMax(_tau);
            }
            _tau = ComputeTau();
            return _tau;
        }
    }

    /// <summary>
    /// Cached value for φ.
    /// </summary>
    private static BigDecimal _phi;

    /// <summary>
    /// The golden ratio (φ).
    /// </summary>
    public static BigDecimal Phi
    {
        get
        {
            if (_phi.NumSigFigs <= MaxSigFigs)
            {
                return RoundSigFigsMax(_phi);
            }
            _phi = ComputePhi();
            return _phi;
        }
    }

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
