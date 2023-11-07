using System.Numerics;
using System.Runtime.CompilerServices;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

/// <summary>
/// Power, root, exponential, and logarithm methods for BigDecimal.
/// </summary>
public partial struct BigDecimal
{
    #region Power functions

    /// <summary>Calculate the square of a BigDecimal value.</summary>
    /// <param name="x">A BigDecimal value.</param>
    /// <returns>The square of the BigDecimal.</returns>
    public static BigDecimal Sqr(BigDecimal x)
    {
        return x * x;
    }

    /// <summary>Calculate the cube of a BigDecimal value.</summary>
    /// <param name="x">A BigDecimal value.</param>
    /// <returns>The cube of the BigDecimal.</returns>
    public static BigDecimal Cube(BigDecimal x)
    {
        return x * x * x;
    }

    /// <summary>
    /// Calculate the value of x^y where x is a BigDecimal and y is a BigInteger.
    /// Uses exponentiation by squaring for non-trivial parameters.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Exponentiation_by_squaring"/>
    /// <param name="x">The BigDecimal base.</param>
    /// <param name="y">The BigInteger exponent.</param>
    /// <returns>
    /// The result of the calculation, rounded off to the current value of MaxSigFigs.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// If trying to raise 0 to a negative power.
    /// </exception>
    public static BigDecimal Pow(BigDecimal x, BigInteger y)
    {
        // Handle non-positive values of y.
        if (y < 0)
        {
            // x^(-y) = (1/x)^y
            // Calculate the reciprocal before calling Pow() so that, if there's going to be a
            // DivideByZeroException, it will be thrown earlier.
            return Pow(1 / x, -y);
        }
        else if (y == 0)
        {
            // x^0 == 1
            return 1;
        }
        else if (y == 1)
        {
            // x^1 == x
            return x;
        }
        else
        {
            // y > 1

            // Handle easy cases of x.
            if (x == 0 || x == 1)
            {
                // 0^y == 0 for all y > 0
                // 1^y == 1 for all y
                return x;
            }
            else if (x == 10 && y >= int.MinValue && y <= int.MaxValue)
            {
                // 10 raised to an integer power is easy, given the structure of the BigDecimal type.
                return new BigDecimal(1, (int)y);
            }

            // Exponentiation by squaring.
            if (y == 2)
            {
                return Sqr(x);
            }
            else if (y == 3)
            {
                return Cube(x);
            }
            else if (IsEvenInteger(y))
            {
                // y is even: x^y = (x^2)^(y/2)
                return Pow(Sqr(x), y / 2);
            }
            else
            {
                // y is odd: x^y = x * (x^2)^((y-1)/2)
                return x * Pow(Sqr(x), (y - 1) / 2);
            }
        }
    }

    /// <inheritdoc/>
    /// <summary>Calculate the value of x^y where x and y are both BigDecimal values.</summary>
    /// <param name="x">The base.</param>
    /// <param name="y">The exponent.</param>
    /// <returns>
    /// The result of the calculation, rounded off to the current value of MaxSigFigs.
    /// </returns>
    /// <exception cref="DivideByZeroException">
    /// If trying to raise 0 to a negative power.
    /// </exception>
    /// <exception cref="ArithmeticException">
    /// If no real result can be computed.
    /// </exception>
    public static BigDecimal Pow(BigDecimal x, BigDecimal y)
    {
        // Defer to the BigInteger version of the method if possible.
        if (IsInteger(y)) return Pow(x, (BigInteger)y);

        // Handle negative exponent.
        // This will throw a DivideByZeroException if x is 0.
        if (y < 0) return Pow(1 / x, -y);

        if (x < 0)
        {
            // For negative x, the Exp(Log) method (below) won't work, because we can't get the log
            // of a negative number as a real value. Instead, we can convert y to a BigRational and
            // call that version of Pow(). Note: this can throw an exception.
            return Pow(x, (BigRational)y);
        }
        else if (x == 0 || x == 1)
        {
            // 0 raised to any positive value is 0.
            // 1 raised to any value is 1.
            return x;
        }
        else
        {
            // x > 0
            // We can compute the result using Exp() and Log().
            // We could use the same method as used above for x < 0 (Pow with BigRational) but
            // I suspect that would be slower. TODO Test that assumption.
            return Exp(y * Log(x));
        }
    }

    /// <summary>
    /// Calculate a BigDecimal raised to the power of a BigRational.
    /// This is useful when you want to do things like x^(1/3), but wish to avoid the problem of
    /// 1/3 not being exactly representable by a floating point number type.
    /// So instead of:
    /// <code>
    /// BigDecimal y = BigDecimal.Pow(x, 0.333);
    /// </code>
    /// you can do:
    /// <code>
    /// BigDecimal y = BigDecimal.Pow(x, new BigRational(1, 3));
    /// </code>
    /// </summary>
    /// <param name="x">The base.</param>
    /// <param name="y">The exponent.</param>
    /// <returns>The result of x raised to the power of y.</returns>
    /// <exception cref="DivideByZeroException">
    /// If trying to raise 0 to a negative power.
    /// </exception>
    /// <exception cref="ArithmeticException">
    /// If the base is negative, the numerator of the exponent is odd, and the denominator of the
    /// exponent is even. In this case, no real result exists (although a complex result will).
    /// </exception>
    public static BigDecimal Pow(BigDecimal x, BigRational y)
    {
        return RootN(Pow(x, y.Numerator), y.Denominator);
    }

    #endregion Power functions

    #region Root functions

    /// <inheritdoc/>
    /// <exception cref="ArithmeticException"></exception>
    /// <exception cref="ArgumentInvalidException"></exception>
    public static BigDecimal RootN(BigDecimal x, int n)
    {
        return RootN(x, (BigInteger)n);
    }

