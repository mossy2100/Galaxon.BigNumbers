using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;
using Galaxon.Core.Types;
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
    /// Get the unit of least precision (ULP) in the provided floating point number.
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Unit_in_the_last_place"/>
    /// <param name="f">A floating point number.</param>
    /// <typeparam name="T">A standard floating point type.</typeparam>
    /// <returns>The value of the unit of least precision.</returns>
    public static BigDecimal UnitOfLeastPrecision<T>(T f) where T : IFloatingPointIeee754<T>
    {
        // Subnormal value.
        if (T.IsSubnormal(f)) return XReflection.Cast<T, BigDecimal>(T.Epsilon);

        // Normal value.
        var (signBit, expBits, fracBits) = f.Disassemble();
        var expBias = XFloatingPoint.GetExpBias<T>();
        var nFracBits = XFloatingPoint.GetNumFracBits<T>();
        return Exp2(expBits - expBias - nFracBits);
    }

    /// <summary>
    /// Get the unit of least precision (ULP) in the provided decimal number.
    /// </summary>
    /// <param name="m">A decimal value.</param>
    /// <returns>The value of the unit of least precision.</returns>
    public static BigDecimal UnitOfLeastPrecision(decimal m)
    {
        var (signBit, scaleBits, fracBits) = m.Disassemble();
        return new decimal(1, 0, 0, false, scaleBits);
    }

    /// <summary>
    /// Get the unit of least precision (ULP) in the provided BigDecimal number.
    /// </summary>
    /// <param name="bd">A BigDecimal value.</param>
    /// <returns>The value of the unit of least precision.</returns>
    public static BigDecimal UnitOfLeastPrecision(BigDecimal bd)
    {
        return new BigDecimal(1, bd.Exponent);
    }

    /// <summary>
    /// See if the BigDecimal value is *effectively* equal (within a given tolerance) to another
    /// numeric value, which could be another BigDecimal, or a standard number type.
    /// </summary>
    /// <typeparam name="T">The other number's type.</typeparam>
    /// <param name="other">The value to compare with.</param>
    /// <param name="delta">The maximum acceptable difference.</param>
    /// <exception cref="ArgumentInvalidException">
    /// If the type of the value being compared with the BigDecimal is unsupported.
    /// </exception>
    public readonly bool FuzzyEquals<T>(T other, BigDecimal? delta = null) where T : INumberBase<T>
    {
        // Get the type.
        var type = typeof(T);

        // Get the "other" value as a BigDecimal for the purpose of the comparison.
        if (other is not BigDecimal bd)
        {
            var ok = TryConvertFromChecked(other, out bd);
            if (!ok)
            {
                throw new ArgumentInvalidException(type.Name, "Unsupported type.");
            }
        }

        // If unspecified, calculate a reasonable delta.
        if (delta == null)
        {
            // Determine the ULP (unit of least precision) for the two values.
            var ulpThis = new BigDecimal(1, Exponent);
            BigDecimal ulpOther;

            // See if the other value is a BigDecimal.
            if (XNumber.IsIntegerType(type))
            {
                // For integers, the ULP is always 1.
                ulpOther = 1;
            }
            else
            {
                // For other types the ULP depends on the type and the exponent.
                ulpOther = other switch
                {
                    Half h => UnitOfLeastPrecision(h),
                    float f => UnitOfLeastPrecision(f),
                    double d => UnitOfLeastPrecision(d),
                    decimal m => UnitOfLeastPrecision(m),
                    BigDecimal => UnitOfLeastPrecision(bd),
                    _ => throw new ArgumentInvalidException(type.Name, "Unsupported type.")
                };
            }

            // Calculate the delta from half the maximum ULP.
            delta = MaxMagnitude(ulpThis, ulpOther) / 2;
        }

        // See if they are close enough.
        var diff = Abs(this - bd);
        return diff <= delta;
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
