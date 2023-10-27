namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Comparison methods

    /// <inheritdoc />
    public bool Equals(BigComplex other)
    {
        return Real == other.Real && Imaginary == other.Imaginary;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        // Null check.
        if (obj == null)
        {
            return false;
        }

        // Try to convert the object to a BigComplex.
        try
        {
            var z = (BigComplex)obj;
            return Equals(z);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc />
    public readonly override int GetHashCode()
    {
        return HashCode.Combine(Real, Imaginary);
    }

    #endregion Comparison methods

    #region Comparison operators (IEqualityOperators)

    /// <inheritdoc />
    public static bool operator ==(BigComplex z1, BigComplex z2)
    {
        return z1.Equals(z2);
    }

    /// <inheritdoc />
    public static bool operator !=(BigComplex z1, BigComplex z2)
    {
        return !z1.Equals(z2);
    }

    #endregion Comparison operators (IEqualityOperators)

    #region Compare magnitude methods (INumberBase)

    /// <inheritdoc />
    public static BigComplex MaxMagnitude(BigComplex x, BigComplex y)
    {
        return x.Magnitude > y.Magnitude ? x : y;
    }

    /// <inheritdoc />
    public static BigComplex MaxMagnitudeNumber(BigComplex x, BigComplex y)
    {
        return MaxMagnitude(x, y);
    }

    /// <inheritdoc />
    public static BigComplex MinMagnitude(BigComplex x, BigComplex y)
    {
        return x.Magnitude < y.Magnitude ? x : y;
    }

    /// <inheritdoc />
    public static BigComplex MinMagnitudeNumber(BigComplex x, BigComplex y)
    {
        return MinMagnitude(x, y);
    }

    #endregion Compare magnitude methods (INumberBase)
}
