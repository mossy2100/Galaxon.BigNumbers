namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Trigonometric methods

    public static BigComplex Sin(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Sin(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Cos(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex SinPi(BigComplex z)
    {
        return Sin(z * Pi);
    }

    public static BigComplex Cos(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Cos(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Sin(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, -b);
    }

    /// <inheritdoc/>
    public static BigComplex CosPi(BigComplex z)
    {
        return Cos(z * Pi);
    }

    /// <inheritdoc/>
    public static (BigComplex Sin, BigComplex Cos) SinCos(BigComplex z)
    {
        return (Sin(z), Cos(z));
    }

    /// <inheritdoc/>
    public static (BigComplex SinPi, BigComplex CosPi) SinCosPi(BigComplex z)
    {
        return (SinPi(z), CosPi(z));
    }

    public static BigComplex Tan(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        BigComplex a = new (BigDecimal.Sin(2 * x), BigDecimal.Sinh(2 * y));
        var b = BigDecimal.Cos(2 * x) + BigDecimal.Cosh(2 * y);
        return a / b;
    }

    /// <inheritdoc/>
    public static BigComplex TanPi(BigComplex z)
    {
        return Tan(z * Pi);
    }

    #endregion Trigonometric methods

    #region Inverse trigonometric methods

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Asin(BigComplex z)
    {
        return I * Log(Sqrt(1 - z * z) - I * z);
    }

    /// <inheritdoc/>
    public static BigComplex AsinPi(BigComplex z)
    {
        return Asin(z) / Pi;
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Acos(BigComplex z)
    {
        return -I * Log(z + I * Sqrt(1 - z * z));
    }

    /// <inheritdoc/>
    public static BigComplex AcosPi(BigComplex z)
    {
        return Acos(z) / Pi;
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Atan(BigComplex z)
    {
        return -I / 2 * Log((I - z) / (I + z));
    }

    /// <inheritdoc/>
    public static BigComplex AtanPi(BigComplex z)
    {
        return Atan(z) / Pi;
    }

    #endregion Inverse trigonometric methods

    #region Hyperbolic methods

    public static BigComplex Sinh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Sinh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Cosh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Cosh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Cosh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Sinh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Tanh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Tanh(x);
        var b = BigDecimal.Tan(y);
        return (a + I * b) / (1 + I * a * b);
    }

    #endregion Hyperbolic methods

    #region Inverse hyperbolic methods

    /// <inheritdoc/>
    public static BigComplex Acosh(BigComplex x)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static BigComplex Asinh(BigComplex x)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static BigComplex Atanh(BigComplex x)
    {
        throw new NotImplementedException();
    }

    #endregion Inverse hyperbolic methods
}
