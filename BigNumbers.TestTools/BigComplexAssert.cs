using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.BigNumbers.TestTools;

public class BigComplexAssert
{
    /// <summary>
    /// See if an actual BigComplex value is effectively equal to to an expected number (real or
    /// complex).
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertFailedException">If the values are not effectively equal.</exception>
    public static void AreEqual<T>(T expected, BigComplex actual) where T : INumberBase<T>
    {
        if (!actual.FuzzyEquals(expected))
        {
            Assert.Fail($"Values are unequal. Expected {expected}, got {actual}.");
        }
    }
}
