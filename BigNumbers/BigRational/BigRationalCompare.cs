namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Equality methods

    public override bool Equals(object? obj)
    {
        return obj is BigRational br2 && Equals(br2);
    }

    public bool Equals(BigRational br2)
    {
        // See if the numerators and denominators are equal.
        return Numerator == br2.Numerator && Denominator == br2.Denominator;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }

    #endregion Equality methods

    #region Comparison methods

    /// <inheritdoc/>
    public static BigRational MaxMagnitude(BigRational x, BigRational y)
    {
        return x > y ? x : y;
    }

    /// <inheritdoc/>
    public static BigRational MaxMagnitudeNumber(BigRational x, BigRational y)
    {
        return MaxMagnitude(x, y);
    }

    /// <inheritdoc/>
    public static BigRational MinMagnitude(BigRational x, BigRational y)
    {
        return x < y ? x : y;
    }

    /// <inheritdoc/>
    public static BigRational MinMagnitudeNumber(BigRational x, BigRational y)
    {
        return MinMagnitude(x, y);
    }

    #endregion Comparison methods

    #region Comparison operators

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(BigRational br, BigRational br2)
    {
        return br.Equals(br2);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(BigRational br, BigRational br2)
    {
        return !br.Equals(br2);
    }

    /// <summary>
    /// Less than operator.
    /// </summary>
    public static bool operator <(BigRational br, BigRational br2)
    {
        // Optimization. Skip the casts to BigDecimal if possible.
        if (br.Denominator == br2.Denominator) return br.Numerator < br2.Numerator;

        return (BigDecimal)br < (BigDecimal)br2;
    }

    /// <summary>
    /// Less than or equal to operator.
    /// </summary>
    public static bool operator <=(BigRational br, BigRational br2)
    {
        return br == br2 || br < br2;
    }

    /// <summary>
    /// Greater than operator.
    /// </summary>
    public static bool operator >(BigRational br, BigRational br2)
    {
        // Optimization. Skip the casts to BigDecimal if possible.
        if (br.Denominator == br2.Denominator) return br.Numerator > br2.Numerator;

        return (BigDecimal)br > (BigDecimal)br2;
    }

    /// <summary>
    /// Greater than or equal to operator.
    /// </summary>
    public static bool operator >=(BigRational br, BigRational br2)
    {
        return br == br2 || br > br2;
    }

    #endregion Comparison operators
}
