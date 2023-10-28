using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Operators for casting from other number types to BigRational

    /// <summary>Cast from sbyte to BigRational.</summary>
    public static implicit operator BigRational(sbyte num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from byte to BigRational.</summary>
    public static implicit operator BigRational(byte num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from short to BigRational.</summary>
    public static implicit operator BigRational(short num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from ushort to BigRational.</summary>
    public static implicit operator BigRational(ushort num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from int to BigRational.</summary>
    public static implicit operator BigRational(int num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from uint to BigRational.</summary>
    public static implicit operator BigRational(uint num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from long to BigRational.</summary>
    public static implicit operator BigRational(long num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from ulong to BigRational.</summary>
    public static implicit operator BigRational(ulong num)
    {
        return new BigRational(num);
    }

    /// <summary>
    /// Implicitly cast an Int128 to a BigRational.
    /// </summary>
    public static implicit operator BigRational(Int128 num)
    {
        return new BigRational(num);
    }

    /// <summary>
    /// Implicitly cast a UInt128 to a BigRational.
    /// </summary>
    public static implicit operator BigRational(UInt128 num)
    {
        return new BigRational(num);
    }

    /// <summary>
    /// Cast from Half to BigRational.
    /// This can be done exactly.
    /// </summary>
    public static implicit operator BigRational(Half x)
    {
        return ConvertFromFloatingPoint(x);
    }

    /// <summary>
    /// Cast from float to BigRational.
    /// This can be done exactly.
    /// </summary>
    public static implicit operator BigRational(float x)
    {
        return ConvertFromFloatingPoint(x);
    }

    /// <summary>
    /// Cast from double to BigRational.
    /// This can be done exactly.
    /// </summary>
    public static implicit operator BigRational(double x)
    {
        return ConvertFromFloatingPoint(x);
    }

    /// <summary>
    /// Cast from decimal to BigRational.
    /// This can be done exactly.
    /// </summary>
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

    /// <summary>Cast from BigInteger to BigRational.</summary>
    public static implicit operator BigRational(BigInteger num)
    {
        return new BigRational(num);
    }

    /// <summary>Cast from BigDecimal to BigRational.</summary>
    public static implicit operator BigRational(BigDecimal n)
    {
        return n.Exponent switch
        {
            // Zero exponent.
            0 => new BigRational(n.Significand),

            // Positive exponent.
            > 0 => new BigRational(n.Significand * BigInteger.Pow(10, n.Exponent)),

            // Negative exponent.
            < 0 => new BigRational(n.Significand, BigInteger.Pow(10, -n.Exponent)),
        };
    }

    #endregion Operators for casting from other number types to BigRational

    #region Operators for casting from BigRational to other number types

    /// <summary>Cast from BigRational to sbyte.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of sbyte.</exception>
    public static explicit operator sbyte(BigRational br)
    {
        return (sbyte)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to byte.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of byte.</exception>
    public static explicit operator byte(BigRational br)
    {
        return (byte)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to short.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of short.</exception>
    public static explicit operator short(BigRational br)
    {
        return (short)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to ushort.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of
    /// ushort.</exception>
    public static explicit operator ushort(BigRational br)
    {
        return (ushort)(BigDecimal)br;
    }

    /// <summary>Explicitly cast a BigRational to an int.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of int.</exception>
    public static explicit operator int(BigRational br)
    {
        return (int)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to uint.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of uint.</exception>
    public static explicit operator uint(BigRational br)
    {
        return (uint)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to long.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of long.</exception>
    public static explicit operator long(BigRational br)
    {
        return (long)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to ulong.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of ulong.</exception>
    public static explicit operator ulong(BigRational br)
    {
        return (ulong)(BigDecimal)br;
    }

    /// <summary>
    /// Explicitly cast a BigRational to an Int128.
    /// </summary>
    /// <exception cref="OverflowException">If the result is outside the range of Int128.</exception>
    public static explicit operator Int128(BigRational br)
    {
        return (Int128)(BigDecimal)br;
    }

    /// <summary>
    /// Cast from BigRational to UInt.28.
    /// </summary>
    /// <exception cref="OverflowException">If the result is outside the range of UInt128.</exception>
    public static explicit operator UInt128(BigRational br)
    {
        return (UInt128)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to Half.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of Half.</exception>
    public static explicit operator Half(BigRational br)
    {
        return (Half)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to float.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of float.</exception>
    public static explicit operator float(BigRational br)
    {
        return (float)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to double.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of double.</exception>
    public static explicit operator double(BigRational br)
    {
        return (double)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to decimal.</summary>
    /// <exception cref="OverflowException">If the result is outside the range of decimal.</exception>
    public static explicit operator decimal(BigRational br)
    {
        return (decimal)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to BigInteger.</summary>
    public static explicit operator BigInteger(BigRational br)
    {
        return (BigInteger)(BigDecimal)br;
    }

    /// <summary>Cast from BigRational to BigDecimal.</summary>
    public static explicit operator BigDecimal(BigRational n)
    {
        return (BigDecimal)n.Numerator / (BigDecimal)n.Denominator;
    }

    #endregion Operators for casting from BigRational to other number types

    #region Methods that convert to an object

    /// <summary>Convert BigRational to array.</summary>
    /// <returns>The equivalent array.</returns>
    public readonly BigInteger[] ToArray()
    {
        return new[] { Numerator, Denominator };
    }

    /// <summary>Convert BigRational to tuple.</summary>
    /// <returns>The equivalent tuple.</returns>
    public readonly (BigInteger, BigInteger) ToTuple()
    {
        return (Numerator, Denominator);
    }

    #endregion Methods that convert to an object

    #region Helper methods

    /// <summary>
    /// Implicitly cast a standard binary floating point value to a BigRational.
    /// This can be done exactly.
    /// </summary>
    public static BigRational ConvertFromFloatingPoint<T>(T x) where T : IFloatingPointIeee754<T>
    {
        // Check the value can be converted.
        if (!T.IsFinite(x))
        {
            throw new InvalidCastException("Cannot cast NaN or ±∞ to a BigRational.");
        }

        // Handle zero.
        if (x == T.Zero) return Zero;

        // Get the parts of the floating point value.
        var (signBit, expBits, fracBits) = x.Disassemble<T>();

        // Convert the fraction bits to a denominator.
        var nFracBits = XFloatingPoint.GetNumFracBits<T>();
        if (T.IsNormal(x))
        {
            // Set the top bit.
            fracBits |= 1uL << nFracBits;
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
