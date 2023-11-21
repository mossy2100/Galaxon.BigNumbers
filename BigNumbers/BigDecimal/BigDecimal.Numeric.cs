using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

/// <summary>Struct members relating to BigDecimal numerical methods.</summary>
public partial struct BigDecimal
{
    /// <inheritdoc/>
    public static BigDecimal Abs(BigDecimal x)
    {
        return new BigDecimal(BigInteger.Abs(x.Significand), x.Exponent);
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
    /// There are multiple ways to define the frac() function for negative numbers:
    /// <see href="https://en.wikipedia.org/wiki/Fractional_part"/>
    /// The definition used in this implementation simply keeps the digits to the right of the
    /// decimal point, and keeps the sign.
    /// e.g.
    /// Frac(12.345) => 0.345
    /// Frac(-12.345) => -0.345
    /// The following expression will be true for both positive and negative numbers:
    /// x == Truncate(x) + Frac(x)
    /// </remarks>
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

        // Add guard digits to reduce accumulated errors due to rounding.
        var sf = AddGuardDigits(2);

        while (true)
        {
            var a1 = (a0 + g0) / 2;
            var g1 = Sqrt(a0 * g0);

            // Test for equality.
            if (a1 == g1)
            {
                result = a1;
                break;
            }

            // Test for equality post-rounding.
            var a1R = RoundSigFigs(a1, sf);
            var g1R = RoundSigFigs(g1, sf);
            if (a1R == g1R)
            {
                result = a1R;
                break;
            }

            a0 = a1;
            g0 = g1;
        }

        // Restore the maximum number of significant figures.
        return RemoveGuardDigits(result, sf);
    }

    /// <summary>
    /// Add guard digits. This reduces errors due to rounding when performing a series of
    /// calculations.
    /// </summary>
    /// <param name="nDigits">The number of guard digits to add.</param>
    /// <returns>The previous number of guard digits.</returns>
    public static int AddGuardDigits(int nDigits)
    {
        var sigFigs = MaxSigFigs;
        MaxSigFigs += nDigits;
        return sigFigs;
    }

    /// <summary>
    /// Restore the maximum number of significant figures to a former value guard, and round off a
    /// BigDecimal value to the new maximum number of significant figures.
    /// </summary>
    /// <param name="x">The BigDecimal value to round off.</param>
    /// <param name="sigFigs">The previous maximum number of significant figures to restore.</param>
    /// <returns>The BigDecimal value rounded off.</returns>
    public static BigDecimal RemoveGuardDigits(BigDecimal x, int sigFigs)
    {
        MaxSigFigs = sigFigs;
        return RoundSigFigs(x);
    }

    // /// <summary>Run a computation with guard digits.</summary>
    // /// <param name="func">A function that returns a BigDecimal value.</param>
    // /// <param name="nGuardDigits">The number of guard digits to add before the method.</param>
    // /// <returns>The result of the calculation.</returns>
    // public static BigDecimal DoWithGuardDigits(Func<BigDecimal> func, int nGuardDigits)
    // {
    //     var sf = AddGuardDigits(nGuardDigits);
    //     var result = func();
    //     return RemoveGuardDigits(result, sf);
    // }

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
        if (nDigitsToCut <= 0)
        {
            return x;
        }

        // Get current num of significant figures.
        var nSigFigs = x.NumSigFigs;

        // Find out how many digits to keep.
        var nDigitsToKeep = nSigFigs - nDigitsToCut;
        if (nDigitsToKeep < 0)
        {
            nDigitsToKeep = 0;
        }

        // Get the sign.
        var sign = x.Sign;

        // Truncate the significand.
        var digits = x.DigitsString;
        var newSig = nDigitsToKeep <= 0 ? 0 : BigInteger.Parse(digits[..nDigitsToKeep]);

        // Determine from the rounding method if we should increment the new significand.
        var increment = false;
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
                increment = firstDecimal > 5
                    || (firstDecimal == 5 && nDigitsToCut > 1)
                    || (firstDecimal == 5 && nDigitsToCut == 1 && BigInteger.IsOddInteger(newSig));
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
    /// The combination of the result significand and the provided exponent parameter represent a
    /// new BigDecimal value equal to the provided value.
    /// </summary>
    private static BigInteger _Shift(BigDecimal x, int newExponent = 0)
    {
        // See if there's anything to do.
        if (x.Exponent == newExponent)
        {
            return x.Significand;
        }

        // Return the shifted significand.
        return x.Significand * XBigInteger.Exp10(x.Exponent - newExponent);
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
}
