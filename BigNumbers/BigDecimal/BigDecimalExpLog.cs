using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

/// <summary>
/// Power, root, exponential, and logarithm methods for BigDecimals.
/// </summary>
public partial struct BigDecimal
{
    #region Power functions

    /// <summary>
    /// Calculate the value of x^y where x and y are both BigDecimals values.
    /// </summary>
    /// <param name="x">The base.</param>
    /// <param name="y">The exponent.</param>
    /// <returns>
    /// The result of the calculation, rounded off to the current value of
    /// MaxSigFigs.
    /// </returns>
    /// <exception cref="ArithmeticException">
    /// If there is no real result or a real result cannot
    /// otherwise be computed.
    /// </exception>
    public static BigDecimal Pow(BigDecimal x, BigDecimal y)
    {
        // Handle negative y.
        if (y < 0) return 1 / Pow(x, -y);

        // Anything to the power of 0 (including 0) is 1.
        if (y == 0) return 1;

        // Anything to the power of 1 is itself.
        if (y == 1) return x;

        // 0 to any power other than 0 is 0.
        if (x == 0) return 0;

        // 1 to any power is 1.
        if (x == 1) return 1;

        // If the exponent is an integer we can compute a result reasonably quickly with
        // exponentiation by squaring (recursion).
        if (IsInteger(y))
        {
            // 10 to an integer power is easy, given the structure of the BigDecimals type.
            if (x == 10 && y >= int.MinValue && y <= int.MaxValue)
            {
                return new BigDecimal(1, (int)y);
            }

            // Even integer powers.
            if (IsEvenInteger(y)) return Pow(Sqr(x), y / 2);

            // Odd integer powers. (We know y >= 3 at this point.)
            return x * Pow(x, y - 1);
        }

        // For positive x with non-integer exponent, compute the result using Exp() and Log().
        if (x > 0) return Exp(y * Log(x));

        // For negative x with non-integer exponent, we can't easily compute a real result.
        // However, one may exist, e.g. Pow(-243, 0.2) == -3
        // We can use the complex methods, and check if the result is real.
        // TODO Test!
        var z = BigComplex.Exp(y * BigComplex.Log(x));

        // See if the result is real.
        if (z.Imaginary == 0) return z.Real;

        throw new ArithmeticException("Cannot compute a real result. Try BigComplex.Pow().");
    }

    /// <summary>
    /// Calculate the square of a number.
    /// </summary>
    /// <param name="x">A real value.</param>
    /// <returns>The square of the argument.</returns>
    public static BigDecimal Sqr(BigDecimal x)
    {
        return x * x;
    }

    /// <summary>
    /// Calculate the cube of a number.
    /// </summary>
    /// <param name="x">A real value.</param>
    /// <returns>The cube of the argument.</returns>
    public static BigDecimal Cube(BigDecimal x)
    {
        return x * x * x;
    }

    #endregion Power functions

    #region Root functions

    /// <inheritdoc />
    /// <exception cref="ArithmeticException"></exception>
    /// <exception cref="ArgumentInvalidException"></exception>
    public static BigDecimal RootN(BigDecimal x, int n)
    {
        // The 0th root is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The 0th root is undefined since any number to the power of 0 is 1.");
        }

        // Optimizations. TODO Check.
        if (x == 0) return 0;
        if (x == 1) return 1;

        // A negative root is the inverse of the positive root.
        if (n < 0) return 1 / RootN(x, -n);

        // The first root of a number is itself.
        if (n == 1) return x;

        if (x > 0)
        {
            // Get an initial estimate using double, which should be pretty fast.
            // Reduce operand to the maximum number of significant digits supported by the double type.
            var xR = RoundSigFigs(x, DoublePrecision);
            var sig = double.RootN((double)xR.Significand, n);
            var exp = (BigDecimal)xR.Exponent / n;
            var y0 = sig * Exp10(exp);

            // Check if our estimate is already our result.
            if (Pow(y0, n) == x) return y0;

            // Temporarily increase the maximum number of significant figures to ensure a correct result.
            var prevMaxSigFigs = MaxSigFigs;
            MaxSigFigs += 2;

            BigDecimal result;

            // Calculate the smallest difference between two adjacent values before we stop.
            // Our initial estimate provides the scale.
            y0.ShiftToSigFigs(prevMaxSigFigs);
            var delta = Exp10(y0.Exponent - 1);

            // Newton's method.
            while (true)
            {
                // Get the next value of y.
                var ym = Pow(y0, n - 1);
                var yn = ym * y0;
                var y1 = y0 - (yn - x) / (n * ym);

                // See if the values are close enough.
                // Simply testing for equality doesn't work because this method will sometimes
                // alternate (bounce) between two very close values. This is caused by rounding in
                // the above operations.
                // Rounding again (to the original, lower sig figs) doesn't solve this, as sometimes
                // even very close values can round to different values.
                // So, we compare the two similar values to see which is best.
                if (Abs(y1 - y0) <= delta)
                {
                    // Test for equality.
                    if (y0 == y1)
                    {
                        result = y0;
                    }
                    else
                    {
                        // Pick the best value. We have to temporarily increase the number of
                        // significant figures again to get an accurate comparison (otherwise we
                        // often end up comparing 1 and 1, etc.).
                        var prevMaxSigFigs2 = MaxSigFigs;
                        MaxSigFigs += 2;
                        var diff0 = Abs(x - Pow(y0, n));
                        var diff1 = Abs(x - Pow(y1, n));
                        result = diff0 < diff1 ? y0 : y1;
                        MaxSigFigs = prevMaxSigFigs2;
                    }
                    break;
                }

                // Next iteration.
                y0 = y1;
            }

            // Restore the maximum number of significant figures.
            MaxSigFigs = prevMaxSigFigs;
            return RoundSigFigs(result);
        }

