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

    #region Comparison operators

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

    #endregion Comparison operators
}
