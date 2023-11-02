using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    #region Adjustment methods

    /// <inheritdoc/>
    public static BigDecimal Abs(BigDecimal bd) =>
        new (BigInteger.Abs(bd.Significand), bd.Exponent);

    /// <inheritdoc/>
    /// <remarks>
    /// The default rounding mode of MidpointRounding.ToEven is the same as used by similar methods
    /// in .NET Core.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.math.round?view=net-7.0#system-math-round(system-double-system-int32)"/>
    /// </remarks>
    public static BigDecimal Round(BigDecimal x, int digits = 0,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        // If it's an integer, no rounding required.
        if (x.Exponent >= 0) return x;

        // Find out how many digits to discard.
        var nDigitsToCut = -digits - x.Exponent;

        // Anything to do?
        if (nDigitsToCut <= 0) return x;

        // Round off the significand.
        var newSig = RoundSignificand(x.Significand, nDigitsToCut, mode);

        return new BigDecimal(newSig, -digits);
    }

    /// <summary>
    /// Given a significand and exponent, and a maximum number of significant figures, determine
    /// the new significand and exponent.
    /// </summary>
    /// <remarks>
    /// The default rounding mode of MidpointRounding.ToEven is the same as used by similar methods
    /// in .NET Core.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.math.round?view=net-7.0#system-math-round(system-double-system-int32)"/>
    /// </remarks>
    private static (BigInteger newSig, BigInteger newExp) RoundSigFigs(BigInteger sig,
        BigInteger exp, BigInteger maxSigFigs, MidpointRounding mode = MidpointRounding.ToEven)
    {
        // Guard.
        if (maxSigFigs <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxSigFigs), "Must be at least 1.");
        }

        // Get current num sig figs.
        var nSigFigs = sig.NumDigits();

        // Find out how many digits to discard.
        var nDigitsToCut = nSigFigs - maxSigFigs;

        // Anything to do?
        if (nDigitsToCut <= 0)
        {
            return (sig, exp);
        }

        // Round off the significand.
        var newSig = RoundSignificand(sig, nDigitsToCut, mode);

        return (newSig, exp + nDigitsToCut);
    }

    /// <summary>Round off a BigDecimal value to a certain number of significant figures.</summary>
    public static BigDecimal RoundSigFigs(BigDecimal x, BigInteger? maxSigFigs = null,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        maxSigFigs ??= MaxSigFigs;
        var (newSig, newExp) = RoundSigFigs(x.Significand, x.Exponent, maxSigFigs.Value,
            mode);
        return new BigDecimal(newSig, newExp);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Truncate(BigDecimal x) => Round(x, 0, MidpointRounding.ToZero);

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
    public static BigDecimal Frac(BigDecimal x) => x - Truncate(x);

    /// <inheritdoc/>
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Floor(BigDecimal x) =>
        Round(x, 0, MidpointRounding.ToNegativeInfinity);

    /// <inheritdoc/>
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Ceiling(BigDecimal x) =>
        Round(x, 0, MidpointRounding.ToPositiveInfinity);

    /// <summary>Divide by a power of 10 and round off to the nearest integer.</summary>
    /// <remarks>
    /// The default rounding mode of MidpointRounding.ToEven is the same as used by similar methods
    /// in .NET Core.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.math.round?view=net-7.0#system-math-round(system-double-system-int32)"/>
    /// </remarks>
    /// <param name="sig">The BigInteger value to round off.</param>
    /// <param name="nDigitsToCut">The power of 10 (number of digits to cut).</param>
    /// <param name="mode">The rounding mode.</param>
    /// <returns>The rounded off result of the division.</returns>
    private static BigInteger RoundSignificand(BigInteger sig, BigInteger nDigitsToCut,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        var exp10 = XBigInteger.Exp10(nDigitsToCut);
        var absSig = BigInteger.Abs(sig);
        var sign = sig.Sign;
        var quotient = absSig / exp10;
        var doubleRemainder = 2 * (absSig % exp10);

        // Check if rounding is necessary.
        var increment = mode switch
        {
            MidpointRounding.ToEven => doubleRemainder > exp10
                || (doubleRemainder == exp10 && BigInteger.IsOddInteger(quotient)),
            MidpointRounding.AwayFromZero => doubleRemainder >= exp10,
            MidpointRounding.ToZero => false,
            MidpointRounding.ToNegativeInfinity => sign < 0,
            MidpointRounding.ToPositiveInfinity => sign > 0,
            _ => false
        };
        if (increment)
        {
            quotient++;
        }

        return sign * quotient;
    }

    /// <summary>
    /// Move the decimal point to the right by the specified number of places.
    /// This will effectively multiply the significand by 10 and decrement the exponent to maintain
    /// the same value, the specified number of times.
    /// NB: The value will probably not be canonical after calling this method, so it should only
    /// be used on temporary variables.
    /// </summary>
    private void ShiftBy(BigInteger nPlaces)
    {
        // Guard.
        if (nPlaces < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nPlaces), "Cannot be negative.");
        }

        // See if there's anything to do.
        if (nPlaces == 0) return;

        // Shift.
        Significand *= XBigInteger.Exp10(nPlaces);
        Exponent -= nPlaces;
    }

    /// <summary>
    /// Shift such that the significand has a certain number of significant digits.
    /// NB: The value will probably not be canonical after calling this method, so it should only
    /// be used on temporary variables.
    /// </summary>
    private void ShiftToSigFigs(int? nSigFigs = null) =>
        ShiftBy((nSigFigs ?? MaxSigFigs) - Significand.NumDigits());

    /// <summary>
    /// Shift such that the exponent has a certain value.
    /// NB: The value will probably not be canonical after calling this method, so it should only
    /// be used on temporary variables.
    /// </summary>
    private void ShiftToExp(int exp) => ShiftBy(Exponent - exp);

    /// <summary>
    /// Adjust the parts of one of the values so both have the same exponent.
    /// Two new objects will be returned.
    /// </summary>
    private static (BigDecimal, BigDecimal) Align(BigDecimal x, BigDecimal y)
    {
        // See if there's anything to do.
        if (x.Exponent == y.Exponent) return (x, y);

        // Shift the value with the larger exponent so both have the same exponents.
        if (y.Exponent > x.Exponent)
        {
            y.ShiftBy(y.Exponent - x.Exponent);
        }
        else
        {
            x.ShiftBy(x.Exponent - y.Exponent);
        }

        return (x, y);
    }

    /// <summary>
    /// Modify the provided significand and exponent as needed to find the canonical form.
    /// Static form of the method, for use in the constructor.
    /// </summary>
    /// <returns>The two updated BigIntegers.</returns>
    private static (BigInteger, BigInteger) MakeCanonical(BigInteger significand,
        BigInteger exponent)
    {
        // Canonical form of zero.
        if (significand == 0)
        {
            exponent = 0;
        }
        else
        {
            // Canonical form of other values.
            // Remove any trailing 0s from the significand while incrementing the exponent.
            while (significand % 10 == 0)
            {
                significand /= 10;
                exponent++;
            }
        }
        return (significand, exponent);
    }

    #endregion Adjustment methods

    #region Arithmetic methods

    /// <summary>Clone method.</summary>
    /// <param name="bd">The BigComplex value to clone.</param>
    /// <returns>A new BigComplex with the same value as the parameter.</returns>
    public static BigDecimal Clone(BigDecimal bd) => new (bd.Significand, bd.Exponent);

    /// <summary>Negate method.</summary>
    /// <param name="bd">The BigDecimal value to negate.</param>
    /// <returns>The negation of the parameter.</returns>
    public static BigDecimal Negate(BigDecimal bd) => new (-bd.Significand, bd.Exponent);

    /// <summary>
    /// Addition method.
    /// </summary>
    /// <param name="a">The left-hand BigDecimal number.</param>
    /// <param name="b">The right-hand BigDecimal number.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigDecimal Add(BigDecimal a, BigDecimal b)
    {
        var (x, y) = Align(a, b);
        return RoundSigFigs(new BigDecimal(x.Significand + y.Significand, x.Exponent));
    }

    /// <summary>
    /// Subtraction method.
    /// </summary>
    /// <param name="a">The left-hand BigDecimal number.</param>
    /// <param name="b">The right-hand BigDecimal number.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static BigDecimal Subtract(BigDecimal a, BigDecimal b)
    {
        var (x, y) = Align(a, b);
        return RoundSigFigs(new BigDecimal(x.Significand - y.Significand, x.Exponent));
    }

    /// <summary>
    /// Increment method.
    /// </summary>
    /// <param name="a">The BigDecimal number.</param>
    /// <returns>The parameter incremented by 1.</returns>
    public static BigDecimal Increment(BigDecimal a) => Add(a, 1);

    /// <summary>
    /// Decrement method.
    /// </summary>
    /// <param name="a">The BigDecimal number.</param>
    /// <returns>The parameter decremented by 1.</returns>
    public static BigDecimal Decrement(BigDecimal a) => Subtract(a, 1);

    /// <summary>
    /// Multiply two BigDecimal values.
    /// </summary>
    /// <param name="a">The left-hand BigDecimal number.</param>
    /// <param name="b">The right-hand BigDecimal number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigDecimal Multiply(BigDecimal a, BigDecimal b) =>
        RoundSigFigs(new BigDecimal(a.Significand * b.Significand, a.Exponent + b.Exponent));

    /// <summary>
    /// Divide a BigDecimal by a BigDecimal.
    /// </summary>
    /// <remarks>
    /// Computes division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division"/>
    /// </remarks>
    /// <param name="a">The left-hand BigDecimal number.</param>
    /// <param name="b">The right-hand BigDecimal number.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="System.DivideByZeroException">If b == 0</exception>
    public static BigDecimal Divide(BigDecimal a, BigDecimal b)
    {
        // Guard.
        if (b == 0)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Optimizations.
        if (b == 1) return a;
        if (a == b) return 1;

        // Find f ~= 1/b as an initial estimate of the multiplication factor.

        // We can quickly get a very good initial estimate by leveraging the decimal type.
        // In other places we've used the double type for calculating estimates, both for speed and
        // to access methods that the decimal type doesn't provide. However, because division may be
        // needed when casting from double to BigDecimal, using double here causes infinite
        // recursion. Casting from decimal to BigDecimal doesn't require division so it doesn't have
        // that problem.

        var bR = RoundSigFigs(b, DecimalPrecision);
        BigDecimal f = 1 / (decimal)bR.Significand;
        f.Exponent -= bR.Exponent;

        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        while (true)
        {
            a *= f;
            b *= f;

            // If y is 1, then n is the result.
            if (b == 1) break;

            f = 2 - b;

            // If y is not 1, but is close to 1, then f can be 1 due to rounding after the
            // subtraction. If it is, there's no point continuing.
            if (f == 1) break;
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(a);
    }

    /// <summary>
    /// Calculate reciprocal.
    /// </summary>
    /// <returns>The reciprocal of the argument.</returns>
    public static BigDecimal Reciprocal(BigDecimal z) => Divide(1, z);

    /// <summary>
    /// Divides two BigDecimal values together to compute their modulus or remainder.
    /// </summary>
    /// <param name="a">The value which b divides.</param>
    /// <param name="b">The value which divides a.</param>
    /// <returns>The modulus or remainder of a divided by b.</returns>
    public static BigDecimal Modulus(BigDecimal a, BigDecimal b) => a - Truncate(a / b) * b;

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
    public static BigDecimal operator +(BigDecimal bd) => Clone(bd);

    /// <inheritdoc/>
    public static BigDecimal operator -(BigDecimal bd) => Negate(bd);

    /// <inheritdoc/>
    public static BigDecimal operator +(BigDecimal a, BigDecimal b) => Add(a, b);

    /// <inheritdoc/>
    public static BigDecimal operator -(BigDecimal a, BigDecimal b) => Subtract(a, b);

    /// <inheritdoc/>
    public static BigDecimal operator ++(BigDecimal bd) => Increment(bd);

    /// <inheritdoc/>
    public static BigDecimal operator --(BigDecimal bd) => Decrement(bd);

    /// <inheritdoc/>
    public static BigDecimal operator *(BigDecimal a, BigDecimal b) => Multiply(a, b);

    /// <inheritdoc/>
    public static BigDecimal operator /(BigDecimal a, BigDecimal b) => Divide(a, b);

    /// <inheritdoc/>
    public static BigDecimal operator %(BigDecimal a, BigDecimal b) => Modulus(a, b);

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    /// <param name="a">The base.</param>
    /// <param name="b">The exponent.</param>
    /// <returns>The first operand raised to the power of the second.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent is negative or imaginary.
    /// </exception>
    public static BigDecimal operator ^(BigDecimal a, BigDecimal b) => Pow(a, b);

    #endregion Arithmetic operators
}
