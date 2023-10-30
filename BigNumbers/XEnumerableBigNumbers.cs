using System.Numerics;

namespace Galaxon.BigNumbers;

/// <summary>
/// LINQ methods for IEnumerable{BigDecimal}.
/// </summary>
public static class XEnumerableBigNumbers
{
    #region Extension methods for IEnumerable<BigInteger>

    /// <summary>
    /// Get the sum of all values in the collection.
    /// </summary>
    public static BigInteger Sum(this IEnumerable<BigInteger> nums) =>
        nums.Aggregate<BigInteger, BigInteger>((BigInteger)0, (sum, num) => sum + num);

    /// <summary>
    /// Get the sum of all values in the collection, transformed by the supplied function.
    /// </summary>
    public static BigInteger Sum(this IEnumerable<BigInteger> source,
        Func<BigInteger, BigInteger> func) =>
        source.Aggregate((BigInteger)0, (sum, value) => sum + func(value));

    #endregion Extension methods for IEnumerable<BigInteger>

    #region Extension methods for IEnumerable<BigDecimal>

    /// <summary>
    /// Given a collection of BigDecimal values, get the sum of the values.
    /// </summary>
    public static BigDecimal Sum(this IEnumerable<BigDecimal> source) =>
        source.Aggregate<BigDecimal, BigDecimal>(0, (sum, num) => sum + num);

    /// <summary>
    /// Given a collection of BigDecimal values, get the average (i.e. the arithmetic mean).
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Arithmetic_mean"/>
    public static BigDecimal Average(this IEnumerable<BigDecimal> source)
    {
        var nums = source.ToList();

        // Guard.
        if (nums.Count == 0)
        {
            throw new ArithmeticException("At least one value must be provided.");
        }

        // Optimization.
        if (nums.Count == 1)
        {
            return nums[0];
        }

        return nums.Sum() / nums.Count;
    }

    /// <summary>
    /// Given a collection of BigDecimal values, get the product of the values.
    /// </summary>
    public static BigDecimal Product(this IEnumerable<BigDecimal> source) =>
        source.Aggregate<BigDecimal, BigDecimal>(1, (prod, num) => prod * num);

    /// <summary>
    /// Given a collection of BigDecimal values, get the geometric mean.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Geometric_mean"/>
    public static BigDecimal GeometricMean(this IEnumerable<BigDecimal> source)
    {
        var nums = source.ToList();

        // Make sure there's at least one value.
        if (nums.Count == 0)
        {
            throw new ArithmeticException("At least one value must be provided.");
        }

        // Ensure all values are non-negative.
        if (nums.Any(x => x < 0))
        {
            throw new ArithmeticException("All values must be non-negative.");
        }

        // Optimization.
        if (nums.Count == 1)
        {
            return nums[0];
        }

        return BigDecimal.RootN(nums.Product(), nums.Count);
    }

    #endregion Extension methods for IEnumerable<BigDecimal>
}
