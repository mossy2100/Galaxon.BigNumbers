namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Equality methods

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is BigComplex bc && Equals(bc);

    /// <inheritdoc/>
    public bool Equals(BigComplex bc) => Real == bc.Real && Imaginary == bc.Imaginary;

    /// <inheritdoc/>
    public readonly override int GetHashCode() => HashCode.Combine(Real, Imaginary);

    #endregion Equality methods

    #region Comparison methods

    /// <inheritdoc/>
    public static BigComplex MaxMagnitude(BigComplex bc, BigComplex bc2)
    {
        var absX = bc.Magnitude;
        var absY = bc2.Magnitude;
        return absX > absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigComplex MaxMagnitudeNumber(BigComplex bc, BigComplex bc2) =>
        MaxMagnitude(bc, bc2);

    /// <inheritdoc/>
    public static BigComplex MinMagnitude(BigComplex bc, BigComplex bc2)
    {
        var absX = bc.Magnitude;
        var absY = bc2.Magnitude;
        return absX < absY ? absX : absY;
    }

    /// <inheritdoc/>
    public static BigComplex MinMagnitudeNumber(BigComplex bc, BigComplex bc2) =>
        MinMagnitude(bc, bc2);

    #endregion Comparison methods

    #region Comparison operators

    /// <inheritdoc/>
    public static bool operator ==(BigComplex bc, BigComplex bc2) => bc.Equals(bc2);

    /// <inheritdoc/>
    public static bool operator !=(BigComplex bc, BigComplex bc2) => !bc.Equals(bc2);

    #endregion Comparison operators
}
