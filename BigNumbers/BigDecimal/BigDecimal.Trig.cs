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
    public static BigDecimal Sin(BigDecimal a)
    {
        // Find the equivalent angle in the interval [-π, π).
        a = NormalizeAngle(in a);

        // Optimizations.
        if (a == 0 || a == Pi) return 0;

        var halfPi = Pi / 2;
        if (a == halfPi) return 1;
        if (a == -halfPi) return NegativeOne;

        // Taylor series.
        var sign = 1;
        BigInteger m = 1; // m = 2n + 1
        var aToM = a;
        var aSqr = a * a;
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
            var newSum = sum + sign * aToM / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            aToM *= aSqr;
            mFact *= m * (m - 1);
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal SinPi(BigDecimal a) => Sin(a * Pi);

    /// <inheritdoc/>
    /// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions"/>
    /// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions"/>
    public static BigDecimal Cos(BigDecimal a)
    {
        // Find the equivalent angle in the interval [-π, π).
        a = NormalizeAngle(in a);

        // Optimizations.
        if (a == 0) return 1;
        if (a == Pi) return NegativeOne;
        if (Abs(a) == Pi / 2) return 0;

        // Taylor series.
        // https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions
        var sign = 1;
        BigInteger m = 0; // m = 2n
        BigDecimal aToM = 1;
        var aSqr = a * a;
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
            var newSum = sum + sign * aToM / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            aToM *= aSqr;
            mFact *= m * (m - 1);
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal CosPi(BigDecimal a) => Cos(a * Pi);

    /// <inheritdoc/>
    public static (BigDecimal Sin, BigDecimal Cos) SinCos(BigDecimal a) => (Sin(a), Cos(a));

    /// <inheritdoc/>
    public static (BigDecimal SinPi, BigDecimal CosPi) SinCosPi(BigDecimal a) =>
        (SinPi(a), CosPi(a));

    /// <inheritdoc/>
    public static BigDecimal Tan(BigDecimal a)
    {
        // Find the equivalent angle in the interval [-π, π).
        a = NormalizeAngle(in a);

        // Test for divide by zero.
        try
        {
            return Sin(a) / Cos(a);
        }
        catch (DivideByZeroException)
        {
            throw new ArithmeticException($"tan({a}) is undefined.");
        }
    }

    /// <inheritdoc/>
    public static BigDecimal TanPi(BigDecimal a) => Tan(a * Pi);

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
        var halfPi = Pi / 2;
        if (x == 1) return halfPi;

        // The Taylor series is slow to converge near x = ±1, but we can the following identity
        // relationship and calculate Asin() accurately and quickly for a smaller value:
        // Asin(θ) = π/2 - Asin(√(1-θ²))
        var xSqr = Sqr(x);
        if (x > 0.75m) return halfPi - Asin(Sqrt(1 - xSqr));

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
    public static BigDecimal AsinPi(BigDecimal x) => Asin(x) / Pi;

    /// <inheritdoc/>
    public static BigDecimal Acos(BigDecimal x) => Pi / 2 - Asin(x);

    /// <inheritdoc/>
    public static BigDecimal AcosPi(BigDecimal x) => Acos(x) / Pi;

    /// <inheritdoc/>
    public static BigDecimal Atan(BigDecimal x)
    {
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
        var sum = small ? 0 : Pi / 2;

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
    public static BigDecimal AtanPi(BigDecimal x) => Atan(x) / Pi;

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

            result = Pi / 2;
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
    public static BigDecimal Atan2Pi(BigDecimal y, BigDecimal x) => Atan2(y, x) / Pi;

    #endregion Inverse trigonometric methods

    #region Hyperbolic methods

    /// <inheritdoc/>
    public static BigDecimal Sinh(BigDecimal a)
    {
        // Optimization.
        if (a == 0) return 0;

        // Taylor series.
        var m = 1;
        var aToM = a;
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
            var newSum = sum + aToM / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            m += 2;
            aToM *= a * a;
            mFact *= m * (m - 1);
            sum = newSum;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal Cosh(BigDecimal a)
    {
        // Optimization.
        if (a == 0) return 1;

        // Taylor series.
        var m = 0;
        BigDecimal aToM = 1;
        var aSqr = a * a;
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
            var newSum = sum + aToM / mFact;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            m += 2;
            aToM *= aSqr;
            mFact *= m * (m - 1);
            sum = newSum;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal Tanh(BigDecimal a) => Sinh(a) / Cosh(a);

    #endregion Hyperbolic methods

    #region Inverse hyperbolic methods

    /// <inheritdoc/>
    public static BigDecimal Asinh(BigDecimal x) => Log(x + Sqrt(Sqr(x) + 1));

    /// <inheritdoc/>
    public static BigDecimal Acosh(BigDecimal x) => Log(x + Sqrt(Sqr(x) - 1));

    /// <inheritdoc/>
    public static BigDecimal Atanh(BigDecimal x) => Log((1 + x) / (1 - x)) / 2;

    #endregion Inverse hyperbolic methods

    #region Methods for converting coordinates

    /// <summary>
    /// Convert cartesian coordinates to polar coordinates.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>A tuple containing the radius (r) and phase angle (theta).</returns>
    public static (BigDecimal r, BigDecimal theta) CartesianToPolar(BigDecimal x, BigDecimal y) =>
        x == 0 && y == 0 ? (0, 0) : (Hypot(x, y), Atan2(y, x));

    /// <summary>
    /// Convert polar coordinates to cartesian coordinates.
    /// </summary>
    /// <param name="r">The radius.</param>
    /// <param name="theta">The phase angle.</param>
    /// <returns>A tuple containing the x and y coordinates.</returns>
    public static (BigDecimal x, BigDecimal y) PolarToCartesian(BigDecimal r, BigDecimal theta) =>
        r == 0 ? (0, 0) : (r * Cos(theta), r * Sin(theta));

    #endregion Methods for converting coordinates

    #region Helper methods

    /// <summary>
    /// Shift given angle to the equivalent angle in the interval [-π, π).
    /// </summary>
    public static BigDecimal NormalizeAngle(in BigDecimal radians)
    {
        var a = radians % Tau;

        // The result of the modulo operator can be anywhere in the interval (-τ, τ) because
        // the default behaviour of modulo is to assign the sign of the dividend (the left-hand
        // operand) to the result. So if radians is negative, the result will be, too.
        // Therefore, we may need to shift the value once more to place it in the desired range.
        if (a < -Pi) return a + Tau;
        if (a >= Pi) return a - Tau;

        return a;
    }

    #endregion Helper methods
}
