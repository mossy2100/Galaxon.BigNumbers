using System.Diagnostics;
using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestExpLog
{
    [TestMethod]
    public void TestSqrt0()
    {
        Assert.AreEqual(BigDecimal.Zero, BigDecimal.Sqrt(0));
    }

    [TestMethod]
    public void TestSqrt1()
    {
        Assert.AreEqual(BigDecimal.One, BigDecimal.Sqrt(1));
    }

    [TestMethod]
    public void TestSqrtPiSquared()
    {
        BigDecimal bd = BigDecimal.Sqrt(BigDecimal.Pi * BigDecimal.Pi);
        Assert.AreEqual(BigDecimal.Pi, bd);
    }

    // No asserts, just want to make sure the method calls complete fast enough and without error.
    // Also testing rounding to required sig figs.
    [TestMethod]
    public void TestSqrtSmallInts()
    {
        for (int i = 1; i <= 10; i++)
        {
            BigDecimal.MaxSigFigs = 55;
            Trace.WriteLine($"√{i} = " + BigDecimal.Sqrt(i));
            BigDecimal.MaxSigFigs = 50;
            Trace.WriteLine($"√{i} = " + BigDecimal.Sqrt(i));
            Trace.WriteLine("");
        }
    }

    [TestMethod]
    public void TestSqrtBig()
    {
        Trace.WriteLine(BigDecimal.Sqrt((BigDecimal)6.02214076E23).ToString());
    }

    [TestMethod]
    public void TestSqrtSmall()
    {
        Trace.WriteLine(BigDecimal.Sqrt((BigDecimal)1.602176634E-19).ToString());
    }

    [TestMethod]
    public void TestSqrtBiggerThanBiggestDouble()
    {
        Trace.WriteLine(BigDecimal.Sqrt(BigDecimal.Parse("1.2345678E789")).ToString());
    }

    [TestMethod]
    public void TestSqrtSmallerThanSmallestDouble()
    {
        BigDecimal bd = BigDecimal.Parse("1.2345678E-789");
        Trace.WriteLine(BigDecimal.Sqrt(bd).ToString());
    }

    [TestMethod]
    public void TestSqrtNegative()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Sqrt(-1));
    }

    [TestMethod]
    public void TestCbrt0()
    {
        Assert.AreEqual(BigDecimal.Zero, BigDecimal.Cbrt(0));
    }

    [TestMethod]
    public void TestCbrt1()
    {
        Assert.AreEqual(BigDecimal.One, BigDecimal.Cbrt(1));
    }

    // No asserts, just want to make sure the method calls complete fast enough and without error.
    [TestMethod]
    public void TestCbrtSmallValues()
    {
        int i;
        for (i = 1; i <= 1000; i++)
        {
            BigDecimal.MaxSigFigs = 55;
            Trace.WriteLine($"³√{i} = " + BigDecimal.Cbrt(i));
            BigDecimal.MaxSigFigs = 50;
            Trace.WriteLine($"³√{i} = " + BigDecimal.Cbrt(i));
            Trace.WriteLine("");
        }
    }

    [TestMethod]
    public void TestRootNLargeNIntegerA()
    {
        BigInteger a = 5;
        int b = 500;
        BigInteger c = BigInteger.Pow(a, b);
        Assert.AreEqual(a, BigDecimal.RootN(c, b));
    }

    [TestMethod]
    public void TestRootNLargeNDecimalA()
    {
        BigDecimal a = BigDecimal.Pi;
        int b = 500;
        BigDecimal c = BigDecimal.Pow(a, b);
        Assert.AreEqual(a, BigDecimal.RootN(c, b));
    }

    // Useful high-precision online calculator for finding what should be the right result.
    // https://keisan.casio.com/calculator
    [TestMethod]
    public void TestExp()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = 1;
        actual = BigDecimal.Exp(0);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.E;
        actual = BigDecimal.Exp(1);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("1.6487212707001281468486507878141635716537761007101");
        actual = BigDecimal.Exp(0.5m);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("7.3890560989306502272304274605750078131803155705518");
        actual = BigDecimal.Exp(2);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("20.085536923187667740928529654581717896987907838554");
        actual = BigDecimal.Exp(3);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("22026.465794806716516957900645284244366353512618557");
        actual = BigDecimal.Exp(10);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("0.13533528323661269189399949497248440340763154590958");
        actual = BigDecimal.Exp(-2);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("4.5399929762484851535591515560550610237918088866565E-5");
        actual = BigDecimal.Exp(-10);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestPowNegativeBaseFractionalExp()
    {
        BigDecimal x = -32;
        BigDecimal y = 0.2m;
        Assert.AreEqual(-2, BigDecimal.Pow(x, y));
    }

    [TestMethod]
    public void TestRootNWithNegativeArgumentAndOddRoot()
    {
        BigDecimal x = -123;
        int y = 71;
        BigDecimal z = BigDecimal.RootN(x, y);
        Assert.IsTrue(z < 0);
    }

    [TestMethod]
    public void TestRootNWithNegativeArgumentAndEvenRoot()
    {
        BigDecimal x = -123;
        int y = 70;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(x, y));
    }

    /// <summary>
    /// This shows how the cube root of a negative value cannot be achieved using Pow(), because a
    /// BigDecimal cannot represent 1/3 exactly. You have to use Cbrt() or RootN().
    /// </summary>
    [TestMethod]
    public void TestCubeRootOfNegativeValueUsingPow()
    {
        BigDecimal oneThird = BigDecimal.One / 3;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(-27, oneThird));
    }

    [TestMethod]
    public void TestECalculation()
    {
        // Use the built-in value.
        BigDecimal.MaxSigFigs = 50;
        BigDecimal expected = BigDecimal.RoundSigFigs(BigDecimal.E);
        Trace.WriteLine(expected);
        BigDecimal actual = BigDecimal.Exp(1);
        Trace.WriteLine(actual);
        Assert.AreEqual(expected, actual);

        // Calculate the value.
        BigDecimal.MaxSigFigs = 201;
        // Get E to 200 decimal places from https://www.math.utah.edu/~pa/math/e
        expected = BigDecimal.Parse(
            "2.71828 18284 59045 23536 02874 71352 66249 77572 47093 69995 "
            + "95749 66967 62772 40766 30353 54759 45713 82178 52516 64274 "
            + "27466 39193 20030 59921 81741 35966 29043 57290 03342 95260 "
            + "59563 07381 32328 62794 34907 63233 82988 07531 95251 01901");
        Trace.WriteLine(expected);
        actual = BigDecimal.Exp(1);
        Trace.WriteLine(actual);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestLog()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = 0;
        actual = BigDecimal.Log(1);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("0.69314718055994530941723212145817656807550013436026");
        actual = BigDecimal.Log(2);
        Assert.AreEqual(expected, actual);

        expected = 1;
        actual = BigDecimal.Log(BigDecimal.E);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("2.3025850929940456840179914546843642076011014886288");
        actual = BigDecimal.Log(10);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestLogAgm()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = 0;
        actual = BigDecimal.LogAgm(1);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("0.69314718055994530941723212145817656807550013436026");
        actual = BigDecimal.LogAgm(2);
        Assert.AreEqual(expected, actual);

        expected = 1;
        actual = BigDecimal.LogAgm(BigDecimal.E);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("2.3025850929940456840179914546843642076011014886288");
        actual = BigDecimal.LogAgm(10);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("4.6051701859880913680359829093687284152022029772575");
        actual = BigDecimal.LogAgm(100);
        Assert.AreEqual(expected, actual);

        expected = BigDecimal.Parse("8.14786712992394624010636056097481309047097261399");
        actual = BigDecimal.LogAgm(3456);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestLogDouble()
    {
        BigDecimal.MaxSigFigs = 17;

        for (int i = 1; i < 100; i++)
        {
            double d = i;
            double logD = double.Log(d);

            BigDecimal bd = i;
            BigDecimal logBD = BigDecimal.Log(bd);

            Console.WriteLine($"double.Log({d})     = {logD}");
            Console.WriteLine($"BigDecimal.Log({bd}) = {logBD}");
            string expected = logD.ToString("G14");
            Console.WriteLine(expected);
            string actual = logBD.ToString("G14");
            Console.WriteLine(actual);
            Console.WriteLine("--------------------------------------------------");

            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void LogSpeedTest()
    {
        for (int i = 1; i < 10; i++)
        {
            Console.WriteLine("------------------------------");

            long t1 = DateTime.Now.Ticks;
            BigDecimal log = BigDecimal.Log(i);
            long t2 = DateTime.Now.Ticks;
            long tLog = t2 - t1;
            Console.WriteLine($"Log({i}) == {log}");
            Console.WriteLine($"Log() took {tLog} ticks.");

            // long t3 = DateTime.Now.Ticks;
            // BigDecimal logH = BigDecimal.LogHalleys(i);
            // long t4 = DateTime.Now.Ticks;
            // long tLogH = t4 - t3;
            // Console.WriteLine($"LogHalleys({i}) == {logH}");
            // Console.WriteLine($"LogHalleys() took {tLogH} ticks.");

            // long t5 = DateTime.Now.Ticks;
            // BigDecimal logAgm = BigDecimal.LogAgm(i);
            // long t6 = DateTime.Now.Ticks;
            // long tLogAgm = t6 - t5;
            // Console.WriteLine($"LogAgm({i}) == {logAgm}");
            // Console.WriteLine($"LogAgm() took {tLogAgm} ticks.");

            // double percent = 100.0 * tLogAgm / tLog;
            // Console.WriteLine($"The AGM method took {percent:F0}% as long as the Taylor series method.");
        }
    }
}
