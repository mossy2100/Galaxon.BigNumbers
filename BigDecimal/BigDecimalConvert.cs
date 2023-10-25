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

    /// <summary>
    /// Implicit cast from Half to BigDecimal.
    /// NB: The resulting BigDecimal value is exactly the value encoded by the Half.
    /// However, since Halfs only approximate decimal values, it's possible that only the first few
    /// digits are valid in terms of the intended value.
    /// Therefore, you may need to use RoundSigFigs() to get the value you really want.
    /// </summary>
    public static implicit operator BigDecimal(Half n)
    {
        return ConvertFromFloatingPoint(n);
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
        return ConvertFromFloatingPoint(n);
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
        return ConvertFromFloatingPoint(n);
    }

    /// <summary>
    /// Explicit cast from BigRational to BigDecimal.
    /// This cast operation has to be explicit as there could be loss of information due to the
    /// limit on the number of significant figures in the result of the division.
    /// </summary>
    public static explicit operator BigDecimal(BigRational n)
    {
        return (BigDecimal)n.Numerator / n.Denominator;
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

    /// <summary>
    /// Explicit cast from BigDecimal to Half.
    /// BigDecimal doesn't use a default precision for the "E" format specifier, so all digits will
    /// be rendered in the call to ToString(). This will produce the closest matching Half possible.
    /// This method will not throw an OverflowException, but will return ±∞ for a value outside the
    /// valid range for Half.
    /// </summary>
    public static explicit operator Half(BigDecimal bd)
    {
        return Half.Parse(bd.ToString("E"));
    }

    /// <summary>
    /// Explicit cast from BigDecimal to float.
    /// (I implemented a method to do this using maths and bits, but it takes much longer.)
    /// BigDecimal doesn't use a default precision for the "E" format specifier, so all digits will
    /// be rendered in the call to ToString(). This will produce the closest matching float
    /// possible.
    /// This method will not throw an OverflowException, but will return ±∞ for a value outside the
    /// valid range for float.
    /// </summary>
    public static explicit operator float(BigDecimal bd)
    {
        return float.Parse(bd.ToString("E"));
    }

    /// <summary>
    /// Explicit cast from BigDecimal to double.
    /// BigDecimal doesn't use a default precision for the "E" format specifier, so all digits will
    /// be rendered in the call to ToString(). This will produce the closest matching double
    /// possible.
    /// This method will not throw an OverflowException, but will return ±∞ for a value outside the
    /// valid range for double.
    /// </summary>
    public static explicit operator double(BigDecimal bd)
    {
        return double.Parse(bd.ToString("E"));
    }

    /// <summary>
    /// Implicit cast from BigDecimal to BigRational.
    /// This cast operation can be implicit because it can be done exactly, without loss of
    /// information, due to the use of BigIntegers inside BigRational.
    /// </summary>
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

    #endregion Cast operators from BigDecimal

    #region TryConvert methods

    /// <inheritdoc />
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        // See if we can cast it.
        if (XReflection.CanCast<TOther, BigDecimal>())
        {
            result = (BigDecimal)(object)value;
            return true;
        }

        // Unsupported type.
        result = 0;
        return false;
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
        // No truncation needed as BigDecimal isn't an integer type.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc />
    public static bool TryConvertToChecked<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Check types with unlimited range.
        if (result is BigInteger or BigRational)
        {
            result = (TOther)(object)value;
            return true;
        }

        // Get the min and max values for the result type.
        var (min, max) = XNumber.GetRange<TOther>();

        // If the type doesn't have MinValue and MaxValue properties, it's unsupported.
        if (min is null || max is null)
        {
            return false;
        }

        // Check for negative overflow.
        if (value < (BigDecimal)(object)min)
        {
            // Check if the type supports negative infinity.
            var negInf = XNumber.GetNegativeInfinity<TOther>();

            // If not, overflow.
            if (negInf is null)
            {
                throw new OverflowException(
                    $"Value {value} is less than the minimum value for {result.GetType()}");
            }

            // Return negative infinity.
            result = negInf;
        }

        // Check for positive overflow.
        if (value > (BigDecimal)(object)max)
        {
            // Check if the type supports positive infinity.
            var posInf = XNumber.GetPositiveInfinity<TOther>();

            // If not, overflow.
            if (posInf is null)
            {
                throw new OverflowException(
                    $"Value {value} is greater than the maximum value for {result.GetType()}");
            }

            // Return positive infinity.
            result = posInf;
        }

        // Value is within range for the type.
        result = (TOther)(object)value;
        return true;
    }

    /// <inheritdoc />
    public static bool TryConvertToSaturating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Check types with unlimited range.
        if (result is BigInteger or BigRational)
        {
            result = (TOther)(object)value;
            return true;
        }

        // Get the min and max values for the result type.
        var (min, max) = XNumber.GetRange<TOther>();

        // If the type doesn't have MinValue and MaxValue properties, it's unsupported.
        if (min is null || max is null)
        {
            return false;
        }

        // Check for overflow.
        if (value < (BigDecimal)(object)min)
        {
            // Overflow. Value is less than the minimum value for TOther. Return MinValue.
            result = min;
        }
        else if (value > (BigDecimal)(object)max)
        {
            // Overflow. Value is greater than the maximum value for TOther. Return MaxValue.
            result = max;
        }
        else
        {
            // Value is within range for the type.
            result = (TOther)(object)value;
        }

        return true;
    }

    /// <inheritdoc />
    /// <see cref="BigDecimal.explicit operator BigInteger(BigDecimal)"/>
    public static bool TryConvertToTruncating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Convert with overflow check.
        // Integers will be truncated by the conversion, in the cast to BigInteger (see ref above).
        // The documentation doesn't mention throwing overflow exceptions for
        // TryConvertToTruncating(), so I'm making an assumption here, but it seems logical.
        // TODO Test behaviour with built-in types.
        return TryConvertToChecked(value, out result);
    }

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>
    /// Private method to convert a floating point value (float or double) to a BigDecimal.
    /// </summary>
    /// <exception cref="InvalidCastException"></exception>
    private static BigDecimal ConvertFromFloatingPoint<T>(T n) where T : IFloatingPoint<T>
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
            var set = (fracBits & 1ul << i) != 0;
            if (set)
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

    #endregion Helper methods
}
