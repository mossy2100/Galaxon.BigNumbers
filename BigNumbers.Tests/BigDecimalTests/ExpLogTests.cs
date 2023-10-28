using System.Diagnostics;
using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests.BigDecimalTests;

[TestClass]
public class ExpLogTests
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
        var bd = BigDecimal.Sqrt(BigDecimal.Pi * BigDecimal.Pi);
        Assert.AreEqual(BigDecimal.Pi, bd);
    }

    // No asserts, just want to make sure the method calls complete fast enough and without error.
    // Also testing rounding to required sig figs.
    [TestMethod]
    public void TestSqrtSmallInts()
    {
        for (var i = 1; i <= 10; i++)
        {
            BigDecimal.MaxSigFigs = 55;
            Trace.WriteLine($"√{i} = " + BigDecimal.Sqrt(i));
            BigDecimal.MaxSigFigs = 50;
            Trace.WriteLine($"√{i} = " + BigDecimal.Sqrt(i));
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtBig()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("6.02214076E23");
        var expected = BigDecimal.Parse("776024533117.34932546664032837511112530578432706889"
            + "69571576562989126786337996022194015376088918609909491309813595319711937386010926");
        var actual = BigDecimal.Sqrt(x);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtSmall()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.602176634E-19");
        var expected = BigDecimal.Parse("4.0027198677899006825970388239053767545702786298616"
            + "66648707342924009987437927221345536742635143445476302206435987095958590772815416E-10");
        var actual = BigDecimal.Sqrt(x);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// In this test, both the initial argument and the square root are larger than the largest
    /// possible double value.
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtBiggerThanBiggestDouble()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.2345678E789");
        var expected = BigDecimal.Parse("3.5136417005722140080009539858670683706660895438958"
            + "9865958869460824551868009859293464600836861863229496438492388219814058056172706E+394");
        var actual = BigDecimal.Sqrt(x);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// In this test, both the initial argument and the square root are smaller than the largest
    /// possible double value.
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtSmallerThanSmallestDouble()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.2345678E-789");
        var expected = BigDecimal.Parse("3.5136417005722140080009539858670683706660895438958"
            + "9865958869460824551868009859293464600836861863229496438492388219814058056172706E-395");
        var actual = BigDecimal.Sqrt(x);
        Assert.AreEqual(expected, actual);
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

    // No asserts, just want to make sure the method calls complete fast enough and without error or
    // infinite looping.
    [TestMethod]
    public void TestCbrtSmallValues()
    {
        for (var i = 1; i <= 1000; i++)
        {
            BigDecimal.MaxSigFigs = 54;
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
        var b = 500;
        var c = BigInteger.Pow(a, b);
        Assert.AreEqual(a, BigDecimal.RootN(c, b));
    }

    [TestMethod]
    public void TestRootNLargeNDecimalA()
    {
        var a = BigDecimal.Pi;
        var b = 500;
        var c = BigDecimal.Pow(a, b);
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
        var expected = -2;
        var actual = BigDecimal.Pow(-32, 0.2);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestRootNWithNegativeArgumentAndOddRoot()
    {
        BigDecimal x = -123;
        var y = 71;
        var z = BigDecimal.RootN(x, y);
        Assert.IsTrue(z < 0);
    }

    [TestMethod]
    public void TestRootNWithNegativeArgumentAndEvenRoot()
    {
        BigDecimal x = -123;
        var y = 70;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(x, y));
    }

    /// <summary>
    /// This shows how the cube root of a negative value cannot be achieved using Pow(), because a
    /// BigDecimals cannot represent 1/3 exactly. You have to use Cbrt() or RootN().
    /// </summary>
    [TestMethod]
    public void TestCubeRootOfNegativeValueUsingPow()
    {
        var oneThird = BigDecimal.One / 3;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(-27, oneThird));
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

    // [TestMethod]
    // public void TestLogAgm()
    // {
    //     BigDecimals.MaxSigFigs = 50;
    //
    //     BigDecimals expected;
    //     BigDecimals actual;
    //
    //     expected = 0;
    //     actual = BigDecimals.LogAgm(1);
    //     Assert.AreEqual(expected, actual);
    //
    //     expected = BigDecimals.Parse("0.69314718055994530941723212145817656807550013436026");
    //     actual = BigDecimals.LogAgm(2);
    //     Assert.AreEqual(expected, actual);
    //
    //     expected = 1;
    //     actual = BigDecimals.LogAgm(BigDecimals.E);
    //     Assert.AreEqual(expected, actual);
    //
    //     expected = BigDecimals.Parse("2.3025850929940456840179914546843642076011014886288");
    //     actual = BigDecimals.LogAgm(10);
    //     Assert.AreEqual(expected, actual);
    //
    //     expected = BigDecimals.Parse("4.6051701859880913680359829093687284152022029772575");
    //     actual = BigDecimals.LogAgm(100);
    //     Assert.AreEqual(expected, actual);
    //
    //     expected = BigDecimals.Parse("8.14786712992394624010636056097481309047097261399");
    //     actual = BigDecimals.LogAgm(3456);
    //     Assert.AreEqual(expected, actual);
    // }

    [TestMethod]
    public void TestLogDouble()
    {
        BigDecimal.MaxSigFigs = 30;

        for (var i = 1; i < 100; i++)
        {
            double d = i;
            var logD = double.Log(d);

            BigDecimal bd = i;
            var logBD = BigDecimal.Log(bd);

            Console.WriteLine($"double.Log({d})     = {logD}");
            Console.WriteLine($"BigDecimal.Log({bd}) = {logBD}");
            var expected = logD.ToString("G13");
            Console.WriteLine(expected);
            var actual = logBD.ToString("G13");
            Console.WriteLine(actual);
            Console.WriteLine("--------------------------------------------------");

            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void LogSpeedTest()
    {
        for (var i = 1; i < 10; i++)
        {
            Console.WriteLine("------------------------------");

            var t1 = DateTime.Now.Ticks;
            var log = BigDecimal.Log(i);
            var t2 = DateTime.Now.Ticks;
            var tLog = t2 - t1;
            Console.WriteLine($"Log({i}) == {log}");
            Console.WriteLine($"Log() took {tLog} ticks.");
        }
    }

    [TestMethod]
    public void TestFirstComplexRootOfNegative()
    {
        BigComplex root;

        // I got the answers to test from this online calculator:
        // https://www.emathhelp.net/en/calculators/algebra-2/nth-roots-of-complex-number-calculator/

        root = BigComplex.RootN(-16, 2);
        Assert.AreEqual(0, root.Real);
        Assert.AreEqual(4, root.Imaginary);

        root = BigComplex.RootN(-16, 3);
        Assert.AreEqual(1.259921049894873, (double)root.Real, 1e-15);
        Assert.AreEqual(2.182247271943443, (double)root.Imaginary, 1e-15);

        root = BigComplex.RootN(-16, 4);
        Assert.AreEqual(1.414213562373095, (double)root.Real, 1e-15);
        Assert.AreEqual(1.414213562373095, (double)root.Imaginary, 1e-15);
    }

    [TestMethod]
    public void TestCbrtOfNegative()
    {
        BigDecimal c;
        c = BigDecimal.Cbrt(-27);
        Assert.AreEqual(-3, c);
    }
}
