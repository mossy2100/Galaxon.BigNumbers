using System.Globalization;
using System.Numerics;

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
            // Failure.
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

    /// <inheritdoc />
    public static bool TryConvertToChecked<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther> =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static bool TryConvertToSaturating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther> =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public static bool TryConvertToTruncating<TOther>(BigDecimal value, out TOther result)
        where TOther : INumberBase<TOther> =>
        throw new NotImplementedException();

    #endregion Conversion methods
}