        // At this point, x < 0.

        // If n is even, a negative value for x is unsupported as there will be no real results,
        // only complex ones.
        if (int.IsEvenInteger(n))
        {
            throw new ArithmeticException(
                "Negative numbers have no real even roots, only complex ones. Try BigComplex.Roots().");
        }

        // Get all the roots, real and complex.
        var roots = BigComplex.Roots(x, n);

        // Find the first real root, if present. We only need to check the first half of the
        // values, because the second half will just be complex conjugates of those.
        for (var i = 0; i < (int)Ceiling(n / 2); i++)
        {
            if (roots[i].Imaginary == 0) return RoundSigFigs(roots[i].Real);
        }

        // No solution found.
        throw new ArithmeticException("No real root found. Try BigComplex.Roots().");
    }

    /// <summary>
    /// Calculate the square root of a real number.
    /// </summary>
    /// <param name="x">The number.</param>
    /// <returns>The square root of the number.</returns>
    /// <exception cref="ArithmeticException">If the argument is negative.</exception>
    public static BigDecimal Sqrt(BigDecimal x)
    {
        return RootN(x, 2);
    }

    /// <summary>
    /// Calculate the cube root of a real number.
    /// </summary>
    /// <param name="x">The number.</param>
    /// <returns>The cube root of the number.</returns>
    public static BigDecimal Cbrt(BigDecimal x)
    {
        return RootN(x, 3);
    }

    /// <summary>
    /// Calculate the length of the hypotenuse of a right triangle.
    /// </summary>
    /// <param name="x">The length of one of the short sides of the triangle.</param>
    /// <param name="y">The length of the other short side of the triangle.</param>
    /// <returns>The length of the hypotenuse.</returns>
    public static BigDecimal Hypot(BigDecimal x, BigDecimal y)
    {
        return Sqrt(Sqr(x) + Sqr(y));
    }

    /// <summary>
    /// Get the first root of a complex number.
    /// </summary>
    /// <param name="z">The complex value.</param>
    /// <param name="n">The degree of the roots to be computed.</param>
    /// <returns>A BigComplex representing the first root.</returns>
    public static BigComplex FirstComplexRoot(BigComplex z, int n)
    {
        // The 0th root is undefined.
        if (n == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n),
                "The 0th root is undefined since any number to the power of 0 is 1.");
        }

        // The first root of a number is itself.
        if (n == 1) return z;

        // Calculate the root.
        var s = RootN(z.Magnitude, n);
        var iota = z.Phase / n;
        return new BigComplex(s * Cos(iota), s * Sin(iota));
    }

    #endregion Root functions

    #region Exponential functions

    /// <inheritdoc />
    public static BigDecimal Exp(BigDecimal x)
    {
        // Optimizations.
        if (x == 0)
        {
            return 1;
        }

        // If the exponent is negative, inverse the result of the positive exponent.
        if (x < 0)
        {
            return 1 / Exp(-x);
        }

        // Taylor/Maclaurin series.
        // https://en.wikipedia.org/wiki/Taylor_series#Exponential_function
        BigInteger n = 0;
        BigDecimal xn = 1; // x^n
        BigInteger nf = 1; // n!
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + xn / nf;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            n++;
            xn *= x;
            nf *= n;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal Exp2(BigDecimal x)
    {
        return Pow(2, x);
    }

    /// <inheritdoc />
    public static BigDecimal Exp10(BigDecimal x)
    {
        return Pow(10, x);
    }

    #endregion Exponential functions

    #region Logarithmic functions

    /// <inheritdoc />
    public static BigDecimal Log(BigDecimal x)
    {
        // Guards.
        if (x == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "The logarithm of 0 is undefined.");
        }

        if (x < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Logarithm of a negative value is a complex number, which cannot be expressed using a BigDecimal.");
        }

        // Optimization.
        if (x == 1)
        {
            return 0;
        }

        // Shortcut for Log(10).
        if (x == 10 && _ln10.NumSigFigs >= MaxSigFigs)
        {
            return RoundSigFigs(_ln10);
        }

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        var nDigits = x.Significand.NumDigits();
        var scale = nDigits + x.Exponent;
        var y = x;
        y.Exponent = -nDigits;

        // Taylor/Newton-Mercator series.
        // https://en.wikipedia.org/wiki/Mercator_series
        y--;
        var n = 1;
        var sign = 1;
        var yn = y;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series
            var newSum = sum + sign * yn / n;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            n++;
            sign = -sign;
            yn *= y;
        }

        // Special handling for Log(10) to avoid infinite recursion.
        var result = x == 10 ? -sum : sum + scale * Ln10;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        // Scale back.
        return RoundSigFigs(result);
    }

    /// <inheritdoc />
    public static BigDecimal Log(BigDecimal x, BigDecimal b)
    {
        if (b == 1)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        // 0^0 == 1. Mimics Math.Log().
        if (x == 1 && b == 0)
        {
            return 0;
        }

        // This will throw if x <= 0 || b <= 0.
        return Log(x) / Log(b);
    }

    /// <inheritdoc />
    public static BigDecimal Log2(BigDecimal x)
    {
        return Log(x, 2);
    }

    /// <inheritdoc />
    public static BigDecimal Log10(BigDecimal x)
    {
        return Log(x, 10);
    }

    #endregion Logarithmic functions
}
