using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.BigNumbers.TestTools;

public class BigComplexAssert
{
    // /// <summary>
    // /// TODO Update to compare BigComplex with BigComplex, as it should. Add delta parameter, same as BigDecimalAssert.AreEqual.
    // /// Helper function to test if a Complex equals a BigComplex.
    // /// </summary>
    // /// <param name="expected">Expected Complex value</param>
    // /// <param name="actual">Actual BigComplex value</param>
    // public static void AreEqual(Complex expected, BigComplex actual)
    // {
    //     BigDecimalAssert.AreEqual(expected.Real, actual.Real);
    //     BigDecimalAssert.AreEqual(expected.Imaginary, actual.Imaginary);
    // }

    /// <summary>Compare two BigComplex values for equality.</summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AreEqual(BigComplex expected, BigComplex actual)
    {
        BigDecimalAssert.AreEqual(expected.Real, actual.Real);
        BigDecimalAssert.AreEqual(expected.Imaginary, actual.Imaginary);
    }

    /// <summary>Compare an actual BigComplex and an expected Complex values for equality.</summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AreEqual(Complex expected, BigComplex actual)
    {
        BigDecimalAssert.AreEqual(expected.Real, actual.Real);
        BigDecimalAssert.AreEqual(expected.Imaginary, actual.Imaginary);
    }
}
