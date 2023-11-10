using System.Numerics;
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
        if (num == 0)
        {
            return (0, 1);
        }
        if (num == den)
        {
            return (1, 1);
        }

        // Make sure the denominator is always positive.
        if (den < 0)
        {
            num = -num;
            den = -den;
        }

        // Check for simple, irreducible fractions.
        if (num == 1)
        {
            return (1, den);
        }
        if (den == 1)
        {
            return (num, 1);
        }

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
    public static BigRational Abs(BigRational br)
    {
        return new BigRational(BigInteger.Abs(br.Numerator), br.Denominator);
    }

    /// <summary>Clone method.</summary>
    /// <param name="br">The BigRational value to clone.</param>
    /// <returns>A new BigRational with the same value as the parameter.</returns>
    public static BigRational Clone(BigRational br)
    {
        return new BigRational(br.Numerator, br.Denominator);
    }

    /// <summary>Negate method.</summary>
    /// <param name="br">The BigRational value to negate.</param>
    /// <returns>The negation of the parameter.</returns>
    public static BigRational Negate(BigRational br)
    {
        return new BigRational(-br.Numerator, br.Denominator);
    }

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
    public static BigRational Subtract(BigRational br, BigRational br2)
    {
        return br + -br2;
    }

    /// <summary>Multiply two BigRational values.</summary>
    /// <param name="br">The left-hand operand.</param>
    /// <param name="br2">The right-hand operand.</param>
    /// <returns>The multiplication of the arguments.</returns>
    public static BigRational Multiply(BigRational br, BigRational br2)
    {
        return new BigRational(br.Numerator * br2.Numerator, br.Denominator * br2.Denominator);
    }

    /// <summary>Divide one BigRational by another.</summary>
    /// <param name="br">The left-hand operand.</param>
    /// <param name="br2">The right-hand operand.</param>
    /// <returns>The division of the arguments.</returns>
    /// <exception cref="DivideByZeroException">If w == 0</exception>
    public static BigRational Divide(BigRational br, BigRational br2)
    {
        return new BigRational(br.Numerator * br2.Denominator, br.Denominator * br2.Numerator);
    }

    /// <summary>Calculate the reciprocal of a BigRational value.</summary>
    /// <param name="br">A BigRational value.</param>
    /// <returns>The reciprocal of the BigRational value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the BigRational value is 0.</exception>
    public static BigRational Reciprocal(BigRational br)
    {
        return new BigRational(br.Denominator, br.Numerator);
    }

    #endregion Arithmetic methods

    #region Arithmetic operators

    /// <inheritdoc/>
    public static BigRational operator +(BigRational br)
    {
        return Clone(br);
    }

    /// <inheritdoc/>
    public static BigRational operator -(BigRational br)
    {
        return Negate(br);
    }

    /// <inheritdoc/>
    public static BigRational operator +(BigRational br, BigRational br2)
    {
        return Add(br, br2);
    }

    /// <inheritdoc/>
    public static BigRational operator -(BigRational br, BigRational br2)
    {
        return Subtract(br, br2);
    }

    /// <inheritdoc/>
    public static BigRational operator ++(BigRational br)
    {
        return Add(br, 1);
    }

    /// <inheritdoc/>
    public static BigRational operator --(BigRational br)
    {
        return Subtract(br, 1);
    }

    /// <summary>
    /// Multiply a rational by a rational.
    /// </summary>
    public static BigRational operator *(BigRational br, BigRational br2)
    {
        return Multiply(br, br2);
    }

    /// <summary>
    /// Divide a rational by a rational.
    /// </summary>
    public static BigRational operator /(BigRational br, BigRational br2)
    {
        return Divide(br, br2);
    }

    /// <summary>
    /// Exponentiation operator.
    /// </summary>
    public static BigRational operator ^(BigRational br, int i)
    {
        return Pow(br, i);
    }

    #endregion Arithmetic operators
}
