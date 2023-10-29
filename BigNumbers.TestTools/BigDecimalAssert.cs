using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Galaxon.BigNumbers.TestTools;

public class BigDecimalAssert
{
    /// <summary>Compare two BigDecimal values for equality.</summary>
    /// <param name="expected">The expected value.</param>
    /// <param name="actual">The actual value.</param>
    /// <param name="delta">The maximum acceptable difference.</param>
    /// <exception cref="AssertFailedException"></exception>
    public static void AreEqual<T>(T expected, BigDecimal actual, BigDecimal? delta = null)
        where T : INumberBase<T>
    {
        // Get the expected value as a BigDecimal.
        if (expected is not BigDecimal bdExpected)
        {
            var ok = BigDecimal.TryConvertFromChecked(expected, out bdExpected);
        }

        if (delta == null)
        {
            switch (expected)
            {
                case sbyte or byte or short or ushort or int or uint or long or ulong or Int128
                    or UInt128 or BigInteger:
                    delta = 0.5;
                    break;

                case Half:
                    delta = 5e-6;
                    break;

                case float:
                    delta = 5e-10;
                    break;

                case double:
                    delta = 5e-16;
                    break;

                case decimal:
                    delta = 5e-29;
                    break;

                case BigDecimal:
                    // Default delta is the maximum value of the least significant digits in the 2
                    // values.
                    var maxExp = BigDecimal.MaxMagnitude(bdExpected.Exponent, actual.Exponent);
                    delta = BigDecimal.Exp10(maxExp);
                    break;
            }
        }

        // Compare values.
        if (BigDecimal.Abs(bdExpected - actual) >= delta)
        {
            Assert.Fail($"Values are unequal. Expected {expected}, got {actual}.");
        }
    }

    // /// <summary>Compare a BigDecimal value with an expected double value.</summary>
    // /// <param name="expected">The expected value.</param>
    // /// <param name="actual">The actual value.</param>
    // /// <exception cref="AssertFailedException"></exception>
    // public static void EqualsDouble(double expected, BigDecimal actual)
    // {
    //     AreEqual(expected, actual, 1e-15);
    // }
}
