namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    /// <inheritdoc/>
    public static bool IsCanonical(BigComplex value) => true;

    /// <inheritdoc/>
    public static bool IsZero(BigComplex value) => value.Real == 0 && value.Imaginary == 0;

    /// <inheritdoc/>
    public static bool IsNegative(BigComplex value)
    {
        if (IsRealNumber(value))
        {
            return value.Real < 0;
        }

        throw new ArithmeticException("Positive and negative are undefined for complex numbers.");
    }

    /// <inheritdoc/>
    public static bool IsPositive(BigComplex value)
    {
        if (IsRealNumber(value))
        {
            return value.Real > 0;
        }

        throw new ArithmeticException("Positive and negative are undefined for complex numbers.");
    }

    /// <inheritdoc/>
    public static bool IsInteger(BigComplex value) =>
        IsRealNumber(value) && BigDecimal.IsInteger(value.Real);

    /// <inheritdoc/>
    public static bool IsOddInteger(BigComplex value) =>
        IsRealNumber(value) && BigDecimal.IsOddInteger(value.Real);

    /// <inheritdoc/>
    public static bool IsEvenInteger(BigComplex value) =>
        IsRealNumber(value) && BigDecimal.IsEvenInteger(value.Real);

    /// <inheritdoc/>
    public static bool IsRealNumber(BigComplex value) => value.Imaginary == 0;

    /// <inheritdoc/>
    public static bool IsImaginaryNumber(BigComplex value) =>
        value.Real == 0 && value.Imaginary != 0;

    /// <inheritdoc/>
    public static bool IsComplexNumber(BigComplex value) => value.Imaginary != 0;

    /// <inheritdoc/>
    public static bool IsFinite(BigComplex value) => true;

    /// <inheritdoc/>
    public static bool IsInfinity(BigComplex value) => false;

    /// <inheritdoc/>
    public static bool IsNegativeInfinity(BigComplex value) => false;

    /// <inheritdoc/>
    public static bool IsPositiveInfinity(BigComplex value) => false;

    /// <inheritdoc/>
    public static bool IsNaN(BigComplex value) => false;

    /// <inheritdoc/>
    public static bool IsNormal(BigComplex value) => true;

    /// <inheritdoc/>
    public static bool IsSubnormal(BigComplex value) => false;
}
