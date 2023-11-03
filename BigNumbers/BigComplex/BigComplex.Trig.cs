namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Trigonometric methods

    /// <inheritdoc/>
    public static BigComplex Sin(BigComplex z)
    {
        var (x, y) = z.ToTuple();
        var a = BigDecimal.Sin(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Cos(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex SinPi(BigComplex z)
    {
        return Sin(z * Pi);
    }

    /// <inheritdoc/>
    public static BigComplex Cos(BigComplex z)
    {
        var (x, y) = z.ToTuple();
        var a = BigDecimal.Cos(x) * BigDecimal.Cosh(y);
        var b = -BigDecimal.Sin(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
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

    /// <inheritdoc/>
    /// <remarks>
    /// I'm using Formulation 4 from this page:
    /// <see href="https://proofwiki.org/wiki/Tangent_of_Complex_Number"/>
    /// I *assume* this is the fastest, as it "only" requires 4 power series calculations, but I
    /// haven't compared all the methods to verify as yet.
    /// There is probably a single power series that converges on the correct result, but I haven't
    /// found it as yet.
    /// </remarks>
    public static BigComplex Tan(BigComplex z)
    {
        var (x, y) = z.ToTuple();
        var x2 = 2 * x;
        var y2 = 2 * y;
        var d = BigDecimal.Cos(x2) + BigDecimal.Cosh(y2);
        return new BigComplex(BigDecimal.Sin(x2) / d, BigDecimal.Sinh(y2) / d);
    }

    /// <inheritdoc/>
    public static BigComplex TanPi(BigComplex z)
    {
        return Tan(z * Pi);
    }

    /// <summary>Calculate the secant of the BigComplex value.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The secant of the value.</returns>
    public static BigComplex Sec(BigComplex z)
    {
        return 1 / Cos(z);
    }

    /// <summary>Calculate the cosecant of the BigComplex value.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The cosecant of the value.</returns>
    public static BigComplex Csc(BigComplex z)
    {
        return 1 / Sin(z);
    }

    /// <summary>Calculate the cotangent of the BigComplex value.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The cotangent of the value.</returns>
    public static BigComplex Cot(BigComplex z)
    {
        return 1 / Tan(z);
    }

    #endregion Trigonometric methods

    #region Inverse trigonometric methods

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    public static BigComplex Asin(BigComplex z)
    {
        return I * Log(Sqrt(1 - z * z) - I * z);
    }

    /// <inheritdoc/>
    public static BigComplex AsinPi(BigComplex z)
    {
        return Asin(z) / Pi;
    }

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    public static BigComplex Acos(BigComplex z)
    {
        return -I * Log(z + I * Sqrt(1 - z * z));
    }

    /// <inheritdoc/>
    public static BigComplex AcosPi(BigComplex z)
    {
        return Acos(z) / Pi;
    }

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms"/>
    public static BigComplex Atan(BigComplex z)
    {
        return -I / 2 * Log((I - z) / (I + z));
    }

    /// <inheritdoc/>
    public static BigComplex AtanPi(BigComplex z)
    {
        return Atan(z) / Pi;
    }

    /// <summary>Calculate the inverse cotangent of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The inverse cotangent.</returns>
    public static BigComplex Acot(BigComplex z)
    {
        return Atan(1 / z);
    }

    /// <summary>Calculate the inverse secant of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The inverse secant.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static BigComplex Asec(BigComplex z)
    {
        // Guard.
        if (Abs(z) < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(z),
                "Must have an absolute value of at least 1.");
        }

        return Acos(1 / z);
    }

    /// <summary>Calculate the inverse cosecant of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The inverse cosecant.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static BigComplex Acsc(BigComplex z)
    {
        // Guard.
        if (Abs(z) < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(z),
                "Must have an absolute value of at least 1.");
        }

        return Asin(1 / z);
    }

    #endregion Inverse trigonometric methods

    #region Hyperbolic methods

    /// <inheritdoc/>
    public static BigComplex Sinh(BigComplex z)
    {
        var (x, y) = z.ToTuple();
        var a = BigDecimal.Sinh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Cosh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex Cosh(BigComplex z)
    {
        var (x, y) = z.ToTuple();
        var a = BigDecimal.Cosh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Sinh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    /// <inheritdoc/>
    public static BigComplex Tanh(BigComplex z)
    {
        var (x, y) = z.ToTuple();
        var a = BigDecimal.Tanh(x);
        var b = BigDecimal.Tan(y);
        return (a + I * b) / (1 + I * a * b);
    }

    /// <summary>Calculate the hyperbolic cotangent of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The hyperbolic cotangent.</returns>
    public static BigComplex Coth(BigComplex z)
    {
        var e = Exp(2 * z);
        return (e + 1) / (e - 1);
    }

    /// <summary>Calculate the hyperbolic secant of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The hyperbolic secant.</returns>
    public static BigComplex Sech(BigComplex z)
    {
        return 2 / (Exp(z) + Exp(-z));
    }

    /// <summary>Calculate the hyperbolic cosecant of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The hyperbolic cosecant.</returns>
    public static BigComplex Csch(BigComplex z)
    {
        return 2 / (Exp(z) - Exp(-z));
    }

    #endregion Hyperbolic methods

    #region Inverse hyperbolic methods

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_hyperbolic_functions#Principal_value_of_the_inverse_hyperbolic_sine"/>
    public static BigComplex Asinh(BigComplex z)
    {
        return Log(z + Sqrt(Sqr(z) + 1));
    }

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_hyperbolic_functions#Principal_value_of_the_inverse_hyperbolic_cosine"/>
    public static BigComplex Acosh(BigComplex z)
    {
        return Log(z + Sqrt(z + 1) * Sqrt(z - 1));
    }

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Inverse_hyperbolic_functions#Principal_values_of_the_inverse_hyperbolic_tangent_and_cotangent"/>
    public static BigComplex Atanh(BigComplex z)
    {
        return Log((1 + z) / (1 - z)) / 2;
    }

    /// <summary>Calculate the inverse hyperbolic cotangent of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The inverse hyperbolic cotangent.</returns>
    public static BigComplex Acoth(BigComplex z)
    {
        return Log((z + 1) / (z - 1)) / 2;
    }

    /// <summary>Calculate the inverse hyperbolic secant of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The inverse hyperbolic secant.</returns>
    public static BigComplex Asech(BigComplex z)
    {
        return Log(1 / z + Sqrt(1 / Sqr(z) - 1));
    }

    /// <summary>Calculate the inverse hyperbolic cosecant of a BigComplex value.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The inverse hyperbolic cosecant.</returns>
    public static BigComplex Acsch(BigComplex z)
    {
        return Log(1 / z + Sqrt(1 / Sqr(z) + 1));
    }

    #endregion Inverse hyperbolic methods
}
