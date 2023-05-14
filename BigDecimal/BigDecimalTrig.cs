using System.Numerics;

namespace Galaxon.Numerics;

/// <summary>
/// Trigonometric methods for BigDecimal.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions" />
/// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions" />
public partial struct BigDecimal : ITrigonometricFunctions<BigDecimal>,
    IHyperbolicFunctions<BigDecimal>
{
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

    #region Trigonometric functions

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

    #endregion Trigonometric functions

    #region Inverse trigonometric functions

    /// <inheritdoc />
    public static BigDecimal Asin(BigDecimal x)
    {
        // Optimization.
        if (x == 0)
        {
            return 0;
        }

        // Handle negative arguments.
        if (x < 0)
        {
            return -Asin(-x);
        }

        // Guard.
        if (x > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Must be in the range -1..1.");
        }

        // Optimization.
        var halfPi = Pi / 2;
        if (x == 1)
        {
            return halfPi;
        }

        // The Taylor series is slow to converge near x = ±1, but we can the following identity
        // relationship and calculate Asin() accurately and quickly for a smaller value:
        // Asin(x) = π/2 - Asin(√(1-x²))
        var x2 = x * x;
        if (x > 0.75m)
        {
            return halfPi - Asin(Sqrt(1 - x2));
        }

        // Taylor series.
        BigInteger n = 1;
        BigInteger a = 1;
        BigInteger b = 2;
        BigInteger c = 3;
        var xc = Cube(x); // x^c
        var sum = x;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + a * xc / (b * c);

            // If adding the new term hasn't affected the result, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            n++;
            a *= 2 * n - 1;
            b *= 2 * n;
            c += 2;
            xc *= x2;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal AsinPi(BigDecimal x) =>
        Asin(x) / Pi;

    /// <inheritdoc />
    public static BigDecimal Acos(BigDecimal x) =>
        Pi / 2 - Asin(x);

    /// <inheritdoc />
    public static BigDecimal AcosPi(BigDecimal x) =>
        Acos(x) / Pi;

    /// <inheritdoc />
    public static BigDecimal Atan(BigDecimal x)
    {
        // Optimization.
        if (x == 0)
        {
            return 0;
        }

        // Handle negative arguments.
        if (x < 0)
        {
            return -Atan(-x);
        }

        // Optimization.
        if (x == 1)
        {
            return Pi / 4;
        }

        // Taylor series.
        var m = 1;
        var xm = x;
        var x2 = x * x;
        var xIsSmall = x < 1;
        var sign = xIsSmall ? 1 : -1;
        var sum = xIsSmall ? 0 : Pi / 2;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            var newSum = sum + sign * (xIsSmall ? xm / m : 1 / (m * xm));

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
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal AtanPi(BigDecimal x) =>
        Atan(x) / Pi;

    #endregion Inverse trigonometric functions

    #region Hyperbolic functions

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

    #endregion Hyperbolic functions

    #region Inverse hyperbolic functions

    /// <inheritdoc />
    public static BigDecimal Asinh(BigDecimal x) =>
        Log(x + Sqrt(x * x + 1));

    /// <inheritdoc />
    public static BigDecimal Acosh(BigDecimal x) =>
        Log(x + Sqrt(x * x - 1));

    /// <inheritdoc />
    public static BigDecimal Atanh(BigDecimal x) =>
        Log((1 + x) / (1 - x)) / 2;

    #endregion Inverse hyperbolic functions
}
