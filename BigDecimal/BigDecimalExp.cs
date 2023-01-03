using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Types;

/// <summary>
/// Power, root, exponential, and logarithm methods for BigDecimal.
/// </summary>
public partial struct BigDecimal : IPowerFunctions<BigDecimal>, IRootFunctions<BigDecimal>,
    IExponentialFunctions<BigDecimal>, ILogarithmicFunctions<BigDecimal>
{
    #region IPowerFunctions

    public static BigDecimal Pow(BigDecimal x, BigDecimal y)
    {
        // Handle negative powers.
        if (y < Zero)
        {
            return One / Pow(x, -y);
        }

        // Anything to the power of 0 (including 0) is 1.
        if (y == Zero)
        {
            return One;
        }

        // Anything to the power of 1 is itself.
        if (y == One)
        {
            return x;
        }

        // 0 to any power other than 0 is 0.
        if (x == Zero)
        {
            return Zero;
        }

        // If the exponent is an integer we can computer a result quickly with exponentiation by
        // squaring.
        if (IsInteger(y))
        {
            // Even integer powers.
            if (IsEvenInteger(y))
            {
                return Pow(Sqr(x), y / Two);
            }

            // Odd integer powers.
            return x * Pow(Sqr(x), (y - One) / Two);
        }

        // For positive x with non-integer exponent, compute the result using Exp() and Ln().
        if (x > Zero)
        {
            return Exp(y * Log(x));
        }

        // For negative x with non-integer exponent, we need to get the exponent as a ratio in order
        // to determine whether or not we can compute a result.
        // Casting to BigRational will create a rational number, with no loss of precision, and
        // reduce it.
        // The only issue here is if the exponent does not exactly represent the intended value.
        // e.g. 1/3 cannot be stored exactly using a BigDecimal.
        // Thus Pow(-27, 1/3), for example, will not work. You have to use Cbrt().
        BigRational br = (BigRational)y;
        // To compute the result using RootN() the denominator of the fraction must be odd and in
        // the range of int.
        if (br.Denominator > int.MaxValue)
        {
            // Can't compute a real result.
            throw new ArgumentInvalidException(nameof(y),
                $"Cannot compute a result as the exponent denominator ({br.Denominator}) is "
                + $"greater than {int.MaxValue}.");
        }
        if (BigInteger.IsEvenInteger(br.Denominator))
        {
            // Can't compute a real result.
            throw new ArgumentInvalidException(nameof(y),
                $"Cannot compute a result as the exponent denominator ({br.Denominator}) is even.");
        }
        return Pow(RootN(x, (int)br.Denominator), br.Numerator);
    }

    public static BigDecimal Sqr(BigDecimal x) =>
        x * x;

    public static BigDecimal Cube(BigDecimal x) =>
        x * x * x;

    #endregion IPowerFunctions

    #region IRootFunctions

    /// <inheritdoc />
    public static BigDecimal RootN(BigDecimal a, int n)
    {
        // Check a.
        // If n is even, a negative value for a is unsupported as there will be no real results,
        // only complex ones.
        if (a < Zero && int.IsEvenInteger(n))
        {
            throw new ArgumentInvalidException(nameof(n),
                "Negative numbers have no real even roots.");
        }
        if (a == Zero)
        {
            return Zero;
        }
        if (a == One)
        {
            return One;
        }

        // Check n.
        // A negative root is the inverse of the positive root.
        if (n < 0)
        {
            return One / RootN(a, -n);
        }
        // 0th root unsolvable.
        if (n == 0)
        {
            throw new ArgumentInvalidException(nameof(n),
                "The 0th root is unsolvable since any number to the power of 0 is 1.");
        }
        // The 1st root of a number is itself.
        if (n == 1)
        {
            return a;
        }

        // Get an initial estimate using double, which should be pretty fast.
        // Reduce operand to the maximum number of significant digits supported by the double type.
        BigDecimal aRound = RoundSigFigs(a, DoubleMaxSigFigs);
        BigDecimal x0 = double.RootN((double)aRound.Significand, n)
            * Exp10((double)aRound.Exponent / n);

        // Check if our estimate is already our result.
        if (Pow(x0, n) == a)
        {
            return x0;
        }

        // Increase the maximum number of significant figures for the duration of the loop.
        // This ensures an accurate result despite loss of precision due to rounding when generating
        // next values in the loop.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs++;

        BigDecimal result;

        // Newton's method. Iterate until we get the result.
        // int nLoops = 0;
        while (true)
        {
            // Get the next value of y.
            BigDecimal x0ToThePowerOfNMinus1 = Pow(x0, n - 1);
            BigDecimal x0ToThePowerOfN = x0ToThePowerOfNMinus1 * x0;
            BigDecimal x1 = x0 - (x0ToThePowerOfN - a) / (n * x0ToThePowerOfNMinus1);

            // Test for equality.
            if (x0 == x1)
            {
                result = RoundSigFigs(x0, prevMaxSigFigs);
                break;
            }

            // Test for equality post-rounding.
            BigDecimal x0Round = RoundSigFigs(x0, prevMaxSigFigs);
            BigDecimal x1Round = RoundSigFigs(x1, prevMaxSigFigs);
            if (x0Round == x1Round)
            {
                result = x0Round;
                break;
            }

            // Compare two results that differ by the smallest possible amount.
            // We need this check to prevent infinite loops that alternate between adjacent values.
            x0Round.ShiftTo(prevMaxSigFigs);
            x1Round.ShiftTo(prevMaxSigFigs);
            if (BigInteger.Abs(x0Round.Significand - x1Round.Significand) == 1)
            {
                // Test both and pick the best one.
                BigDecimal diff0 = Abs(a - Pow(x0Round, n));
                BigDecimal diff1 = Abs(a - Pow(x1Round, n));
                result = diff0 < diff1 ? x0Round : x1Round;
                break;
            }

            // Next iteration.
            x0 = x1;

            // Prevent infinite loops. Remove later, after testing.
            // nLoops++;
            // if (nLoops == 10)
            // {
            //     throw new Exception($"Problem with RootN({a}, {n}).");
            // }
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return result;
    }

    public static BigDecimal Sqrt(BigDecimal x) =>
        RootN(x, 2);

    public static BigDecimal Cbrt(BigDecimal x) =>
        RootN(x, 3);

    public static BigDecimal Hypot(BigDecimal x, BigDecimal y) =>
        Sqrt(Sqr(x) + Sqr(y));

    #endregion IRootFunctions

    #region IExponentialFunctions

    /// <inheritdoc />
    public static BigDecimal Exp(BigDecimal x)
    {
        // Optimizations.
        if (x == Zero)
        {
            return One;
        }

        BigDecimal result;

        // If the exponent is negative, inverse the result of the positive exponent.
        if (x < Zero)
        {
            MaxSigFigs += 2;
            result = One / Exp(-x);
            MaxSigFigs -= 2;
            return RoundMaxSigFigs(result);
        }

        // Taylor/Maclaurin series.
        // https://en.wikipedia.org/wiki/Taylor_series#Exponential_function
        BigInteger n = 0;
        BigDecimal xToThePowerOfN = 1;
        BigInteger nFactorial = 1;
        result = 0;

        // Temporarily increase precision to ensure a correct result.
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series.
            BigDecimal newResult = result + xToThePowerOfN / nFactorial;

            // If adding the new term hasn't affected the result, we're done.
            if (result == newResult)
            {
                break;
            }

            // Prepare for next iteration.
            result = newResult;
            n++;
            xToThePowerOfN *= x;
            nFactorial *= n;
        }

        // Restore significant figures.
        MaxSigFigs -= 2;

        return RoundMaxSigFigs(result);
    }

    /// <inheritdoc />
    public static BigDecimal Exp2(BigDecimal x) =>
        Pow(Two, x);

    /// <inheritdoc />
    public static BigDecimal Exp10(BigDecimal x) =>
        Pow(Ten, x);

    #endregion IExponentialFunctions

    #region ILogarithmicFunctions

    /// <inheritdoc />
    public static BigDecimal Log(BigDecimal a)
    {
        // Guards.
        if (a == Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(a),
                "Logarithm of 0 is -∞, which cannot be expressed using a BigDecimal.");
        }
        if (a < Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(a),
                "Logarithm of a negative value is a complex number, which cannot be expressed using a BigDecimal.");
        }

        // Optimization.
        if (a == One)
        {
            return 0;
        }

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        BigInteger scale = a.Significand.NumDigits() + a.Exponent;
        BigDecimal x = a / Exp10(scale);

        // Taylor/Newton-Mercator series.
        // https://en.wikipedia.org/wiki/Mercator_series
        x--;
        int n = 1;
        int sign = 1;
        BigDecimal xToThePowerOfN = x;
        BigDecimal result = 0;

        // Temporarily increase precision to ensure a correct result.
        MaxSigFigs += 2;

        // Add terms until the process ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        while (true)
        {
            // Add the next term in the series
            BigDecimal newResult = result + (sign * xToThePowerOfN / n);

            // If adding the new term hasn't affected the result, we're done.
            if (newResult == result)
            {
                break;
            }

            // Prepare for next iteration.
            result = newResult;
            n++;
            sign = -sign;
            xToThePowerOfN *= x;
        }

        // Restore significant figures.
        MaxSigFigs -= 2;

        // Scale back.
        return result + scale * Log(10);
    }

    /// <inheritdoc />
    public static BigDecimal Log(BigDecimal x, BigDecimal b)
    {
        if (b == One)
        {
            throw new ArgumentOutOfRangeException(nameof(b),
                "Logarithms are undefined for a base of 1.");
        }

        // 0^0 == 1. Mimics Math.Log().
        if (x == One && b == Zero)
        {
            return Zero;
        }

        // This will throw if x <= 0 || b <= 0.
        return Log(x) / Log(b);
    }

    /// <inheritdoc />
    public static BigDecimal Log2(BigDecimal x) =>
        Log(x, Two);

    /// <inheritdoc />
    public static BigDecimal Log10(BigDecimal x) =>
        Log(x, Ten);

    #endregion ILogarithmicFunctions
}
