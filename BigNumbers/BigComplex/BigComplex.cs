using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex :
    IPowerFunctions<BigComplex>,
    IRootFunctions<BigComplex>,
    IExponentialFunctions<BigComplex>,
    ILogarithmicFunctions<BigComplex>,
    ITrigonometricFunctions<BigComplex>,
    IHyperbolicFunctions<BigComplex>
{
    #region Instance fields and properties

    /// <summary>The real part of the BigComplex number.</summary>
    public BigDecimal Real { get; set; }

    /// <summary>The imaginary part of the BigComplex number.</summary>
    public BigDecimal Imaginary { get; set; }

    /// <summary>The magnitude (or absolute value) of the BigComplex number.</summary>
    public readonly BigDecimal Magnitude => Abs(this);

    /// <summary>The phase angle of the BigComplex number.</summary>
    public readonly BigDecimal Phase => BigDecimal.Atan2(Imaginary, Real);

    #endregion Instance fields and properties

    #region Static fields and properties

    /// <inheritdoc/>
    public static BigComplex Zero { get; } = new (0, 0);

    /// <inheritdoc/>
    public static BigComplex One { get; } = new (1, 0);

    /// <summary>Value of the imaginary unit, equal to Sqrt(-1).</summary>
    /// <see cref="Complex.ImaginaryOne"/>
    public static BigComplex ImaginaryOne { get; } = new (0, 1);

    /// <summary>Convenient shorthand for ImaginaryOne.</summary>
    public static BigComplex I { get; } = ImaginaryOne;

    /// <inheritdoc/>
    public static int Radix { get; } = 10;

    /// <inheritdoc/>
    public static BigComplex AdditiveIdentity { get; } = Zero;

    /// <inheritdoc/>
    public static BigComplex MultiplicativeIdentity { get; } = One;

    #endregion Static fields and properties

    #region Constructors

    /// <summary>
    /// Construct a BigComplex from 2 BigDecimal values.
    /// </summary>
    /// <param name="real">The real part.</param>
    /// <param name="imaginary">The imaginary part.</param>
    public BigComplex(BigDecimal real, BigDecimal imaginary)
    {
        // Assign parts.
        Real = real;
        Imaginary = imaginary;
    }

    /// <summary>
    /// Construct a BigComplex from a single BigDecimal value.
    /// </summary>
    /// <param name="real">The real part.</param>
    public BigComplex(BigDecimal real) : this(real, 0)
    {
    }

    /// <summary>
    /// Construct a zero BigComplex.
    /// </summary>
    public BigComplex() : this(0, 0)
    {
    }

    /// <summary>
    /// Construct BigComplex from an tuple of 2 BigDecimal values.
    /// </summary>
    /// <param name="complex">The tuple.</param>
    public BigComplex((BigDecimal, BigDecimal) complex) : this(complex.Item1, complex.Item2)
    {
    }

    /// <summary>
    /// Construct BigComplex from an array of 2 BigDecimal values.
    /// </summary>
    /// <param name="complex">The array.</param>
    /// <exception cref="ArgumentException">If the array does not contain exactly 2
    /// values.</exception>
    public BigComplex(BigDecimal[] complex)
    {
        // Guard.
        if (complex.Length != 2)
        {
            throw new ArgumentException("The array must contain exactly two elements.");
        }

        // Assign parts.
        Real = complex[0];
        Imaginary = complex[1];
    }

    #endregion Constructors
}