    /// <summary>
    /// Find the nth root of a BigDecimal value.
    /// Unlike the method required by the IRootFunctions interface, this version supports a
    /// BigInteger degree.
    /// </summary>
    /// <param name="x">The radicand. A BigDecimal value to find the nth root of.</param>
    /// <param name="n">
    /// The degree of root to find (2 for square root, 3 for cube root, etc.).
    /// </param>
    /// <returns>The nth root of the BigDecimal value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the degree is zero.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// If the radicand is 0 and the degree is negative.
    /// </exception>
    /// <exception cref="ArithmeticException">
    /// If the radicand is negative and the degree is even.
    /// </exception>
    public static BigDecimal RootN(BigDecimal x, BigInteger n)
    {
        // Handle special values of n.
        if (n < 0)
        {
            // A negative root is the reciprocal of the positive root.
            return RootN(1 / x, -n);
        }
        else if (n == 0)
        {
            // The 0th root is undefined.
            throw new ArgumentOutOfRangeException(nameof(n),
                "The 0th root is undefined since any number to the power of 0 is 1.");
        }
        else if (n == 1)
        {
            // The 1st root of a number is itself.
            return x;
        }

        // Handle special values of x.
        if (x < 0)
        {
            // If n is even there will be no real results, only complex ones.
            if (BigInteger.IsEvenInteger(n))
            {
                throw new ArithmeticException(
                    "Negative numbers have no real even roots, only complex ones. Try BigComplex.Roots().");
            }

            // Calculate the only real root, which will be negative.
            return -RootN(-x, n);
        }
        else if (x == 0 || x == 1)
        {
            // If x == 0 the root will be 0, since 0^n = 0 for all n > 0.
            // If x == 1 the root will be 1, since 1^n = 1 for all n.
            return x;
        }

        // At this point we know x > 0 and n > 1.
        // Use Newton's method to find the root.

        // Temporarily increase the maximum number of significant figures to ensure a correct
        // result.
        var origMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Set the initial estimate to x.
        BigDecimal yk = x;
        // Next term, y(k+1)
        BigDecimal ykp1;
        // Keep up to 2 previous terms, for bounce detection.
        // Previous term, y(k-1)
        BigDecimal ykm1 = 0;
        // Term before the previous term, y(k-2)
        BigDecimal ykm2 = 0;

        // Precalculate some values.
        var m = (n - 1) / (BigDecimal)n;
        var p = x / n;

        // DEBUG
        var count = 0;

        // Newton's method.
        while (true)
        {
            // Compute the value of ykp1 and add it to the list.
            ykp1 = m * yk + p / Pow(yk, n - 1);

            // If the new term is the same, we're done.
            if (ykp1 == yk) break;

            // // Detect repeated value. This can occur due to rounding.
            // if (ykp1 == ykm1)
            // {
            //     // Figure out which value is best.
            //     BigDecimal d0 = Abs(Pow(yk, n) - x);
            //     BigDecimal d1 = Abs(Pow(ykp1, n) - x);
            //     if (d0 < d1)
            //     {
            //         ykp1 = yk;
            //     }
            //     break;
            // }
            //
            // // Detect repeated value. This can occur due to rounding.
            // if (ykp1 == ykm2)
            // {
            //     // Figure out value one is best.
            //     BigDecimal d0 = Abs(Pow(ykm1, n) - x);
            //     BigDecimal d1 = Abs(Pow(yk, n) - x);
            //     BigDecimal d2 = Abs(Pow(ykp1, n) - x);
            //     if (d0 <= d1 && d0 <= d2)
            //     {
            //         ykp1 = ykm1;
            //     }
            //     else if (d1 <= d0 && d1 <= d2)
            //     {
            //         ykp1 = yk;
            //     }
            //     break;
            // }

            // DEBUG
            Console.WriteLine($"{ykp1:E50}");
            count++;
            if (count > 100)
            {
                // return y2;
                throw new TimeoutException($"Too many iterations. x={x}, n={n}");
            }

            // Next iteration.
            ykm2 = ykm1;
            ykm1 = yk;
            yk = ykp1;
        }

        // Restore the maximum number of significant figures, and round off.
        MaxSigFigs = origMaxSigFigs;
        return RoundSigFigs(ykp1);
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
        return x == 0 ? y : y == 0 ? x : Sqrt(Sqr(x) + Sqr(y));
    }

    #endregion Root functions

    #region Exponential functions

    /// <inheritdoc/>
    public static BigDecimal Exp(BigDecimal x)
    {
        // Optimizations.
        if (x == 0) return 1;

        // If the exponent is negative, inverse the result of the positive exponent.
        if (x < 0) return 1 / Exp(-x);

        // Note, we can't just call Pow(E, x) for this, because this would require computing E
        // (which is not stored as a constant in the class), which in turn calls this method.
        // This is faster than Pow() anyway.

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
            if (sum == newSum) break;

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

    /// <inheritdoc/>
    public static BigDecimal Exp2(BigDecimal x)
    {
        return Pow(2, x);
    }

    /// <inheritdoc/>
    public static BigDecimal Exp10(BigDecimal x)
    {
        return Pow(10, x);
    }

    #endregion Exponential functions

    #region Logarithmic functions

    /// <inheritdoc/>
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
        var nDigits = x.NumSigFigs;
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public static BigDecimal Log2(BigDecimal x)
    {
        return Log(x, 2);
    }

    /// <inheritdoc/>
    public static BigDecimal Log10(BigDecimal x)
    {
        return Log(x, 10);
    }

    #endregion Logarithmic functions
}
