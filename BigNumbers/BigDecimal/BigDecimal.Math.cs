using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    #region Numeric methods

    /// <inheritdoc/>
    public static BigDecimal Abs(BigDecimal bd)
    {
        return new BigDecimal(BigInteger.Abs(bd.Significand), bd.Exponent);
    }

    /// <inheritdoc/>
    /// <summary>Round off a value to a given number of decimal places.</summary>
    /// <remarks>
    /// The default rounding mode of MidpointRounding.ToEven is the same as used by similar methods
    /// in .NET Core.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.math.round?view=net-7.0#system-math-round(system-double-system-int32)"/>
    /// </remarks>
    public static BigDecimal Round(BigDecimal x, int nDecimals = 0,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        // Guard.
        if (nDecimals < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nDecimals), "Must not be negative.");
        }

        // Calculate how many digits to cut.
        var nDigitsToCut = -x.Exponent - nDecimals;

        return _CutDigits(x, nDigitsToCut, mode);
    }

    /// <summary>Round off a BigDecimal value to a certain number of significant figures.</summary>
    public static BigDecimal RoundSigFigs(BigDecimal x, int? maxSigFigs = null,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        // Guard.
        if (maxSigFigs is < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxSigFigs), "Must be at least 1.");
        }

        // Calculate how many digits to cut.
        var nDigitsToCut = x.NumSigFigs - (maxSigFigs ?? MaxSigFigs);

        return _CutDigits(x, nDigitsToCut, mode);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Truncate(BigDecimal x)
    {
        return Round(x, 0, MidpointRounding.ToZero);
    }

    /// <summary>Return the fractional part of the value.</summary>
    /// <remarks>
    /// There are multiple ways to define the frac() function for negative numbers.
    /// (Refer to the Wikipedia link below.)
    /// The definition used in this implementation simply takes the digits to the right of the
    /// decimal point, with the sign matching the argument.
    /// e.g.
    /// Frac(12.345) => 0.345
    /// Frac(-12.345) => -0.345
    /// The following expression will be true for both positive and negative numbers:
    /// x == Truncate(x) + Frac(x)
    /// </remarks>
    /// <see href="https://en.wikipedia.org/wiki/Fractional_part"/>
    public static BigDecimal Frac(BigDecimal x)
    {
        return x - Truncate(x);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Floor(BigDecimal x)
    {
        return Round(x, 0, MidpointRounding.ToNegativeInfinity);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Ceiling(BigDecimal x)
    {
        return Round(x, 0, MidpointRounding.ToPositiveInfinity);
    }

    #endregion Numeric methods

    #region Arithmetic methods

    /// <summary>Negate method.</summary>
    /// <param name="bd">The BigDecimal value to negate.</param>
    /// <returns>The negation of the parameter.</returns>
    public static BigDecimal Negate(BigDecimal bd)
    {
        return new BigDecimal(-bd.Significand, bd.Exponent);
    }

    /// <summary>Add 2 BigDecimals.</summary>
    /// <param name="x">The left-hand BigDecimal number.</param>
    /// <param name="y">The right-hand BigDecimal number.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigDecimal Add(BigDecimal x, BigDecimal y)
    {
        // If the orders of magnitude of the two values are too different, adding them will have no
        // effect. We want to detect this situation to save time, and avoid the call to Align(),
        // which can cause overflow if the difference in exponents is very large.
        var xMaxExp = x.Exponent + x.NumSigFigs - 1;
        var yMaxExp = y.Exponent + y.NumSigFigs - 1;
        if (xMaxExp - MaxSigFigs > yMaxExp)
        {
            return x;
        }
        else if (yMaxExp - MaxSigFigs > xMaxExp)
        {
            return y;
        }

        // Make the exponents the same.
        var (x2Sig, y2Sig, exp) = Align(x, y);

        // Sum the significands and round off.
        return RoundSigFigs(new BigDecimal(x2Sig + y2Sig, exp));
    }

    /// <summary>Subtract one BigDecimal from another.</summary>
    /// <param name="a">The left-hand BigDecimal number.</param>
    /// <param name="b">The right-hand BigDecimal number.</param>
    /// <returns>The result of the subtraction.</returns>
    public static BigDecimal Subtract(BigDecimal a, BigDecimal b)
    {
        return Add(a, -b);
    }

    /// <summary>
    /// Increment method.
    /// </summary>
    /// <param name="a">The BigDecimal number.</param>
    /// <returns>The parameter incremented by 1.</returns>
    public static BigDecimal Increment(BigDecimal a)
    {
        return Add(a, 1);
    }

    /// <summary>
    /// Decrement method.
    /// </summary>
    /// <param name="a">The BigDecimal number.</param>
    /// <returns>The parameter decremented by 1.</returns>
    public static BigDecimal Decrement(BigDecimal a)
    {
        return Add(a, -1);
    }

    /// <summary>
    /// Multiply two BigDecimal values.
    /// </summary>
    /// <param name="a">The left-hand BigDecimal number.</param>
    /// <param name="b">The right-hand BigDecimal number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigDecimal Multiply(BigDecimal a, BigDecimal b)
    {
        return RoundSigFigs(new BigDecimal(a.Significand * b.Significand, a.Exponent + b.Exponent));
    }

    /// <summary>
    /// Divide a BigDecimal by a BigDecimal.
    /// </summary>
    /// <remarks>
    /// Computes division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division"/>
    /// </remarks>
    /// <param name="x">The left-hand BigDecimal number.</param>
    /// <param name="y">The right-hand BigDecimal number.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If b == 0</exception>
    public static BigDecimal Divide(BigDecimal x, BigDecimal y)
    {
        // Guard.
        if (y == 0)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Optimizations.
        if (y == 1) return x;
        if (x == y) return 1;

        // Find f ~= 1/b as an initial estimate of the multiplication factor.

        // We can quickly get a very good initial estimate by leveraging the decimal type.
        // In other places we've used the double type for calculating estimates, both for speed and
        // to access methods that the decimal type doesn't provide. However, because division may be
        // needed when casting from double to BigDecimal, using double here causes infinite
        // recursion. Casting from decimal to BigDecimal doesn't require division so it doesn't have
        // that problem.

        var yRounded = RoundSigFigs(y, DecimalPrecision);
        BigDecimal f = 1 / (decimal)yRounded.Significand;
        f.Exponent -= yRounded.Exponent;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        while (true)
        {
            x *= f;
            y *= f;

            // If y is 1, then n is the result.
            if (y == 1) break;

            f = 2 - y;

            // If y is not 1, but is close to 1, then f can be 1 due to rounding after the
            // subtraction. If it is, there's no point continuing.
            if (f == 1) break;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(x);
    }

    /// <summary>
    /// Calculate reciprocal.
    /// </summary>
    /// <returns>The reciprocal of the argument.</returns>
    public static BigDecimal Reciprocal(BigDecimal x)
    {
        return Divide(1, x);
    }

    /// <summary>
    /// Divides two BigDecimal values together to compute their modulus or remainder.
    /// </summary>
    /// <param name="x">The value which y divides.</param>
    /// <param name="y">The value which divides x.</param>
    /// <returns>The modulus or remainder of x divided by y.</returns>
    public static BigDecimal Modulus(BigDecimal x, BigDecimal y)
    {
        return x - Truncate(x / y) * y;
    }

    /// <summary>
    /// Compute the arithmetic-geometric mean of two values.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Arithmetic%E2%80%93geometric_mean"/>
    public static BigDecimal ArithmeticGeometricMean(BigDecimal x, BigDecimal y)
    {
        // Guards.
        if (x <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "Must be positive.");
        }
        if (y <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y), "Must be positive.");
        }

        var a0 = x;
        var g0 = y;
        BigDecimal result;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        while (true)
        {
            var a1 = (a0 + g0) / 2;
            var g1 = Sqrt(a0 * g0);

            // Test for equality.
            if (a1 == g1)
            {
                result = RoundSigFigs(a1, prevMaxSigFigs);
                break;
            }

            // Test for equality post-rounding.
            var a1R = RoundSigFigs(a1, prevMaxSigFigs);
            var g1R = RoundSigFigs(g1, prevMaxSigFigs);
            if (a1R == g1R)
            {
                result = a1R;
                break;
            }

            a0 = a1;
            g0 = g1;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return result;
    }

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <inheritdoc/>
    public static BigDecimal operator +(BigDecimal bd)
    {
        return bd;
    }

    /// <inheritdoc/>
    public static BigDecimal operator -(BigDecimal bd)
    {
        return Negate(bd);
    }

    /// <inheritdoc/>
    public static BigDecimal operator +(BigDecimal a, BigDecimal b)
    {
        return Add(a, b);
    }

    /// <inheritdoc/>
    public static BigDecimal operator -(BigDecimal a, BigDecimal b)
    {
        return Subtract(a, b);
    }

    /// <inheritdoc/>
    public static BigDecimal operator ++(BigDecimal bd)
    {
        return Increment(bd);
    }

    /// <inheritdoc/>
    public static BigDecimal operator --(BigDecimal bd)
    {
        return Decrement(bd);
    }

    /// <inheritdoc/>
    public static BigDecimal operator *(BigDecimal a, BigDecimal b)
    {
        return Multiply(a, b);
    }

    /// <inheritdoc/>
    public static BigDecimal operator /(BigDecimal a, BigDecimal b)
    {
        return Divide(a, b);
    }

    /// <inheritdoc/>
    public static BigDecimal operator %(BigDecimal a, BigDecimal b)
    {
        return Modulus(a, b);
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    /// <param name="a">The base.</param>
    /// <param name="b">The exponent.</param>
    /// <returns>The first operand raised to the power of the second.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent is negative or imaginary.
    /// </exception>
    public static BigDecimal operator ^(BigDecimal a, BigDecimal b)
    {
        return Pow(a, b);
    }

    #endregion Arithmetic operators

    #region Helper methods

    /// <summary>
    /// Remove some digits from the end of the significand, rounding off if needed, using the
    /// strategy specified by the rounding mode.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="nDigitsToCut"></param>
    /// <param name="mode"></param>
    /// <returns>The rounded-off value.</returns>
    private static BigDecimal _CutDigits(BigDecimal x, int nDigitsToCut,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        // Anything to do?
        if (nDigitsToCut <= 0) return x;

        // Get current num of significant figures.
        var nSigFigs = x.NumSigFigs;

        // Find out how many digits to keep.
        var nDigitsToKeep = nSigFigs - nDigitsToCut;

        // Get the sign.
        var sign = x.Sign;

        // Truncate the significand.
        var digits = x.DigitsString;
        var newSig = nDigitsToKeep <= 0 ? 0 : BigInteger.Parse(digits[..nDigitsToKeep]);

        // Determine from the rounding method if we should increment the new significand.
        bool increment = false;
        switch (mode)
        {
            case MidpointRounding.AwayFromZero:
            {
                // See if the first decimal place is 5 or higher.
                var firstDecimal = BigInteger.Parse(digits[nDigitsToKeep].ToString());
                increment = firstDecimal >= 5;
                break;
            }

            case MidpointRounding.ToEven:
            {
                // If the decimal fraction is > 0.5, round up.
                // If the decimal fraction is < 0.5, don't round up.
                // If it's exactly 0.5, round up if the new significand is odd.
                var firstDecimal = BigInteger.Parse(digits[nDigitsToKeep].ToString());
                increment = firstDecimal > 5 || (firstDecimal == 5 && (nSigFigs - nDigitsToKeep > 1
                    || (nSigFigs - nDigitsToKeep == 1 && BigInteger.IsOddInteger(newSig))));
                break;
            }

            case MidpointRounding.ToPositiveInfinity:
                increment = sign > 0;
                break;

            case MidpointRounding.ToNegativeInfinity:
                increment = sign < 0;
                break;

            case MidpointRounding.ToZero:
                // Do nothing.
                break;
        }

        // Do the increment if necessary.
        if (increment)
        {
            newSig++;
        }

        return new BigDecimal(sign * newSig, x.Exponent + nDigitsToCut);
    }

    /// <summary>
    /// Generate a new significand, found by shifting the exponent of the BigDecimal to the provided
    /// new exponent.
    /// The combination of the result significand and the new exponent parameter represent a new
    /// BigDecimal value equal to the provided value.
    /// </summary>
    private static BigInteger _Shift(BigDecimal bd, int newExponent = 0)
    {
        // See if there's anything to do.
        if (bd.Exponent == newExponent)
        {
            return bd.Significand;
        }

        // Return the shifted significand.
        return bd.Significand * XBigInteger.Exp10(bd.Exponent - newExponent);
    }

    /// <summary>
    /// Adjust the significand and exponent of one of the values so both have the same exponent.
    /// </summary>
    public static (BigInteger, BigInteger, int) Align(BigDecimal x, BigDecimal y)
    {
        // See if there's anything to do.
        if (x.Exponent == y.Exponent)
        {
            return (x.Significand, y.Significand, x.Exponent);
        }

        // Shift the value with the larger exponent so both have the same exponents.
        if (y.Exponent > x.Exponent)
        {
            var ySig = _Shift(y, x.Exponent);
            return (x.Significand, ySig, x.Exponent);
        }

        // x.Exponent > y.Exponent
        var xSig = _Shift(x, y.Exponent);
        return (xSig, y.Significand, y.Exponent);
    }

    /// <summary>
    /// Modify the provided significand and exponent as needed to find the canonical form.
    /// Static form of the method, for use in the constructor.
    /// </summary>
    /// <returns>The two updated BigIntegers.</returns>
    private static (BigInteger, int) _MakeCanonical(BigInteger significand, int exponent)
    {
        // Canonical form of zero.
        if (significand == 0)
        {
            exponent = 0;
        }
        else
        {
            // Remove any trailing 0s from the significand while incrementing the exponent.
            while (significand % 10 == 0)
            {
                significand /= 10;
                exponent++;
            }
        }

        return (significand, exponent);
    }

    #endregion Helper methods
}
