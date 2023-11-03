namespace Galaxon.BigNumbers;

/// <summary>
/// Encapsulates a complex number with BigDecimal parts, allowing high levels of precision.
/// </summary>
public partial struct BigComplex
{
    #region Power functions

    /// <summary>Complex exponentiation.</summary>
    /// <remarks>Only the principal value is returned.</remarks>
    /// <see href="https://en.wikipedia.org/wiki/Exponentiation#Complex_exponentiation"/>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The result of the exponentiation.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent is negative or imaginary.
    /// </exception>
    public static BigComplex Pow(BigComplex z, BigComplex w)
    {
        // Guards.
        if (z == 0)
        {
            // 0 raised to a negative real value is undefined (it is equivalent to division by 0).
            // Math.Pow() returns double.Infinity in this case, but BigDecimal can't represent
            // infinities.
            if (w.Imaginary == 0 && w.Real < 0)
            {
                throw new ArithmeticException("0 raised to a negative power is undefined.");
            }

            // 0 to the power of an imaginary number is also undefined.
            if (w.Imaginary != 0)
            {
                throw new ArithmeticException("0 raised to an imaginary power is undefined.");
            }
        }

        // Any value (real or complex) raised to the 0 power is 1.
        // 0^0 has no agreed-upon value, but some programming languages, including C#, return 1
        // (i.e. Math.Pow(0, 0) == 1). We'll do that here, too, for consistency.
        if (w == 0) return 1;

        // Any value (real or complex) raised to the power of 1 is itself.
        if (w == 1) return z;

        // 1 raised to any real value is 1.
        // 1 raised to any complex value has multiple results, including 1.
        // We'll just return 1 (the principal value) for simplicity and consistency with
        // Complex.Pow().
        if (z == 1) return 1;

        // i^2 == -1 by definition.
        if (z == I && w == 2) return -1;

        // If the values are both real, pass it to the BigDecimal calculation.
        if (z.Imaginary == 0 && w.Imaginary == 0) return BigDecimal.Pow(z.Real, w.Real);

        // Use formula for principal value.
        return Exp(w * Log(z));
    }

    /// <summary>Calculate the square of a BigComplex number.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The square of the BigComplex value.</returns>
    public static BigComplex Sqr(BigComplex z)
    {
        return z * z;
    }

    /// <summary>Calculate the cube of a BigComplex number.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The cube of the BigComplex value.</returns>
    public static BigComplex Cube(BigComplex z)
    {
        return z * z * z;
    }

    #endregion Power functions

    #region Root functions

    /// <inheritdoc/>
    public static BigComplex Hypot(BigComplex x, BigComplex y)
    {
        return Sqrt(Sqr(x) + Sqr(y));
    }

    /// <inheritdoc/>
    public static BigComplex RootN(BigComplex z, int n)
    {
        // The 0th root is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The 0th root is undefined since any number to the power of 0 is 1.");
        }

        // The first root of a number is itself.
        if (n == 1) return z;

