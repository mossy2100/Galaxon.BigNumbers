using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Casts to BigComplex (all implicit)

    /// <summary>
    /// Implicit cast from Complex to BigComplex.
    /// </summary>
    /// <param name="z">The Complex value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Complex z)
    {
        return new BigComplex(z.Real, z.Imaginary);
    }

    /// <summary>
    /// Implicit cast from BigDecimal to BigComplex.
    /// </summary>
    /// <param name="n">The BigDecimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigDecimal n)
    {
        return new BigComplex(n, 0);
    }

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
    /// Implicit cast from byte to BigDecimal.
    /// </summary>
    /// <param name="n">The byte value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(byte n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from short to BigDecimal.
    /// </summary>
    /// <param name="n">The short value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(short n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from ushort to BigDecimal.
    /// </summary>
    /// <param name="n">The ushort value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ushort n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from int to BigDecimal.
    /// </summary>
    /// <param name="n">The int value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(int n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from uint to BigDecimal.
    /// </summary>
    /// <param name="n">The uint value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(uint n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from long to BigDecimal.
    /// </summary>
    /// <param name="n">The long value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(long n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from ulong to BigDecimal.
    /// </summary>
    /// <param name="n">The ulong value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(ulong n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from Int128 to BigDecimal.
    /// </summary>
    /// <param name="n">The Int128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Int128 n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from UInt128 to BigDecimal.
    /// </summary>
    /// <param name="n">The UInt128 value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(UInt128 n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from BigInteger to BigDecimal.
    /// </summary>
    /// <param name="n">The BigInteger value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(BigInteger n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from Half to BigDecimal.
    /// </summary>
    /// <param name="n">The Half value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(Half n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from float to BigDecimal.
    /// </summary>
    /// <param name="n">The float value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(float n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from double to BigDecimal.
    /// </summary>
    /// <param name="n">The double value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(double n)
    {
        return new BigComplex(n, 0);
    }

    /// <summary>
    /// Implicit cast from decimal to BigDecimal.
    /// </summary>
    /// <param name="n">The decimal value.</param>
    /// <returns>The equivalent BigComplex value.</returns>
    public static implicit operator BigComplex(decimal n)
    {
        return new BigComplex(n, 0);
    }

    #endregion Cast to BigComplex

    #region Cast from BigComplex (all explicit)

    /// <summary>
    /// Explicit cast of BigComplex to a Complex.
    /// </summary>
    /// <param name="z">A BigComplex value.</param>
    /// <returns>The equivalent Complex value.</returns>
    public static explicit operator Complex(BigComplex z)
    {
        return new Complex((double)z.Real, (double)z.Imaginary);
    }

    #endregion Cast from BigComplex
}
