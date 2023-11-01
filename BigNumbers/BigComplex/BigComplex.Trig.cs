namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Trigonometric methods

    /// <inheritdoc/>
    public static BigComplex Sin(BigComplex bc)
    {
        var (x, y) = bc.ToTuple();
        var a = BigDecimal.Sin(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Cos(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex SinPi(BigComplex bc) => Sin(bc * Pi);

    /// <inheritdoc/>
    public static BigComplex Cos(BigComplex bc)
    {
        var (x, y) = bc.ToTuple();
        var a = BigDecimal.Cos(x) * BigDecimal.Cosh(y);
        var b = -BigDecimal.Sin(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex CosPi(BigComplex bc) => Cos(bc * Pi);

    /// <inheritdoc/>
    public static (BigComplex Sin, BigComplex Cos) SinCos(BigComplex bc) => (Sin(bc), Cos(bc));

    /// <inheritdoc/>
    public static (BigComplex SinPi, BigComplex CosPi) SinCosPi(BigComplex bc) =>
        (SinPi(bc), CosPi(bc));

    /// <inheritdoc/>
    /// <remarks>
    /// I'm using Formulation 4 from this page:
    /// <see href="https://proofwiki.org/wiki/Tangent_of_Complex_Number"/>
    /// I *assume* this is the fastest, as it "only" requires 4 power series calculations, but I
    /// haven't compared all the methods to verify as yet.
    /// There is probably a single power series that converges on the correct result, but I haven't
    /// found it as yet.
    /// </remarks>
    public static BigComplex Tan(BigComplex bc)
    {
        var (x, y) = bc.ToTuple();
        var x2 = 2 * x;
        var y2 = 2 * y;
        var d = BigDecimal.Cos(x2) + BigDecimal.Cosh(y2);
        return new BigComplex(BigDecimal.Sin(x2) / d, BigDecimal.Sinh(y2) / d);
    }

    /// <inheritdoc/>
    public static BigComplex TanPi(BigComplex bc) => Tan(bc * Pi);

    /// <summary>Calculate the secant of the BigComplex value.</summary>
    /// <param name="bc">A BigComplex value.</param>
    /// <returns>The secant of the value.</returns>
    public static BigComplex Sec(BigComplex bc) => 1 / Cos(bc);

    /// <summary>Calculate the cosecant of the BigComplex value.</summary>
    /// <param name="bc">A BigComplex value.</param>
    /// <returns>The cosecant of the value.</returns>
    public static BigComplex Csc(BigComplex bc) => 1 / Sin(bc);

    /// <summary>Calculate the cotangent of the BigComplex value.</summary>
    /// <param name="bc">A BigComplex value.</param>
    /// <returns>The cotangent of the value.</returns>
    public static BigComplex Cot(BigComplex bc) => 1 / Tan(bc);

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
        var (x, y) = bc.ToTuple();
        var a = BigDecimal.Sinh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Cosh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex Cosh(BigComplex bc)
    {
        var (x, y) = bc.ToTuple();
        var a = BigDecimal.Cosh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Sinh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex Tanh(BigComplex bc)
    {
        var (x, y) = bc.ToTuple();
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

    #endregion Inverse hyperbolic methods
}