        // Calculate the first root.
        var s = BigDecimal.RootN(z.Magnitude, n);
        var iota = z.Phase / n;
        return new BigComplex(s * BigDecimal.Cos(iota), s * BigDecimal.Sin(iota));
    }

    /// <summary>Computes the n-th roots of a complex value.</summary>
    /// <param name="z">The value whose <paramref name="n"/>-th roots are to be computed.</param>
    /// <param name="n">The degree of the roots to be computed.</param>
    /// <returns>The <paramref name="n"/>-th roots of <paramref name="z"/>.</returns>
    public static BigComplex[] Roots(BigComplex z, int n)
    {
        // The 0th root is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The 0th root is undefined since any number to the power of 0 is 1.");
        }

        // Create array to store results.
        var roots = new BigComplex[n];

        // The first root of a number is itself.
        if (n == 1)
        {
            roots[0] = z;
            return roots;
        }

        // Calculate values that will be constant during the loop.
        var s = BigDecimal.RootN(z.Magnitude, n);
        var iota = z.Phase / n;
        var beta = BigDecimal.Tau / n;

        // Calculate all the roots as complex numbers.
        var midpoint = (n + n % 2) / 2;
        for (var k = 0; k < n; k++)
        {
            if (k < midpoint)
            {
                // Calculate the root.
                var alpha = iota + k * beta;
                roots[k] = new BigComplex(s * BigDecimal.Cos(alpha), s * BigDecimal.Sin(alpha));
            }
            else
            {
                // To save time, get the complex conjugate of one we already found.
                roots[k] = Conjugate(roots[n - 1 - k]);
            }
        }

        return roots;
    }

    /// <summary>
    /// Calculate the square root of a BigComplex number.
    /// The second root can be found by the conjugate of the result.
    /// You can use this method to get the square root of a negative value (including a BigDecimal
    /// value).
    /// e.g. BigComplex z = BigComplex.Sqrt(-5);
    /// <see cref="System.Math.Sqrt"/>
    /// <see cref="System.Numerics.Complex.Sqrt"/>
    /// <see cref="BigDecimal.Sqrt"/>
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The positive square root as a BigComplex number.</returns>
    public static BigComplex Sqrt(BigComplex z)
    {
        // Optimizations.
        if (z == 0) return 0;
        if (z == 1) return 1;
        if (z == -1) return I;

        // Handle real values.
        var (a, b) = z.ToTuple();
        if (b == 0)
        {
            return a > 0
                ? new BigComplex(BigDecimal.Sqrt(a), 0)
                : new BigComplex(0, BigDecimal.Sqrt(-a));
        }

        // Handle complex values.
        var m = Abs(z);
        var x = BigDecimal.Sqrt((m + a) / 2);
        var y = b / BigDecimal.Abs(b) * BigDecimal.Sqrt((m - a) / 2);
        return new BigComplex(x, y);
    }

    /// <inheritdoc/>
    public static BigComplex Cbrt(BigComplex z)
    {
        return RootN(z, 3);
    }

    #endregion Root functions

    #region Exponential functions

    /// <inheritdoc/>
    public static BigComplex Exp(BigComplex z)
    {
        // TODO Compare this approach (which uses BigDecimal.Exp, Sin, and Cos) with using the
        // power series.
        // See https://en.wikipedia.org/wiki/Euler%27s_formula#Power_series_definition
        // If there's no imaginary component, use the real version of the method.

        // Optimizations using Euler's identity.
        if (z.Real == 0)
        {
            // Euler's identity with π: e^πi = -1
            if (z.Imaginary == Pi) return -1;

            // Euler's identity with τ: e^τi = 1
            if (z.Imaginary == Tau) return 1;
        }

        // Euler's formula.
        var r = BigDecimal.Exp(z.Real);
        return new BigComplex(r * BigDecimal.Cos(z.Imaginary), r * BigDecimal.Sin(z.Imaginary));
    }

    /// <inheritdoc/>
    public static BigComplex Exp2(BigComplex z)
    {
        return 2 ^ z;
    }

    /// <inheritdoc/>
    public static BigComplex Exp10(BigComplex z)
    {
        return 10 ^ z;
    }

    #endregion Exponential functions

    #region Logarithmic functions

    /// <inheritdoc/>
    /// <remarks>Finds the principal value only.</remarks>
    /// <exception cref="ArgumentOutOfRangeException">If z is 0.</exception>
    public static BigComplex Log(BigComplex z)
    {
        // Optimization.
        // For non-negative real values, use the real version of the method, avoiding the calculation of magnitude and phase.
        // This will throw an exception if real == 0.
        if (z.Real >= 0 && z.Imaginary == 0) return BigDecimal.Log(z.Real);

        // Calculate the complex logarithm.
        return new BigComplex(BigDecimal.Log(z.Magnitude), z.Phase);
    }

    /// <inheritdoc/>
    public static BigComplex Log(BigComplex x, BigComplex b)
    {
        // Guard.
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        // 0^0 == 1. Mimics Math.Log().
        if (x == 1 && b == 0) return 0;

        return Log(x) / Log(b);
    }

    /// <summary>
    /// Logarithm of a complex number in a specified base.
    /// <see cref="Log(BigComplex)"/>
    /// <see cref="BigDecimal.Log(BigDecimal, BigDecimal)"/>
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <param name="b">The base.</param>
    /// <returns>The logarithm of z in base b.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the complex value is 0.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the base is less than or equal to 0, or equal to 1.
    /// </exception>
    public static BigComplex Log(BigComplex z, BigDecimal b)
    {
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        return Log(z) / BigDecimal.Log(b);
    }

    /// <summary>
    /// Logarithm of a complex number in base 2.
    /// <see cref="BigDecimal.Log2"/>
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 2.</returns>
    public static BigComplex Log2(BigComplex z)
    {
        return Log(z, 2);
    }

    /// <summary>
    /// Logarithm of a complex number in base 10.
    /// <see cref="BigDecimal.Log10"/>
    /// <see href="https://en.wikipedia.org/wiki/Euler%27s_identity"/>
    /// <see href="https://tauday.com/tau-manifesto#sec-euler_s_identity"/>
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 10.</returns>
    public static BigComplex Log10(BigComplex z)
    {
        return Log(z, 10);
    }

    #endregion Logarithmic functions
}
