using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Casting to BigComplex

    /// <summary>Cast sbyte to BigComplex.</summary>
    /// <param name="x">The sbyte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(sbyte x) => new (x);

    /// <summary>Cast byte to BigDecimal.</summary>
    /// <param name="x">The byte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(byte x) => new (x);

    /// <summary>Cast short to BigDecimal.</summary>
    /// <param name="x">The short value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(short x) => new (x);

    /// <summary>Cast ushort to BigDecimal.</summary>
    /// <param name="x">The ushort value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ushort x) => new (x);

    /// <summary>Cast int to BigDecimal.</summary>
    /// <param name="x">The int value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(int x) => new (x);

    /// <summary>Cast uint to BigDecimal.</summary>
    /// <param name="x">The uint value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(uint x) => new (x);

    /// <summary>Cast long to BigDecimal.</summary>
    /// <param name="x">The long value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(long x) => new (x);

    /// <summary>Cast ulong to BigDecimal.</summary>
    /// <param name="x">The ulong value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ulong x) => new (x);

    /// <summary>Cast Int128 to BigDecimal.</summary>
    /// <param name="x">The Int128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Int128 x) => new (x);

    /// <summary>Cast UInt128 to BigDecimal.</summary>
    /// <param name="x">The UInt128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(UInt128 x) => new (x);

    /// <summary>Cast BigInteger to BigDecimal.</summary>
    /// <param name="x">The BigInteger value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigInteger x) => new (x);

    /// <summary>Cast Half to BigDecimal.</summary>
    /// <param name="x">The Half value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Half x) => new (x);

    /// <summary>Cast float to BigDecimal.</summary>
    /// <param name="x">The float value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(float x) => new (x);

    /// <summary>Cast double to BigDecimal.</summary>
    /// <param name="x">The double value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(double x) => new (x);

    /// <summary>Cast decimal to BigDecimal.</summary>
    /// <param name="x">The decimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(decimal x) => new (x);

    /// <summary>Cast Complex to BigComplex.</summary>
    /// <param name="x">The Complex value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Complex x) => new (x.Real, x.Imaginary);

    /// <summary>Cast BigDecimal to BigComplex.</summary>
    /// <param name="x">The BigDecimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigDecimal x) => new (x);

    /// <summary>Cast BigRational to BigComplex.</summary>
    /// <remarks>
    /// This cast has to be explicit because there can be information loss when converting the
    /// BigRational to BigDecimal.
    /// </remarks>
    /// <param name="x">The BigRational value.</param>
    /// <returns>The closest BigComplex value.</returns>
    public static explicit operator BigComplex(BigRational x) => new ((BigDecimal)x);

    #endregion Casting to BigComplex

    #region Casting from BigComplex

    /// <summary>Cast BigComplex to sbyte.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent sbyte value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of sbyte.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator sbyte(BigComplex bc) => ConvertToReal<sbyte>(bc);

    /// <summary>Cast BigComplex to byte.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent byte value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of byte.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator byte(BigComplex bc) => ConvertToReal<byte>(bc);

    /// <summary>Cast BigComplex to short.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent short value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of short.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator short(BigComplex bc) => ConvertToReal<short>(bc);

    /// <summary>Cast BigComplex to ushort.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent ushort value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of ushort.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator ushort(BigComplex bc) => ConvertToReal<ushort>(bc);

    /// <summary>Cast BigComplex to int.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent int value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of int.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator int(BigComplex bc) => ConvertToReal<int>(bc);

    /// <summary>Cast BigComplex to uint.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent uint value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of uint.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator uint(BigComplex bc) => ConvertToReal<uint>(bc);

    /// <summary>Cast BigComplex to long.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent long value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of long.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator long(BigComplex bc) => ConvertToReal<long>(bc);

    /// <summary>Cast BigComplex to ulong.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent ulong value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of ulong.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator ulong(BigComplex bc) => ConvertToReal<ulong>(bc);

    /// <summary>Cast BigComplex to Int128.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent Int128 value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of Int128.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator Int128(BigComplex bc) => ConvertToReal<Int128>(bc);

    /// <summary>Cast BigComplex to UInt128.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent UInt128 value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of UInt128.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator UInt128(BigComplex bc) => ConvertToReal<UInt128>(bc);

    /// <summary>Cast BigComplex to BigInteger.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent BigInteger value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator BigInteger(BigComplex bc) => ConvertToReal<BigInteger>(bc);

    /// <summary>Cast BigComplex to Half.</summary>
    /// <remarks>
    /// If the real part is outside the valid range for Half, the result will be
    /// Half.NegativeInfinity or Half.PositiveInfinity; no exception will be thrown.
    /// </remarks>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent Half value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator Half(BigComplex bc) => ConvertToReal<Half>(bc);

    /// <summary>Cast BigComplex to float.</summary>
    /// <remarks>
    /// If the real part is outside the valid range for float, the result will be
    /// float.NegativeInfinity or float.PositiveInfinity; no exception will be thrown.
    /// </remarks>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent float value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator float(BigComplex bc) => ConvertToReal<float>(bc);

    /// <summary>Cast BigComplex to double.</summary>
    /// <remarks>
    /// If the real part is outside the valid range for double, the result will be
    /// double.NegativeInfinity or double.PositiveInfinity; no exception will be thrown.
    /// </remarks>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent double value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator double(BigComplex bc) => ConvertToReal<double>(bc);

    /// <summary>Cast BigComplex to decimal.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent decimal value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of decimal.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator decimal(BigComplex bc) => ConvertToReal<decimal>(bc);

    /// <summary>Cast of BigComplex to Complex.</summary>
    /// <remarks>
    /// If either the Real or Imaginary parts of the BigComplex value are outside the valid range
    /// for double, these will be converted to double.NegativeInfinity or double.PositiveInfinity,
    /// and no exception will be thrown.
    /// </remarks>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent Complex value.</returns>
    public static explicit operator Complex(BigComplex bc) =>
        new ((double)bc.Real, (double)bc.Imaginary);

    /// <summary>Cast BigComplex to BigDecimal.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary component.
    /// </exception>
    public static explicit operator BigDecimal(BigComplex bc) => ConvertToReal<BigDecimal>(bc);

    /// <summary>Cast BigComplex to BigRational.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <returns>The equivalent BigRational value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary component.
    /// </exception>
    public static explicit operator BigRational(BigComplex bc) => ConvertToReal<BigRational>(bc);

    #endregion Casting from BigComplex

    #region Convert to object

    /// <summary>Convert BigComplex to array.</summary>
    /// <returns>The equivalent array.</returns>
    public readonly BigDecimal[] ToArray() => new[] { Real, Imaginary };

    /// <summary>Convert BigComplex to tuple.</summary>
    /// <returns>The equivalent tuple.</returns>
    public readonly (BigDecimal, BigDecimal) ToTuple() => (Real, Imaginary);

    #endregion Convert to object

    #region TryConvert methods

    /// <inheritdoc/>
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = Zero;

        // See if we can cast it.
        if (XReflection.CanCast<TOther, BigComplex>())
        {
            result = (BigComplex)(object)value;
            return true;
        }

        // Unsupported type. Should never happen.
        // It means a cast from a standard number type to BigComplex is missing.
        throw new NotImplementedException(
            $"The cast operation from {typeof(TOther).Name} to BigComplex has not been implemented.");
    }

    /// <inheritdoc/>
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther> =>
        // No saturation needed, as BigDecimal does not specify a min or max value.
        TryConvertFromChecked(value, out result);

    /// <inheritdoc/>
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther> =>
        // No truncation needed, as BigDecimal isn't an integer type.
        TryConvertFromChecked(value, out result);

    /// <inheritdoc/>
    public static bool TryConvertToChecked<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Complex is the only supported type that can handle both real and imaginary parts.
        if (result is Complex)
        {
            // Cast to Complex.
            result = (TOther)(object)value;
            return true;
        }

        // If the number is real, convert the real part use the BigDecimal method.
        if (IsRealNumber(value))
        {
            return BigDecimal.TryConvertToChecked(value.Real, out result);
        }

        // Otherwise, we'll assume TOther is supported, but value is outside its valid range.
        throw new OverflowException(
            $"The value is outside the valid range for the {typeof(TOther).Name} type.");
    }

    /// <inheritdoc/>
    public static bool TryConvertToSaturating<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Complex is the only supported type that can handle both real and imaginary parts.
        if (result is Complex)
        {
            // Convert real and imaginary parts to doubles, with saturation.
            BigDecimal.TryConvertToSaturating(value.Real, out double real);
            BigDecimal.TryConvertToSaturating(value.Imaginary, out double imag);
            result = (TOther)(object)new Complex(real, imag);
            return true;
        }

        // For other number types, we'll just ignore the imaginary part.
        return BigDecimal.TryConvertToSaturating(value.Real, out result);
    }

    /// <inheritdoc/>
    public static bool TryConvertToTruncating<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther> =>
        // TODO This should work fine, need to test.
        TryConvertToChecked(value, out result);

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>Convert a BigComplex to a real number type.</summary>
    /// <param name="bc">The BigComplex value.</param>
    /// <typeparam name="T">The real number type.</typeparam>
    /// <returns>The closest value of the real number type.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the range of the other number type.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    private static T ConvertToReal<T>(BigComplex bc) where T : INumberBase<T>
    {
        if (IsRealNumber(bc)) return XReflection.Cast<BigDecimal, T>(bc.Real);

        throw new InvalidCastException("Cannot cast a complex number to a real number.");
    }

    #endregion Helper methods
}
