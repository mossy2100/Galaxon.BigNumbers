using System.Numerics;
using Galaxon.Core.Numbers;
using Galaxon.Core.Types;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    #region Casting to BigDecimal

    /// <summary>Cast sbyte to BigDecimal.</summary>
    /// <param name="x">The sbyte value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(sbyte x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast byte to BigDecimal.</summary>
    /// <param name="x">The byte value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(byte x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast short to BigDecimal.</summary>
    /// <param name="x">The short value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(short x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast ushort to BigDecimal.</summary>
    /// <param name="x">The ushort value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(ushort x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast int to BigDecimal.</summary>
    /// <param name="x">The int value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(int x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast uint to BigDecimal.</summary>
    /// <param name="x">The uint value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(uint x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast long to BigDecimal.</summary>
    /// <param name="x">The long value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(long x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast ulong to BigDecimal.</summary>
    /// <param name="x">The ulong value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(ulong x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast Int128 to BigDecimal.</summary>
    /// <param name="x">The Int128 value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(Int128 x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast UInt128 to BigDecimal.</summary>
    /// <param name="x">The UInt128 value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(UInt128 x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast BigInteger to BigDecimal.</summary>
    /// <param name="x">The BigInteger value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(BigInteger x)
    {
        return new BigDecimal(x);
    }

    /// <summary>Cast Half to BigDecimal.</summary>
    /// <remarks>
    /// The resulting BigDecimal value is *exactly* the value encoded by the Half, which may not
    /// exactly match the value *assigned* to the Half, due to limitations of binary floating point
    /// representation.
    /// </remarks>
    /// <param name="x">The Half value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(Half x)
    {
        return ConvertFromFloatingPoint(x);
    }

    /// <summary>Cast float to BigDecimal.</summary>
    /// <remarks>
    /// The resulting BigDecimal value is *exactly* the value encoded by the float, which may not
    /// exactly match the value *assigned* to the float, due to limitations of binary floating point
    /// representation.
    /// </remarks>
    /// <param name="x">The float value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(float x)
    {
        return ConvertFromFloatingPoint(x);
    }

    /// <summary>Cast double to BigDecimal.</summary>
    /// <remarks>
    /// The resulting BigDecimal value is *exactly* the value encoded by the double, which may not
    /// exactly match the value *assigned* to the double, due to limitations of binary floating
    /// point representation.
    /// </remarks>
    /// <param name="x">The double value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(double x)
    {
        return ConvertFromFloatingPoint(x);
    }

    /// <summary>Cast decimal to BigDecimal.</summary>
    /// <param name="x">The double value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(decimal x)
    {
        var parts = decimal.GetBits(x);

        // Get the sign and scale from the bits.
        var sign = (parts[3] & 0x80000000) == 0 ? 1 : -1;
        var scale = (byte)((parts[3] >> 16) & 0x7F);

        // Calculate the significand.
        BigInteger sig = 0;
        BigInteger mult = 1;
        for (var i = 0; i < 3; i++)
        {
            sig += (uint)parts[i] * mult;
            mult *= 0x1_0000_0000;
        }

        return new BigDecimal(sign * sig, -scale);
    }

    #endregion Casting to BigDecimal

    #region Casting from BigDecimal

    /// <summary>Cast BigDecimal to sbyte.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The sbyte value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for sbyte.
    /// </exception>
    public static explicit operator sbyte(BigDecimal bd)
    {
        return (sbyte)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to byte.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The byte value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for sbyte.
    /// </exception>
    public static explicit operator byte(BigDecimal bd)
    {
        return (byte)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to short.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The short value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for short.
    /// </exception>
    public static explicit operator short(BigDecimal bd)
    {
        return (short)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to ushort.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The ushort value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for ushort.
    /// </exception>
    public static explicit operator ushort(BigDecimal bd)
    {
        return (ushort)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to int.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The int value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for int.
    /// </exception>
    public static explicit operator int(BigDecimal bd)
    {
        return (int)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to uint.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The uint value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for uint.
    /// </exception>
    public static explicit operator uint(BigDecimal bd)
    {
        return (uint)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to long.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The long value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for long.
    /// </exception>
    public static explicit operator long(BigDecimal bd)
    {
        return (long)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to ulong.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The ulong value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for ulong.
    /// </exception>
    public static explicit operator ulong(BigDecimal bd)
    {
        return (ulong)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to Int128.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The Int128 value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for Int128.
    /// </exception>
    public static explicit operator Int128(BigDecimal bd)
    {
        return (Int128)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to UInt128.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The UInt128 value formed by truncating the BigDecimal.</returns>
    /// <exception cref="OverflowException">
    /// If the truncated value is outside the valid range for UInt128.
    /// </exception>
    public static explicit operator UInt128(BigDecimal bd)
    {
        return (UInt128)(BigInteger)bd;
    }

    /// <summary>Cast BigDecimal to BigInteger.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The BigInteger value formed by truncating the BigDecimal.</returns>
    public static explicit operator BigInteger(BigDecimal bd)
    {
        return _Shift(Truncate(bd));
    }

    /// <summary>Cast BigDecimal to Half.</summary>
    /// <remarks>
    /// For values outside the valid range for Half, this method will return ±∞ and not throw an
    /// OverflowException.
    /// </remarks>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The closest Half value.</returns>
    public static explicit operator Half(BigDecimal bd)
    {
        return ConvertToFloatingPoint<Half>(bd);
    }

    /// <summary>Cast BigDecimal to float.</summary>
    /// <remarks>
    /// For values outside the valid range for float, this method will return ±∞ and not throw an
    /// OverflowException.
    /// </remarks>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The closest float value.</returns>
    public static explicit operator float(BigDecimal bd)
    {
        return ConvertToFloatingPoint<float>(bd);
    }

    /// <summary>Cast BigDecimal to double.</summary>
    /// <remarks>
    /// For values outside the valid range for double, this method will return ±∞ and not throw an
    /// OverflowException.
    /// </remarks>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The closest double value.</returns>
    public static explicit operator double(BigDecimal bd)
    {
        return ConvertToFloatingPoint<double>(bd);
    }

    /// <summary>Cast BigDecimal to decimal.</summary>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The closest decimal value.</returns>
    /// <exception cref="OverflowException">
    /// If the BigDecimal value is outside the valid range for decimal.
    /// </exception>
    public static explicit operator decimal(BigDecimal bd)
    {
        // Check the value is within the supported range for decimal.
        if (bd < decimal.MinValue || bd > decimal.MaxValue)
        {
            throw new OverflowException("The value is outside the valid range for decimal.");
        }

        BigInteger sig;
        int exp;
        if (bd.Exponent > 0)
        {
            // If the exponent is greater than 0, shift to exponent 0 to get the correct scale.
            exp = 0;
            sig = _Shift(bd);
        }
        else
        {
            (sig, exp) = bd.ToTuple();
        }

        // Get the scale.
        var scale = (byte)-exp;

        // Check the scale is not too large.
        if (scale > _DECIMAL_PRECISION)
        {
            throw new
                OverflowException($"The exponent must be in the range -{_DECIMAL_PRECISION}..0.");
        }

        // Get the bytes for the absolute value of the significand.
        var sigBytes = BigInteger.Abs(sig).ToByteArray(true);

        // Check we have at most 12 bytes. In theory this shouldn't happen since we already checked
        // the BigDecimal value was within the valid range for decimal.
        if (sigBytes.Length > 12)
        {
            throw new OverflowException("The significand is too large.");
        }

        // Convert the bytes to the integers necessary to construct the decimal.
        var decInts = new int[3];
        for (var i = 0; i < 12; i++)
        {
            var b = i < sigBytes.Length ? sigBytes[i] : (byte)0;
            decInts[i / 4] |= b << (i % 4 * 8);
        }

        // Get the sign.
        var isNegative = sig < 0;

        return new decimal(decInts[0], decInts[1], decInts[2], isNegative, scale);
    }

    #endregion Casting from BigDecimal

    #region Conversion methods

    /// <summary>Convert BigDecimal to tuple.</summary>
    /// <returns>A tuple containing the significand and exponent.</returns>
    public readonly (BigInteger, int) ToTuple()
    {
        return (Significand, Exponent);
    }

    /// <summary>Construct BigDecimal from tuple of 2 BigInteger values.</summary>
    /// <param name="values">The tuple.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static BigDecimal FromTuple((BigInteger, int) values)
    {
        return new BigDecimal(values.Item1, values.Item2);
    }

    /// <summary>
    /// Construct a BigDecimal from a decimal significand and an integer exponent.
    /// </summary>
    /// <remarks>
    /// This method is useful for converting a value expressed in scientific notation to a
    /// BigDecimal.
    /// e.g. Avogadro's number = 6.02214 * 10^23
    /// <code>
    /// var a = BigDecimal.FromDecimal(6.02214m, 23);
    /// </code>
    /// </remarks>
    /// <param name="significand">The significand or mantissa.</param>
    /// <param name="exponent">The exponent (defaults to 0).</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static BigDecimal FromDecimal(decimal significand, int exponent = 0)
    {
        BigDecimal x = significand;
        x.Exponent += exponent;
        return x;
    }

    #endregion Convert to and from tuple

    #region TryConvert methods

    /// <inheritdoc/>
    /// <exception cref="InvalidCastException">
    /// If no cast operation exists from TOther to BigDecimal.
    /// </exception>
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        switch (value)
        {
            case sbyte:
                result = (sbyte)(object)value;
                return true;

            case byte:
                result = (byte)(object)value;
                return true;

            case short:
                result = (short)(object)value;
                return true;

            case ushort:
                result = (ushort)(object)value;
                return true;

            case int:
                result = (int)(object)value;
                return true;

            case uint:
                result = (uint)(object)value;
                return true;

            case long:
                result = (long)(object)value;
                return true;

            case ulong:
                result = (ulong)(object)value;
                return true;

            case Int128:
                result = (Int128)(object)value;
                return true;

            case UInt128:
                result = (UInt128)(object)value;
                return true;

            case BigInteger:
                result = (BigInteger)(object)value;
                return true;

            case Half:
                result = (Half)(object)value;
                return true;

            case float:
                result = (float)(object)value;
                return true;

            case double:
                result = (double)(object)value;
                return true;

            case decimal:
                result = (decimal)(object)value;
                return true;

            default:
                // Unsupported type.
                result = 0;
                return false;
        }
    }

    /// <inheritdoc/>
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        // No saturation needed, as BigDecimal does not specify a min or max value.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc/>
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        // No truncation needed, as BigDecimal isn't an integer type.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidCastException">
    /// If no cast operation exists from TOther to BigDecimal.
    /// </exception>
    /// <exception cref="OverflowException">
    /// If the BigDecimal value is outside the range of TOther.
    /// </exception>
    public static bool TryConvertToChecked<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Initialize result to silence the compiler warning.
        result = TOther.Zero;

        switch (result)
        {
            case sbyte:
                result = (TOther)(object)(sbyte)value;
                return true;

            case byte:
                result = (TOther)(object)(byte)value;
                return true;

            case short:
                result = (TOther)(object)(short)value;
                return true;

            case ushort:
                result = (TOther)(object)(ushort)value;
                return true;

            case int:
                result = (TOther)(object)(int)value;
                return true;

            case uint:
                result = (TOther)(object)(uint)value;
                return true;

            case long:
                result = (TOther)(object)(long)value;
                return true;

            case ulong:
                result = (TOther)(object)(ulong)value;
                return true;

            case Int128:
                result = (TOther)(object)(Int128)value;
                return true;

            case UInt128:
                result = (TOther)(object)(UInt128)value;
                return true;

            case BigInteger:
                result = (TOther)(object)(BigInteger)value;
                return true;

            case Half:
                result = (TOther)(object)(Half)value;
                return true;

            case float:
                result = (TOther)(object)(float)value;
                return true;

            case double:
                result = (TOther)(object)(double)value;
                return true;

            case decimal:
                result = (TOther)(object)(decimal)value;
                return true;

            default:
                // Unsupported type.
                return false;
        }
    }

    /// <inheritdoc/>
    public static bool TryConvertToSaturating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Initialize result to silence the compiler warning.
        result = TOther.Zero;

        switch (result)
        {
            case sbyte:
                var sbResult = value >= sbyte.MaxValue ? sbyte.MaxValue :
                    value <= sbyte.MinValue ? sbyte.MinValue : (sbyte)value;
                result = (TOther)(object)sbResult;
                return true;

            case byte:
                var bResult = value >= byte.MaxValue ? byte.MaxValue :
                    value <= byte.MinValue ? byte.MinValue : (byte)value;
                result = (TOther)(object)bResult;
                return true;

            case short:
                var sResult = value >= short.MaxValue ? short.MaxValue :
                    value <= short.MinValue ? short.MinValue : (short)value;
                result = (TOther)(object)sResult;
                return true;

            case ushort:
                var usResult = value >= ushort.MaxValue ? ushort.MaxValue :
                    value <= ushort.MinValue ? ushort.MinValue : (ushort)value;
                result = (TOther)(object)usResult;
                return true;

            case int:
                var iResult = value >= int.MaxValue ? int.MaxValue :
                    value <= int.MinValue ? int.MinValue : (int)value;
                result = (TOther)(object)iResult;
                return true;

            case uint:
                var uiResult = value >= uint.MaxValue ? uint.MaxValue :
                    value <= uint.MinValue ? uint.MinValue : (uint)value;
                result = (TOther)(object)uiResult;
                return true;

            case long:
                var lResult = value >= long.MaxValue ? long.MaxValue :
                    value <= long.MinValue ? long.MinValue : (long)value;
                result = (TOther)(object)lResult;
                return true;

            case ulong:
                var ulResult = value >= ulong.MaxValue ? ulong.MaxValue :
                    value <= ulong.MinValue ? ulong.MinValue : (ulong)value;
                result = (TOther)(object)ulResult;
                return true;

            case Int128:
                var i128Result = value >= Int128.MaxValue ? Int128.MaxValue :
                    value <= Int128.MinValue ? Int128.MinValue : (Int128)value;
                result = (TOther)(object)i128Result;
                return true;

            case UInt128:
                var ui128Result = value >= UInt128.MaxValue ? UInt128.MaxValue :
                    value <= UInt128.MinValue ? UInt128.MinValue : (UInt128)value;
                result = (TOther)(object)ui128Result;
                return true;

            case BigInteger:
                result = (TOther)(object)(BigInteger)value;
                return true;

            case Half:
                var hResult = value >= Half.MaxValue ? Half.MaxValue :
                    value <= Half.MinValue ? Half.MinValue : (Half)value;
                result = (TOther)(object)hResult;
                return true;

            case float:
                var fResult = value >= float.MaxValue ? float.MaxValue :
                    value <= float.MinValue ? float.MinValue : (float)value;
                result = (TOther)(object)fResult;
                return true;

            case double:
                var dResult = value >= double.MaxValue ? double.MaxValue :
                    value <= double.MinValue ? double.MinValue : (double)value;
                result = (TOther)(object)dResult;
                return true;

            case decimal:
                var mResult = value >= decimal.MaxValue ? decimal.MaxValue :
                    value <= decimal.MinValue ? decimal.MinValue : (decimal)value;
                result = (TOther)(object)mResult;
                return true;

            default:
                // Unsupported type.
                return false;
        }
    }

    /// <inheritdoc/>
    /// <see cref="BigDecimal.explicit operator BigInteger(BigDecimal)"/>
    public static bool TryConvertToTruncating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Integers will be truncated in the cast to BigInteger.
        // The documentation doesn't mention throwing overflow exceptions for this method, but
        // that's what is done for integer, so we'll do the same thing here.
        // See: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Int32.cs
        return TryConvertToChecked(value, out result);
    }

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>
    /// Helper method to convert a standard binary floating point type (Half, float, or double) to a
    /// BigDecimal.
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    private static BigDecimal ConvertFromFloatingPoint<T>(T x) where T : IFloatingPointIeee754<T>
    {
        // Guard.
        if (!T.IsFinite(x))
        {
            throw new InvalidCastException("Cannot convert ±∞ or NaN to BigDecimal.");
        }

        // Get the value's parts.
        var (signBit, expBits, fracBits) = x.Disassemble();

        // Check for ±0.
        if (expBits == 0 && fracBits == 0)
        {
            return 0;
        }

        // Check if the number is normal or subnormal.
        var isSubnormal = expBits == 0;

        // Get sign.
        var sign = signBit == 1 ? -1 : 1;

        // Get some information about the type.
        var nFracBits = XFloatingPoint.GetNumFracBits<T>();
        var maxExp = XFloatingPoint.GetMaxExp<T>();

        // Get the significand.
        // The bit values are taken to have the value 1..2^(nFracBits - 1) and the exponent is
        // correspondingly shifted. Doing this avoids division operations.
        BigDecimal sig = 0;
        BigDecimal pow = 1;
        for (var i = 0; i < nFracBits; i++)
        {
            if ((fracBits & (1ul << i)) != 0)
            {
                sig += pow;
            }
            pow *= 2;
        }

        // One more addition for normal numbers, which have the most significant bit set implicitly.
        if (!isSubnormal)
        {
            sig += pow;
        }

        // Get the power of 2.
        var exp = (isSubnormal ? 1 : expBits) - maxExp - nFracBits;

        // Calculate the result.
        return sign * sig * Exp2(exp);
    }

    /// <summary>
    /// Helper method to convert a BigDecimal to a standard binary floating point type (Half, float,
    /// or double).
    /// </summary>
    /// <remarks>
    /// If the BigDecimal is outside the range for this type, this method will return negative or
    /// positive infinity as needed, without throwing an exception.
    /// </remarks>
    /// <typeparam name="T">The standard binary floating point type.</typeparam>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The converted value.</returns>
    public static T ConvertToFloatingPoint<T>(BigDecimal bd)
        where T : IBinaryFloatingPointIeee754<T>
    {
        // Check for 0.
        if (bd == 0)
        {
            return T.Zero;
        }

        // Check for -∞.
        var minValue = XReflection.Cast<T, BigDecimal>(XNumber.GetMinValue<T>());
        if (bd < minValue)
        {
            return XFloatingPoint.GetNegativeInfinity<T>();
        }

        // Check for +∞.
        var maxValue = XReflection.Cast<T, BigDecimal>(XNumber.GetMaxValue<T>());
        if (bd > maxValue)
        {
            return XFloatingPoint.GetPositiveInfinity<T>();
        }

        // Check if its subnormal.
        var minPosNormalValue = XFloatingPoint.GetMinPosNormalValue<T>();
        var bdMinPosNormalValue = XReflection.Cast<T, BigDecimal>(minPosNormalValue);
        var abs = Abs(bd);
        var isSubnormal = abs < bdMinPosNormalValue;

        // Get the minimum and maximum exponent.
        var minExp = XFloatingPoint.GetMinExp<T>();
        var maxExp = XFloatingPoint.GetMaxExp<T>();

        // Calculate the exponent.
        var exp = isSubnormal ? minExp : (BigInteger)Floor(Log2(abs));
        var sig = abs / Exp2(exp);
        var fracBits = 0ul;
        byte nextBit = 0;

        // Get the number of fraction bits.
        var nFracBits = XFloatingPoint.GetNumFracBits<T>();

        // Calculate fraction bits.
        for (var i = 0; i < nFracBits + 1; i++)
        {
            // Get the next bit.
            nextBit = (byte)(sig < 1 ? 0 : 1);

            // Add the bit.
            fracBits = (fracBits << 1) + nextBit;

            // Prepare for next iteration.
            sig = (sig - nextBit) * 2;
        }

        // Round up if necessary, using MidpointRounding.ToEven method.
        var rollover = (ulong)Exp2(nFracBits);
        var maxFraction = rollover * 2 - 1;
        if ((nextBit == 1 && sig >= 0.5) || (nextBit == 0 && sig > 0.5))
        {
            // Don't go over 24 bits.
            if (fracBits == maxFraction)
            {
                fracBits = rollover;
                exp--;
            }
            else
            {
                fracBits += 1;
            }
        }

        // Get the float's parts.
        var signBit = (byte)(bd < 0 ? 1 : 0);
        ushort expBits = 0;
        if (!isSubnormal)
        {
            // Convert exponent to stored value.
            expBits = (ushort)(exp + maxExp);
            // Clear top bit as this isn't stored in the float.
            fracBits &= rollover - 1;
        }

        // Assemble the final value.
        return XFloatingPoint.Assemble<T>(signBit, expBits, fracBits);
    }

    #endregion Helper methods
}
