using System.Numerics;

namespace Galaxon.Numerics.Types;

/// <summary>
/// Trigonometric methods for BigDecimal.
/// </summary>
/// <see href="https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions" />
/// <see href="https://en.wikipedia.org/wiki/Sine_and_cosine#Series_definitions" />
public partial struct BigDecimal : ITrigonometricFunctions<BigDecimal>,
    IHyperbolicFunctions<BigDecimal>
{
    /// <summary>
    /// Add or subtract multiples of τ to find an equivalent angle in the interval [-π, π)
    /// </summary>
    public static BigDecimal NormalizeAngle(in BigDecimal radians)
    {
        BigDecimal x = radians % Tau;
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
        BigDecimal HalfPi = Pi / 2;
        if (x == HalfPi)
        {
            return 1;
        }
        if (x == 3 * HalfPi)
        {
            return NegativeOne;
        }

        // Taylor series.
        int sign = 1;
        BigInteger m = 1; // m = 2n + 1
        BigDecimal xm = x;
        BigDecimal x2 = x * x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal newSum = sum + sign * xm / mf;

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

        return RoundMaxSigFigs(sum);
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

        BigDecimal HalfPi = Pi / 2;
        if (x == HalfPi || x == 3 * HalfPi)
        {
            return 0;
        }

        // Taylor series.
        // https://en.wikipedia.org/wiki/Taylor_series#Trigonometric_functions
        int sign = 1;
        BigInteger m = 0; // m = 2n
        BigDecimal xm = 1;
        BigDecimal x2 = x * x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal newSum = sum + sign * xm / mf;

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

        return RoundMaxSigFigs(sum);
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

    /// <summary>
    /// Cotangent function.
    /// </summary>
    public static BigDecimal Cot(BigDecimal x)
    {
        // Guard.
        if (x % Pi == 0)
        {
            throw new NotFiniteNumberException($"Cot(x) function is undefined at x={x}");
        }

        return 1 / Tan(x);
    }

    /// <summary>
    /// Secant function.
    /// </summary>
    public static BigDecimal Sec(BigDecimal x)
    {
        // Handle negative values.
        if (x < 0)
        {
            return Sec(-x);
        }

        // Find the equivalent angle in the interval [-π, π).
        BigDecimal x0 = x;
        x = NormalizeAngle(in x);

        // Guard.
        if (x == Pi / 2)
        {
            throw new NotFiniteNumberException(
                $"The secant function Sec(x) is undefined at x={x0}");
        }

        return 1 / Cos(x);
    }

    /// <summary>
    /// Cosecant function.
    /// </summary>
    public static BigDecimal Csc(BigDecimal x)
    {
        // Guard.
        if (x % Pi == 0)
        {
            throw new NotFiniteNumberException(
                $"The cosecant function Csc(x) is undefined at x={x}");
        }

        return 1 / Sin(x);
    }

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
        BigDecimal halfPi = Pi / 2;
        if (x == 1)
        {
            return halfPi;
        }

        // The Taylor series is slow to converge near x = ±1, but we can the following identity
        // relationship and calculate Asin() accurately and quickly for a smaller value:
        // Asin(x) = π/2 - Asin(√(1-x²))
        BigDecimal x2 = x * x;
        if (x > (BigDecimal)0.75)
        {
            return halfPi - Asin(Sqrt(1 - x2));
        }

        // Taylor series.
        BigInteger n = 1;
        BigInteger a = 1;
        BigInteger b = 2;
        BigInteger c = 3;
        BigDecimal xc = Cube(x); // x^c
        BigDecimal sum = x;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal term = (BigDecimal)a / b * xc / c;
            BigDecimal newSum = sum + term;

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

        return RoundMaxSigFigs(sum);
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
        int m = 1;
        BigDecimal xm = x;
        BigDecimal x2 = x * x;
        bool xIsSmall = x < 1;
        int sign = xIsSmall ? 1 : -1;
        BigDecimal sum = xIsSmall ? 0 : Pi / 2;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal term = (BigDecimal)sign / m * (xIsSmall ? xm : 1 / xm);
            BigDecimal newSum = sum + term;

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

        return RoundMaxSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal AtanPi(BigDecimal x) =>
        Atan(x) / Pi;

    /// <summary>
    /// Inverse cotangent function.
    /// </summary>
    public static BigDecimal Acot(BigDecimal x)
    {
        // Optimization.
        if (x == 0)
        {
            return 0;
        }

        return Atan(1 / x);
    }

    /// <summary>
    /// Inverse secant function.
    /// </summary>
    public static BigDecimal Asec(BigDecimal x)
    {
        if (Abs(x) < 1)
        {
            throw new NotFiniteNumberException(
                "The inverse secant function Asec(x) is undefined for |x| < 1.");
        }

        return Acos(1 / x);
    }

    /// <summary>
    /// Inverse cosecant function.
    /// </summary>
    public static BigDecimal Acsc(BigDecimal x)
    {
        if (Abs(x) < 1)
        {
            throw new NotFiniteNumberException(
                "The inverse cosecant function Acsc(x) is undefined for |x| < 1.");
        }

        return Asin(1 / x);
    }

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
        int m = 1;
        BigDecimal xm = x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal newSum = sum + xm / mf;

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

        return RoundMaxSigFigs(sum);
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
        int m = 0;
        BigDecimal xm = 1;
        BigDecimal x2 = x * x;
        BigInteger mf = 1;
        BigDecimal sum = 0;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal newSum = sum + xm / mf;

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

        return RoundMaxSigFigs(sum);
    }

    /// <inheritdoc />
    public static BigDecimal Tanh(BigDecimal x) =>
        Sinh(x) / Cosh(x);

    /// <summary>
    /// Hyperbolic cotangent function.
    /// </summary>
    public static BigDecimal Coth(BigDecimal x)
    {
        // Guard.
        if (x == 0)
        {
            throw new NotFiniteNumberException(
                "The hyperbolic cotangent function Coth(x) is undefined at x=0");
        }

        return Cosh(x) / Sinh(x);
    }

    /// <summary>
    /// Hyperbolic secant function.
    /// </summary>
    public static BigDecimal Sech(BigDecimal x) =>
        1 / Cosh(x);

    /// <summary>
    /// Hyperbolic cosecant function.
    /// </summary>
    public static BigDecimal Csch(BigDecimal x)
    {
        // Guard.
        if (x == 0)
        {
            throw new NotFiniteNumberException(
                "The hyperbolic cosecant function Csch(x) is undefined at x=0");
        }

        return 1 / Sinh(x);
    }

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

    /// <summary>
    /// Inverse hyperbolic cotangent function.
    /// </summary>
    public static BigDecimal Acoth(BigDecimal x) =>
        Log((x + 1) / (x - 1)) / 2;

    /// <summary>
    /// Inverse hyperbolic secant function.
    /// </summary>
    public static BigDecimal Asech(BigDecimal x) =>
        Log(1 / x + Sqrt(1 / (x * x) - 1));

    /// <summary>
    /// Inverse hyperbolic cosecant function.
    /// </summary>
    public static BigDecimal Acsch(BigDecimal x) =>
        Log(1 / x + Sqrt(1 / (x * x) + 1));

    #endregion Inverse hyperbolic functions
}
