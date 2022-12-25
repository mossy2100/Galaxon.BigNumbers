using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Types;

public partial struct BigDecimal
{
    #region Adjustment methods

    /// <inheritdoc />
    public object Clone() =>
        (BigDecimal)MemberwiseClone();

    /// <inheritdoc />
    public static BigDecimal Abs(BigDecimal bd) =>
        new (BigInteger.Abs(bd.Significand), bd.Exponent);

    /// <inheritdoc />
    public static BigDecimal Round(BigDecimal x, int nDecimalPlaces = 0,
        MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        // If it's an integer, no rounding required.
        if (x.Exponent >= 0)
        {
            return x;
        }

        // Find out how many digits to discard.
        int nDigitsToCut = -nDecimalPlaces - x.Exponent;

        // Anything to do?
        if (nDigitsToCut <= 0)
        {
            return x;
        }

        // Round off the significand.
        BigInteger newSig = RoundSignificand(x.Significand, nDigitsToCut, mode);

        return new BigDecimal(newSig, -nDecimalPlaces);
    }

    /// <summary>
    /// Remove fraction part.
    /// </summary>
    public static BigDecimal Trunc(BigDecimal x) =>
        Round(x, 0, MidpointRounding.ToZero);

    /// <summary>
    /// Round to nearest integer less than or equal to the argument.
    /// </summary>
    public static BigDecimal Floor(BigDecimal x) =>
        Round(x, 0, MidpointRounding.ToNegativeInfinity);

    /// <summary>
    /// Round to nearest integer less than or equal to the argument.
    /// </summary>
    public static BigDecimal Ceiling(BigDecimal x) =>
        Round(x, 0, MidpointRounding.ToPositiveInfinity);

    private static BigInteger RoundSignificand(BigInteger sig, int nDigitsToCut,
        MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        BigInteger pow = BigInteger.Pow(10, nDigitsToCut);
        BigInteger absSig = BigInteger.Abs(sig);
        int sign = sig.Sign;
        BigInteger q = absSig / pow;
        BigInteger twoRem = 2 * (absSig % pow);

        // Check if rounding is necessary.
        bool increment = mode switch
        {
            MidpointRounding.ToEven => twoRem > pow || twoRem == pow && BigInteger.IsOddInteger(q),
            MidpointRounding.AwayFromZero => twoRem >= pow,
            MidpointRounding.ToZero => false,
            MidpointRounding.ToNegativeInfinity => sign < 0,
            MidpointRounding.ToPositiveInfinity => sign > 0,
            _ => false
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
        int exp, int maxSigFigs, MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        // Guard.
        if (maxSigFigs <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxSigFigs), "Must be at least 1.");
        }

        // Get current num sig figs.
        int nSigFigs = sig.NumDigits();

        // Find out how many digits to discard.
        int nDigitsToCut = nSigFigs - maxSigFigs;

        // Anything to do?
        if (nDigitsToCut <= 0)
        {
            return (sig, exp);
        }

        // Round off the significand.
        BigInteger newSig = RoundSignificand(sig, nDigitsToCut, mode);

