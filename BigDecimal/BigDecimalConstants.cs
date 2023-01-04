using System.Numerics;

namespace Galaxon.Numerics.Types;

/// <summary>
/// Contains everything relating to constants.
/// </summary>
public partial struct BigDecimal
{
    /// <summary>
    /// Cached value for e.
    /// </summary>
    private static BigDecimal _e;

    /// <inheritdoc />
    public static BigDecimal E
    {
        get
        {
            if (_e.NumSigFigs >= MaxSigFigs)
            {
                return RoundMaxSigFigs(_e);
            }
            _e = ComputeE();
            return _e;
        }
    }

    /// <summary>
    /// Compute e.
    /// </summary>
    public static BigDecimal ComputeE() =>
        Exp(1);

    /// <summary>
    /// Cached value for π.
    /// </summary>
    private static BigDecimal _pi;

    /// <inheritdoc />
    public static BigDecimal Pi
    {
        get
        {
            if (_pi.NumSigFigs >= MaxSigFigs)
            {
                return RoundMaxSigFigs(_pi);
            }
            _pi = ComputePi();
            return _pi;
        }
    }

    /// <summary>
    /// Compute π.
    /// <see href="https://en.wikipedia.org/wiki/Chudnovsky_algorithm" />
    /// </summary>
    public static BigDecimal ComputePi()
    {
        // Temporarily increase the maximum number of significant figures to ensure a correct result.
        int prevMaxSigFigs = MaxSigFigs;
        MaxSigFigs += 2;

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

        // Restore the maximum number of significant figures.
        MaxSigFigs = prevMaxSigFigs;

        return RoundMaxSigFigs(pi);
    }

    /// <summary>
    /// Cached value for τ.
    /// </summary>
    private static BigDecimal _tau;

    /// <inheritdoc />
    public static BigDecimal Tau
    {
        get
        {
            if (_tau.NumSigFigs >= MaxSigFigs)
            {
                return RoundMaxSigFigs(_tau);
            }
            _tau = ComputeTau();
            return _tau;
        }
    }

    /// <summary>
    /// Compute τ.
    /// </summary>
    public static BigDecimal ComputeTau()
        => 2 * Pi;

    /// <summary>
    /// Cached value for φ, the golden ratio.
    /// </summary>
    private static BigDecimal _phi;

    /// <summary>
    /// The golden ratio (φ).
    /// </summary>
    public static BigDecimal Phi
    {
        get
        {
            if (_phi.NumSigFigs >= MaxSigFigs)
            {
                return RoundMaxSigFigs(_phi);
            }
            _phi = ComputePhi();
            return _phi;
        }
    }

    /// <summary>
    /// Compute φ.
    /// </summary>
    public static BigDecimal ComputePhi() =>
        (1 + Sqrt(5)) / 2;
}
