using System.Globalization;
using System.Numerics;
using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics.Types;

public partial struct BigDecimal
{
    #region Implicit cast operators to BigDecimal

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

    public static implicit operator BigDecimal(Int128 n) =>
        new (n);

    public static implicit operator BigDecimal(UInt128 n) =>
        new (n);

    public static implicit operator BigDecimal(BigInteger n) =>
        new (n);

    public static implicit operator BigDecimal(Half n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    public static implicit operator BigDecimal(float n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    public static implicit operator BigDecimal(double n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    public static implicit operator BigDecimal(decimal n) =>
        Parse(n.ToString("G", NumberFormatInfo.InvariantInfo));

    #endregion Implicit cast operators to BigDecimal

    #region Explicit cast operators to BigDecimal

    /// <summary>
    /// Explicit cast of a BigRational to a BigDecimal.
    /// This case operation has to be explicit as there could be loss of information due to the
    /// limit on the number of significant figures in the result of the division.
    /// </summary>
    public static explicit operator BigDecimal(BigRational n) =>
        (BigDecimal)n.Numerator / n.Denominator;

    #endregion Explicit cast operators to BigDecimal

    #region Explicit cast operators from BigDecimal

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
    /// Explicit cast of a BigDecimal to an int.
    /// TODO Update to not use strings.
    /// </summary>
    public static explicit operator int(BigDecimal bd)
    {
        if (int.TryParse(bd.ToString("F0"), out int i))
        {
            return i;
        }
        throw new OverflowException("Value is outside the int range.");
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a Half.
    /// TODO Update to not use strings.
    /// </summary>
    public static explicit operator Half(BigDecimal bd)
    {
        if (Half.TryParse(bd.ToString("G17"), out Half d))
        {
            return d;
        }
        throw new OverflowException("Value is outside the Half range.");
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a double.
    /// TODO Update to not use strings.
    /// </summary>
    public static explicit operator double(BigDecimal bd)
    {
        if (double.TryParse(bd.ToString("G17"), out double d))
        {
            return d;
        }
        throw new OverflowException("Value is outside the double range.");
    }

    /// <summary>
    /// Explicit cast of a BigDecimal to a decimal.
    /// TODO Update to not use strings.
    /// </summary>
    public static explicit operator decimal(BigDecimal bd)
    {
        if (decimal.TryParse(bd.ToString("G29"), out decimal m))
        {
            return m;
        }
        throw new OverflowException("Value is outside the decimal range.");
    }

    public static explicit operator BigRational(BigDecimal n) =>
        n.Exponent switch
        {
            // Zero exponent.
            0 => new BigRational(n.Significand),

            // Positive exponent.
            > 0 => new BigRational(n.Significand * BigInteger.Pow(10, n.Exponent)),

            // Negative exponent.
            < 0 => new BigRational(n.Significand, BigInteger.Pow(10, -n.Exponent), true)
        };

    #endregion Explicit cast operators from BigDecimal

    #region Conversion methods

    /// <inheritdoc />
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigDecimal result)
        where TOther : INumberBase<TOther>
    {
        // Attempt to convert to BigDecimal.
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
            Int128 n => (BigDecimal)n,
            UInt128 n => (BigDecimal)n,
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
            Int128 => Int128.MinValue,
            UInt128 => UInt128.MinValue,
            Half => Half.MinValue,
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
            Int128 => Int128.MaxValue,
            UInt128 => UInt128.MaxValue,
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
                case Half:
                    result = (TOther)(object)Half.NegativeInfinity;
                    return true;

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
                case Half:
                    result = (TOther)(object)Half.PositiveInfinity;
                    return true;

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
}
