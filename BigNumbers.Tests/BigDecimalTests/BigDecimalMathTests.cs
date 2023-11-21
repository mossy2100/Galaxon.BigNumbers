using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

/// <summary>
/// Test class for checking the number of guard digits used by each operator and method is correct.
/// </summary>
[TestClass]
public class BigDecimalMathTests
{
    #region Test methods

    /// <summary>Test a unary function for rounding errors using random values.</summary>
    /// <param name="fn">The function to test.</param>
    public void TestUnaryFunction(Func<BigDecimal, BigDecimal> fn)
    {
        var rnd = new Random();
        var nTests = 100000;
        var nCalls = 0;

        for (var i = 0; i < nTests; i++)
        {
            // Get 2 random BigDecimal values.
            BigDecimal x = rnd.GetDouble();

            try
            {
                // Get a result likely to be correct.
                var sf = BigDecimal.AddGuardDigits(10);
                BigDecimal expected = fn(x);
                expected = BigDecimal.RemoveGuardDigits(expected, sf);

                // Get a result that might be different if there aren't enough guard digits.
                BigDecimal actual = fn(x);

                // Stop on error.
                if (expected != actual)
                {
                    Console.WriteLine($"{expected} != {actual}");
                    break;
                }
            }
            catch (Exception)
            {
                // Ignore.
            }

            nCalls++;
        }

        var result = nCalls == nTests ? "SUCCESS" : "FAIL";
        Console.WriteLine(
            $"{result}: With {BigDecimal.MaxSigFigs} sigfigs, {nCalls} calls without error.");
    }

    /// <summary>Test a binary function for rounding errors using random values.</summary>
    /// <param name="fn">The function to test.</param>
    /// <param name="nTests">The number of tests to run.</param>
    public void TestBinaryFunction(Func<BigDecimal, BigDecimal, BigDecimal> fn,
        int nTests = 1000000)
    {
        var rnd = new Random();
        var nCalls = 0;

        for (var i = 0; i < nTests; i++)
        {
            // Get 2 random BigDecimal values.
            BigDecimal x = rnd.GetDouble();
            BigDecimal y = rnd.GetDouble();
            // Console.WriteLine($"x = {x}, y = {y}");

            try
            {
                // Get a result likely to be correct.
                var sf = BigDecimal.AddGuardDigits(10);
                BigDecimal expected = fn(x, y);
                expected = BigDecimal.RemoveGuardDigits(expected, sf);

                // Get a result that might be different if there aren't enough guard digits.
                BigDecimal actual = fn(x, y);

                // Stop on error.
                if (expected != actual)
                {
                    Console.WriteLine($"{expected} != {actual}");
                    break;
                }
            }
            catch (Exception)
            {
                // Ignore.
            }

            nCalls++;
        }

        var result = nCalls == nTests ? "SUCCESS" : "FAIL";
        Console.WriteLine(
            $"{result}: With {BigDecimal.MaxSigFigs} sigfigs, {nCalls} calls without error.");
    }

    #endregion Test methods

    // ---------------------------------------------------------------------------------------------

    #region Unary operators

    /// <summary>Test guard digits for the increment operator.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForIncrement()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(x => x++);
        }
    }

    /// <summary>Test guard digits for the decrement operator.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForDecrement()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(x => x--);
        }
    }

    #endregion Unary operators

    // ---------------------------------------------------------------------------------------------

    #region Unary functions

    /// <summary>Test guard digits for the Sqr method.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForSqr()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(BigDecimal.Sqr);
        }
    }

    /// <summary>Test guard digits for the Cube method.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForCube()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(BigDecimal.Cube);
        }
    }

    /// <summary>Test guard digits for Truncate().</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForTruncate()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(BigDecimal.Truncate);
        }
    }

    #endregion Unary functions

    // ---------------------------------------------------------------------------------------------

    #region Binary operators

    /// <summary>Test guard digits for the add operator.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForAdd()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x + y);
        }
    }

    /// <summary>Test guard digits for the subtract operator.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForSubtract()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x - y);
        }
    }

    /// <summary>Test guard digits for the multiply operator.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForMultiply()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x * y);
        }
    }

    /// <summary>Test guard digits for the divide operator.</summary>
    /// <remarks>DONE</remarks>
    [TestMethod]
    public void TestGuardDigitsForDivide()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x / y);
        }
    }

    /// <summary>Test guard digits for the modulo operator.</summary>
    /// <remarks>
    /// This one fails regardless of the number of guard digits used. I think it's because
    /// Truncate() works with decimal places but the results of methods and operations are
    /// constrained by the number of significant figures.
    /// </remarks>
    [TestMethod]
    public void TestGuardDigitsForModulo()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x % y);
        }
    }

    /// <summary>Test guard digits for the exponentiation operator.</summary>
    /// <remarks>TODO</remarks>
    [TestMethod]
    public void TestGuardDigitsForExponentiation()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x ^ y);
        }
    }

    #endregion Binary operators

    // ---------------------------------------------------------------------------------------------

    #region Binary functions

    /// <summary>Test guard digits for the Pow() method.</summary>
    /// <remarks>TODO</remarks>
    [TestMethod]
    public void TestGuardDigitsForPow()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction(BigDecimal.Pow, 5);
        }
    }

    #endregion Binary functions
}
