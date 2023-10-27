using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    /// <summary>
    /// Compare two BigNumbers values for equality.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="delta">The maximum acceptable difference.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AssertAreEqual(BigDecimal expected, BigDecimal actual,
        BigDecimal? delta = null)
    {
        // Cannot set a default delta in the method signature because a BigNumbers value cannot be a
        // compile-time constant.
        delta ??= 0;

        // Compare values.
        if (Abs(expected - actual) > delta)
        {
            throw new AssertFailedException(
                $"Values are unequal. Expected {expected}, got {actual}.");
        }
    }

    /// <summary>
    /// Compare a BigNumbers value with an expected double value.
    /// </summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AssertAreEqual(double expected, BigDecimal actual)
    {
        // If the expected value is >= 10, scale both to a value between 0 and 10 before comparison.
        var a = actual;
        var e = (BigDecimal)expected;
        var m = e.NumSigFigs + e.Exponent - 1;
        if (m > 0)
        {
            a.Exponent -= m;
            e.Exponent -= m;
        }

        // This delta is large enough to produce fuzzy equality when expected.
        var delta = new BigDecimal(1, -13);

        // Compare values.
        AssertAreEqual(e, a, delta);
    }
}
