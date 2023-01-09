using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics;

/// <summary>
/// Operators and methods for comparing BigDecimals.
/// </summary>
public partial struct BigDecimal
{
    #region Comparison methods

    /// <inheritdoc />
    public int CompareTo(BigDecimal other)
    {
        if (Sign < other.Sign)
        {
            return -1;
        }
        if (Sign > other.Sign)
        {
            return 1;
        }
        (BigDecimal x, BigDecimal y) = Align(this, other);
        if (x.Significand < y.Significand)
        {
            return -1;
        }
        if (x.Significand > y.Significand)
        {
            return 1;
        }
        return 0;
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is not BigDecimal other)
        {
            throw new ArgumentInvalidException(nameof(obj), "Must be a BigDecimal.");
        }
        return CompareTo(other);
    }

    /// <inheritdoc />
    public bool Equals(BigDecimal other) =>
        CompareTo(other) == 0;

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is BigDecimal bd)
        {
            return Equals(bd);
        }
        return false;
    }

    /// <inheritdoc />
    public override int GetHashCode() =>
        HashCode.Combine(Significand, Exponent);

    /// <inheritdoc />
    public static BigDecimal MaxMagnitude(BigDecimal x, BigDecimal y) =>
        x > y ? x : y;

    /// <inheritdoc />
    public static BigDecimal MaxMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MaxMagnitude(x, y);

    /// <inheritdoc />
    public static BigDecimal MinMagnitude(BigDecimal x, BigDecimal y) =>
        x < y ? x : y;

    /// <inheritdoc />
    public static BigDecimal MinMagnitudeNumber(BigDecimal x, BigDecimal y) =>
        MinMagnitude(x, y);

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc />
    public static bool operator ==(BigDecimal x, BigDecimal y) =>
        x.Equals(y);

    /// <inheritdoc />
    public static bool operator !=(BigDecimal x, BigDecimal y) =>
        !x.Equals(y);

    /// <inheritdoc />
    public static bool operator <(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) < 0;

    /// <inheritdoc />
    public static bool operator <=(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) <= 0;

    /// <inheritdoc />
    public static bool operator >(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) > 0;

    /// <inheritdoc />
    public static bool operator >=(BigDecimal x, BigDecimal y) =>
        x.CompareTo(y) >= 0;

    #endregion Comparison operators
}
