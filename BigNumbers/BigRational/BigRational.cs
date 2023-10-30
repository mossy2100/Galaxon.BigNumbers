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
    IComparable,
    IComparable<BigRational>,
    IComparisonOperators<BigRational, BigRational, bool>
{
    #region Instance fields and properties

    /// <summary>The numerator of the rational number.</summary>
    /// <remarks>This can be any integer.</remarks>
    public BigInteger Numerator { get; set; }

    /// <summary>The denominator of the rational number.</summary>
    /// <remarks>
    /// This value should always be positive.
    /// It should never 0 because this would not be a rational number.
    /// It should also never be negative. The sign of the rational is determined by the numerator.
    /// </remarks>
    public BigInteger Denominator { get; set; }

    /// <summary>The sign of the value.</summary>
    /// <remarks>
    /// The same convention is used as for BigInteger:
    /// -1 means negative
    /// 0 means zero
    /// 1 means positive
    /// </remarks>
    /// <see cref="BigInteger.Sign"/>
    public readonly int Sign => Numerator.Sign;

    #endregion Instance fields and properties

    #region Static fields and properties

    /// <inheritdoc/>
    public static BigRational Zero => new (0, 1);

    /// <inheritdoc/>
    public static BigRational One => new (1, 1);

    /// <inheritdoc/>
    public static int Radix { get; } = 10;

    /// <inheritdoc/>
    public static BigRational AdditiveIdentity { get; } = Zero;

    /// <inheritdoc/>
    public static BigRational MultiplicativeIdentity { get; } = One;

    #endregion Instance fields and properties

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

    /// <summary>Construct a BigRational from a single integer, taken to be the numerator.</summary>
    /// <param name="num">The numerator.</param>
    public BigRational(BigInteger num) : this(num, 1) { }

    /// <summary>Construct a zero BigRational.</summary>
    public BigRational() : this(0, 1) { }

    #endregion Constructors
}
