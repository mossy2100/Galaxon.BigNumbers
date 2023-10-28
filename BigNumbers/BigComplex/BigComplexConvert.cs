using System.Numerics;
using Galaxon.Core.Numbers;
using Galaxon.Core.Types;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Casts operators to BigComplex

    /// <summary>
    /// Implicit cast from sbyte to BigComplex.
    /// </summary>
    /// <param name="n">The sbyte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(sbyte n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from byte to BigDecimals.
    /// </summary>
    /// <param name="n">The byte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(byte n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from short to BigDecimals.
    /// </summary>
    /// <param name="n">The short value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(short n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from ushort to BigDecimals.
    /// </summary>
    /// <param name="n">The ushort value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ushort n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from int to BigDecimals.
    /// </summary>
    /// <param name="n">The int value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(int n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from uint to BigDecimals.
    /// </summary>
    /// <param name="n">The uint value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(uint n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from long to BigDecimals.
    /// </summary>
    /// <param name="n">The long value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(long n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from ulong to BigDecimals.
    /// </summary>
    /// <param name="n">The ulong value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ulong n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from Int128 to BigDecimals.
    /// </summary>
    /// <param name="n">The Int128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Int128 n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from UInt128 to BigDecimals.
    /// </summary>
    /// <param name="n">The UInt128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(UInt128 n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from BigInteger to BigDecimals.
    /// </summary>
    /// <param name="n">The BigInteger value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigInteger n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from Half to BigDecimals.
    /// </summary>
    /// <param name="n">The Half value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Half n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from float to BigDecimals.
    /// </summary>
    /// <param name="n">The float value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(float n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from double to BigDecimals.
    /// </summary>
    /// <param name="n">The double value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(double n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from decimal to BigDecimals.
    /// </summary>
    /// <param name="n">The decimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(decimal n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from BigDecimals to BigComplex.
    /// </summary>
    /// <param name="n">The BigDecimals value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigDecimal n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from BigRational to BigComplex.
    /// </summary>
    /// <param name="n">The BigRational value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigRational n)
    {
        return new BigComplex((BigDecimal)n, 0);
    }

    /// <summary>
    /// Implicit cast from Complex to BigComplex.
    /// </summary>
    /// <param name="z">The Complex value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Complex z)
    {
        return new BigComplex(z.Real, z.Imaginary);
    }

    #endregion Casts operators to BigComplex

    #region Cast operators from BigComplex

    /// <summary>
    /// Explicit cast of BigComplex to a Complex.
    /// </summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent Complex value.</returns>
    public static explicit operator Complex(BigComplex z)
    {
        return new Complex((double)z.Real, (double)z.Imaginary);
    }

    #endregion Cast operators from BigComplex

    #region Methods that convert to an object.

    /// <summary>
    /// Convert BigComplex to array.
    /// </summary>
    /// <returns>The equivalent array.</returns>
    public readonly BigDecimal[] ToArray()
    {
        return new[] { Real, Imaginary };
    }

    /// <summary>
    /// Convert BigComplex to tuple.
    /// </summary>
    /// <returns>The equivalent tuple.</returns>
    public readonly (BigDecimal, BigDecimal) ToTuple()
    {
        return (Real, Imaginary);
    }

    #endregion Methods that convert to an object.

    #region TryConvert methods

    /// <inheritdoc />
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = Zero;

        // Check for supported type.
        if (!IsTypeSupported(typeof(TOther)))
        {
            return false;
        }

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

    /// <inheritdoc />
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        // No saturation needed, as BigDecimals does not specify a min or max value.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc />
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        // No truncation needed, as BigDecimals isn't an integer type.
        return TryConvertFromChecked(value, out result);
    }

    /// <inheritdoc />
    public static bool TryConvertToChecked<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Check for supported type.
        if (!IsTypeSupported(typeof(TOther)))
        {
            return false;
        }

        // Complex is the only supported type that can handle both real and imaginary parts.
        if (result is Complex)
        {
            // Cast to Complex.
            result = (TOther)(object)value;
            return true;
        }

        // If the number is real, convert the real part use the BigDecimals method.
        if (IsRealNumber(value))
        {
            return BigDecimal.TryConvertToChecked(value.Real, out result);
        }

        // Otherwise, we'll assume TOther is supported, but value is outside its valid range.
        throw new OverflowException(
            $"The value is outside the valid range for the {typeof(TOther).Name} type.");
    }

    /// <inheritdoc />
    public static bool TryConvertToSaturating<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // Set a default result.
        result = TOther.Zero;

        // Check for supported type.
        if (!IsTypeSupported(typeof(TOther)))
        {
            return false;
        }

        // Complex is the only supported type that can handle both real and imaginary parts.
        if (result is Complex)
        {
            // Convert real and imaginary parts to doubles, with saturation.
            BigDecimal.TryConvertToSaturating(value.Real, out double real);
            BigDecimal.TryConvertToSaturating(value.Imaginary, out double imag);
            result = (TOther)(object)(new Complex(real, imag));
            return true;
        }

        // For other number types, we'll just ignore the imaginary part.
        return BigDecimal.TryConvertToSaturating(value.Real, out result);
    }

    /// <inheritdoc />
    public static bool TryConvertToTruncating<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        // TODO This should work fine, need to test.
        return TryConvertToChecked(value, out result);
    }

    #endregion TryConvert methods

    #region Helper methods

    /// <summary>
    /// Which types are supported for conversions to and from BigComplex.
    /// </summary>
    /// <param name="type">A type.</param>
    /// <returns>If the type is supported.</returns>
    private static bool IsTypeSupported(Type type)
    {
        return XNumber.IsStandardNumberType(type) || type == typeof(BigRational)
            || type == typeof(BigDecimal);
    }

    #endregion Helper methods
}
