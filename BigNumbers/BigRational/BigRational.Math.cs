using System.Numerics;
using Galaxon.Core.Functional;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Arithmetic methods

    /// <summary>Reduce a rational given as a numerator and denominator.</summary>
    /// <remarks>
    /// I've made this version, which doesn't receive or return a BigRational object, so it can be
    /// called from the constructor.
    /// </remarks>
    /// <param name="num">The numerator.</param>
    /// <param name="den">The denominator.</param>
    /// <returns>The numerator and denominator of the reduced fraction, as s tuple.</returns>
    private static (BigInteger, BigInteger) Reduce(BigInteger num, BigInteger den)
    {
        // Guard.
        if (den == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(den),
                "The denominator of a rational number cannot be 0.");
        }

        // Optimizations.
        if (num == 0) return (0, 1);
        if (num == den) return (1, 1);

        // Make sure the denominator is always positive.
        if (den < 0)
        {
            num = -num;
            den = -den;
        }

        // Check for simple, irreducible fractions.
        if (num == 1) return (1, den);
        if (den == 1) return (num, 1);

        // Get the greatest common divisor.
        var gcd = XBigInteger.GreatestCommonDivisor(num, den);

        // If we found one greater than 1, divide.
        if (gcd > 1)
        {
            num /= gcd;
            den /= gcd;
        }

        return (num, den);
    }

    /// <inheritdoc/>
    public static BigRational Abs(BigRational br) =>
        new (BigInteger.Abs(br.Numerator), br.Denominator);

    /// <summary>Clone method.</summary>
    /// <param name="br">The BigRational value to clone.</param>
    /// <returns>A new BigRational with the same value as the parameter.</returns>
    public static BigRational Clone(BigRational br) => new (br.Numerator, br.Denominator);

    /// <summary>Negate method.</summary>
    /// <param name="br">The BigRational value to negate.</param>
    /// <returns>The negation of the parameter.</returns>
    public static BigRational Negate(BigRational br) => new (-br.Numerator, br.Denominator);

    /// <summary>Addition method.</summary>
    /// <param name="br">The left-hand operand.</param>
    /// <param name="br2">The right-hand operand.</param>
    /// <returns>The addition of the arguments.</returns>
    public static BigRational Add(BigRational br, BigRational br2)
    {
        var num = br.Numerator * br2.Denominator + br2.Numerator * br.Denominator;
        var den = br.Denominator * br2.Denominator;
        return new BigRational(num, den);
    }

    /// <summary>Subtraction method.</summary>
    /// <param name="br">The left-hand operand.</param>
    /// <param name="br2">The right-hand operand.</param>
    /// <returns>The subtraction of the arguments.</returns>
    public static BigRational Subtract(BigRational br, BigRational br2) => br + -br2;

    /// <summary>Multiply two BigRational values.</summary>
    /// <param name="br">The left-hand operand.</param>
    /// <param name="br2">The right-hand operand.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigRational Multiply(BigRational br, BigRational br2) =>
        new (br.Numerator * br2.Numerator, br.Denominator * br2.Denominator);

    /// <summary>Divide one BigRational by another.</summary>
    /// <param name="br">The left-hand operand.</param>
    /// <param name="br2">The right-hand operand.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="DivideByZeroException">If w == 0</exception>
    public static BigRational Divide(BigRational br, BigRational br2) =>
        new (br.Numerator * br2.Denominator, br.Denominator * br2.Numerator);

    /// <summary>Calculate the reciprocal of a BigRational value.</summary>
    /// <param name="br">A BigRational value.</param>
    /// <returns>The reciprocal of the BigRational value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the BigRational value is 0.</exception>
    public static BigRational Reciprocal(BigRational br) => new (br.Denominator, br.Numerator);

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <inheritdoc/>
    public static BigRational operator +(BigRational br) => Clone(br);

    /// <inheritdoc/>
    public static BigRational operator -(BigRational br) => Negate(br);

    /// <inheritdoc/>
    public static BigRational operator +(BigRational br, BigRational br2) => Add(br, br2);

    /// <inheritdoc/>
    public static BigRational operator -(BigRational br, BigRational br2) => Subtract(br, br2);

    /// <inheritdoc/>
    public static BigRational operator ++(BigRational br) => Add(br, 1);

    /// <inheritdoc/>
    public static BigRational operator --(BigRational br) => Subtract(br, 1);

    /// <summary>
    /// Multiply a rational by a rational.
    /// </summary>
    public static BigRational operator *(BigRational br, BigRational br2) => Multiply(br, br2);

    /// <summary>
    /// Divide a rational by a rational.
    /// </summary>
    public static BigRational operator /(BigRational br, BigRational br2) => Divide(br, br2);

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    public static BigRational operator ^(BigRational br, BigInteger bi) => Pow(br, bi);

    #endregion Arithmetic operators

    #region Bernoulli stuff

    /// <summary>Calculate a Bernoulli number.</summary>
    /// <see href="https://en.wikipedia.org/wiki/Bernoulli_number"/>
    /// <param name="n">The index of the Bernoulli number to calculate.</param>
    /// <returns>The Bernoulli number as a BigRational.</returns>
    private static BigRational _Bernoulli(int n)
    {
        // Guard.
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "Cannot be negative.");

        // Optimizations.
        if (n == 0) return 1;

        // For all odd indices greater than 1, the Bernoulli number is 0.
        if (n > 1 && int.IsOddInteger(n)) return 0;

        // Compute result.
        BigRational b = 1;
        for (var k = 0; k < n; k++)
        {
            b -= XBigInteger.BinomialCoefficient(n, k) * Bernoulli(k) / (n - k + 1);
        }
        return b;
    }

    /// <summary>Calculate a Bernoulli number.</summary>
    /// <returns>The memoized version of the method.</returns>
    public static readonly Func<int, BigRational> Bernoulli =
        Memoization.Memoize<int, BigRational>(_Bernoulli);

    #endregion Bernoulli stuff
}
