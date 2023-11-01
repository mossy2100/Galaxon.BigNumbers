using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

/// <summary>
/// Old, cut code, keeping just in case.
/// </summary>
public partial struct BigDecimal
{
    public static BigDecimal LogHalleys(BigDecimal a)
    {
        // Guards.
        if (a == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(a),
                "Logarithm of 0 is -∞, which cannot be expressed using a BigDecimal.");
        }
        if (a < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(a),
                "Logarithm of a negative value is a complex number, which cannot be expressed using a BigDecimal.");
        }

        // Optimization.
        if (a == 1)
        {
            return 0;
        }

        // Shortcut for Log(10).
        if (a == 10 && _ln10.NumSigFigs >= MaxSigFigs)
        {
            return RoundSigFigs(_ln10);
        }

        // Scale the value to the range (0..1).
        var nDigits = a.Significand.NumDigits();
        var scale = nDigits + a.Exponent;
        var x = a;
        x.Exponent = -nDigits;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Halley's method.
        BigDecimal y0 = 0;
        BigDecimal expY0 = 1;
        var dY0 = 2 * (x - 1) / (x + 1);
        BigDecimal result = 0;

        while (true)
        {
            // Get the next value.
            var expY1 = expY0 + expY0 * (Exp(dY0) - 1);
            var y1 = y0 + dY0;

            // Test for equality.
            if (y0 == y1)
            {
                result = y0;
                break;
            }

            // Test for equality post-rounding.
            var y0R = RoundSigFigs(y0, prevMaxSigFigs);
            var y1R = RoundSigFigs(y1, prevMaxSigFigs);
            if (y0R == y1R)
            {
                result = y0R;
                break;
            }

            // Compare two results that differ by the smallest possible amount.
            // We need this check to prevent infinite loops that alternate between adjacent values.
            y0R.ShiftToSigFigs(prevMaxSigFigs);
            y1R.ShiftToSigFigs(prevMaxSigFigs);
            if (BigInteger.Abs(y0R.Significand - y1R.Significand) == 1)
            {
                // Test both and pick the best one.
                var diff0 = Abs(a - Exp(y0R));
                var diff1 = Abs(a - Exp(y1R));
                result = diff0 < diff1 ? y0R : y1R;
                break;
            }

            // Next iteration.
            y0 = y1;
            expY0 = expY1;
            dY0 = 2 * (x - expY1) / (x + expY1);
        }

        // Special handling for Log(10) to avoid infinite recursion.
        result = a == 10 ? -result : result + scale * Ln10;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        // Scale back.
        return RoundSigFigs(result);
    }

    public static BigDecimal LogAgm(BigDecimal x)
    {
        // Guards.
        if (x == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x),
                "Logarithm of 0 is -∞, which cannot be expressed using a BigDecimal.");
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

        // Scale the value to the range (0..1) so the Taylor series converges quickly and to avoid
        // overflow.
        // int nDigits = a.Significand.NumDigits();
        // int scale = nDigits + a.Exponent;
        // BigDecimal x = a;
        // x.Exponent = -nDigits;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 3;

        // Select a value "m" such that m bits can store the required number of decimal digits.
        // A quick and easy way to find m without using Log() uses 10 bits per 3 decimal digits.
        // m must be an int to avoid recursively calling Log() when we call Exp2() below, so we
        // round up to the nearest integer.
        var m = (int)double.Ceiling((double)MaxSigFigs / 3 * 10);

        // Calculate result.
        var s = x * Exp2(m - 2);
        var agm = ArithmeticGeometricMean(1, 1 / s);
        var p = Pi / (2 * agm);
        var result = x == 2
            ? p / (1 + m)
            : p - m * LogAgm(2);

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(result);
    }
    
    /// <summary>
    /// Possibly faster method of calculating tan, using a Taylor series.
    /// TODO Debug. Still isn't working properly. I may just cut it.
    /// </summary>
    /// <param name="x">The value to calculate tan(x) for.</param>
    /// <returns>The tangent of the value x.</returns>
    public static BigDecimal Tan2(BigDecimal x)
    {
        // Find the equivalent angle in the interval [-π, π).
        x = NormalizeAngle(in x);

        // Initialize sum.
        BigDecimal sum = 0;

        // Initially, k = 1, but since we don't use k directly, we don't need to create a variable.

        // Sign starts positive, alternating for each term.
        var sign = 1;

        // p = 4^k
        var p = 4;

        // k2 = 2 * k
        var k2 = 2;

        // f = (2k)! = Factorial(2 * k)
        var f = 2;

        // xx = x^(2k - 1)
        var xx = x;

        while (true)
        {
            // Calculate the new term.
            var term = sign * p * (p - 1) * Bernoulli(k2) * xx / f;

            // Add it to the sum and see if it made any difference.
            var newSum = sum + term;
            if (sum == newSum) break;

            // Prepare for next loop iteration.
            sum = newSum;
            sign = -sign;
            p *= 4;
            xx *= Sqr(x);
            k2 += 2;
            f *= k2 * (k2 - 1);
        }

        return RoundSigFigs(sum);
    }

}
