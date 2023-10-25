using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Power functions

    /// <summary>
    /// Complex exponentiation.
    /// Only the principal value is returned.
    /// <see href="https://en.wikipedia.org/wiki/Exponentiation#Complex_exponentiation" />
    /// </summary>
    /// <param name="z">The base.</param>
    /// <param name="w">The exponent.</param>
    /// <returns>The result.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent
    /// is negative or imaginary.
    /// </exception>
    public static BigComplex Pow(BigComplex z, BigComplex w)
    {
        // Guards.
        if (z == 0)
        {
            // 0 raised to a negative real value is undefined.
            // Math.Pow() returns double.Infinity, but BigDecimal doesn't have this.
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

        // Any non-zero value (real or complex) raised to the 0 power is 1.
        // 0^0 has no agreed-upon value, but some programming languages,
        // including C#, return 1 (i.e. Math.Pow(0, 0) == 1). Rather than throw
        // an exception, we'll do that here, too, for consistency.
        if (w == 0)
        {
            return 1;
        }

        // Any value (real or complex) raised to the power of 1 is itself.
        if (w == 1)
        {
            return z;
        }

        // 1 raised to any real value is 1.
        // 1 raised to any complex value has multiple results, including 1.
        // We'll just return 1 (the principal value) for simplicity and
        // consistency with Complex.Pow().
        if (z == 1)
        {
            return 1;
        }

        // i^2 == -1 by definition.
        if (z == I && w == 2)
        {
            return -1;
        }

        // If the values are both real, pass it to the BigDecimal calculation.
        if (z.Imaginary == 0 && w.Imaginary == 0)
        {
            return BigDecimal.Pow(z.Real, w.Real);
        }

        // Use formula for principal value.
        return Exp(w * Log(z));
    }

    /// <summary>
    /// Calculate the square of a complex number.
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>The square of the parameter.</returns>
    public static BigComplex Sqr(BigComplex z)
    {
        return z * z;
    }

    /// <summary>
    /// Calculate the cube of a complex number.
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>The cube of the parameter.</returns>
    public static BigComplex Cube(BigComplex z)
    {
        return z * z * z;
    }

    #endregion Power functions

    #region Root functions

    // TODO
    /// <inheritdoc />
    public static BigComplex Hypot(BigComplex x, BigComplex y)
    {
        throw new NotImplementedException();
    }

    public static BigComplex RootN(BigComplex z, int n)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculate the square root of a BigComplex number.
    /// The second root can be found by the negative of the result, as with other Sqrt() methods.
    /// You can use this method to get the square root of a negative value (including a BigDecimal
    /// value).
    /// e.g. BigComplex z = BigComplex.Sqrt(-5);
    /// (There will be an implicit cast of the -5 to a BigComplex.)
    /// TODO Test.
    /// <see cref="System.Math.Sqrt" />
    /// <see cref="System.Numerics.Complex.Sqrt" />
    /// <see cref="BigDecimal.Sqrt" />
    /// </summary>
    /// <param name="z">A BigComplex number.</param>
    /// <returns>The positive square root as a BigComplex number.</returns>
    public static BigComplex Sqrt(BigComplex z)
    {
        if (z == 0)
        {
            return 0;
        }

        if (z == 1)
        {
            return 1;
        }

        if (z == -1)
        {
            return I;
        }

        var a = z.Real;
        var b = z.Imaginary;
        if (b == 0)
        {
            return a > 0
                ? new BigComplex(BigDecimal.Sqrt(a), 0)
                : new BigComplex(0, BigDecimal.Sqrt(-a));
        }

        var m = Abs(z);
        var x = BigDecimal.Sqrt((m + a) / 2);
        var y = b / BigDecimal.Abs(b) * BigDecimal.Sqrt((m - a) / 2);
        return new BigComplex(x, y);
    }

    // TODO
    public static BigComplex Cbrt(BigComplex z)
    {
        throw new NotImplementedException();
    }

    #endregion Root functions

    #region Exponential functions

    public static BigComplex Exp(BigComplex z)
    {
        // Optimizations.
        if (z.Real == 0)
        {
            // e^0 = 1
            if (z.Imaginary == 0)
            {
                return 1;
            }

            // Euler's identity with π: e^πi = -1
            if (z.Imaginary == BigDecimal.Pi)
            {
                return -1;
            }

            // Euler's identity with τ: e^τi = 1
            if (z.Imaginary == BigDecimal.Tau)
            {
                return 1;
            }
        }

        if (z.Imaginary == 0)
        {
            if (z.Real == 1)
            {
                // e^1 == e
                return BigDecimal.E;
            }

            if (z.Real == BigDecimal.Ln10)
            {
                // e^ln10 == 10
                return 10;
            }
        }

        // Euler's formula.
        var r = BigDecimal.Exp(z.Real);
        var x = r * BigDecimal.Cos(z.Imaginary);
        var y = r * BigDecimal.Sin(z.Imaginary);
        return new BigComplex(x, y);
    }

    /// <summary>
    /// Calculate 2 raised to a complex power.
    /// <see cref="BigDecimal.Exp2" />
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>2^z</returns>
    public static BigComplex Exp2(BigComplex z)
    {
        return 2 ^ z;
    }

    /// <summary>
    /// Calculate 10 raised to a complex power.
    /// <see cref="BigDecimal.Exp10" />
    /// </summary>
    /// <param name="z">A complex value.</param>
    /// <returns>10^z</returns>
    public static BigComplex Exp10(BigComplex z)
    {
        return 10 ^ z;
    }

    #endregion Exponential functions
    #region Logarithmic functions

    /// <summary>
    /// Natural logarithm of a complex number.
    /// <see cref="Math.Log(double)" />
    /// <see cref="Complex.Log(Complex)" />
    /// </summary>
    /// <param name="z">A complex number.</param>
    /// <returns>The natural logarithm of the given value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If z == 0.</exception>
    public static BigComplex Log(BigComplex z)
    {
        if (z.Imaginary == 0)
        {
            if (z.Real == 0)
            {
                // Log(0) is undefined.
                // Math.Log(0) returns -Infinity, which BigDecimal can't represent.
                throw new ArgumentOutOfRangeException(nameof(z), "Logarithm of 0 is undefined.");
            }

            if (z.Real < 0)
            {
                // ln(x) = ln(|x|) + πi, for x < 0
                return new BigComplex(BigDecimal.Log(-z.Real), BigDecimal.Pi);
            }

            // For positive real values, pass to the BigDecimal method.
            return BigDecimal.Log(z.Real);
        }

        return new BigComplex(BigDecimal.Log(z.Magnitude), z.Phase);
    }

    /// <inheritdoc />
    public static BigComplex Log(BigComplex x, BigComplex newBase)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculate the natural logarithm of a BigComplex value.
    /// Alias for Log(BigComplex).
    /// </summary>
    /// <see cref="Log(BigComplex)" />
    /// <param name="z">The BigComplex value.</param>
    /// <returns>The natural logarithm of the parameter.</returns>
    public static BigComplex Ln(BigComplex z)
    {
        return Log(z);
    }

    /// <summary>
    /// Logarithm of a complex number in a specified base.
    /// <see cref="Log(BigComplex)" />
    /// <see cref="BigDecimal.Log(BigDecimal, BigDecimal)" />
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
    /// <see cref="BigDecimal.Log2" />
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 2.</returns>
    public static BigComplex Log2(BigComplex z)
    {
        return Log(z, 2);
    }

    /// <summary>
    /// Logarithm of a complex number in base 10.
    /// <see cref="BigDecimal.Log10" />
    /// <see href="https://en.wikipedia.org/wiki/Euler%27s_identity" />
    /// <see href="https://tauday.com/tau-manifesto#sec-euler_s_identity" />
    /// </summary>
    /// <param name="z">The complex arg.</param>
    /// <returns>The logarithm of z in base 10.</returns>
    public static BigComplex Log10(BigComplex z)
    {
        return Log(z, 10);
    }

    #endregion Logarithmic functions
}
