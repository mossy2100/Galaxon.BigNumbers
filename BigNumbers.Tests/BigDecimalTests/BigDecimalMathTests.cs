using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalMathTests
{
    /// <summary>
    /// TODO
    /// There's an issue here in that this method is running with the default number of sig figs of
    /// 50. The result might be different with different initial number of sig figs.
    /// </summary>
    /// <param name="fn"></param>
    /// <returns></returns>
    public int TestBinaryFunctionWithGuardDigits(Func<BigDecimal, BigDecimal, BigDecimal> fn)
    {
        var rnd = new Random();
        var nGuardDigits = 0;

        // Limit the duration of the experiment.
        var timeLimit = TimeSpan.FromSeconds(60);
        var startTime = DateTime.Now;

        while (true)
        {
            // Get 2 random BigDecimal values.
            BigDecimal x = rnd.GetDouble();
            BigDecimal y = rnd.GetDouble();

            // Get a result likely to be correct.
            BigDecimal expected = BigDecimal.DoWithGuardDigits(() => fn(x, y), nGuardDigits + 5);

            // Get a result that might be different if there aren't enough guard digits.
            BigDecimal actual = BigDecimal.DoWithGuardDigits(() => fn(x, y), nGuardDigits);

            // If there's an error.
            if (expected != actual)
            {
                // Restart the time with the next largest number of guard digits.
                nGuardDigits++;
                startTime = DateTime.Now;
            }

            // Check if we used up all the time yet.
            if (DateTime.Now - startTime >= timeLimit)
            {
                break;
            }
        }

        return nGuardDigits;
    }

    [TestMethod]
    public void FindGuardDigitsForDivide()
    {
        var n = TestBinaryFunctionWithGuardDigits((x, y) => x / y);
        Console.WriteLine($"The Divide() method requires {n} guard digits for 0 errors.");
    }

    [TestMethod]
    public void FindGuardDigitsForModulo()
    {
        var n = TestBinaryFunctionWithGuardDigits((x, y) => x % y);
        Console.WriteLine($"The Modulo() method requires {n} guard digits for 0 errors.");
    }
}
