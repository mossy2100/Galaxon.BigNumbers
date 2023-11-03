using Galaxon.Core.Exceptions;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Equality methods

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is BigRational br && Equals(br);
    }

    /// <inheritdoc/>
    public bool Equals(BigRational br)
    {
        return Numerator == br.Numerator && Denominator == br.Denominator;
    }

    /// <inheritdoc/>
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }

    #endregion Equality methods

    #region Comparison methods

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is BigRational br) return CompareTo(br);

        throw new ArgumentInvalidException(nameof(obj), "Must be a BigRational.");
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Although the specification says any negative value indicates the "this" operand comes before
    /// the "br" operand (similar for positive values), in this method the result is constrained
    /// to only 3 possible values:
    /// -1 means "this" comes before (and is less than) "br"
    /// 0 means they are equal
    /// 1 means "this" comes after (and is greater than) "br"
    /// </remarks>
    public int CompareTo(BigRational br)
    {
        // Compare signs.
        if (Sign < br.Sign) return -1;
        if (Sign > br.Sign) return 1;

        // Signs are the same.
        // If the denominators are the same, just compare the numerators, avoiding the casts to
        // BigDecimal.
        if (Denominator == br.Denominator) return Numerator.CompareTo(br.Numerator);

        // Convert both to BigDecimal values and compare them.
        var bd = (BigDecimal)this;
        var bd2 = (BigDecimal)br;
        return bd.CompareTo(bd2);
    }

    /// <inheritdoc/>
    public static BigRational MaxMagnitude(BigRational br, BigRational br2)
    {
        var absX = Abs(br);
        var absY = Abs(br2);
        return absX > absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigRational MaxMagnitudeNumber(BigRational br, BigRational br2)
    {
        return MaxMagnitude(br, br2);
    }

    /// <inheritdoc/>
    public static BigRational MinMagnitude(BigRational br, BigRational br2)
    {
        var absX = Abs(br);
        var absY = Abs(br2);
        return absX < absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigRational MinMagnitudeNumber(BigRational br, BigRational br2)
    {
        return MinMagnitude(br, br2);
    }

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc/>
    public static bool operator ==(BigRational br, BigRational br2)
    {
        return br.CompareTo(br2) == 0;
    }

    /// <inheritdoc/>
    public static bool operator !=(BigRational br, BigRational br2)
    {
        return br.CompareTo(br2) != 0;
    }

    /// <inheritdoc/>
    public static bool operator <(BigRational br, BigRational br2)
    {
        return br.CompareTo(br2) < 0;
    }

    /// <inheritdoc/>
    public static bool operator <=(BigRational br, BigRational br2)
    {
        return br.CompareTo(br2) <= 0;
    }

    /// <inheritdoc/>
    public static bool operator >(BigRational br, BigRational br2)
    {
        return br.CompareTo(br2) > 0;
    }

    /// <inheritdoc/>
    public static bool operator >=(BigRational br, BigRational br2)
    {
        return br.CompareTo(br2) >= 0;
    }

    #endregion Comparison operators
}
