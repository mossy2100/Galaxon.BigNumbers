using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Casting to BigRational

    /// <summary>Cast sbyte to BigRational.</summary>
    public static implicit operator BigRational(sbyte x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast byte to BigRational.</summary>
    public static implicit operator BigRational(byte x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast short to BigRational.</summary>
    public static implicit operator BigRational(short x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast ushort to BigRational.</summary>
    public static implicit operator BigRational(ushort x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast int to BigRational.</summary>
    public static implicit operator BigRational(int x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast uint to BigRational.</summary>
    public static implicit operator BigRational(uint x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast long to BigRational.</summary>
    public static implicit operator BigRational(long x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast ulong to BigRational.</summary>
    public static implicit operator BigRational(ulong x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast Int128 to BigRational.</summary>
    public static implicit operator BigRational(Int128 x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast UInt128 to BigRational.</summary>
    public static implicit operator BigRational(UInt128 x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast BigInteger to BigRational.</summary>
    public static implicit operator BigRational(BigInteger x)
    {
        return new BigRational(x);
    }

    /// <summary>Cast Half to BigRational.</summary>
    public static implicit operator BigRational(Half x)
    {
        return ConvertFromFloatingPoint<Half>(x);
    }

    /// <summary>Cast float to BigRational.</summary>
    public static implicit operator BigRational(float x)
    {
        return ConvertFromFloatingPoint<float>(x);
    }

    /// <summary>Cast double to BigRational.</summary>
    public static implicit operator BigRational(double x)
    {
        return ConvertFromFloatingPoint<double>(x);
    }

    /// <summary>Cast decimal to BigRational.</summary>
    public static implicit operator BigRational(decimal x)
    {
        // Handle zero.
        if (x == 0m) return Zero;

        // Get the parts of the floating point value.
        (var signBit, ushort scaleBits, var intBits) = x.Disassemble();

        // Get the numerator.
        var num = (signBit == 1 ? -1 : 1) * (BigInteger)intBits;

        // Get the denominator.
        var den = BigInteger.Pow(10, scaleBits);

        // Construct and return the new value.
        return new BigRational(num, den);
    }

    /// <summary>Cast BigDecimal to BigRational.</summary>
    public static implicit operator BigRational(BigDecimal x)
    {
        return x.Exponent switch
        {
            // Zero exponent.
            0 => new BigRational(x.Significand),

            // Positive exponent.
            > 0 => new BigRational(x.Significand * BigInteger.Pow(10, x.Exponent)),

            // Negative exponent.
            < 0 => new BigRational(x.Significand, BigInteger.Pow(10, -x.Exponent)),
        };
    }

    #endregion Casting to BigRational

    #region Casting from BigRational

    /// <summary>Cast BigRational to sbyte.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of sbyte.</exception>
    public static explicit operator sbyte(BigRational br)
    {
        return (sbyte)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to byte.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of byte.</exception>
    public static explicit operator byte(BigRational br)
    {
        return (byte)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to short.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of short.</exception>
    public static explicit operator short(BigRational br)
    {
        return (short)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to ushort.</summary>
    /// <exception cref="OverflowException">
    /// If the result is outside the range of ushort.
    /// </exception>
    public static explicit operator ushort(BigRational br)
    {
        return (ushort)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to an int.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of int.</exception>
    public static explicit operator int(BigRational br)
    {
        return (int)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to uint.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of uint.</exception>
    public static explicit operator uint(BigRational br)
    {
        return (uint)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to long.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of long.</exception>
    public static explicit operator long(BigRational br)
    {
        return (long)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to ulong.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of ulong.</exception>
    public static explicit operator ulong(BigRational br)
    {
        return (ulong)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to an Int128.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of Int128.</exception>
    public static explicit operator Int128(BigRational br)
    {
        return (Int128)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to UInt128.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of UInt128.</exception>
    public static explicit operator UInt128(BigRational br)
    {
        return (UInt128)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to BigInteger.</summary>
    public static explicit operator BigInteger(BigRational br)
    {
        return (BigInteger)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to Half.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of Half.</exception>
    public static explicit operator Half(BigRational br)
    {
        return (Half)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to float.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of float.</exception>
    public static explicit operator float(BigRational br)
    {
        return (float)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to double.</summary>
    /// <exception cref="OverflowException">
    /// If the result is outside the range of double.
    /// </exception>
    public static explicit operator double(BigRational br)
    {
        return (double)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to decimal.</summary>
    /// <exception cref="OverflowException">
    /// If the result is outside the range of decimal.
    /// </exception>
    public static explicit operator decimal(BigRational br)
    {
        return (decimal)(BigDecimal)br;
    }

    /// <summary>Cast BigRational to BigDecimal.</summary>
    public static explicit operator BigDecimal(BigRational br)
    {
        return (BigDecimal)br.Numerator / (BigDecimal)br.Denominator;
    }

    #endregion Casting from BigRational

    #region Convert to object

    /// <summary>Convert BigRational to array.</summary>
    /// <returns>An array of 2 BigIntegers, equal to the numerator and denominator.</returns>
    public readonly BigInteger[] ToArray()
    {
        return new[] { Numerator, Denominator };
    }

    /// <summary>Convert BigRational to tuple.</summary>
    /// <returns>A tuple with 2 BigIntegers, equal to the numerator and denominator.</returns>
    public readonly (BigInteger, BigInteger) ToTuple()
    {
        return (Numerator, Denominator);
    }

    #endregion Convert to object

    #region TryConvert methods

    /// <inheritdoc/>
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigRational result)
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

            case BigDecimal:
                result = (BigDecimal)(object)value;
                return true;

            default:
                // Unsupported type.
                result = 0;
                return false;
        }
    }

    /// <inheritdoc/>
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigRational result)
        where TOther : INumberBase<TOther>
    {
        // No saturation needed, as BigRational does not specify a min or max value.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc/>
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigRational result)
        where TOther : INumberBase<TOther>
    {
        // No truncation needed, as BigRational isn't an integer type.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc/>
    public static bool TryConvertToChecked<TOther>(BigRational value, out TOther result)
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

            case BigDecimal:
                result = (TOther)(object)(BigDecimal)value;
                return true;

            default:
                // Unsupported type.
                return false;
        }
    }

    /// <inheritdoc/>
    public static bool TryConvertToSaturating<TOther>(BigRational value, out TOther result)
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

            case BigDecimal:
                result = (TOther)(object)(BigDecimal)value;
                return true;

            default:
                // Unsupported type.
                return false;
        }
    }

    /// <inheritdoc/>
    public static bool TryConvertToTruncating<TOther>(BigRational value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        return TryConvertToChecked(value, out result);
    }

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>
    /// Cast standard binary floating point value to BigRational.
    /// This can be done exactly.
    /// </summary>
    public static BigRational ConvertFromFloatingPoint<T>(T x) where T : IFloatingPointIeee754<T>
    {
        // Check the value can be converted.
        if (!T.IsFinite(x))
        {
            throw new InvalidCastException("Cannot cast NaN or ±∞ to BigRational.");
        }

        // Handle zero.
        if (x == T.Zero) return Zero;

        // Get the parts of the floating point value.
        var (signBit, expBits, fracBits) = x.Disassemble();

        // Convert the fraction bits to a denominator.
        var nFracBits = XFloatingPoint.GetNumFracBits<T>();
        if (T.IsNormal(x))
        {
            // Set the top bit.
            fracBits |= 1ul << nFracBits;
        }
        BigInteger num = fracBits;

        // Apply the sign.
        if (signBit == 1)
        {
            num = -num;
        }

        // Apply the exponent.
        BigInteger den = 1;
        var maxExp = XFloatingPoint.GetMaxExp<T>();
        var exp = (short)(expBits - maxExp - nFracBits);
        var pow = BigInteger.Pow(2, XShort.Abs(exp));
        if (exp < 0)
        {
            den = pow;
        }
        else
        {
            num *= pow;
        }

        // Construct and return the new value.
        return new BigRational(num, den);
    }

    #endregion Helper methods
}
