using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Power functions

    /// <summary>
    /// Calculate the value of x^y where x is a BigRational and y is an int.
    /// </summary>
    /// <param name="x">The base (BigRational).</param>
    /// <param name="y">The exponent (int).</param>
    /// <returns>The result of the calculation.</returns>
    /// <see cref="BigInteger.Pow"/>
    /// <exception cref="ArgumentOutOfRangeException">If x is 0 and y is negative.</exception>
    public static BigRational Pow(BigRational x, int y)
    {
        if (y < 0)
        {
            // x^(-y) == 1/(x^y)
            return Reciprocal(Pow(x, -y));
        }
        if (y == 0)
        {
            // x^0 == 1 for all x
            return 1;
        }
        if (y == 1)
        {
            return x;
        }
        // y > 1
        // Raise both the numerator and denominator to the power of y.
        var num = BigInteger.Pow(x.Numerator, y);
        var den = BigInteger.Pow(x.Denominator, y);
        return new BigRational(num, den);
    }

    /// <summary>Calculate the square of a BigRational number.</summary>
    /// <param name="x">A BigRational value.</param>
    /// <returns>The square of the parameter.</returns>
    public static BigRational Sqr(BigRational x)
    {
        return x * x;
    }

    /// <summary>Calculate the cube of a BigRational number.</summary>
    /// <param name="x">A BigRational value.</param>
    /// <returns>The cube of the parameter.</returns>
    public static BigRational Cube(BigRational x)
    {
        return x * x * x;
    }

    #endregion Power functions
}
