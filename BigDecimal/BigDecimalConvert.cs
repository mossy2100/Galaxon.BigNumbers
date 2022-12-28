using System.Globalization;
using System.Numerics;

namespace Galaxon.Numerics.Types;

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
    /// See operator BigDecimal(double) below for notes.
    /// </summary>
    public static implicit operator BigDecimal(float n)
    {
        if (float.IsInfinity(n) || float.IsNaN(n))
        {
            throw new InvalidCastException("Cannot convert ±∞ or NaN to BigDecimal.");
        }

        return Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));
    }

    /// <summary>
    /// For the floating point types I've found that using Parse(ToString()) is much simpler
    /// than attempting to decompose the values into bits and calculate a decimal value.
    /// Besides, because the division operation currently uses a cast to double in order to find
    /// an initial estimate, infinite recursion occurs. And division is needed to calculate a
    /// decimal value from the bits in a floating point.
    /// </summary>
    public static implicit operator BigDecimal(double n)
    {
        if (double.IsInfinity(n) || double.IsNaN(n))
        {
            throw new InvalidCastException("Cannot convert ±∞ or NaN to BigDecimal.");
        }

        return Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));
    }

    /// <summary>
    /// Cast a decimal to a BigDecimal.
    /// Any decimal value can be cast to a BigDecimal precisely, without loss of information.
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
    /// Explicit cast of a BigRational to a BigDecimal.
    /// This case operation has to be explicit as there could be loss of information due to the
    /// limit on the number of significant figures in the result of the division.
    /// </summary>
    public static explicit operator BigDecimal(BigRational n) =>
        (BigDecimal)n.Numerator / n.Denominator;

    /// <summary>
    /// Explicit cast of a Complex to a BigDecimal.
    /// </summary>
    public static explicit operator BigDecimal(Complex n)
    {
        if (n.Imaginary != 0)
        {
            throw new InvalidCastException();
        }

        return (BigDecimal)n.Real;
    }

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
        BigDecimal trunc = Trunc(bd);
        trunc.ShiftBy(trunc.Exponent);
        return trunc.Significand;
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a float.
    /// </summary>
    public static explicit operator float(BigDecimal bd)
    {
        if (float.TryParse(bd.ToString("G27"), out float f))
        {
            return f;
        }
        throw new OverflowException();
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a double.
    /// </summary>
    public static explicit operator double(BigDecimal bd)
    {
        if (double.TryParse(bd.ToString("G56"), out double d))
        {
            return d;
        }
        throw new OverflowException();
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a decimal.
    /// </summary>
    public static explicit operator decimal(BigDecimal bd)
    {
        if (decimal.TryParse(bd.ToString("G30"), out decimal d))
        {
            return d;
        }
        throw new OverflowException();
    }

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

    /// <summary>
    /// Explicit cast of a BigDecimal to a Complex.
    /// TODO test because it may be that the cast to double returns ±∞ and doesn't throw an
    /// exception, and this might be accepted by the Complex constructor.
    /// </summary>
    /// <exception cref="OverflowException">If the BigDecimal is outside the range for double.</exception>
    public static explicit operator Complex(BigDecimal bd) =>
        new ((double)bd, 0);

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
            float n => (BigDecimal)n,
            double n => (BigDecimal)n,
            decimal n => (BigDecimal)n,
            BigInteger n => (BigDecimal)n,
            BigRational n => (BigDecimal)n,
            _ => null
        };

        if (!tmp.HasValue)
        {
            // Unsupported type.
            result = Zero;
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
            float => float.MinValue,
            double => double.MinValue,
            decimal => decimal.MinValue,
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
            float => float.MaxValue,
            double => double.MaxValue,
            decimal => decimal.MaxValue,
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
        Significand != 0;

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
        (char)(ushort)this;

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
