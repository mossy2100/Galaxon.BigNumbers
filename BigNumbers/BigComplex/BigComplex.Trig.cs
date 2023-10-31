namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Trigonometric methods

    /// <inheritdoc/>
    public static BigComplex Sin(BigComplex bc)
    {
        var x = bc.Real;
        var y = bc.Imaginary;
        var a = BigDecimal.Sin(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Cos(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex SinPi(BigComplex bc) => Sin(bc * Pi);

    /// <inheritdoc/>
    public static BigComplex Cos(BigComplex bc)
    {
        var x = bc.Real;
        var y = bc.Imaginary;
        var a = BigDecimal.Cos(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Sin(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, -b);
    }

    /// <inheritdoc/>
    public static BigComplex CosPi(BigComplex bc) => Cos(bc * Pi);

    /// <inheritdoc/>
    public static (BigComplex Sin, BigComplex Cos) SinCos(BigComplex bc) => (Sin(bc), Cos(bc));

    /// <inheritdoc/>
    public static (BigComplex SinPi, BigComplex CosPi) SinCosPi(BigComplex bc) =>
        (SinPi(bc), CosPi(bc));

    /// <inheritdoc/>
    public static BigComplex Tan(BigComplex bc)
    {
        var x = bc.Real;
        var y = bc.Imaginary;
        BigComplex a = new (BigDecimal.Sin(2 * x), BigDecimal.Sinh(2 * y));
        var b = BigDecimal.Cos(2 * x) + BigDecimal.Cosh(2 * y);
        return a / b;
    }

    /// <inheritdoc/>
    public static BigComplex TanPi(BigComplex bc) => Tan(bc * Pi);

    #endregion Trigonometric methods

    #region Inverse trigonometric methods

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    public static BigComplex Asin(BigComplex bc) => I * Log(Sqrt(1 - bc * bc) - I * bc);

    /// <inheritdoc/>
    public static BigComplex AsinPi(BigComplex bc) => Asin(bc) / Pi;

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    public static BigComplex Acos(BigComplex bc) => -I * Log(bc + I * Sqrt(1 - bc * bc));

    /// <inheritdoc/>
    public static BigComplex AcosPi(BigComplex bc) => Acos(bc) / Pi;

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    public static BigComplex Atan(BigComplex bc) => -I / 2 * Log((I - bc) / (I + bc));

    /// <inheritdoc/>
    public static BigComplex AtanPi(BigComplex bc) => Atan(bc) / Pi;

    #endregion Inverse trigonometric methods

    #region Hyperbolic methods

    /// <inheritdoc/>
    public static BigComplex Sinh(BigComplex bc)
    {
        var x = bc.Real;
        var y = bc.Imaginary;
        var a = BigDecimal.Sinh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Cosh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex Cosh(BigComplex bc)
    {
        var x = bc.Real;
        var y = bc.Imaginary;
        var a = BigDecimal.Cosh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Sinh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex Tanh(BigComplex bc)
    {
        var x = bc.Real;
        var y = bc.Imaginary;
        var a = BigDecimal.Tanh(x);
        var b = BigDecimal.Tan(y);
        return (a + I * b) / (1 + I * a * b);
    }

    #endregion Hyperbolic methods

    #region Inverse hyperbolic methods

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_hyperbolic_functions#Principal_value_of_the_inverse_hyperbolic_sine"/>
    public static BigComplex Asinh(BigComplex bc) => Log(bc + Sqrt(Sqr(bc) + 1));

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_hyperbolic_functions#Principal_value_of_the_inverse_hyperbolic_cosine"/>
    public static BigComplex Acosh(BigComplex bc) => Log(bc + Sqrt(bc + 1) * Sqrt(bc - 1));

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_hyperbolic_functions#Principal_values_of_the_inverse_hyperbolic_tangent_and_cotangent"/>
    public static BigComplex Atanh(BigComplex bc) => Log((1 + bc) / (1 - bc)) / 2;

    // TODO Add versions for cot, sec, csc.

    #endregion Inverse hyperbolic methods
}
