using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics;

public partial struct BigDecimal : IConvertible
{
    #region Cast operators to BigDecimal

    public static implicit operator BigDecimal(sbyte n) =>
        new (n);

    public static implicit operator BigDecimal(byte n) =>
        new (n);

    public static implicit operator BigDecimal(short n) =>
        new (n);

    public static implicit operator BigDecimal(ushort n) =>
        new (n);

    public static implicit operator BigDecimal(int n) =>
        new (n);

    public static implicit operator BigDecimal(uint n) =>
        new (n);

    public static implicit operator BigDecimal(long n) =>
        new (n);

    public static implicit operator BigDecimal(ulong n) =>
        new (n);

    public static implicit operator BigDecimal(BigInteger n) =>
        new (n);

    /// <summary>
    /// Cast a decimal to a BigDecimal.
    ///
    /// The cast is implicit because any decimal value can be cast to a BigDecimal exactly, without
    /// loss of information. However, rounding off using Round() or RoundSigFigs() can cause
    /// information loss.
    ///
    /// We don't need to use Parse() or division operations here, because the base is decimal.
    /// We can just extract the parts of the decimal from the bits and construct a BigDecimal from
    /// those. This method should be faster than using ToString() and Parse().
    /// </summary>
    public static implicit operator BigDecimal(decimal n)
    {
        int[] parts = decimal.GetBits(n);

        // Get the sign and scale from the bits.
        int sign = (parts[3] & 0x80000000) == 0 ? 1 : -1;
        byte scale = (byte)((parts[3] >> 16) & 0x7F);

        // Calculate the significand.
        BigInteger sig = 0;
        BigInteger mult = 1;
        for (int i = 0; i < 3; i++)
        {
            sig += (uint)parts[i] * mult;
            mult *= 0x100000000;
        }

        return new BigDecimal(sign * sig, -scale);
    }

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
        (byte signBit, ushort expBits, ulong fracBits) = n.Disassemble();

        // Check for ±0.
        if (expBits == 0 && fracBits == 0)
        {
            return 0;
        }

        // Check if the number is normal or subnormal.
        bool isSubnormal = expBits == 0;

        // Get sign.
        int sign = signBit == 1 ? -1 : 1;

        // Get some information about the type.
        byte nFracBits = XFloatingPoint.GetNumFracBits<T>();
        short maxExp = XFloatingPoint.GetMaxExp<T>();

        // Get the significand.
        // The bit values are taken to have the value 1..2^(nFracBits - 1) and the exponent is
        // correspondingly shifted. Doing this avoids division operations.
        BigDecimal sig = 0;
        BigDecimal pow = 1;
        for (int i = 0; i < nFracBits; i++)
        {
            bool set = (fracBits & (1ul << i)) != 0;
            if (set)
            {
                sig += pow;
            }
            pow *= 2;
        }

        // One more addition for normal numbers.
        if (!isSubnormal)
        {
            sig += pow;
        }

        // Get the power of 2.
        int exp = (isSubnormal ? 1 : expBits) - maxExp - nFracBits;

