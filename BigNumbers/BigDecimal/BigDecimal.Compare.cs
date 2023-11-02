using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    /// <summary>
    /// See if the BigDecimal value is *effectively* equal to another value of (any number type).
    /// If unspecified, the maximum difference (delta) will be determined by the type of the other
    /// value.
    /// </summary>
    /// <param name="other">The value to compare with.</param>
    /// <param name="delta">The maximum acceptable difference.</param>
    /// <exception cref="AssertFailedException"></exception>
    public readonly bool FuzzyEquals<T>(T other, BigDecimal? delta = null) where T : INumberBase<T>
    {
        // Get the expected value as a BigDecimal.
        if (other is not BigDecimal bd) TryConvertFromChecked(other, out bd);

        if (delta == null)
        {
            if (XNumber.IsIntegerType(typeof(T)))
            {
                // Just round it off and compare for equality.
                return Round(this) == bd;
            }

            // For floating point types we can't trust all the significant digits that will be
            // produced by the cast to BigDecimal, so round off and just keep the ones we can trust.
            switch (other)
            {
                case Half:
                    bd = RoundSigFigs(bd, 3);
                    break;

                case float:
                    bd = RoundSigFigs(bd, 6);
                    break;

                case double:
                    bd = RoundSigFigs(bd, 15);
                    break;
            }

            // Default delta is the maximum value of the least significant digits in the 2 values.
            var maxExp = MaxMagnitude(Exponent, bd.Exponent);
            delta = Exp10(maxExp);
        }

        // Compare values.
        return Abs(this - bd) <= delta;
    }

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
    /// The specification says any negative result indicates the "this" operand comes before (is
    /// less than) the "other" operand, and any positive result indicates the "this" operand comes
    /// after (is greater then) the "other" operand.
    /// However, in this method the result is constrained to only 3 possible values:
    ///    -1 means "this" comes before (is less than) "other"
    ///     0 means they are equal
    ///     1 means "this" comes after (is greater than) "other"
    /// </remarks>
    public int CompareTo(BigDecimal other)
    {
        // Compare signs.
        if (Sign < other.Sign) return -1;
        if (Sign > other.Sign) return 1;

        // Signs are the same, so compare magnitude.
        var (bd1, bd2) = Align(this, other);
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
