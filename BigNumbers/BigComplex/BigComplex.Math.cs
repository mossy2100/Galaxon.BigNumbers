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
    /// <see cref="Complex.Abs"/>
    public static BigDecimal Abs(BigComplex z)
    {
        return BigDecimal.Hypot(z.Real, z.Imaginary);
    }

    /// <summary>This method isn't needed for practical purposes, just for the interface.</summary>
    /// <inheritdoc/>
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
    /// <see cref="Complex.FromPolarCoordinates"/>
    public static BigComplex FromPolarCoordinates(BigDecimal magnitude, BigDecimal phase)
    {
        return FromTuple(BigDecimal.PolarToCartesian(magnitude, phase));
    }

    #endregion Methods related to magnitude and phase

    #region Arithmetic methods

    /// <summary>Clone method.</summary>
    /// <param name="z">The BigComplex value to clone.</param>
    /// <returns>A new BigComplex with the same value as the parameter.</returns>
    public static BigComplex Clone(BigComplex z)
    {
        return new BigComplex(z.Real, z.Imaginary);
    }

    /// <summary>Negate method.</summary>
    /// <param name="z">The BigComplex value to negate.</param>
    /// <returns>The negation of the parameter.</returns>
    public static BigComplex Negate(BigComplex z)
    {
        return new BigComplex(-z.Real, -z.Imaginary);
    }

    /// <summary>Complex conjugate method.</summary>
    /// <param name="z">The BigComplex value to conjugate.</param>
    /// <returns>The complex conjugate of the argument.</returns>
    public static BigComplex Conjugate(BigComplex z)
    {
        return new BigComplex(z.Real, -z.Imaginary);
    }

    /// <summary>Addition method.</summary>
    /// <param name="z">The left-hand operand.</param>
    /// <param name="w">The right-hand operand.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigComplex Add(BigComplex z, BigComplex w)
    {
        return new BigComplex(z.Real + w.Real, z.Imaginary + w.Imaginary);
    }

    /// <summary>Subtraction method.</summary>
    /// <param name="z">The left-hand operand.</param>
    /// <param name="w">The right-hand operand.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static BigComplex Subtract(BigComplex z, BigComplex w)
    {
        return new BigComplex(z.Real - w.Real, z.Imaginary - w.Imaginary);
    }

    /// <summary>Multiply two BigComplex values.</summary>
    /// <param name="z">The left-hand operand.</param>
    /// <param name="w">The right-hand operand.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigComplex Multiply(BigComplex z, BigComplex w)
    {
        // Extract parts for convenience.
        var (a, b) = z.ToTuple();
        var (c, d) = w.ToTuple();

        // Calculate.
        return new BigComplex(a * c - b * d, a * d + b * c);
    }

    /// <summary>Divide one BigComplex by another.</summary>
    /// <param name="z">The left-hand operand.</param>
    /// <param name="w">The right-hand operand.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="DivideByZeroException">If w == 0</exception>
    public static BigComplex Divide(BigComplex z, BigComplex w)
    {
        // Guard.
        if (w == 0) throw new DivideByZeroException();

        // Extract parts for convenience.
        var (a, b) = z.ToTuple();
        var (c, d) = w.ToTuple();

        // Calculate.
        var e = c * c + d * d;
        return new BigComplex((a * c + b * d) / e, (b * c - a * d) / e);
    }

    /// <summary>Calculate the reciprocal of a BigComplex value.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The reciprocal of the BigComplex value.</returns>
    /// <exception cref="DivideByZeroException">If the BigComplex value is 0.</exception>
    public static BigComplex Reciprocal(BigComplex z)
    {
        return Divide(1, z);
    }

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <inheritdoc/>
    public static BigComplex operator +(BigComplex z)
    {
        return Clone(z);
    }

    /// <inheritdoc/>
    public static BigComplex operator -(BigComplex z)
    {
        return Negate(z);
    }

    /// <inheritdoc/>
    public static BigComplex operator +(BigComplex z, BigComplex w)
    {
        return Add(z, w);
    }

    /// <inheritdoc/>
    public static BigComplex operator -(BigComplex z, BigComplex w)
    {
        return Subtract(z, w);
    }

    /// <inheritdoc/>
    public static BigComplex operator --(BigComplex z)
    {
        return z - 1;
    }

    /// <inheritdoc/>
    public static BigComplex operator ++(BigComplex z)
    {
        return z + 1;
    }

    /// <inheritdoc/>
    public static BigComplex operator *(BigComplex z, BigComplex w)
    {
        return Multiply(z, w);
    }

    /// <inheritdoc/>
    public static BigComplex operator /(BigComplex z, BigComplex w)
    {
        return Divide(z, w);
    }

    /// <summary>Exponentiation operator. </summary>
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
