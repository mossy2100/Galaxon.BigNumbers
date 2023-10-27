using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    #region Adjustment methods

    /// <inheritdoc />
    public static BigDecimal Abs(BigDecimal bd)
    {
        return new BigDecimal(BigInteger.Abs(bd.Significand), bd.Exponent);
    }

    /// <inheritdoc />
    public static BigDecimal Round(BigDecimal x, int digits = 0,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        // If it's an integer, no rounding required.
        if (x.Exponent >= 0)
        {
            return x;
        }

        // Find out how many digits to discard.
        var nDigitsToCut = -digits - x.Exponent;

        // Anything to do?
        if (nDigitsToCut <= 0)
        {
            return x;
        }

        // Round off the significand.
        var newSig = RoundSignificand(x.Significand, nDigitsToCut, mode);

        return new BigDecimal(newSig, -digits);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Round(BigDecimal x, MidpointRounding mode)
    {
        return Round(x, 0, mode);
    }

    /// <summary>
    /// Round off a value to a certain number of significant figures.
    /// </summary>
    public static BigDecimal RoundSigFigs(BigDecimal x, int? maxSigFigs = null,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        maxSigFigs ??= MaxSigFigs;
        var (newSig, newExp) = RoundSigFigs(x.Significand, x.Exponent, maxSigFigs.Value,
            mode);
        return new BigDecimal(newSig, newExp);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Truncate(BigDecimal x)
    {
        return Round(x, 0, MidpointRounding.ToZero);
    }

    /// <summary>
    /// Return the fractional part of the value.
    /// There are multiple ways to define the frac() function for negative numbers.
    /// (Refer to the Wikipedia link below.)
    /// The definition used in this implementation simply takes the digits to the right of the
    /// decimal point, with the sign matching the argument.
    /// e.g.
    /// Frac(12.345) => 0.345
    /// Frac(-12.345) => -0.345
    /// The following expression will be true for both positive and negative numbers:
    /// x == Truncate(x) + Frac(x)
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Fractional_part" />
    public static BigDecimal Frac(BigDecimal x)
    {
        return x - Truncate(x);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Floor(BigDecimal x)
    {
        return Round(x, 0, MidpointRounding.ToNegativeInfinity);
    }

    /// <inheritdoc />
    /// <remarks>
    /// This method should not need to be implemented because it's a static virtual method and the
    /// default implementation is what we want. However, static virtual methods are not yet
    /// supported by Rider so we need this here for now.
    /// </remarks>
    public static BigDecimal Ceiling(BigDecimal x)
    {
        return Round(x, 0, MidpointRounding.ToPositiveInfinity);
    }

    private static BigInteger RoundSignificand(BigInteger sig, int nDigitsToCut,
        MidpointRounding mode = MidpointRounding.ToEven)
    {
        var pow = BigInteger.Pow(10, nDigitsToCut);
        var absSig = BigInteger.Abs(sig);
        var sign = sig.Sign;
        var q = absSig / pow;
        var twoRem = 2 * (absSig % pow);

        // Check if rounding is necessary.
        var increment = mode switch
        {
            MidpointRounding.ToEven => twoRem > pow || twoRem == pow && BigInteger.IsOddInteger(q),
            MidpointRounding.AwayFromZero => twoRem >= pow,
            MidpointRounding.ToZero => false,
            MidpointRounding.ToNegativeInfinity => sign < 0,
            MidpointRounding.ToPositiveInfinity => sign > 0,
            _ => false,
        };

        if (increment)
        {
            q++;
        }

        return sign * q;
    }

    /// <summary>
    /// Given a significand and exponent, and a maximum number of significant figures, determine
    /// the new significand and exponent.
    /// </summary>
    private static (BigInteger newSig, int newExp) RoundSigFigs(BigInteger sig,
        int exp, int maxSigFigs, MidpointRounding mode = MidpointRounding.ToEven)
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

    /// <summary>
    /// Move the decimal point to the right by the specified number of places.
    /// This will effectively multiply the significand by 10 and decrement the exponent to maintain
    /// the same value, the specified number of times.
    /// NB: The value will probably not be canonical after calling this method, so it should only
    /// be used on temporary variables.
    /// </summary>
    private void ShiftBy(int nPlaces)
    {
        switch (nPlaces)
        {
            // Guard.
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(nPlaces), "Cannot be negative.");

            // See if there's anything to do.
            case 0:
                return;

            default:
                Significand *= BigInteger.Pow(10, nPlaces);
                Exponent -= nPlaces;
                break;
        }
    }

    /// <summary>
    /// Shift such that the significand has a certain number of significant digits.
    /// NB: The value will probably not be canonical after calling this method, so it should only
    /// be used on temporary variables.
    /// </summary>
    private void ShiftToSigFigs(int? nSigFigs = null)
    {
        ShiftBy((nSigFigs ?? MaxSigFigs) - Significand.NumDigits());
    }

    /// <summary>
    /// Shift such that the exponent has a certain value.
    /// NB: The value will probably not be canonical after calling this method, so it should only
    /// be used on temporary variables.
    /// </summary>
    private void ShiftToExp(int exp)
    {
        ShiftBy(Exponent - exp);
    }

    /// <summary>
    /// Adjust the parts of one of the values so both have the same exponent.
    /// Two new objects will be returned.
    /// </summary>
    private static (BigDecimal, BigDecimal) Align(BigDecimal x, BigDecimal y)
    {
        // See if there's anything to do.
        if (x.Exponent == y.Exponent)
        {
            return (x, y);
        }

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
    private static (BigInteger, int) MakeCanonical(BigInteger significand, int exponent)
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

    /// <summary>
    /// Make the value into its canonical form.
    /// Any trailing 0s on the significand are removed, and this information is transferred to the
    /// exponent.
    /// This method mutates the object; it doesn't return a new object like most of the other
    /// methods, because no information is lost.
    /// </summary>
    /// <returns>The instance, which is useful for method chaining.</returns>
    private BigDecimal MakeCanonical()
    {
        (Significand, Exponent) = MakeCanonical(Significand, Exponent);
        return this;
    }

    #endregion Adjustment methods

    #region Arithmetic methods

    /// <summary>
    /// Clone method.
    /// </summary>
    /// <returns>A copy of the parameter.</returns>
    public static BigDecimal Clone(BigDecimal z)
    {
        return new BigDecimal(z.Significand, z.Exponent);
    }

    /// <summary>
    /// Negate method.
    /// </summary>
    /// <returns>The negation of the parameter.</returns>
    public static BigDecimal Negate(BigDecimal bd)
    {
        return new BigDecimal(-bd.Significand, bd.Exponent, true);
    }

    /// <summary>
    /// Addition method.
    /// </summary>
    /// <param name="a">The left-hand BigNumbers number.</param>
    /// <param name="b">The right-hand BigNumbers number.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigDecimal Add(BigDecimal a, BigDecimal b)
    {
        var (x, y) = Align(a, b);
        return new BigDecimal(x.Significand + y.Significand, x.Exponent, true);
    }

    /// <summary>
    /// Subtraction method.
    /// </summary>
    /// <param name="a">The left-hand BigNumbers number.</param>
    /// <param name="b">The right-hand BigNumbers number.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static BigDecimal Subtract(BigDecimal a, BigDecimal b)
    {
        var (x, y) = Align(a, b);
        return new BigDecimal(x.Significand - y.Significand, x.Exponent, true);
    }

    /// <summary>
    /// Increment method.
    /// </summary>
    /// <param name="a">The BigNumbers number.</param>
    /// <returns>The parameter incremented by 1.</returns>
    public static BigDecimal Increment(BigDecimal a)
    {
        return Add(a, 1);
    }

    /// <summary>
    /// Decrement method.
    /// </summary>
    /// <param name="a">The BigNumbers number.</param>
    /// <returns>The parameter decremented by 1.</returns>
    public static BigDecimal Decrement(BigDecimal a)
    {
        return Subtract(a, 1);
    }

    /// <summary>
    /// Multiply two BigNumbers values.
    /// </summary>
    /// <param name="a">The left-hand BigNumbers number.</param>
    /// <param name="b">The right-hand BigNumbers number.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigDecimal Multiply(BigDecimal a, BigDecimal b)
    {
        return new BigDecimal(a.Significand * b.Significand, a.Exponent + b.Exponent, true);
    }

    /// <summary>
    /// Divide a BigNumbers by a BigNumbers.
    /// </summary>
    /// <remarks>
    /// Computes division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division" />
    /// </remarks>
    /// <param name="a">The left-hand BigNumbers number.</param>
    /// <param name="b">The right-hand BigNumbers number.</param>
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
        if (b == 1)
        {
            return a;
        }
        if (a == b)
        {
            return 1;
        }

        // Find f ~= 1/b as an initial estimate of the multiplication factor.

        // We can quickly get a very good initial estimate by leveraging the decimal type.
        // In other places we've used the double type for calculating estimates, both for speed and
        // to access methods that the decimal type doesn't provide. However, because division may be
        // needed when casting from double to BigNumbers, using double here causes infinite
        // recursion. Casting from decimal to BigNumbers doesn't require division so it doesn't have
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
            if (b == 1)
            {
                break;
            }

            f = 2 - b;

            // If y is not 1, but is close to 1, then f can be 1 due to rounding after the
            // subtraction. If it is, there's no point continuing.
            if (f == 1)
            {
                break;
            }
        }

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(a);
    }

    /// <summary>
    /// Calculate reciprocal.
    /// </summary>
    /// <returns>The reciprocal of the argument.</returns>
    public static BigDecimal Reciprocal(BigDecimal z)
    {
        return Divide(1, z);
    }

    /// <summary>
    /// Divides two BigNumbers values together to compute their modulus or remainder.
    /// </summary>
    /// <param name="a">The value which b divides.</param>
    /// <param name="b">The value which divides a.</param>
    /// <returns>The modulus or remainder of a divided by b.</returns>
    public static BigDecimal Modulus(BigDecimal a, BigDecimal b)
    {
        return a - Truncate(a / b) * b;
    }

    /// <summary>
    /// Compute the arithmetic-geometric mean of two values.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Arithmetic%E2%80%93geometric_mean" />
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

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal bd)
    {
        return Clone(bd);
    }

    /// <inheritdoc />
    public static BigDecimal operator -(BigDecimal bd)
    {
        return Negate(bd);
    }

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal a, BigDecimal b)
    {
        return Add(a, b);
    }

    /// <inheritdoc />
    public static BigDecimal operator -(BigDecimal a, BigDecimal b)
    {
        return Subtract(a, b);
    }

    /// <inheritdoc />
    public static BigDecimal operator ++(BigDecimal bd)
    {
        return Increment(bd);
    }

    /// <inheritdoc />
    public static BigDecimal operator --(BigDecimal bd)
    {
        return Decrement(bd);
    }

    /// <inheritdoc />
    public static BigDecimal operator *(BigDecimal a, BigDecimal b)
    {
        return Multiply(a, b);
    }

    /// <inheritdoc />
    public static BigDecimal operator /(BigDecimal a, BigDecimal b)
    {
        return Divide(a, b);
    }

    /// <inheritdoc />
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
}
