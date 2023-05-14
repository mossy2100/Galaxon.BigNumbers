namespace Galaxon.Numerics;

/// <summary>
/// LINQ methods for IEnumerable{BigDecimal}.
/// </summary>
public static class XEnumerableBigDecimal
{
    /// <summary>
    /// Given a collection of BigDecimal values, get the sum of the values.
    /// </summary>
    public static BigDecimal Sum(this IEnumerable<BigDecimal> source) =>
        source.Aggregate<BigDecimal, BigDecimal>(0, (sum, num) => sum + num);

    /// <summary>
    /// Given a collection of BigDecimal values, get the average.
    /// </summary>
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
        source.Aggregate<BigDecimal, BigDecimal>(1, (sum, num) => sum * num);

    /// <summary>
    /// Given a collection of BigDecimal values, get the geometric mean.
    /// </summary>
    public static BigDecimal GeometricMean(this IEnumerable<BigDecimal> source)
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

        return BigDecimal.RootN(nums.Product(), nums.Count);
    }
}
