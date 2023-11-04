using System.Diagnostics;
using System.Numerics;
using Galaxon.BigNumbers.TestTools;

namespace Galaxon.BigNumbers.Tests;

/// <summary>
/// Test all methods in BigDecimal.Exp.cs.
/// That means all methods for exponentiation, finding roots, and logarithms.
/// </summary>
[TestClass]
public class BigDecimalExpTests
{
    #region Test exponentiation methods

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
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.E;
        actual = BigDecimal.Exp(1);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("1.6487212707001281468486507878141635716537761007101");
        actual = BigDecimal.Exp(0.5);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("7.3890560989306502272304274605750078131803155705518");
        actual = BigDecimal.Exp(2);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("20.085536923187667740928529654581717896987907838554");
        actual = BigDecimal.Exp(3);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("22026.465794806716516957900645284244366353512618557");
        actual = BigDecimal.Exp(10);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("0.13533528323661269189399949497248440340763154590958");
        actual = BigDecimal.Exp(-2);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("4.5399929762484851535591515560550610237918088866565E-5");
        actual = BigDecimal.Exp(-10);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void TestPowNegativeBaseFractionalExp()
    {
        BigDecimal x = -32;
        // Note, we have to use a decimal here, because a double will not exactly represent the
        // value, and we won't be able to find a real root.
        BigDecimal y = 0.2m;
        var actual = BigDecimal.Pow(x, y);
        var expected = -2;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        x = -32;
        y = 0.4m;
        actual = BigDecimal.Pow(x, y);
        expected = 4;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    #endregion Test exponentiation methods

    #region Test methods for finding roots

    [TestMethod]
    public void TestSqrt0()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Zero, BigDecimal.Sqrt(0));
    }

    [TestMethod]
    public void TestSqrt1()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.One, BigDecimal.Sqrt(1));
    }

    [TestMethod]
    public void TestSqrtPiSquared()
    {
        var bd = BigDecimal.Sqrt(BigDecimal.Pi * BigDecimal.Pi);
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Pi, bd);
    }

    // No asserts, just want to make sure the method calls complete fast enough and without error.
    // Also testing rounding to required sig figs.
    [TestMethod]
    public void TestSqrtSmallInts()
    {
        for (var i = 1; i <= 10; i++)
        {
            BigDecimal.MaxSigFigs = 55;
            var bd1 = BigDecimal.Sqrt(i);
            Trace.WriteLine($"√{i} = {bd1}");
            BigDecimal.MaxSigFigs = 50;
            var bd2 = BigDecimal.Sqrt(i);
            Trace.WriteLine($"√{i} = {bd2}");
            BigDecimalAssert.AreFuzzyEqual(bd1, bd2);
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
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
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
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
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
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
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
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void TestSqrtNegative()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Sqrt(-1));
    }

    [TestMethod]
    public void TestCbrt0()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Zero, BigDecimal.Cbrt(0));
    }

    [TestMethod]
    public void TestCbrt1()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.One, BigDecimal.Cbrt(1));
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
        BigDecimalAssert.AreFuzzyEqual(a, BigDecimal.RootN(c, b));
    }

    [TestMethod]
    public void TestRootNLargeNDecimalA()
    {
        var a = BigDecimal.Pi;
        var b = 500;
        var c = BigDecimal.Pow(a, b);
        BigDecimalAssert.AreFuzzyEqual(a, BigDecimal.RootN(c, b));
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
    /// BigDecimal cannot represent 1/3 exactly. You have to use Cbrt() or RootN().
    /// </summary>
    [TestMethod]
    public void TestCubeRootOfNegativeValueUsingPow()
    {
        var oneThird = BigDecimal.One / 3;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(-27, oneThird));
    }

    [TestMethod]
    public void TestCbrtOfNegative()
    {
        BigDecimal c;
        c = BigDecimal.Cbrt(-27);
        BigDecimalAssert.AreFuzzyEqual(-3, c);

        c = BigDecimal.Cbrt(-16);
        BigDecimalAssert.AreFuzzyEqual(-2.519842099789746, c);

        c = BigDecimal.Cbrt(-1234.5678);
        BigDecimalAssert.AreFuzzyEqual(-10.727659535728732, c);
    }

    /// <summary>
    /// Test odd roots of some negative values.
    /// </summary>
    [TestMethod]
    public void TestOddRootOfNegative()
    {
        BigDecimal c;
        c = BigDecimal.RootN(-27, 5);
        BigDecimalAssert.AreFuzzyEqual(-1.933182044931763, c);

        c = BigDecimal.RootN(-16, 7);
        BigDecimalAssert.AreFuzzyEqual(-1.485994289136948, c);

        c = BigDecimal.RootN(-1234.5678, 17);
        BigDecimalAssert.AreFuzzyEqual(-1.52003581302643, c);
    }

    /// <summary>
    /// Test even roots of negative values throw exceptions.
    /// </summary>
    [TestMethod]
    public void TestEvenRootOfNegativeThrowsException()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-27, 4));
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-16, 6));
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-1234.5678, 16));
    }

    #endregion Test methods for finding roots

    #region Test logarithm methods

    [TestMethod]
    public void TestLog()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = 0;
        actual = BigDecimal.Log(1);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("0.69314718055994530941723212145817656807550013436026");
        actual = BigDecimal.Log(2);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = 1;
        actual = BigDecimal.Log(BigDecimal.E);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("2.3025850929940456840179914546843642076011014886288");
        actual = BigDecimal.Log(10);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void TestLogDouble()
    {
        BigDecimal.MaxSigFigs = 30;

        for (var i = 1; i < 100; i++)
        {
            Trace.WriteLine("--------------------------------------------------");

            double d = i;
            var logD = double.Log(d);
            Trace.WriteLine($"double.Log({d})     = {logD}");

            BigDecimal bd = i;
            var logBD = BigDecimal.Log(bd);
            Trace.WriteLine($"BigDecimal.Log({bd}) = {logBD}");

            BigDecimalAssert.AreFuzzyEqual(logD, logBD);
        }
    }

    [TestMethod]
    public void LogSpeedTest()
    {
        for (var i = 1; i < 10; i++)
        {
            Trace.WriteLine("--------------------------------------------------");

            var t1 = DateTime.Now.Ticks;
            var log = BigDecimal.Log(i);
            var t2 = DateTime.Now.Ticks;
            var tLog = t2 - t1;
            Trace.WriteLine($"Log({i}) == {log}");
            Trace.WriteLine($"{tLog} ticks.");
        }
    }

    #endregion Test logarithm methods
}
