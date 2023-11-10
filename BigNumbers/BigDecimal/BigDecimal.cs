using System.Numerics;

namespace Galaxon.BigNumbers;

/// <summary>
/// Core partial struct for the BigDecimal type.
/// Contains the core fields and properties, and the constructors.
/// The other partials contain members grouped by purpose.
///
/// A BigDecimal is represented internally by a BigInteger value representing the significand,
/// and an int representing the exponent. The value of a BigDecimal can easily be calculated from:
///     value = Significand * 10^Exponent
///
/// No trailing zeros are retained in the significand; rather, the exponent is adjusted instead.
/// This minimised the size of the BigInteger being used to store the value. This varies from the
/// scientific meaning of significant digits, which can include significant trailing zeros, but it
/// seems like the right design choice here.
/// </summary>
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

    /// <summary>Backing field for the Significand property.</summary>
    private BigInteger _significand;

    /// <summary>
    /// The part of a number in scientific notation or in floating-point representation consisting
    /// of its significant digits. Also known as the mantissa.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Significand"/>
    public BigInteger Significand
    {
        readonly get => _significand;

        set
        {
            if (value != _significand)
            {
                // Update the significant and exponent fields together, making sure the BigDecimal
                // remains in canonical form.
                (_significand, _exponent) = _MakeCanonical(value, _exponent);

                // Null the digit string, which will now be different.
                _digitString = null;
            }
        }
    }

    /// <summary>Backing field for the Exponent property.</summary>
    private int _exponent;

    /// <summary>The power of 10.</summary>
    public int Exponent
    {
        readonly get => _exponent;

        set => _exponent = value;
    }

    /// <summary>The sign of the value (-1, 0, or 1).</summary>
    /// <remarks>
    /// The same convention is used as for BigInteger:
    ///    -1 means negative
    ///     0 means zero
    ///     1 means positive
    /// </remarks>
    /// <see cref="BigInteger.Sign"/>
    public readonly int Sign => Significand.Sign;

    /// <summary>Backing field for the DigitsString property.</summary>
    private string? _digitString;

    /// <summary>
    /// A string containing the digits of the absolute value of the significand.
    /// This will not include a leading minus sign if the significand is negative.
    /// Also, because the BigDecimal is maintained in canonical form, this string will not have any
    /// leading or trailing zeros, unless the BigDecimal is actually equal to 0.
    /// </summary>
    public string DigitsString
    {
        get
        {
            _digitString ??= BigInteger.Abs(Significand).ToString();
            return _digitString;
        }
    }

    /// <summary>Get the number of significant figures.</summary>
    public int NumSigFigs => DigitsString.Length;

    #endregion Instance fields and properties

    #region Static fields and properties

    /// <summary>Private backing field for MaxSigFigs.</summary>
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

    // /// <summary>Precision supported by the Half type.</summary>
    // /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation"/>
    // public const int HalfPrecision = 5;
    //
    // /// <summary>Precision supported by the float type.</summary>
    // /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation"/>
    // public const int FloatPrecision = 9;
    //
    // /// <summary>Precision supported by the double type.</summary>
    // /// <see href="https://en.wikipedia.org/wiki/IEEE_754#Character_representation"/>
    // public const int DoublePrecision = 17;

    /// <summary>Precision supported by the decimal type.</summary>
    private const int _DECIMAL_PRECISION = 28;

    #endregion Static fields and properties

    #region Constructors

    /// <summary>
    /// Construct a BigDecimal from a BigInteger significand and an integer exponent.
    /// </summary>
    /// <param name="significand">The significand or mantissa.</param>
    /// <param name="exponent">The exponent (defaults to 0).</param>
    public BigDecimal(BigInteger significand, int exponent = 0)
    {
        // Make sure the values are canonical and set the fields.
        (_significand, _exponent) = _MakeCanonical(significand, exponent);
    }

    /// <summary>Construct a zero BigDecimal.</summary>
    public BigDecimal()
    {
        (_significand, _exponent) = (0, 0);
    }

    #endregion Constructors
}
