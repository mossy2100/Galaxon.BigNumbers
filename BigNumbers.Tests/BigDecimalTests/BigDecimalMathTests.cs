using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalMathTests
{
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

            nCalls++;
        }

        var result = nCalls == nTests ? "SUCCESS" : "FAIL";
        Console.WriteLine(
            $"{result}: With {BigDecimal.MaxSigFigs} sigfigs, {nCalls} calls without error.");
    }

    /// <summary>Test a binary function for rounding errors using random values.</summary>
    /// <param name="fn">The function to test.</param>
    public void TestBinaryFunction(Func<BigDecimal, BigDecimal, BigDecimal> fn)
    {
        var rnd = new Random();
        var nTests = 100000;
        var nCalls = 0;

        for (var i = 0; i < nTests; i++)
        {
            // Get 2 random BigDecimal values.
            BigDecimal x = rnd.GetDouble();
            BigDecimal y = rnd.GetDouble();

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

            nCalls++;
        }

        var result = nCalls == nTests ? "SUCCESS" : "FAIL";
        Console.WriteLine(
            $"{result}: With {BigDecimal.MaxSigFigs} sigfigs, {nCalls} calls without error.");
    }

    // ---------------------------------------------------------------------------------------------
    // Unary functions.

    [TestMethod]
    public void FindGuardDigitsForIncrement()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(x => x++);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForDecrement()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(x => x--);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForSqr()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(BigDecimal.Sqr);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForCube()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestUnaryFunction(BigDecimal.Cube);
        }
    }

    // ---------------------------------------------------------------------------------------------
    // Binary functions.

    [TestMethod]
    public void FindGuardDigitsForAdd()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x + y);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForSubtract()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x - y);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForMultiply()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x * y);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForDivide()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x / y);
        }
    }

    [TestMethod]
    public void FindGuardDigitsForModulo()
    {
        for (var sf = 30; sf <= 100; sf += 10)
        {
            BigDecimal.MaxSigFigs = sf;
            TestBinaryFunction((x, y) => x % y);
        }
    }
}