        return (newSig, exp + nDigitsToCut);
    }

    /// <summary>
    /// Round off a value to a certain number of significant figures.
    /// </summary>
    public static BigDecimal RoundSigFigs(BigDecimal x, int maxSigFigs,
        MidpointRounding mode = MidpointRounding.AwayFromZero)
    {
        (BigInteger newSig, int newExp) = RoundSigFigs(x.Significand, x.Exponent, maxSigFigs,
            mode);
        return new BigDecimal(newSig, newExp);
    }

    /// <summary>
    /// Round off a value to the maximum number of significant figures.
    /// </summary>
    public static BigDecimal RoundSigFigsMax(BigDecimal x) =>
        RoundSigFigs(x, MaxSigFigs);

    /// <summary>
    /// Round off a value to the maximum number of significant figures.
    /// </summary>
    public static BigDecimal RoundSigFigsDouble(BigDecimal x) =>
        RoundSigFigs(x, _DoubleMaxSigFigs);

    /// <summary>
    /// Multiply the significand by 10 and decrement the exponent to maintain the same value,
    /// <paramref name="nPlaces"/> times.
    /// </summary>
    /// <param name="nPlaces"></param>
    private void ShiftBy(int nPlaces)
    {
        // Guard.
        if (nPlaces < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nPlaces), "Cannot be negative.");
        }

        // See if there's anything to do.
        if (nPlaces == 0)
        {
            return;
        }

        Significand *= BigInteger.Pow(10, nPlaces);
        Exponent -= nPlaces;
    }

    private void ShiftTo(int? nSigFigs = null)
    {
        nSigFigs ??= MaxSigFigs;
        ShiftBy(nSigFigs.Value - Significand.NumDigits());
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

    #region Arithmetic operators

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal bd) =>
        (BigDecimal)bd.Clone();

    /// <inheritdoc />
    public static BigDecimal operator +(BigDecimal a, BigDecimal b)
    {
        (BigDecimal x, BigDecimal y) = Align(a, b);
        return new BigDecimal(x.Significand + y.Significand, x.Exponent, true);
    }

    /// <inheritdoc />
    public static BigDecimal operator ++(BigDecimal bd) =>
        bd + One;

    /// <inheritdoc />
    public static BigDecimal operator -(BigDecimal bd) =>
        new (-bd.Significand, bd.Exponent, true);

    /// <inheritdoc />
    public static BigDecimal operator -(BigDecimal a, BigDecimal b)
    {
        (BigDecimal x, BigDecimal y) = Align(a, b);
        return new BigDecimal(x.Significand - y.Significand, x.Exponent, true);
    }

    /// <inheritdoc />
    public static BigDecimal operator --(BigDecimal bd) =>
        bd - One;

    /// <inheritdoc />
    public static BigDecimal operator *(BigDecimal a, BigDecimal b) =>
        new (a.Significand * b.Significand, a.Exponent + b.Exponent, true);

    /// <inheritdoc />
    /// <remarks>
    /// Computes division using the Goldschmidt algorithm.
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division" />
    /// </remarks>
    public static BigDecimal operator /(BigDecimal a, BigDecimal b)
    {
        // Guard.
        if (b == Zero)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Optimizations.
        if (b == One)
        {
            return a;
        }
        if (a == b)
        {
            return One;
        }

        // Find f, a good initial estimate of the multiplication factor. Approximately 1/b.
        // The fastest way is by using doubles.

        // Get the first 17 digits of the significand. This matches the maximum precision of double.
        BigDecimal bRound = RoundSigFigsDouble(b);

        // Calculate f.
        BigDecimal f = 1 / (double)bRound.Significand;
        f.Exponent -= bRound.Exponent;

        while (true)
        {
            a *= f;
            b *= f;

            // If y is 1, then n is the result.
            if (b == One)
            {
                break;
            }

            f = Two - b;

            // If y is not 1, but is close to 1, then f can be 1 due to rounding after the
            // subtraction. If it is, there's no point continuing.
            if (f == One)
            {
                break;
            }
        }

        return a;
    }

    /// <inheritdoc />
    public static BigDecimal operator %(BigDecimal a, BigDecimal b) =>
        a - Trunc(a / b) * b;

    #endregion Arithmetic operators

    #region Methods for computing constants

    /// <summary>
    /// Compute π up to a specified number of decimal places.
    /// <see href="https://en.wikipedia.org/wiki/Chudnovsky_algorithm" />
    /// </summary>
    public static BigDecimal ComputePi(int nDecimalPlaces)
    {
        int nSigFigs = nDecimalPlaces + 1;

        // Use the built-in constant if possible.
        if (nSigFigs <= Pi.NumSigFigs)
        {
            return RoundSigFigs(Pi, nSigFigs);
        }

        // Set the max sig figs for the calculation.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs = nSigFigs + 2;

        // Chudnovsky algorithm.
        int q = 0;
        BigInteger L = 13_591_409;
        BigInteger X = 1;
        BigInteger K = -6;
        BigDecimal M = 1;

        // Add terms in the series until doing so ceases to affect the result.
        // The more significant figures wanted, the longer the process will take.
        BigDecimal sum = 0;
        while (true)
        {
            // Add the next term.
            BigDecimal newSum = sum + (M * L / X);

            // If adding the new term hasn't affected the sum, we're done.
            if (sum == newSum)
            {
                break;
            }

            // Prepare for next iteration.
            sum = newSum;
            L += 545_140_134;
            X *= -262_537_412_640_768_000;
            K += 12;
            M *= (Cube(K) - 16 * K) / Cube(q + 1);
            q++;
        }

        // Calculate pi.
        BigDecimal pi = 426_880 * Sqrt(10_005) / sum;

        // Restore significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(pi, nSigFigs);
    }

    /// <summary>
    /// Compute τ up to a specified number of decimal places.
    /// </summary>
    public static BigDecimal ComputeTau(int nDecimalPlaces)
    {
        int nSigFigs = nDecimalPlaces + 1;

        // Use the built-in constant if possible.
        if (nSigFigs <= Tau.NumSigFigs)
        {
            return RoundSigFigs(Tau, nSigFigs);
        }

        // Set the max sig figs for the calculation.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs = nSigFigs + 2;

        // Do the computation.
        BigDecimal tau = 2 * ComputePi(nDecimalPlaces + 2);

        // Restore significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(tau, nSigFigs);
    }

    /// <summary>
    /// Compute e up to a specified number of decimal places.
    /// </summary>
    public static BigDecimal ComputeE(int nDecimalPlaces)
    {
        int nSigFigs = nDecimalPlaces + 1;

        // Use the built-in constant if possible.
        if (nSigFigs <= E.NumSigFigs)
        {
            return RoundSigFigs(E, nSigFigs);
        }

        // Set the max sig figs for the calculation.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs = nSigFigs + 2;

        // Compute e.
        BigDecimal e = Exp(1);

        // Restore significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(e, nSigFigs);
    }

    /// <summary>
    /// Compute φ up to a specified number of decimal places.
    /// </summary>
    public static BigDecimal ComputePhi(int nDecimalPlaces)
    {
        int nSigFigs = nDecimalPlaces + 1;

        // Use the built-in constant if possible.
        if (nSigFigs <= Phi.NumSigFigs)
        {
            return RoundSigFigs(Phi, nSigFigs);
        }

        // Set the max sig figs for the calculation.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs = nSigFigs + 2;

        // Do the computation.
        BigDecimal phi = (1 + Sqrt(5)) / 2;

        // Restore significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(phi, nSigFigs);
    }

    #endregion Methods for computing constants
}
