using System.Numerics;
using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics;

/// <summary>
/// Trigonometric methods for BigDecimal.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions" />
/// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions" />
public partial struct BigDecimal :
    ITrigonometricFunctions<BigDecimal>, IHyperbolicFunctions<BigDecimal>
{
    #region Trigonometric methods

    /// <inheritdoc />
    public static BigDecimal Sin(BigDecimal x)
    {
        // Find the equivalent angle in the interval [-π, π).
        x = NormalizeAngle(in x);

        // Optimizations.
        if (x == 0 || x == Pi)
        {
            return 0;
        }
        var halfPi = Pi / 2;
        if (x == halfPi)
        {
            return 1;
        }
        if (x == 3 * halfPi)
        {
            return NegativeOne;
        }

        // Taylor series.
        var sign = 1;
        BigInteger m = 1; // m = 2n + 1
        var xm = x;
        var x2 = x * x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + sign * xm / mf;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            xm *= x2;
            mf *= m * (m - 1);
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal SinPi(BigDecimal x) =>
        Sin(x * Pi);

    /// <inheritdoc />
    /// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions" />
    /// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions" />
    public static BigDecimal Cos(BigDecimal x)
    {
        // Find the equivalent angle in the interval [-π, π).
        x = NormalizeAngle(in x);

        // Optimizations.
        if (x == 0)
        {
            return 1;
        }

        if (x == Pi)
        {
            return NegativeOne;
        }

        var halfPi = Pi / 2;
        if (x == halfPi || x == 3 * halfPi)
        {
            return 0;
        }

        // Taylor series.
        // https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions
        var sign = 1;
        BigInteger m = 0; // m = 2n
        BigDecimal xm = 1;
        var x2 = x * x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + sign * xm / mf;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            xm *= x2;
            mf *= m * (m - 1);
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal CosPi(BigDecimal x) =>
        Cos(x * Pi);

    /// <inheritdoc />
    public static (BigDecimal Sin, BigDecimal Cos) SinCos(BigDecimal x) =>
        (Sin(x), Cos(x));

    /// <inheritdoc />
    public static (BigDecimal SinPi, BigDecimal CosPi) SinCosPi(BigDecimal x) =>
        (SinPi(x), CosPi(x));

    /// <inheritdoc />
    public static BigDecimal Tan(BigDecimal x)
    {
        // Find the equivalent angle in the interval [-π, π).
        x = NormalizeAngle(in x);

        // Check for valid argument.
        if (Abs(x) == Pi / 2)
        {
            throw new NotFiniteNumberException($"Tan(x) function is undefined at x={x}");
        }

        return Sin(x) / Cos(x);
    }

    /// <inheritdoc />
    public static BigDecimal TanPi(BigDecimal x) =>
        Tan(x * Pi);

    #endregion Trigonometric methods

    #region Inverse trigonometric methods

    /// <inheritdoc />
    public static BigDecimal Asin(BigDecimal a)
    {
        // Optimization.
        if (a == 0)
        {
            return 0;
        }

        // Handle negative arguments.
        if (a < 0)
        {
            return -Asin(-a);
        }

        // Guard.
        if (a > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(a), "Must be in the range -1..1.");
        }

        // Optimization.
        var halfPi = Pi / 2;
        if (a == 1)
        {
            return halfPi;
        }

        // The Taylor series is slow to converge near x = ±1, but we can the following identity
        // relationship and calculate Asin() accurately and quickly for a smaller value:
        // Asin(θ) = π/2 - Asin(√(1-θ²))
        var a2 = a * a;
        if (a > 0.75m)
        {
            return halfPi - Asin(Sqrt(1 - a2));
        }

        // Taylor series.
        BigInteger n = 1;
        BigInteger b = 1;
        BigInteger c = 2;
        BigInteger d = 3;
        var ac = Cube(a); // a^c
        var sum = a;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + b * ac / (c * d);

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            n++;
            b *= 2 * n - 1;
            c *= 2 * n;
            d += 2;
            ac *= a2;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal AsinPi(BigDecimal a) =>
        Asin(a) / Pi;

    /// <inheritdoc />
    public static BigDecimal Acos(BigDecimal a) =>
        Pi / 2 - Asin(a);

    /// <inheritdoc />
    public static BigDecimal AcosPi(BigDecimal a) =>
        Acos(a) / Pi;

    /// <inheritdoc />
    public static BigDecimal Atan(BigDecimal a)
    {
        // Optimization.
        if (a == 0)
        {
            return 0;
        }

        // Handle negative arguments.
        if (a < 0)
        {
            return -Atan(-a);
        }

        // Optimization.
        if (a == 1)
        {
            return Pi / 4;
        }

        // Taylor series.
        var m = 1;
        var am = a;
        var a2 = a * a;
        var small = a < 1;
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
            var newSum = sum + sign * (small ? am / m : 1 / (m * am));

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            sign = -sign;
            m += 2;
            am *= a2;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal AtanPi(BigDecimal a) =>
        Atan(a) / Pi;

    /// <summary>
    /// This two-argument variation of the Atan() method comes originally from FORTRAN.
    /// If x is non-negative, it will find the same result as Atan(y / x).
    /// If x is negative, the result will be offset by π.
    /// The purpose of the method is to produce a correct value for the polar angle when converting
    /// from cartesian coordinates to polar coordinates.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Atan2" />
    /// <see cref="CartesianToPolar" />
    /// <param name="y">The y coordinate.</param>
    /// <param name="x">The x coordinate.</param>
    /// <returns>The polar angle.</returns>
    /// <exception cref="ArgumentInvalidException">if x and y both equal 0.</exception>
    /// <see cref="double.AtanPi" />
    public static BigDecimal Atan2(BigDecimal y, BigDecimal x)
    {
        BigDecimal result;

        if (x == 0)
        {
            if (y == 0)
            {
                throw new ArgumentInvalidException(nameof(y),
                    "The atan() function is undefined for 0/0.");
            }

            result = Pi / 2;
            return y > 0 ? result : -result;
        }

        result = Atan(y / x);
        if (x > 0)
        {
            return result;
        }

        // x < 0
        return result + (y < 0 ? -Pi : Pi);
    }

    /// <summary>
    /// Computes the arc-tangent for the quotient of two values and divides the result by pi.
    /// </summary>
    /// <param name="y">The y coordinate.</param>
    /// <param name="x">The x coordinate.</param>
    /// <returns>The polar angle.</returns>
    /// <exception cref="ArgumentInvalidException">if x and y both equal 0.</exception>
    /// <see cref="double.Atan2Pi" />
    public static BigDecimal Atan2Pi(BigDecimal y, BigDecimal x) =>
        Atan2(y, x) / Pi;

    #endregion Inverse trigonometric methods

    #region Hyperbolic methods

    /// <inheritdoc />
    public static BigDecimal Sinh(BigDecimal x)
    {
        // Optimization.
        if (x == 0)
        {
            return 0;
        }

        // Taylor series.
        var m = 1;
        var xm = x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + xm / mf;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            m += 2;
            xm *= x * x;
            mf *= m * (m - 1);
            sum = newSum;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal Cosh(BigDecimal x)
    {
        // Optimization.
        if (x == 0)
        {
            return 1;
        }

        // Taylor series.
        var m = 0;
        BigDecimal xm = 1;
        var x2 = x * x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + xm / mf;

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            m += 2;
            xm *= x2;
            mf *= m * (m - 1);
            sum = newSum;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal Tanh(BigDecimal x) =>
        Sinh(x) / Cosh(x);

    #endregion Hyperbolic methods

    #region Inverse hyperbolic methods

    /// <inheritdoc />
    public static BigDecimal Asinh(BigDecimal a) =>
        Log(a + Sqrt(a * a + 1));

    /// <inheritdoc />
    public static BigDecimal Acosh(BigDecimal a) =>
        Log(a + Sqrt(a * a - 1));

    /// <inheritdoc />
    public static BigDecimal Atanh(BigDecimal a) =>
        Log((1 + a) / (1 - a)) / 2;

    #endregion Inverse hyperbolic methods

    #region Methods for converting coordinates

    /// <summary>
    /// Convert cartesian coordinates to polar coordinates.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>A tuple containing the radius (r) and angle (a).</returns>
    public (BigDecimal r, BigDecimal a) CartesianToPolar(BigDecimal x, BigDecimal y) =>
        x == 0 && y == 0 ? (0, 0) : (Hypot(x, y), Atan2(y, x));

    /// <summary>
    /// Convert polar coordinates to cartesian coordinates.
    /// </summary>
    /// <param name="r">The radius.</param>
    /// <param name="a">The angle.</param>
    /// <returns>A tuple containing the x and y coordinates.</returns>
    public (BigDecimal x, BigDecimal y) PolarToCartesian(BigDecimal r, BigDecimal a) =>
        r == 0 ? (0, 0) : (r * Cos(a), r * Sin(a));

    #endregion Methods for converting coordinates

    #region Other methods

    /// <summary>
    /// Shift given angle to the equivalent angle in the interval [-π, π).
    /// </summary>
    public static BigDecimal NormalizeAngle(in BigDecimal radians)
    {
        var x = radians % Tau;
        // The result of the modulo operator can be anywhere in the interval (-τ, τ) because
        // the default behaviour of modulo is to assign the sign of the dividend (the left-hand
        // operand) to the result. So if radians is negative, the result will be, too.
        // Therefore, we may need to shift the value once more to place it in the desired range.
        if (x < -Pi)
        {
            x += Tau;
        }
        else if (x >= Pi)
        {
            x -= Tau;
        }
        return x;
    }

    #endregion Other methods
}
