using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Calculated properties (same as Complex)

    public BigDecimal Magnitude
    {
        readonly get => BigDecimal.Hypot(Real, Imaginary);

        set => (Real, Imaginary) = PolarToCartesian(value, Phase);
    }

    public BigDecimal Phase
    {
        readonly get => BigDecimal.Atan2(Imaginary, Real);

        set => (Real, Imaginary) = PolarToCartesian(Magnitude, value);
    }

    #endregion Calculated properties (same as Complex)

    #region Methods equivalent to those provided by Complex.

    /// <summary>
    /// Calculate absolute value (also known as magnitude).
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The magnitude of the argument.</returns>
    /// <see cref="Complex.Abs" />
    public static BigDecimal Abs(BigComplex z)
    {
        return BigDecimal.Hypot(z.Real, z.Imaginary);
    }

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
        return new BigComplex(PolarToCartesian(magnitude, phase));
    }

    #endregion Methods equivalent to those provided by Complex.

    #region Helper functions

    private static (BigDecimal x, BigDecimal y) PolarToCartesian(BigDecimal r, BigDecimal a)
    {
        return (r * BigDecimal.Cos(a), r * BigDecimal.Sin(a));
    }

    private static (BigDecimal r, BigDecimal a) CartesianToPolar(BigDecimal x, BigDecimal y)
    {
        return (BigDecimal.Hypot(x, y), BigDecimal.Atan2(y, x));
    }

    #endregion Helper functions
}
