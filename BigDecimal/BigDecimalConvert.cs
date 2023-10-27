using System.Numerics;
using Galaxon.Core.Numbers;
using Galaxon.Core.Types;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    #region Cast operators to BigDecimal

    /// <summary>
    /// Implicit cast from sbyte to BigDecimal.
    /// </summary>
    /// <param name="n">The sbyte value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(sbyte n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from byte to BigDecimal.
    /// </summary>
    /// <param name="n">The byte value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(byte n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from short to BigDecimal.
    /// </summary>
    /// <param name="n">The short value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(short n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from ushort to BigDecimal.
    /// </summary>
    /// <param name="n">The ushort value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(ushort n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from int to BigDecimal.
    /// </summary>
    /// <param name="n">The int value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(int n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from uint to BigDecimal.
    /// </summary>
    /// <param name="n">The uint value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(uint n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from long to BigDecimal.
    /// </summary>
    /// <param name="n">The long value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(long n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from ulong to BigDecimal.
    /// </summary>
    /// <param name="n">The ulong value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(ulong n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from Int128 to BigDecimal.
    /// </summary>
    /// <param name="n">The Int128 value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(Int128 n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from UInt128 to BigDecimal.
    /// </summary>
    /// <param name="n">The UInt128 value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(UInt128 n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from BigInteger to BigDecimal.
    /// </summary>
    /// <param name="n">The BigInteger value.</param>
    /// <returns>The equivalent BigDecimal value.</returns>
    public static implicit operator BigDecimal(BigInteger n)
    {
        return new BigDecimal(n);
    }

    /// <summary>
    /// Implicit cast from Half to BigDecimal.
    /// NB: The resulting BigDecimal value is exactly the value encoded by the Half.
    /// However, since Halves only approximate decimal values, it's possible that only the first few
    /// digits are valid in terms of the intended value.
    /// Therefore, you may need to use RoundSigFigs() to get the value you really want.
    /// </summary>
    public static implicit operator BigDecimal(Half n)
    {
        return ConvertFromFloatingPoint<Half>(n);
    }

    /// <summary>
    /// Implicit cast from float to BigDecimal.
    /// NB: The resulting BigDecimal value is exactly the value encoded by the float.
    /// However, since floats only approximate decimal values, it's possible that only the first 6-9
    /// digits are valid in terms of the intended value.
    /// Therefore, you may need to use RoundSigFigs() to get the value you really want, e.g.
    /// <code>
    /// BigDecimal bd = BigDecimal.RoundSigFigs(1.2345f, FloatMaxSigFigs);
    /// </code>
    /// </summary>
    public static implicit operator BigDecimal(float n)
    {
        return ConvertFromFloatingPoint<float>(n);
    }

    /// <summary>
    /// Implicit cast from double to BigDecimal.
    /// NB: The resulting BigDecimal value is exactly the value encoded by the double.
    /// However, since doubles only approximate decimal values, it's possible that only the first
    /// 15-17 digits are valid in terms of the intended value.
    /// Therefore, you may need to use RoundSigFigs() to get the value you really want, e.g.
    /// <code>
    /// BigDecimal bd = BigDecimal.RoundSigFigs(1.2345, DoubleMaxSigFigs);
    /// </code>
    /// </summary>
    public static implicit operator BigDecimal(double n)
    {
        return ConvertFromFloatingPoint<double>(n);
    }

    /// <summary>
    /// Implicit cast from decimal to BigDecimal.
    /// The cast is implicit because any decimal value can be cast to a BigDecimal exactly, without
    /// loss of information. However, rounding off using Round() or RoundSigFigs() can cause
    /// information loss.
    /// We don't need to use Parse() or division operations here, because the base is decimal.
    /// We can just extract the parts of the decimal from the bits and construct a BigDecimal from
    /// those. This method should be faster than using ToString() and Parse().
    /// </summary>
    public static implicit operator BigDecimal(decimal n)
    {
        var parts = decimal.GetBits(n);

        // Get the sign and scale from the bits.
        var sign = (parts[3] & 0x80000000) == 0 ? 1 : -1;
        var scale = (byte)(parts[3] >> 16 & 0x7F);

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

    #endregion Cast operators to BigDecimal

    #region Cast operators from BigDecimal

    /// <summary>
    /// Explicit cast from BigDecimal to sbyte.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for sbyte.
    /// </exception>
    public static explicit operator sbyte(BigDecimal bd)
    {
        return (sbyte)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to byte.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for byte.
    /// </exception>
    public static explicit operator byte(BigDecimal bd)
    {
        return (byte)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to short.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for short.
    /// </exception>
    public static explicit operator short(BigDecimal bd)
    {
        return (short)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to ushort.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for ushort.
    /// </exception>
    public static explicit operator ushort(BigDecimal bd)
    {
        return (ushort)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to int.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for int.
    /// </exception>
    public static explicit operator int(BigDecimal bd)
    {
        return (int)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to uint.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for uint.
    /// </exception>
    public static explicit operator uint(BigDecimal bd)
    {
        return (uint)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to long.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for long.
    /// </exception>
    public static explicit operator long(BigDecimal bd)
    {
        return (long)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to ulong.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for ulong.
    /// </exception>
    public static explicit operator ulong(BigDecimal bd)
    {
        return (ulong)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to Int128.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for Int128.
    /// </exception>
    public static explicit operator Int128(BigDecimal bd)
    {
        return (Int128)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to UInt128.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for uInt128.
    /// </exception>
    public static explicit operator UInt128(BigDecimal bd)
    {
        return (UInt128)(BigInteger)bd;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to BigInteger.
    /// </summary>
    public static explicit operator BigInteger(BigDecimal bd)
    {
        var trunc = Truncate(bd);
        trunc.ShiftToExp(0);
        return trunc.Significand;
    }

    /// <summary>
    /// Explicit cast from BigDecimal to Half.
    /// This method will not throw an OverflowException, but will return ±∞ for a value outside the
    /// valid range for Half.
    /// </summary>
    public static explicit operator Half(BigDecimal bd)
    {
        return ConvertToFloatingPoint<Half>(bd);
    }

    /// <summary>
    /// Explicit cast from BigDecimal to float.
    /// This method will not throw an OverflowException, but will return ±∞ for a value outside the
    /// valid range for float.
    /// </summary>
    public static explicit operator float(BigDecimal bd)
    {
        return ConvertToFloatingPoint<float>(bd);
    }

    /// <summary>
    /// Explicit cast from BigDecimal to double.
    /// This method will not throw an OverflowException, but will return ±∞ for a value outside the
    /// valid range for double.
    /// </summary>
    public static explicit operator double(BigDecimal bd)
    {
        return ConvertToFloatingPoint<double>(bd);
    }

    /// <summary>
    /// Explicit cast from BigDecimal to decimal.
    /// </summary>
    /// <exception cref="OverflowException">
    /// If the value is outside the valid range for decimal.
    /// </exception>
    public static explicit operator decimal(BigDecimal bd)
    {
        // Check the value is within the supported range for decimal.
        if (bd < decimal.MinValue || bd > decimal.MaxValue)
        {
            throw new OverflowException("The value is outside the valid range for decimal.");
        }

        // If the exponent is greater than 0, shift to exponent 0 to get the correct scale.
        if (bd.Exponent > 0)
        {
            bd.ShiftToExp(0);
        }

        // Get the scale.
        var scale = (byte)-bd.Exponent;

        // Check the scale is not too large.
        if (scale > DecimalPrecision)
        {
            throw new
                OverflowException($"The exponent must be in the range -{DecimalPrecision}..0.");
        }

        // Get the bytes for the absolute value of the significand.
        var sigBytes = BigInteger.Abs(bd.Significand).ToByteArray(true);

        // Check we have at most 12 bytes.
        if (sigBytes.Length > 12)
        {
            throw new OverflowException("The significand is too large.");
        }

        // Convert the bytes to the integers necessary to construct the decimal.
        var decInts = new int[3];
        for (var i = 0; i < 12; i++)
        {
            var b = i < sigBytes.Length ? sigBytes[i] : (byte)0;
            decInts[i / 4] |= b << i % 4 * 8;
        }

        // Get the sign.
        var isNegative = bd.Significand < 0;

        return new decimal(decInts[0], decInts[1], decInts[2], isNegative, scale);
    }

    #endregion Cast operators from BigDecimal

    #region TryConvert methods

    /// <inheritdoc />
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

    /// <inheritdoc />
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        // No saturation needed, as BigDecimal does not specify a min or max value.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc />
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        // No truncation needed, as BigDecimal isn't an integer type.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public static bool TryConvertToSaturating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Initialize result to silence the compiler warning.
        result = TOther.Zero;

        switch (result)
        {
            case sbyte:
                sbyte sbResult = (value >= sbyte.MaxValue) ? sbyte.MaxValue :
                    (value <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value;
                result = (TOther)(object)sbResult;
                return true;

            case byte:
                byte bResult = (value >= byte.MaxValue) ? byte.MaxValue :
                    (value <= byte.MinValue) ? byte.MinValue : (byte)value;
                result = (TOther)(object)bResult;
                return true;

            case short:
                short sResult = (value >= short.MaxValue) ? short.MaxValue :
                    (value <= short.MinValue) ? short.MinValue : (short)value;
                result = (TOther)(object)sResult;
                return true;

            case ushort:
                ushort usResult = (value >= ushort.MaxValue) ? ushort.MaxValue :
                    (value <= ushort.MinValue) ? ushort.MinValue : (ushort)value;
                result = (TOther)(object)usResult;
                return true;

            case int:
                int iResult = (value >= int.MaxValue) ? int.MaxValue :
                    (value <= int.MinValue) ? int.MinValue : (int)value;
                result = (TOther)(object)iResult;
                return true;

            case uint:
                uint uiResult = (value >= uint.MaxValue) ? uint.MaxValue :
                    (value <= uint.MinValue) ? uint.MinValue : (uint)value;
                result = (TOther)(object)uiResult;
                return true;

            case long:
                long lResult = (value >= long.MaxValue) ? long.MaxValue :
                    (value <= long.MinValue) ? long.MinValue : (long)value;
                result = (TOther)(object)lResult;
                return true;

            case ulong:
                ulong ulResult = (value >= ulong.MaxValue) ? ulong.MaxValue :
                    (value <= ulong.MinValue) ? ulong.MinValue : (ulong)value;
                result = (TOther)(object)ulResult;
                return true;

            case Int128:
                Int128 i128Result = (value >= Int128.MaxValue) ? Int128.MaxValue :
                    (value <= Int128.MinValue) ? Int128.MinValue : (Int128)value;
                result = (TOther)(object)i128Result;
                return true;

            case UInt128:
                UInt128 ui128Result = (value >= UInt128.MaxValue) ? UInt128.MaxValue :
                    (value <= UInt128.MinValue) ? UInt128.MinValue : (UInt128)value;
                result = (TOther)(object)ui128Result;
                return true;

            case Half:
                Half hResult = (value >= Half.MaxValue) ? Half.MaxValue :
                    (value <= Half.MinValue) ? Half.MinValue : (Half)value;
                result = (TOther)(object)hResult;
                return true;

            case float:
                float fResult = (value >= float.MaxValue) ? float.MaxValue :
                    (value <= float.MinValue) ? float.MinValue : (float)value;
                result = (TOther)(object)fResult;
                return true;

            case double:
                double dResult = (value >= double.MaxValue) ? double.MaxValue :
                    (value <= double.MinValue) ? double.MinValue : (double)value;
                result = (TOther)(object)dResult;
                return true;

            case decimal:
                decimal mResult = (value >= decimal.MaxValue) ? decimal.MaxValue :
                    (value <= decimal.MinValue) ? decimal.MinValue : (decimal)value;
                result = (TOther)(object)mResult;
                return true;

            default:
                // Unsupported type.
                return false;
        }
    }

    /// <inheritdoc />
    /// <see cref="BigDecimal.explicit operator BigInteger(BigDecimal)"/>
    public static bool TryConvertToTruncating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Integers will be truncated by the conversion, in the cast to BigInteger (see ref above).
        // The documentation doesn't mention throwing overflow exceptions for
        // TryConvertToTruncating(), but that's what is done for integer, so we'll do
        // the same thing here.
        // See: https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Int32.cs
        return TryConvertToChecked(value, out result);
    }

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>
    /// Private method to convert a floating point value (float or double) to a BigDecimal.
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    private static BigDecimal ConvertFromFloatingPoint<T>(T n) where T : IFloatingPointIeee754<T>
    {
        // Guard.
        if (!T.IsFinite(n))
        {
            throw new InvalidCastException("Cannot convert ±∞ or NaN to BigDecimal.");
        }

        // Get the value's parts.
        var (signBit, expBits, fracBits) = n.Disassemble();

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
            if ((fracBits & 1ul << i) != 0)
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
    /// Convert a BigDecimal to a standard binary floating point type.
    /// If the BigDecimal is outside the range for this type, this method will return negative or
    /// positive infinity as needed, without throwing an exception.
    /// </summary>
    /// <typeparam name="T">The standard binary floating point type.</typeparam>
    /// <param name="bd">The BigDecimal value.</param>
    /// <returns>The converted value.</returns>
    public static T ConvertToFloatingPoint<T>(BigDecimal bd)
        where T : IBinaryFloatingPointIeee754<T>
    {
        // Check for 0.
        if (bd == 0) return T.Zero;

        // Check for -∞.
        var minValue = XReflection.Cast<T, BigDecimal>(XNumber.GetMinValue<T>());
        if (bd < minValue) return XFloatingPoint.GetNegativeInfinity<T>();

        // Check for +∞.
        var maxValue = XReflection.Cast<T, BigDecimal>(XNumber.GetMaxValue<T>());
        if (bd > maxValue) return XFloatingPoint.GetPositiveInfinity<T>();

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
        var fracBits = 0uL;
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
        if (nextBit == 1 && sig >= 0.5 || nextBit == 0 && sig > 0.5)
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
