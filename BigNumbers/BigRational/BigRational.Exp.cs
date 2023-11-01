using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Power functions

    /// <summary>
    /// Calculate the value of x^y where x is a BigRational and y is a BigInteger.
    /// </summary>
    /// <param name="x">The base (BigRational).</param>
    /// <param name="y">The exponent (BigInteger).</param>
    /// <returns>The result of the calculation.</returns>
    /// <see cref="BigInteger.Pow"/>
    public static BigRational Pow(BigRational x, BigInteger y)
    {
        // Optimizations.
        if (y == 0) return 1;
        if (y == 1) return x;
        if (y == -1) return Reciprocal(x);

        // Handle negative exponent.
        if (y < 0)
        {
            x = Reciprocal(x);
            y = -y;
        }

        // Raise both the numerator and denominator to the power of y.
        var num = XBigInteger.Pow(x.Numerator, y);
        var den = XBigInteger.Pow(x.Denominator, y);
        return new BigRational(num, den);
    }

    /// <summary>Calculate the square of a BigRational number.</summary>
    /// <param name="x">A BigRational value.</param>
    /// <returns>The square of the parameter.</returns>
    public static BigRational Sqr(BigRational x) => x * x;

    /// <summary>Calculate the cube of a BigRational number.</summary>
    /// <param name="x">A BigRational value.</param>
    /// <returns>The cube of the parameter.</returns>
    public static BigRational Cube(BigRational x) => x * x * x;

    #endregion Power functions
}
