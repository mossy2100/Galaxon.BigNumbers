using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal :
    IFloatingPoint<BigDecimal>,
    IPowerFunctions<BigDecimal>,
    IRootFunctions<BigDecimal>,
    IExponentialFunctions<BigDecimal>,
    ILogarithmicFunctions<BigDecimal>,
    ITrigonometricFunctions<BigDecimal>,
    IHyperbolicFunctions<BigDecimal>
{
    #region Instance fields and properties

    /// <summary>
    /// The part of a number in scientific notation or in floating-point representation, consisting
    /// of its significant digits.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Significand">Wikipedia: Significand</see>
    public BigInteger Significand { get; set; }

    /// <summary>The power of 10.</summary>
    public int Exponent { get; set; }

    /// <summary>The sign of the value.</summary>
    /// <remarks>
    /// The same convention is used as for BigInteger:
    ///    -1 means negative
    ///     0 means zero
    ///     1 means positive
    /// </remarks>
    /// <see cref="BigInteger.Sign"/>
    public readonly int Sign => Significand.Sign;

    /// <summary>
    /// Get the number of significant figures.
    /// </summary>
    public readonly int NumSigFigs => Significand.NumDigits();

    #endregion Instance fields and properties

    #region Static fields and properties

    /// <summary>
    /// Private backing field for MaxSigFigs.
    /// </summary>
    private static int _maxSigFigs = 100;

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
        get => _maxSigFigs;

        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(MaxSigFigs), "Must be at least 1.");
            }

            _maxSigFigs = value;
        }
    }

    /// <inheritdoc/>
    public static BigDecimal Zero { get; } = new (0);

    /// <inheritdoc/>
    public static BigDecimal One { get; } = new (1);

    /// <inheritdoc/>
    public static BigDecimal NegativeOne { get; } = new (-1);

    /// <inheritdoc/>
    public static int Radix { get; } = 10;

    /// <inheritdoc/>
    public static BigDecimal AdditiveIdentity { get; } = Zero;

    /// <inheritdoc/>
    public static BigDecimal MultiplicativeIdentity { get; } = One;

    /// <summary>Precision supported by the Half type.</summary>
    /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation"/>
    public const int HalfPrecision = 5;

    /// <summary>Precision supported by the float type.</summary>
    /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation"/>
    public const int FloatPrecision = 9;

    /// <summary>Precision supported by the double type.</summary>
    /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation"/>
    public const int DoublePrecision = 17;

    /// <summary>Precision supported by the decimal type.</summary>
    public const int DecimalPrecision = 28;

    #endregion Static fields and properties

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
        // If the significant is 0, make sure the exponent is also 0.
        if (significand == 0)
        {
            Significand = 0;
            Exponent = 0;
            return;
        }

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
}