        // Calculate the result.
        return sign * sig * Exp2(exp);
    }

    /// <summary>
    /// Convert float to BigDecimal.
    /// NB: The resulting BigDecimal value is exactly the value encoded by the float.
    /// However, floats only approximate decimal values and it's possible that only the first 6-9
    /// digits are valid in terms of the intended value.
    /// So, you may need to use RoundSigFigs() to get the value you really want, e.g.
    /// <code>
    /// BigDecimal bd = BigDecimal.RoundSigFigs(1.2345f, FloatMaxSigFigs);
    /// </code>
    /// </summary>
    public static implicit operator BigDecimal(float n) =>
        ConvertFromFloatingPoint(n);

    /// <summary>
    /// Convert double to BigDecimal.
    /// NB: The resulting BigDecimal value is exactly the value encoded by the double.
    /// However, doubles only approximate decimal values and it's possible that only the first 15-17
    /// digits are valid in terms of the intended value.
    /// So, you may need to use RoundSigFigs() to get the value you really want, e.g.
    /// <code>
    /// BigDecimal bd = BigDecimal.RoundSigFigs(1.2345, DoubleMaxSigFigs);
    /// </code>
    /// </summary>
    public static implicit operator BigDecimal(double n) =>
        ConvertFromFloatingPoint(n);

    /// <summary>
    /// Cast of a BigRational to a BigDecimal.
    /// This cast operation has to be explicit as there could be loss of information due to the
    /// limit on the number of significant figures in the result of the division.
    /// </summary>
    public static explicit operator BigDecimal(BigRational n) =>
        (BigDecimal)n.Numerator / n.Denominator;

    #endregion Cast operators to BigDecimal

    #region Cast operators from BigDecimal

    /// <summary>
    /// Explicit cast of a BigDecimal to an sbyte.
    /// </summary>
    public static explicit operator sbyte(BigDecimal bd) =>
        (sbyte)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a byte.
    /// </summary>
    public static explicit operator byte(BigDecimal bd) =>
        (byte)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a short.
    /// </summary>
    public static explicit operator short(BigDecimal bd) =>
        (short)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a ushort.
    /// </summary>
    public static explicit operator ushort(BigDecimal bd) =>
        (ushort)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to an int.
    /// </summary>
    public static explicit operator int(BigDecimal bd) =>
        (int)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a uint.
    /// </summary>
    public static explicit operator uint(BigDecimal bd) =>
        (uint)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a long.
    /// </summary>
    public static explicit operator long(BigDecimal bd) =>
        (long)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a ulong.
    /// </summary>
    public static explicit operator ulong(BigDecimal bd) =>
        (ulong)(BigInteger)bd;

    /// <summary>
    /// Explicit cast of a BigDecimal to a BigInteger.
    /// </summary>
    public static explicit operator BigInteger(BigDecimal bd)
    {
        BigDecimal trunc = Truncate(bd);
        trunc.ShiftToExp(0);
        return trunc.Significand;
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a decimal.
    /// </summary>
    public static explicit operator decimal(BigDecimal bd)
    {
        // Check the value is within the supported range for decimal.
        if (bd < decimal.MinValue || bd > decimal.MaxValue)
        {
            throw new OverflowException();
        }

        // If the exponent is greater than 0, shift to exponent 0 to get the correct scale.
        if (bd.Exponent > 0)
        {
            bd.ShiftToExp(0);
        }

        // Get the scale and check it's within the valid range for decimal.
        int iScale = -bd.Exponent;
        if (iScale is < 0 or > DecimalMinSigFigs)
        {
            throw new OverflowException();
        }
        byte scale = (byte)iScale;

        // Get the sign.
        bool isNegative = bd.Significand < 0;

        // Get the bytes for the absolute value of the significand.
        byte[] sigBytes = BigInteger.Abs(bd.Significand).ToByteArray(true);

        // Check we have at most 12 bytes.
        if (sigBytes.Length > 12)
        {
            throw new OverflowException();
        }

        // Convert the bytes to the integers necessary to construct the decimal.
        int[] decInts = new int[3];
        for (int i = 0; i < 12; i++)
        {
            byte b = (i < sigBytes.Length) ? sigBytes[i] : (byte)0;
            decInts[i / 4] |= b << ((i % 4) * 8);
        }

        return new decimal(decInts[0], decInts[1], decInts[2], isNegative, scale);
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a Half.
    /// BigDecimal doesn't use a default precision for 'E', so all digits will be rendered in the
    /// call to ToString(), to produce the closest matching Half possible.
    /// </summary>
    /// <exception cref="OverflowException">
    /// The BigDecimal is outside the supported range for Half.
    /// </exception>
    public static explicit operator Half(BigDecimal bd) =>
        Half.Parse(bd.ToString("E"));

    /// <summary>
    /// Explicit cast of a BigDecimal to a float.
    /// I implemented a method to do this using maths and bits, but it takes much longer.
    /// BigDecimal doesn't use a default precision for 'E', so all digits will be rendered in the
    /// call to ToString(), to produce the closest matching float possible.
    /// </summary>
    /// <exception cref="OverflowException">
    /// The BigDecimal is outside the supported range for float.
    /// </exception>
    public static explicit operator float(BigDecimal bd) =>
        float.Parse(bd.ToString("E"));

    /// <summary>
    /// Explicit cast of a BigDecimal to a double.
    /// BigDecimal doesn't use a default precision for 'E', so all digits will be rendered in the
    /// call to ToString(), to produce the closest matching double possible.
    /// </summary>
    /// <exception cref="OverflowException">
    /// The BigDecimal is outside the supported range for double.
    /// </exception>
    public static explicit operator double(BigDecimal bd) =>
        double.Parse(bd.ToString("E"));

    /// <summary>
    /// Cast from BigDecimal to BigRational.
    /// This cast can be implicit because it can be done exactly, without loss of information, due
    /// to the use of BigIntegers inside BigRational.
    /// </summary>
    public static implicit operator BigRational(BigDecimal n) =>
        n.Exponent switch
        {
            // Zero exponent.
            0 => new BigRational(n.Significand),

            // Positive exponent.
            > 0 => new BigRational(n.Significand * BigInteger.Pow(10, n.Exponent)),

            // Negative exponent.
            < 0 => new BigRational(n.Significand, BigInteger.Pow(10, -n.Exponent), true)
        };

    #endregion Cast operators from BigDecimal

    #region Conversion methods

    /// <inheritdoc />
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        BigDecimal? tmp = value switch
        {
            sbyte n => (BigDecimal)n,
            byte n => (BigDecimal)n,
            short n => (BigDecimal)n,
            ushort n => (BigDecimal)n,
            int n => (BigDecimal)n,
            uint n => (BigDecimal)n,
            long n => (BigDecimal)n,
            ulong n => (BigDecimal)n,
            BigInteger n => (BigDecimal)n,
            decimal n => (BigDecimal)n,
            float n => (BigDecimal)n,
            double n => (BigDecimal)n,
            BigRational n => (BigDecimal)n,
            _ => null
        };

        if (!tmp.HasValue)
        {
            // Unsupported type.
            result = 0;
            return false;
        }

        // Success.
        result = tmp.Value;
        return true;
    }

    /// <inheritdoc />
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther> =>
        TryConvertFromChecked(value, out result);

    /// <inheritdoc />
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther> =>
        TryConvertFromChecked(value, out result);

    private static (BigDecimal? min, BigDecimal? max) GetRange<TOther>(TOther n)
        where TOther : INumberBase<TOther>
    {
        // Declare a temporary variable for use in the switch expressions.
        BigDecimal? min = n switch
        {
            sbyte => sbyte.MinValue,
            byte => byte.MinValue,
            short => short.MinValue,
            ushort => ushort.MinValue,
            int => int.MinValue,
            uint => uint.MinValue,
            long => long.MinValue,
            ulong => ulong.MinValue,
            decimal => decimal.MinValue,
            float => (BigDecimal)float.MinValue,
            double => (BigDecimal)double.MinValue,
            _ => null
        };
        BigDecimal? max = n switch
        {
            sbyte => sbyte.MaxValue,
            byte => byte.MaxValue,
            short => short.MaxValue,
            ushort => ushort.MaxValue,
            int => int.MaxValue,
            uint => uint.MaxValue,
            long => long.MaxValue,
            ulong => ulong.MaxValue,
            decimal => decimal.MaxValue,
            float => (BigDecimal)float.MaxValue,
            double => (BigDecimal)double.MaxValue,
            _ => null
        };
        return (min, max);
    }

    /// <inheritdoc />
    public static bool TryConvertToChecked<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Check types with unlimited range.
        if (result is BigDecimal or BigInteger or BigRational)
        {
            result = (TOther)(object)value;
            return true;
        }

        // Get the min and max values for the result type.
        (BigDecimal? min, BigDecimal? max) = GetRange(result);

        // Check for unsupported type.
        if (!min.HasValue || !max.HasValue)
        {
            return false;
        }

        // Check for underflow.
        if (value < min.Value)
        {
            switch (result)
            {
                case float:
                    result = (TOther)(object)float.NegativeInfinity;
                    return true;

                case double:
                    result = (TOther)(object)double.NegativeInfinity;
                    return true;

                default:
                    throw new OverflowException(
                        $"Value {value} is less than the minimum value for {result.GetType()}");
            }
        }

        // Check for overflow.
        if (value > max.Value)
        {
            switch (result)
            {
                case float:
                    result = (TOther)(object)float.PositiveInfinity;
                    return true;

                case double:
                    result = (TOther)(object)double.PositiveInfinity;
                    return true;

                default:
                    throw new OverflowException(
                        $"Value {value} is greater than the maximum value for {result.GetType()}");
            }
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
        if (result is BigDecimal or BigInteger or BigRational)
        {
            result = (TOther)(object)value;
            return true;
        }

        // Get the min and max values for the result type.
        (BigDecimal? min, BigDecimal? max) = GetRange(result);

        // Check for unsupported type.
        if (!min.HasValue || !max.HasValue)
        {
            return false;
        }

        if (value < min.Value)
        {
            // Value is less than the minimum value for TOther.
            result = (TOther)(object)min.Value;
        }
        else if (value > max.Value)
        {
            // Value is greater than the maximum value for TOther.
            result = (TOther)(object)max.Value;
        }
        else
        {
            // Value is within range for the type.
            result = (TOther)(object)value;
        }

        return true;
    }

    /// <inheritdoc />
    public static bool TryConvertToTruncating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Check types with unlimited range.
        if (result is BigDecimal or BigInteger or BigRational)
        {
            result = (TOther)(object)value;
            return true;
        }

        (BigDecimal? min, BigDecimal? max) = GetRange(result);

        // Signed types.
        if (min < 0)
        {
            result = (TOther)(object)(value % -min);
            return true;
        }

        // Unsigned types.
        if (max != null)
        {
            result = (TOther)(object)(value % max + 1);
            return true;
        }

        return false;
    }

    #endregion Conversion methods

    #region IConvertible methods

    /// <inheritdoc />
    public TypeCode GetTypeCode() =>
        TypeCode.Object;

    /// <inheritdoc />
    public bool ToBoolean(IFormatProvider? provider) =>
        throw new InvalidCastException("Converting from BigDecimal to bool is unsupported.");

    /// <inheritdoc />
    public sbyte ToSByte(IFormatProvider? provider) =>
        (sbyte)this;

    /// <inheritdoc />
    public byte ToByte(IFormatProvider? provider) =>
        (byte)this;

    /// <inheritdoc />
    public short ToInt16(IFormatProvider? provider) =>
        (short)this;

    /// <inheritdoc />
    public ushort ToUInt16(IFormatProvider? provider) =>
        (ushort)this;

    /// <inheritdoc />
    public int ToInt32(IFormatProvider? provider) =>
        (int)this;

    /// <inheritdoc />
    public uint ToUInt32(IFormatProvider? provider) =>
        (uint)this;

    /// <inheritdoc />
    public long ToInt64(IFormatProvider? provider) =>
        (long)this;

    /// <inheritdoc />
    public ulong ToUInt64(IFormatProvider? provider) =>
        (ulong)this;

    /// <inheritdoc />
    public float ToSingle(IFormatProvider? provider) =>
        (float)this;

    /// <inheritdoc />
    public double ToDouble(IFormatProvider? provider) =>
        (double)this;

    /// <inheritdoc />
    public decimal ToDecimal(IFormatProvider? provider) =>
        (decimal)this;

    /// <inheritdoc />
    public char ToChar(IFormatProvider? provider) =>
        throw new InvalidCastException("Converting from BigDecimal to char is unsupported.");

    /// <inheritdoc />
    public string ToString(IFormatProvider? provider) =>
        ToString("G", provider);

    /// <inheritdoc />
    public DateTime ToDateTime(IFormatProvider? provider) =>
        throw new InvalidCastException("Converting from BigDecimal to DateTime is unsupported.");

    /// <inheritdoc />
    public object ToType(Type conversionType, IFormatProvider? provider) =>
        throw new InvalidCastException(
            $"Converting from BigDecimal to {conversionType.Name} is unsupported.");

    #endregion IConvertible methods
}
