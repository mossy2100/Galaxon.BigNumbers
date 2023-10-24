using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
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
}
