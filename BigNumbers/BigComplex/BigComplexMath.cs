using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Methods related to magnitude and phase

    /// <summary>
    /// Calculate the absolute value of the BigComplex value (also known as magnitude).
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The magnitude of the argument.</returns>
    /// <see cref="Complex.Abs" />
    public static BigDecimal Abs(BigComplex z)
    {
        return BigDecimal.Hypot(z.Real, z.Imaginary);
    }

    /// <summary>This method isn't needed for practical purposes, just for the interface.</summary>
    /// <inheritdoc />
    static BigComplex INumberBase<BigComplex>.Abs(BigComplex z)
    {
        return Abs(z);
    }

    /// <summary>
    /// Construct a complex number from the magnitude and phase.
    /// </summary>
    /// <param name="magnitude">The magnitude of the complex number.</param>
    /// <param name="phase">The phase angle in radians.</param>
    /// <returns>The new BigComplex number.</returns>
    /// <see cref="Complex.FromPolarCoordinates" />
    public static BigComplex FromPolarCoordinates(BigDecimal magnitude, BigDecimal phase)
    {
        return new BigComplex(BigDecimal.PolarToCartesian(magnitude, phase));
    }

    #endregion Methods related to magnitude and phase

    #region Arithmetic methods

    /// <summary>
    /// Clone method.
    /// </summary>
    /// <returns>A copy of the argument.</returns>
    public static BigComplex Clone(BigComplex z)
    {
        return new BigComplex(z.Real, z.Imaginary);
    }

    /// <summary>
    /// Negate method.
    /// </summary>
    /// <returns>The negation of the argument.</returns>
    public static BigComplex Negate(BigComplex z)
    {
        return new BigComplex(-z.Real, -z.Imaginary);
    }

    /// <summary>
    /// Complex conjugate method.
    /// </summary>
    /// <returns>The complex conjugate of the argument.</returns>
    public static BigComplex Conjugate(BigComplex z)
    {
        return new BigComplex(z.Real, -z.Imaginary);
    }

    /// <summary>
    /// Calculate reciprocal.
    /// </summary>
    /// <returns>The reciprocal of the argument.</returns>
    public static BigComplex Reciprocal(BigComplex z)
    {
        return Divide(1, z);
    }

    /// <summary>
    /// Addition method.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigComplex Add(BigComplex z1, BigComplex z2)
    {
        return new BigComplex(z1.Real + z2.Real, z1.Imaginary + z2.Imaginary);
    }

    /// <summary>
    /// Subtraction method.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static BigComplex Subtract(BigComplex z1, BigComplex z2)
    {
        return new BigComplex(z1.Real - z2.Real, z1.Imaginary - z2.Imaginary);
    }

    /// <summary>
    /// Multiply two BigComplex values.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigComplex Multiply(BigComplex z1, BigComplex z2)
    {
        var a = z1.Real;
        var b = z1.Imaginary;
        var c = z2.Real;
        var d = z2.Imaginary;
        return new BigComplex(a * c - b * d, a * d + b * c);
    }

    /// <summary>
    /// Divide a BigComplex by a BigComplex.
    /// </summary>
    /// <param name="z1">The left-hand BigComplex number.</param>
    /// <param name="z2">The right-hand BigComplex number.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If z2 == 0</exception>
    public static BigComplex Divide(BigComplex z1, BigComplex z2)
    {
        // Guard.
        if (z2 == 0) throw new DivideByZeroException();

        // Extract parts for convenience.
        var (a, b) = z1.ToTuple();
        var (c, d) = z2.ToTuple();
        var e = c * c + d * d;
        return new BigComplex((a * c + b * d) / e, (b * c - a * d) / e);
    }

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <inheritdoc />
    public static BigComplex operator +(BigComplex z)
    {
        return Clone(z);
    }

    /// <inheritdoc />
    public static BigComplex operator -(BigComplex z)
    {
        return Negate(z);
    }

    /// <summary>
    /// Complex conjugate operator.
    /// The use of the tilde (~) for this operator is non-standard, but it seems a good fit and
    /// could be useful.
    /// </summary>
    /// <returns>The complex conjugate of the operand.</returns>
    public static BigComplex operator ~(BigComplex z)
    {
        return Conjugate(z);
    }

    /// <inheritdoc />
    public static BigComplex operator +(BigComplex z1, BigComplex z2)
    {
        return Add(z1, z2);
    }

    /// <inheritdoc />
    public static BigComplex operator -(BigComplex z1, BigComplex z2)
    {
        return Subtract(z1, z2);
    }

    /// <inheritdoc />
    public static BigComplex operator --(BigComplex z)
    {
        return z - 1;
    }

    /// <inheritdoc />
    public static BigComplex operator ++(BigComplex z)
    {
        return z + 1;
    }

    /// <inheritdoc />
    public static BigComplex operator *(BigComplex z1, BigComplex z2)
    {
        return Multiply(z1, z2);
    }

    /// <inheritdoc />
    public static BigComplex operator /(BigComplex z1, BigComplex z2)
    {
        return Divide(z1, z2);
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The first operand raised to the power of the second.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent is negative or imaginary.
    /// </exception>
    public static BigComplex operator ^(BigComplex z, BigComplex w)
    {
        return Pow(z, w);
    }

    #endregion Arithmetic operators
}
