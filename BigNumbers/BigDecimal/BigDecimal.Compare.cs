using Galaxon.Core.Exceptions;

namespace Galaxon.BigNumbers;

/// <summary>
/// Operators and methods for comparing BigDecimal.
/// </summary>
public partial struct BigDecimal
{
    #region Equality methods

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is BigDecimal bd && Equals(bd);

    /// <inheritdoc/>
    public bool Equals(BigDecimal bd) => CompareTo(bd) == 0;

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Significand, Exponent);

    #endregion Equality methods

    #region Comparison methods

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is BigDecimal bd) return CompareTo(bd);

        throw new ArgumentInvalidException(nameof(obj), "Must be a BigDecimal.");
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Although the specification says any negative value indicates the "this" operand comes before
    /// the "other" operand (similar for positive values), in this method the result is constrained
    /// to only 3 possible values:
    /// -1 means "this" comes before (and is less than) "other"
    /// 0 means they are equal
    /// 1 means "this" comes after (and is greater than) "other"
    /// </remarks>
    public int CompareTo(BigDecimal bd)
    {
        // Compare signs.
        if (Sign < bd.Sign) return -1;
        if (Sign > bd.Sign) return 1;

        // Signs are the same, so compare magnitude.
        var (bd1, bd2) = Align(this, bd);
        return bd1.Significand.CompareTo(bd2.Significand);
    }

    /// <inheritdoc/>
    public static BigDecimal MaxMagnitude(BigDecimal bd, BigDecimal bd2)
    {
        var absX = Abs(bd);
        var absY = Abs(bd2);
        return absX > absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigDecimal MaxMagnitudeNumber(BigDecimal bd, BigDecimal bd2) =>
        MaxMagnitude(bd, bd2);

    /// <inheritdoc/>
    public static BigDecimal MinMagnitude(BigDecimal bd, BigDecimal bd2)
    {
        var absX = Abs(bd);
        var absY = Abs(bd2);
        return absX < absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigDecimal MinMagnitudeNumber(BigDecimal bd, BigDecimal bd2) =>
        MinMagnitude(bd, bd2);

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc/>
    public static bool operator ==(BigDecimal bd, BigDecimal bd2) => bd.CompareTo(bd2) == 0;

    /// <inheritdoc/>
    public static bool operator !=(BigDecimal bd, BigDecimal bd2) => bd.CompareTo(bd2) != 0;

    /// <inheritdoc/>
    public static bool operator <(BigDecimal bd, BigDecimal bd2) => bd.CompareTo(bd2) < 0;

    /// <inheritdoc/>
    public static bool operator <=(BigDecimal bd, BigDecimal bd2) => bd.CompareTo(bd2) <= 0;

    /// <inheritdoc/>
    public static bool operator >(BigDecimal bd, BigDecimal bd2) => bd.CompareTo(bd2) > 0;

    /// <inheritdoc/>
    public static bool operator >=(BigDecimal bd, BigDecimal bd2) => bd.CompareTo(bd2) >= 0;

    #endregion Comparison operators
}
