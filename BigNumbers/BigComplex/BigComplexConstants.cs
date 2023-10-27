namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    /// <inheritdoc />
    public static BigComplex E => new (BigDecimal.E);

    /// <inheritdoc />
    public static BigComplex Pi => new (BigDecimal.Pi);

    /// <inheritdoc />
    public static BigComplex Tau => new (BigDecimal.Tau);

    /// <summary>
    /// The golden ratio (φ).
    /// </summary>
    public static BigComplex Phi => new (BigDecimal.Phi);

    /// <summary>
    /// The natural logarithm of 10.
    /// </summary>
    public static BigComplex Ln10 => new (BigDecimal.Ln10);
}
