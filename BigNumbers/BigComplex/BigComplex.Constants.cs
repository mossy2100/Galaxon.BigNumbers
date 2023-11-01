namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    /// <inheritdoc/>
    public static BigComplex E => new (BigDecimal.E);

    /// <inheritdoc/>
    public static BigComplex Pi => new (BigDecimal.Pi);

    /// <inheritdoc/>
    public static BigComplex Tau => new (BigDecimal.Tau);
}
