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
            // 0 raised to a negative real value is undefined (it is equivalent to division by 0).
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

        // Any value (real or complex) raised to the 0 power is 1.
        // 0^0 has no agreed-upon value, but some programming languages, including C#, return 1
        // (i.e. Math.Pow(0, 0) == 1). We'll do that here, too, for consistency.
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

    /// <inheritdoc />
    public static BigComplex Hypot(BigComplex x, BigComplex y)
    {
        return Sqrt(Sqr(x) + Sqr(y));
    }

    /// <inheritdoc />
    public static BigComplex RootN(BigComplex z, int n)
    {
        return Root(z, n);
    }

    /// <summary>Computes the n-th root of a value.</summary>
    /// <param name="z">The value whose <paramref name="n" />-th root is to be computed.</param>
    /// <param name="n">The degree of the root to be computed.</param>
    /// <returns>The <paramref name="n" />-th root of <paramref name="z" />.</returns>
    public static BigComplex Root(BigComplex z, BigInteger n)
    {
        // The 0th root is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The 0th root is undefined since any number to the power of 0 is 1.");
        }

        // The first root of a number is itself.
        if (n == 1) return z;

        // Can't solve.
        throw new ArithmeticException(
            $"There are {n} solutions, whereas this method is designed to return only 1. Try calling BigComplex.Roots() to find all the roots.");
    }

    /// <summary>Computes the n-th roots of a complex value.</summary>
    /// <param name="z">The value whose <paramref name="n" />-th roots are to be computed.</param>
    /// <param name="n">The degree of the roots to be computed.</param>
    /// <returns>The <paramref name="n" />-th roots of <paramref name="z" />.</returns>
    public static List<BigComplex> Roots(BigComplex z, BigInteger n)
    {
        return BigDecimal.ComplexRoots(z.Real, z.Imaginary, n)
            .Select(tup => new BigComplex(tup)).ToList();
    }

    /// <summary>
    /// Calculate the square root of a BigComplex number.
    /// The second root can be found by the conjugate of the result.
    /// You can use this method to get the square root of a negative value (including a BigDecimal
    /// value).
    /// e.g. BigComplex z = BigComplex.Sqrt(-5);
    /// <see cref="System.Math.Sqrt" />
    /// <see cref="System.Numerics.Complex.Sqrt" />
    /// <see cref="BigDecimal.Sqrt" />
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

    /// <summary>
    /// Get both square roots of a complex number.
    /// </summary>
    /// <param name="z">The complex value to find the square roots of</param>
    /// <returns>A list with up to 2 complex numbers, which are the roots.</returns>
    public static List<BigComplex> Sqrts(BigComplex z)
    {
        // The only parameter with 1 solution is 0.
        if (z == 0) return new List<BigComplex> { 0 };

        // Find the 2 roots.
        return Roots(z, 2);
    }

    /// <inheritdoc/>
    public static BigComplex Cbrt(BigComplex z)
    {
        // The only parameter with 1 solution is 0.
        if (z == 0) return 0;

        throw new ArithmeticException(
            "There are 3 solutions, whereas this method is designed to return only 1. Try calling BigComplex.Cbrts()");
    }

    /// <summary>
    /// Get all 3 cube roots of a complex number.
    /// </summary>
    /// <param name="z">The complex value to find the cube roots of</param>
    /// <returns>A list with up to 3 complex numbers, which are the roots.</returns>
    public static List<BigComplex> Cbrts(BigComplex z)
    {
        // The only parameter with 1 solution is 0.
        if (z == 0) return new List<BigComplex> { 0 };

        // Find the 3 roots.
        return Roots(z, 3);
    }

    #endregion Root functions

    #region Exponential functions

    /// <inheritdoc/>
    public static BigComplex Exp(BigComplex z)
    {
        // TODO Compare this approach (which uses BigDecimal.Exp, Sin, and Cos) with using the
        // power series.
        // See https://en.wikipedia.org/wiki/Euler%27s_formula#Power_series_definition
        return new BigComplex(BigDecimal.ComplexExp(z.Real, z.Imaginary));
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
        return new BigComplex(BigDecimal.ComplexLog(z.Real, z.Imaginary));
    }

    /// <inheritdoc />
    public static BigComplex Log(BigComplex x, BigComplex newBase)
    {
        throw new NotImplementedException();
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
