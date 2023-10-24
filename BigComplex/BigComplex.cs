using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Properties

    public BigDecimal Real { get; set; }

    public BigDecimal Imaginary { get; set; }

    #endregion Properties

    #region TODO

    /// <inheritdoc />
    public static int Radix { get; }

    /// <inheritdoc />
    public static bool IsCanonical(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsComplexNumber(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsEvenInteger(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsFinite(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsImaginaryNumber(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsInfinity(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsInteger(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNaN(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNegative(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNegativeInfinity(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsNormal(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsOddInteger(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsPositive(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsPositiveInfinity(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsRealNumber(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsSubnormal(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool IsZero(BigComplex value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex MaxMagnitude(BigComplex x, BigComplex y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex MaxMagnitudeNumber(BigComplex x, BigComplex y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex MinMagnitude(BigComplex x, BigComplex y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex MinMagnitudeNumber(BigComplex x, BigComplex y)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertFromChecked<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertFromSaturating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertFromTruncating<TOther>(TOther value, out BigComplex result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertToChecked<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertToSaturating<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryConvertToTruncating<TOther>(BigComplex value, out TOther result)
        where TOther : INumberBase<TOther>
    {
        throw new NotImplementedException();
    }

    #endregion TODO

    #region Constants

    public static BigComplex Zero => new (0, 0);

    public static BigComplex One => new (1, 0);

    // Equivalent to System.Numerics.Complex.ImaginaryOne.
    public static BigComplex ImaginaryOne => new (0, 1);

    // Convenient shorthand for ImaginaryOne.
    public static BigComplex I => ImaginaryOne;

    #endregion Constants

    #region Constructors

    public BigComplex(BigDecimal real, BigDecimal imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public BigComplex(BigDecimal real) : this(real, 0)
    {
    }

    public BigComplex() : this(0, 0)
    {
    }

    #endregion Constructors

    #region Trigonometic methods

    public static BigComplex Sin(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Sin(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Cos(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Cos(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Cos(x) * BigDecimal.Cosh(y);
        var b = BigDecimal.Sin(x) * BigDecimal.Sinh(y);
        return new BigComplex(a, -b);
    }

    public static BigComplex Tan(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        BigComplex a = new (BigDecimal.Sin(2 * x), BigDecimal.Sinh(2 * y));
        var b = BigDecimal.Cos(2 * x) + BigDecimal.Cosh(2 * y);
        return a / b;
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms" />
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Asin(BigComplex z)
    {
        return I * Log(Sqrt(1 - z * z) - I * z);
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms" />
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Acos(BigComplex z)
    {
        return -I * Log(z + I * Sqrt(1 - z * z));
    }

    /// <summary>
    ///     <see href="https://en.wikipedia.org/wiki/Inverse_trigonometric_functions#Logarithmic_forms" />
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    public static BigComplex Atan(BigComplex z)
    {
        return -I / 2 * Log((I - z) / (I + z));
    }

    public static BigComplex Sinh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Sinh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Cosh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Cosh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Cosh(x) * BigDecimal.Cos(y);
        var b = BigDecimal.Sinh(x) * BigDecimal.Sin(y);
        return new BigComplex(a, b);
    }

    public static BigComplex Tanh(BigComplex z)
    {
        var x = z.Real;
        var y = z.Imaginary;
        var a = BigDecimal.Tanh(x);
        var b = BigDecimal.Tan(y);
        return (a + I * b) / (1 + I * a * b);
    }

    #endregion Trigonometic methods

    #region Testing methods

    /// <summary>
    /// Helper function to test if a Complex equals a BigComplex.
    /// </summary>
    /// <param name="expected">Expected Complex value</param>
    /// <param name="actual">Actual BigComplex value</param>
    public static void AssertAreEqual(Complex expected, BigComplex actual)
    {
        BigDecimal.AssertAreEqual(expected.Real, actual.Real);
        BigDecimal.AssertAreEqual(expected.Imaginary, actual.Imaginary);
    }

    #endregion Testing methods

    /// <inheritdoc />
    public static BigComplex AdditiveIdentity { get; }

    /// <inheritdoc />
    public static BigComplex MultiplicativeIdentity { get; }
}
