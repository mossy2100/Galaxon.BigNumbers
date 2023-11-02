using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.BigNumbers.TestTools;

public class BigDecimalAssert
{
    /// <summary>
    /// See if an actual BigDecimal value is equal with an expected value (any number type).
    /// The tolerance (delta) is based off the type of the expected value.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="delta">The maximum acceptable difference.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AreEqual<T>(T expected, BigDecimal actual, BigDecimal? delta = null)
        where T : INumberBase<T>
    {
        if (!actual.FuzzyEquals(expected, delta))
        {
            Assert.Fail($"Values are unequal. Expected {expected}, got {actual}.");
        }
    }
}
