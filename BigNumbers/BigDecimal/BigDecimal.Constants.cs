using System.Numerics;

namespace Galaxon.BigNumbers;

/// <summary>
/// Contains everything relating to constants.
/// </summary>
public partial struct BigDecimal
{
    #region E

    /// <summary>Cached value for e.</summary>
    private static BigDecimal _e;

    /// <inheritdoc/>
    public static BigDecimal E
    {
        get
        {
            if (_e.NumSigFigs == MaxSigFigs) return _e;
            if (_e.NumSigFigs > MaxSigFigs) return RoundSigFigs(_e);

            // Calculate e:
            _e = Exp(1);
            return _e;
        }
    }

    #endregion E

    #region Pi

    /// <summary>Cached value for π.</summary>
    private static BigDecimal _pi;

    /// <inheritdoc/>
    public static BigDecimal Pi
    {
        get
        {
            if (_pi.NumSigFigs == MaxSigFigs) return _pi;
            if (_pi.NumSigFigs > MaxSigFigs) return RoundSigFigs(_pi);

            _pi = ComputePi();
            return _pi;
        }
    }

    /// <summary>Compute π.</summary>
    /// <remarks>
    /// The Chudnovsky algorithm used here was the one used to generate π to 6.2 trillion decimal
    /// places, the current world record.
    /// <see href="https://en.wikipedia.org/wiki/Chudnovsky_algorithm"/>
    /// </remarks>
    public static BigDecimal ComputePi()
    {
        // Temporarily increase the maximum number of significant figures to ensure a correct
        // result. Tests have revealed 3 extra decimal places are needed.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 3;

        // Chudnovsky algorithm.
        var q = 0;
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
            var newSum = sum + M * L / X;

            // If adding the new term hasn't affected the sum, we're done.
            if (sum == newSum) break;

            // Prepare for next iteration.
            sum = newSum;
            L += 545_140_134;
            X *= -262_537_412_640_768_000;
            K += 12;
            M *= (Cube(K) - 16 * K) / Cube(q + 1);
            q++;
        }

        // Calculate pi.
        var pi = 426_880 * Sqrt(10_005) / sum;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(pi);
    }

    #endregion Pi

    #region Tau

    /// <summary>Cached value for τ.</summary>
    private static BigDecimal _tau;

    /// <inheritdoc/>
    public static BigDecimal Tau
    {
        get
        {
            if (_tau.NumSigFigs == MaxSigFigs) return _tau;
            if (_tau.NumSigFigs > MaxSigFigs) return RoundSigFigs(_tau);

            _tau = ComputeTau();
            return _tau;
        }
    }

    /// <summary>Compute the value of tau (τ), equal to 2 * pi (2π).</summary>
    /// <returns>The value of τ to the current number of significant figures.</returns>
    public static BigDecimal ComputeTau()
    {
        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // τ = 2π
        var tau = 2 * Pi;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(tau);
    }

    #endregion Tau

    #region HalfPi

    /// <summary>Cached value for π/2.</summary>
    private static BigDecimal _halfPi;

    /// <summary>Half pi (π/2).</summary>
    /// <remarks>
    /// This value is used in a number of calculations, so it felt warranted to cache it.
    /// </remarks>
    public static BigDecimal HalfPi
    {
        get
        {
            if (_halfPi.NumSigFigs == MaxSigFigs) return _halfPi;
            if (_halfPi.NumSigFigs > MaxSigFigs) return RoundSigFigs(_halfPi);

            _halfPi = ComputeHalfPi();
            return _halfPi;
        }
    }

    /// <summary>Compute the value of π/2.</summary>
    /// <returns>The value of π/2 to the current number of significant figures.</returns>
    public static BigDecimal ComputeHalfPi()
    {
        // Temporarily increase the maximum number of significant figures to ensure a correct
        // result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Compute the value.
        var halfPi = Pi / 2;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(halfPi);
    }

    #endregion HalfPi

    #region Phi

    /// <summary>Cached value for φ, the golden ratio.</summary>
    private static BigDecimal _phi;

    /// <summary>The golden ratio (φ).</summary>
    public static BigDecimal Phi
    {
        get
        {
            if (_phi.NumSigFigs == MaxSigFigs) return _phi;
            if (_phi.NumSigFigs > MaxSigFigs) return RoundSigFigs(_phi);

            _phi = ComputePhi();
            return _phi;
        }
    }

    /// <summary>Compute the value of phi (φ), the golden ratio.</summary>
    /// <returns>The value of φ to the current number of significant figures.</returns>
    public static BigDecimal ComputePhi()
    {
        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        var prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

        // Calculate phi.
        var phi = (1 + Sqrt(5)) / 2;

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundSigFigs(phi);
    }

    #endregion Phi

    #region Ln10

    /// <summary>Cached value for Log(10), the natural logarithm of 10.</summary>
    /// <remarks>
    /// This value is cached because of its use in the Log() method. We don't want to have to
    /// recompute Log(10) every time we call Log().
    /// </remarks>
    private static BigDecimal _ln10;

    /// <summary>The natural logarithm of 10.</summary>
    public static BigDecimal Ln10
    {
        get
        {
            if (_ln10.NumSigFigs == MaxSigFigs) return _ln10;
            if (_ln10.NumSigFigs > MaxSigFigs) return RoundSigFigs(_ln10);

            _ln10 = Log(10);
            return _ln10;
        }
    }

    #endregion Ln10
}
