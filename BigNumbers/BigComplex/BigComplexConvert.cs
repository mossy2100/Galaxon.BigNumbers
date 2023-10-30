using System.Numerics;
using Galaxon.Core.Types;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Casting to BigComplex

    /// <summary>Cast sbyte to BigComplex.</summary>
    /// <param name="n">The sbyte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(sbyte n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast byte to BigDecimal.</summary>
    /// <param name="n">The byte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(byte n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast short to BigDecimal.</summary>
    /// <param name="n">The short value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(short n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast ushort to BigDecimal.</summary>
    /// <param name="n">The ushort value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ushort n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast int to BigDecimal.</summary>
    /// <param name="n">The int value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(int n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast uint to BigDecimal.</summary>
    /// <param name="n">The uint value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(uint n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast long to BigDecimal.</summary>
    /// <param name="n">The long value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(long n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast ulong to BigDecimal.</summary>
    /// <param name="n">The ulong value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ulong n)
    {
        return new BigComplex(n);
    }

    /// <summary>
    /// Cast Int128 to BigDecimal.
    /// </summary>
    /// <param name="n">The Int128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Int128 n)
    {
        return new BigComplex(n);
    }

    /// <summary>
    /// Cast UInt128 to BigDecimal.
    /// </summary>
    /// <param name="n">The UInt128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(UInt128 n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast BigInteger to BigDecimal.</summary>
    /// <param name="n">The BigInteger value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigInteger n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast Half to BigDecimal.</summary>
    /// <param name="n">The Half value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Half n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast float to BigDecimal.</summary>
    /// <param name="n">The float value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(float n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast double to BigDecimal.</summary>
    /// <param name="n">The double value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(double n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast decimal to BigDecimal.</summary>
    /// <param name="n">The decimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(decimal n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast Complex to BigComplex.</summary>
    /// <param name="z">The Complex value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Complex z)
    {
        return new BigComplex(z.Real, z.Imaginary);
    }

    /// <summary>Cast BigDecimal to BigComplex.</summary>
    /// <param name="n">The BigDecimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigDecimal n)
    {
        return new BigComplex(n);
    }

    /// <summary>Cast BigRational to BigComplex.</summary>
    /// <remarks>
    /// This cast has to be explicit because there can be information loss when converting the
    /// BigRational to BigDecimal.
    /// </remarks>
    /// <param name="n">The BigRational value.</param>
    /// <returns>The closest BigComplex value.</returns>
    public static explicit operator BigComplex(BigRational n)
    {
        return new BigComplex((BigDecimal)n);
    }

    #endregion Casting to BigComplex

    #region Casting from BigComplex

    /// <summary>Cast BigComplex to sbyte.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent sbyte value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of sbyte.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator sbyte(BigComplex z)
    {
        return ConvertToReal<sbyte>(z);
    }

    /// <summary>Cast BigComplex to byte.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent byte value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of byte.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator byte(BigComplex z)
    {
        return ConvertToReal<byte>(z);
    }

    /// <summary>Cast BigComplex to short.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent short value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of short.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator short(BigComplex z)
    {
        return ConvertToReal<short>(z);
    }

    /// <summary>Cast BigComplex to ushort.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent ushort value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of ushort.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator ushort(BigComplex z)
    {
        return ConvertToReal<ushort>(z);
    }

    /// <summary>Cast BigComplex to int.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent int value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of int.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator int(BigComplex z)
    {
        return ConvertToReal<int>(z);
    }

    /// <summary>Cast BigComplex to uint.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent uint value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of uint.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator uint(BigComplex z)
    {
        return ConvertToReal<uint>(z);
    }

    /// <summary>Cast BigComplex to long.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent long value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of long.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator long(BigComplex z)
    {
        return ConvertToReal<long>(z);
    }

    /// <summary>Cast BigComplex to ulong.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent ulong value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of ulong.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator ulong(BigComplex z)
    {
        return ConvertToReal<ulong>(z);
    }

    /// <summary>Cast BigComplex to Int128.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent Int128 value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of Int128.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator Int128(BigComplex z)
    {
        return ConvertToReal<Int128>(z);
    }

    /// <summary>Cast BigComplex to UInt128.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent UInt128 value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of UInt128.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator UInt128(BigComplex z)
    {
        return ConvertToReal<UInt128>(z);
    }

    /// <summary>Cast BigComplex to BigInteger.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent BigInteger value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator BigInteger(BigComplex z)
    {
        return ConvertToReal<BigInteger>(z);
    }

    /// <summary>Cast BigComplex to Half.</summary>
    /// <remarks>
    /// If the real part is outside the valid range for Half, the result will be
    /// Half.NegativeInfinity or Half.PositiveInfinity; no exception will be thrown.
    /// </remarks>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent Half value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator Half(BigComplex z)
    {
        return ConvertToReal<Half>(z);
    }

    /// <summary>Cast BigComplex to float.</summary>
    /// <remarks>
    /// If the real part is outside the valid range for float, the result will be
    /// float.NegativeInfinity or float.PositiveInfinity; no exception will be thrown.
    /// </remarks>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent float value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator float(BigComplex z)
    {
        return ConvertToReal<float>(z);
    }

    /// <summary>Cast BigComplex to double.</summary>
    /// <remarks>
    /// If the real part is outside the valid range for double, the result will be
    /// double.NegativeInfinity or double.PositiveInfinity; no exception will be thrown.
    /// </remarks>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent double value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator double(BigComplex z)
    {
        return ConvertToReal<double>(z);
    }

    /// <summary>Cast BigComplex to decimal.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent decimal value.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the valid range of decimal.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    public static explicit operator decimal(BigComplex z)
    {
        return ConvertToReal<decimal>(z);
    }

    /// <summary>Cast of BigComplex to Complex.</summary>
    /// <remarks>
    /// If either the Real or Imaginary parts of the BigComplex value are outside the valid range
    /// for double, these will be converted to double.NegativeInfinity or double.PositiveInfinity,
    /// and no exception will be thrown.
    /// </remarks>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent Complex value.</returns>
    public static explicit operator Complex(BigComplex z)
    {
        return new Complex((double)z.Real, (double)z.Imaginary);
    }

    /// <summary>Cast BigComplex to BigDecimal.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary component.
    /// </exception>
    public static explicit operator BigDecimal(BigComplex z)
    {
        return ConvertToReal<BigDecimal>(z);
    }

    /// <summary>Cast BigComplex to BigRational.</summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent BigRational value.</returns>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary component.
    /// </exception>
    public static explicit operator BigRational(BigComplex z)
    {
        return ConvertToReal<BigRational>(z);
    }

    #endregion Casting from BigComplex

    #region Convert to object

    /// <summary>Convert BigComplex to array.</summary>
    /// <returns>The equivalent array.</returns>
    public readonly BigDecimal[] ToArray()
    {
        return new[] { Real, Imaginary };
    }

    /// <summary>Convert BigComplex to tuple.</summary>
    /// <returns>The equivalent tuple.</returns>
    public readonly (BigDecimal, BigDecimal) ToTuple()
    {
        return (Real, Imaginary);
    }

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
        where TOther : INumberBase<TOther>
    {
        // No saturation needed, as BigDecimal does not specify a min or max value.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc/>
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        // No truncation needed, as BigDecimal isn't an integer type.
        return TryConvertFromChecked(value, out result);
    }

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
        where TOther : INumberBase<TOther>
    {
        // TODO This should work fine, need to test.
        return TryConvertToChecked(value, out result);
    }

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>Convert a BigComplex to a real number type.</summary>
    /// <param name="z">The BigComplex value.</param>
    /// <typeparam name="T">The real number type.</typeparam>
    /// <returns>The closest value of the real number type.</returns>
    /// <exception cref="OverflowException">
    /// If the real part of the BigComplex is outside the range of the other number type.
    /// </exception>
    /// <exception cref="InvalidCastException">
    /// If the BigComplex value has an imaginary part.
    /// </exception>
    private static T ConvertToReal<T>(BigComplex z) where T : INumberBase<T>
    {
        if (IsRealNumber(z)) return XReflection.Cast<BigDecimal, T>(z.Real);

        throw new InvalidCastException("Cannot cast an imaginary number to a real number.");
    }

    #endregion Helper methods
}
