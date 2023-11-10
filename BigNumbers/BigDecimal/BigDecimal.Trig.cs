using System.Numerics;
using Galaxon.Core.Exceptions;

namespace Galaxon.BigNumbers;

/// <summary>
/// Trigonometric methods for BigDecimal.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions"/>
/// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions"/>
public partial struct BigDecimal
{
    #region Trigonometric methods

    /// <inheritdoc/>
    public static BigDecimal Sin(BigDecimal x)
    {
        // Find the equivalent angle in the interval [0, 𝞃).
        x = NormalizeAngle(in x);

        // Make use of identities to reduce x to the interval [0, π/2].
        if (x > Tau - HalfPi)
        {
            return -Sin(Tau - x);
        }
        if (x > Pi)
        {
            return -Sin(x - Pi);
        }
        if (x > HalfPi)
        {
            return Sin(Pi - x);
        }

        // Optimizations.
        if (x == 0)
        {
            return 0;
        }
        if (x == Pi / 6)
        {
            return One / 2;
        }
        if (x == Pi / 4)
        {
            return Sqrt(2) / 2;
        }
        if (x == Pi / 3)
        {
            return Sqrt(3) / 2;
        }
        if (x == HalfPi)
        {
            return 1;
        }

        // Taylor series.
        var sign = 1;
        BigInteger m = 1; // m = 2n + 1
        var xm = x;
        var x2 = x * x;
        BigInteger mFact = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct
        // result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + sign * xm / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            xm *= x2;
            mFact *= m * (m - 1);
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal SinPi(BigDecimal x)
    {
        return Sin(x * Pi);
    }

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions"/>
    /// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions"/>
    public static BigDecimal Cos(BigDecimal x)
    {
        return Sin(HalfPi - x);
    }

    /// <inheritdoc/>
    public static BigDecimal CosPi(BigDecimal x)
    {
        return Cos(x * Pi);
    }

    /// <inheritdoc/>
    public static (BigDecimal Sin, BigDecimal Cos) SinCos(BigDecimal x)
    {
        return (Sin(x), Cos(x));
    }

    /// <inheritdoc/>
    public static (BigDecimal SinPi, BigDecimal CosPi) SinCosPi(BigDecimal x)
    {
        return (SinPi(x), CosPi(x));
    }

    /// <inheritdoc/>
    public static BigDecimal Tan(BigDecimal x)
    {
        // Test for divide by zero.
        var c = Cos(x);
        if (c == 0) throw new ArithmeticException($"tan({x}) is undefined.");

        return Sin(x) / c;
    }

    /// <inheritdoc/>
    public static BigDecimal TanPi(BigDecimal x)
    {
        return Tan(x * Pi);
    }

    /// <summary>Calculate the cotangent of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The cotangent.</returns>
    /// <exception cref="DivideByZeroException">If the sine of the angle is 0.</exception>
    public static BigDecimal Cot(BigDecimal x)
    {
        // Test for divide by zero.
        var s = Sin(x);
        if (s == 0) throw new ArithmeticException($"cot({x}) is undefined.");

        return Cos(x) / s;
    }

    /// <summary>Calculate the secant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The secant.</returns>
    /// <exception cref="DivideByZeroException">If the cosine of the angle is 0.</exception>
    public static BigDecimal Sec(BigDecimal x)
    {
        // Test for divide by zero.
        var c = Cos(x);
        if (c == 0) throw new ArithmeticException($"sec({x}) is undefined.");

        return 1 / c;
    }

    /// <summary>Calculate the cosecant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The cosecant.</returns>
    /// <exception cref="DivideByZeroException">If the sine of the angle is 0.</exception>
    public static BigDecimal Csc(BigDecimal x)
    {
        // Test for divide by zero.
        var s = Sin(x);
        if (s == 0) throw new ArithmeticException($"csc({x}) is undefined.");

        return 1 / s;
    }

    #endregion Trigonometric methods

    #region Inverse trigonometric methods

    /// <inheritdoc/>
    public static BigDecimal Asin(BigDecimal x)
    {
        // Optimization.
        if (x == 0) return 0;

        // Handle negative arguments.
        if (x < 0) return -Asin(-x);

        // Guard.
        if (x > 1) throw new ArgumentOutOfRangeException(nameof(x), "Must be in the range -1..1.");

        // Optimization.
        if (x == 1) return HalfPi;

        // The Taylor series is slow to converge near x = ±1, but we can the following identity
        // relationship and calculate Asin() accurately and quickly for a smaller value:
        // Asin(θ) = π/2 - Asin(√(1-θ²))
        var xSqr = Sqr(x);
        if (x > 0.75m) return HalfPi - Asin(Sqrt(1 - xSqr));

        // Taylor series.
        BigInteger n = 1;
        BigInteger b = 1;
        BigInteger c = 2;
        BigInteger m = 3;
        var xToM = Cube(x);
        var sum = x;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + b * xToM / (c * m);

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            sum = newSum;
            n++;
            b *= 2 * n - 1;
            c *= 2 * n;
            m += 2;
            xToM *= xSqr;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal AsinPi(BigDecimal x)
    {
        return Asin(x) / Pi;
    }

    /// <inheritdoc/>
    public static BigDecimal Acos(BigDecimal x)
    {
        return HalfPi - Asin(x);
    }

    /// <inheritdoc/>
    public static BigDecimal AcosPi(BigDecimal x)
    {
        return Acos(x) / Pi;
    }

    /// <inheritdoc/>
    public static BigDecimal Atan(BigDecimal x)
    {
        return Asin(x / Sqrt(1 + Sqr(x)));

        // Handle negative arguments.
        if (x < 0) return -Atan(-x);

        // Optimization.
        if (x == 0) return 0;
        if (x == 1) return Pi / 4;

        // Taylor series.
        var m = 1;
        var xToM = x;
        var xSqr = Sqr(x);
        var small = x < 1;
        var sign = small ? 1 : -1;
        var sum = small ? 0 : HalfPi;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + sign * (small ? xToM / m : 1 / (m * xToM));

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            xToM *= xSqr;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal AtanPi(BigDecimal x)
    {
        return Atan(x) / Pi;
    }

    /// <summary>
    /// This two-argument variation of the Atan() method comes originally from FORTRAN.
    /// If x is non-negative, it will find the same result as Atan(y / x).
    /// If x is negative, the result will be offset by π.
    /// The purpose of the method is to produce a correct value for the polar angle when converting
    /// from cartesian coordinates to polar coordinates.
    /// It also avoids division by 0 exceptions.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Atan2"/>
    /// <see cref="CartesianToPolar"/>
    /// <param name="y">The y coordinate.</param>
    /// <param name="x">The x coordinate.</param>
    /// <returns>The polar angle.</returns>
    /// <see cref="double.Atan2"/>
    public static BigDecimal Atan2(BigDecimal y, BigDecimal x)
    {
        BigDecimal result;

        if (x == 0)
        {
            if (y == 0) return 0;

            result = HalfPi;
            return y > 0 ? result : -result;
        }

        result = Atan(y / x);
        return x > 0 ? result : result + (y < 0 ? -Pi : Pi);
    }

    /// <summary>
    /// Computes the arc-tangent for the quotient of two values and divides the result by pi.
    /// </summary>
    /// <param name="y">The y coordinate.</param>
    /// <param name="x">The x coordinate.</param>
    /// <returns>The polar angle.</returns>
    /// <exception cref="ArgumentInvalidException">If x and y both equal 0.</exception>
    /// <see cref="double.Atan2Pi"/>
    public static BigDecimal Atan2Pi(BigDecimal y, BigDecimal x)
    {
        return Atan2(y, x) / Pi;
    }

    /// <summary>Calculate the inverse cotangent of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The inverse cotangent.</returns>
    public static BigDecimal Acot(BigDecimal x)
    {
        return Atan2(1, x);
    }

    /// <summary>Calculate the inverse secant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The inverse secant.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static BigDecimal Asec(BigDecimal x)
    {
        // Guards.
        if (Abs(x) < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Must have an absolute value of at least 1.");
        }

        return Acos(1 / x);
    }

    /// <summary>Calculate the inverse cosecant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The inverse cosecant.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static BigDecimal Acsc(BigDecimal x)
    {
        // Guard.
        if (Abs(x) < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Must have an absolute value of at least 1.");
        }

        return Asin(1 / x);
    }

    #endregion Inverse trigonometric methods

    #region Hyperbolic methods

    /// <inheritdoc/>
    public static BigDecimal Sinh(BigDecimal x)
    {
        var ex = Exp(x);
        return (ex - 1 / ex) / 2;

        // Optimization.
        if (x == 0) return 0;

        // Taylor series.
        var m = 1;
        var xm = x;
        BigInteger mFact = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + xm / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            m += 2;
            xm *= x * x;
            mFact *= m * (m - 1);
            sum = newSum;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal Cosh(BigDecimal x)
    {
        var ex = Exp(x);
        return (ex + 1 / ex) / 2;

        // Optimization.
        if (x == 0) return 1;

        // Taylor series.
        var m = 0;
        BigDecimal xm = 1;
        var x2 = x * x;
        BigInteger mFact = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + xm / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            m += 2;
            xm *= x2;
            mFact *= m * (m - 1);
            sum = newSum;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal Tanh(BigDecimal x)
    {
        var e2x = Exp(2 * x);
        return (e2x - 1) / (e2x + 1);
        // return Sinh(x) / Cosh(x);
    }

    /// <summary>Calculate the hyperbolic cotangent of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The hyperbolic cotangent.</returns>
    public static BigDecimal Coth(BigDecimal x)
    {
        var e2x = Exp(2 * x);
        return (e2x + 1) / (e2x - 1);
    }

    /// <summary>Calculate the hyperbolic secant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The hyperbolic secant.</returns>
    public static BigDecimal Sech(BigDecimal x)
    {
        var ex = Exp(x);
        return 2 / (ex + 1 / ex);
    }

    /// <summary>Calculate the hyperbolic cosecant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The hyperbolic cosecant.</returns>
    public static BigDecimal Csch(BigDecimal x)
    {
        var ex = Exp(x);
        return 2 / (ex - 1 / ex);
    }

    #endregion Hyperbolic methods

    #region Inverse hyperbolic methods

    /// <inheritdoc/>
    public static BigDecimal Asinh(BigDecimal x)
    {
        return Log(x + Sqrt(Sqr(x) + 1));
    }

    /// <inheritdoc/>
    public static BigDecimal Acosh(BigDecimal x)
    {
        return Log(x + Sqrt(Sqr(x) - 1));
    }

    /// <inheritdoc/>
    public static BigDecimal Atanh(BigDecimal x)
    {
        return Log((1 + x) / (1 - x)) / 2;
    }

    /// <summary>Calculate the inverse hyperbolic cotangent of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The inverse hyperbolic cotangent.</returns>
    public static BigDecimal Acoth(BigDecimal x)
    {
        return Log((x + 1) / (x - 1)) / 2;
    }

    /// <summary>Calculate the inverse hyperbolic secant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The inverse hyperbolic secant.</returns>
    public static BigDecimal Asech(BigDecimal x)
    {
        return Log(1 / x + Sqrt(1 / Sqr(x) - 1));
    }

    /// <summary>Calculate the inverse hyperbolic cosecant of a BigDecimal value.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The inverse hyperbolic cosecant.</returns>
    public static BigDecimal Acsch(BigDecimal x)
    {
        return Log(1 / x + Sqrt(1 / Sqr(x) + 1));
    }

    #endregion Inverse hyperbolic methods

    #region Helper methods

    /// <summary>
    /// Convert cartesian coordinates to polar coordinates.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>A tuple containing the radius (r) and phase angle (theta).</returns>
    public static (BigDecimal r, BigDecimal theta) CartesianToPolar(BigDecimal x, BigDecimal y)
    {
        return x == 0 && y == 0 ? (0, 0) : (Hypot(x, y), Atan2(y, x));
    }

    /// <summary>
    /// Convert polar coordinates to cartesian coordinates.
    /// </summary>
    /// <param name="r">The radius.</param>
    /// <param name="theta">The phase angle.</param>
    /// <returns>A tuple containing the x and y coordinates.</returns>
    public static (BigDecimal x, BigDecimal y) PolarToCartesian(BigDecimal r, BigDecimal theta)
    {
        return r == 0 ? (0, 0) : (r * Cos(theta), r * Sin(theta));
    }

    /// <summary>Find the equivalent angle in the interval [0, 𝞃).</summary>
    public static BigDecimal NormalizeAngle(in BigDecimal theta)
    {
        // Use floored division here instead of the mod operator (which uses truncated division).
        // This will ensure an answer in the desired range.
        return theta - Floor(theta / Tau) * Tau;
    }

    #endregion Helper methods
}
