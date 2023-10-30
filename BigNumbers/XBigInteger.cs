using System.Numerics;
using Galaxon.Core.Strings;

namespace Galaxon.BigNumbers;

/// <summary>Extension methods for BigInteger.</summary>
public static class XBigInteger
{
    #region Fields

    /// <summary>
    /// Cache for GreatestCommonDivisor().
    /// </summary>
    private static readonly Dictionary<string, BigInteger> s_gcdCache = new ();

    #endregion Fields

    #region Miscellaneous other methods

    /// <summary>
    /// Get the unsigned, twos-complement version of the value, containing the fewest number of
    /// bytes.
    /// </summary>
    public static BigInteger ToUnsigned(this BigInteger n)
    {
        // Check if anything to do.
        if (n >= 0)
        {
            return n;
        }

        // Get the bytes.
        var bytes = n.ToByteArray().ToList();

        // Check the most-significant bit, and, if it's 1, add a zero byte to ensure the bytes are
        // interpreted as a positive value when constructing the result BigInteger.
        if ((bytes[^1] & 0b10000000) != 0)
        {
            bytes.Add(0);
        }

        // Construct a new unsigned value.
        return new BigInteger(bytes.ToArray());
    }

    #endregion Miscellaneous other methods

    #region Digit-related methods

    /// <summary>
    /// Reverse a BigInteger.
    /// e.g. 123 becomes 321.
    /// </summary>
    public static BigInteger Reverse(this BigInteger n)
    {
        return BigInteger.Parse(n.ToString().Reverse());
    }

    /// <summary>
    /// Check if a BigInteger is palindromic.
    /// </summary>
    public static bool IsPalindromic(this BigInteger n)
    {
        return n == n.Reverse();
    }

    /// <summary>
    /// Sum of the digits in a BigInteger.
    /// If present, a negative sign is ignored.
    /// </summary>
    public static BigInteger DigitSum(this BigInteger n)
    {
        return BigInteger.Abs(n).ToString().Sum(c => c - '0');
    }

    /// <summary>
    /// Get the number of digits in the BigInteger.
    /// The result will be the same for a positive or negative value.
    /// I tried doing this with double.Log() but because double is imprecise it gives wrong results
    /// for values close to but less than powers of 10.
    /// </summary>
    public static int NumDigits(this BigInteger n)
    {
        return BigInteger.Abs(n).ToString().Length;
    }

    #endregion Digit-related methods

    #region Methods relating to factors

    /// <summary>
    /// Find the smallest integer which is a multiple of both arguments.
    /// Synonyms: lowest common multiple, smallest common multiple.
    /// For example, the LCM of 4 and 6 is 12.
    /// When adding fractions, the lowest common denominator is equal to the LCM of the
    /// denominators.
    /// </summary>
    /// <param name="a">First integer.</param>
    /// <param name="b">Second integer.</param>
    /// <returns>The least common multiple.</returns>
    public static BigInteger LeastCommonMultiple(BigInteger a, BigInteger b)
    {
        // Optimizations.
        if (a == 0 || b == 0)
        {
            return 0;
        }
        if (a == b)
        {
            return a;
        }

        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);
        var gcd = GreatestCommonDivisor(a, b);

        return a > b ? a / gcd * b : b / gcd * a;
    }

    /// <summary>
    /// Determine the greatest common divisor of two integers.
    /// Synonyms: greatest common factor, highest common factor.
    /// </summary>
    public static BigInteger GreatestCommonDivisor(BigInteger a, BigInteger b)
    {
        // Make a and b non-negative, since the result will be the same for negative values.
        a = BigInteger.Abs(a);
        b = BigInteger.Abs(b);

        // Make a < b.
        if (a > b)
        {
            (a, b) = (b, a);
        }

        // Optimization/terminating conditions.
        if (a == b || a == 0)
        {
            return b;
        }
        if (a == 1)
        {
            return 1;
        }

        // Check the cache.
        var key = $"{a}/{b}";
        if (s_gcdCache.TryGetValue(key, out var gcd))
        {
            return gcd;
        }

        // Get the result by recursion.
        gcd = GreatestCommonDivisor(a, b % a);

        // Store the result in the cache.
        s_gcdCache[key] = gcd;

        return gcd;
    }

    #endregion Methods relating to factors

    #region Methods relating to exponentation

    /// <summary>Calculate the value of x^y where x and y are BigIntegers.</summary>
    /// <remarks>
    /// BigInteger provides a Pow() method, but it only permits an int as the exponent.
    /// Uses exponentiation by squaring.
    /// </remarks>
    /// <param name="x">The base (BigRational).</param>
    /// <param name="y">The exponent (BigInteger).</param>
    /// <returns>The result of the calculation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If y is negative.</exception>
    /// <see cref="BigInteger.Pow"/>
    public static BigInteger Pow(BigInteger x, BigInteger y)
    {
        // Guard.
        if (y < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y), "Cannot be negative.");
        }

        // Optimizations.
        if (y == 0) return 1;
        if (y == 1) return x;

        // Defer to the BigInteger method if possible, which I'm guessing is faster.
        if (y >= int.MinValue && y <= int.MaxValue) return BigInteger.Pow(x, (int)y);

        // Even integer powers. x^y = (x^2)^(y/2)
        if (BigInteger.IsEvenInteger(y)) return Pow(x * x, y / 2);

        // Odd integer powers. x^y = x * x^(y-1)
        return x * Pow(x, y - 1);
    }

    /// <summary>Compute 2 raised to a given power.</summary>
    /// <param name="y">The power to which 2 is raised.</param>
    /// <returns>2 raised to the given BigInteger value.</returns>
    public static BigInteger Exp2(BigInteger y)
    {
        return Pow(2, y);
    }

    /// <summary>Compute 10 raised to a given power.</summary>
    /// <param name="y">The power to which 10 is raised.</param>
    /// <returns>10 raised to the given BigInteger value.</returns>
    public static BigInteger Exp10(BigInteger y)
    {
        return Pow(10, y);
    }

    #endregion Power methods
}
