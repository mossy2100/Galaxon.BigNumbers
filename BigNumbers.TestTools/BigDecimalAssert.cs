using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.BigNumbers.TestTools;

/// <summary>
/// Assert methods for BigDecimal. Analogous to StringAssert.
/// </summary>
public class BigDecimalAssert
{
    /// <summary>
    /// See if an actual BigDecimal value is approximately equal to an expected value, which can be
    /// any supported real number type (i.e. all standard real number types, and BigDecimal).
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AreEqual<T>(T expected, BigDecimal actual) where T : INumberBase<T>
    {
        if (!actual.FuzzyEquals(expected))
        {
            Assert.Fail($"Values are unequal. Expected {expected}, got {actual}.");
        }
    }
}
