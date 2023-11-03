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
    public override bool Equals(object? obj)
    {
        return obj is BigDecimal bd && Equals(bd);
    }

    /// <inheritdoc/>
    public bool Equals(BigDecimal other)
    {
        return Significand == other.Significand && Exponent == other.Exponent;
    }

    /// <summary>
    /// Get the value of the least significant bit in the provided floating point number.
    /// </summary>
    /// <param name="f">A floating point number.</param>
    /// <typeparam name="T">A standard floating point type.</typeparam>
    /// <returns>The value of the least significant bit</returns>
    private static BigDecimal LeastSignificantBit<T>(T f) where T : IFloatingPointIeee754<T>
    {
        // Subnormal value.
        if (T.IsSubnormal(f)) return (BigDecimal)(object)T.Epsilon;

        // Normal value.
        var (signBit, expBits, fracBits) = f.Disassemble();
        var expBias = XFloatingPoint.GetExpBias<T>();
        var nFracBits = XFloatingPoint.GetNumFracBits<T>();
        return Exp2(expBits - expBias - nFracBits);
    }

    /// <summary>
    /// See if the BigDecimal value is *effectively* equal to another value of another number, which
    /// could be another BigDecimal, or a standard number type.
    /// </summary>
    /// <param name="other">The value to compare with.</param>
    /// <exception cref="AssertFailedException"></exception>
    public readonly bool FuzzyEquals<T>(T other) where T : INumberBase<T>
    {
        // See if the other value is a BigDecimal.
        if (other is BigDecimal bdOther)
        {
            // Make sure both are rounded off to the same number of significant figures
            // (MaxSigFigs), then compare for equality.
            var bd1 = RoundSigFigs(this);
            var bd2 = RoundSigFigs(bdOther);
            return bd1 == bd2;
        }

        // Convert the "other" value to a BigDecimal.
        TryConvertFromChecked(other, out bdOther);

        var type = typeof(T);

        // For integers, just round off the BigDecimal to the nearest integer, then compare for
        // equality.
        if (XNumber.IsIntegerType(type))
        {
            return Round(this).Equals(bdOther);
        }

        // For floating point types, find out the value of the least significant bit, which will
        // depend on the type and the exponent.
        if (XNumber.IsFloatingPointType(type))
        {
            var lsb = other switch
            {
                Half h => LeastSignificantBit(h),
                float f => LeastSignificantBit(f),
                double d => LeastSignificantBit(d)
            };

            // The maximum difference is half the value of the least significant bit.
            return Abs(this - bdOther) <= lsb / 2;
        }

        throw new ArgumentInvalidException(nameof(other), "Unsupported type.");
    }

    /// <inheritdoc/>
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Significand, Exponent);
    }

    #endregion Equality methods

    #region Comparison methods

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is BigDecimal bd) return CompareTo(bd);

        throw new ArgumentInvalidException(nameof(obj), "Must be a BigDecimal.");
    }

    /// <inheritdoc/>
    public int CompareTo(BigDecimal other)
    {
        // Check for equality.
        if (Equals(other)) return 0;

        // Compare signs.
        if (Sign < other.Sign) return -1;
        if (Sign > other.Sign) return 1;

        // Compare values.
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
    public static BigDecimal MaxMagnitudeNumber(BigDecimal bd, BigDecimal bd2)
    {
        return MaxMagnitude(bd, bd2);
    }

    /// <inheritdoc/>
    public static BigDecimal MinMagnitude(BigDecimal bd, BigDecimal bd2)
    {
        var absX = Abs(bd);
        var absY = Abs(bd2);
        return absX < absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigDecimal MinMagnitudeNumber(BigDecimal bd, BigDecimal bd2)
    {
        return MinMagnitude(bd, bd2);
    }

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc/>
    public static bool operator ==(BigDecimal bd, BigDecimal bd2)
    {
        return bd.Equals(bd2);
    }

    /// <inheritdoc/>
    public static bool operator !=(BigDecimal bd, BigDecimal bd2)
    {
        return !bd.Equals(bd2);
    }

    /// <inheritdoc/>
    public static bool operator <(BigDecimal bd, BigDecimal bd2)
    {
        return bd.CompareTo(bd2) < 0;
    }

    /// <inheritdoc/>
    public static bool operator <=(BigDecimal bd, BigDecimal bd2)
    {
        return bd.CompareTo(bd2) <= 0;
    }

    /// <inheritdoc/>
    public static bool operator >(BigDecimal bd, BigDecimal bd2)
    {
        return bd.CompareTo(bd2) > 0;
    }

    /// <inheritdoc/>
    public static bool operator >=(BigDecimal bd, BigDecimal bd2)
    {
        return bd.CompareTo(bd2) >= 0;
    }

    #endregion Comparison operators
}
