using System.Numerics;
using Galaxon.BigNumbers.TestTools;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalRootTests
{
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
            Console.WriteLine($"√{i} = {bd1}");
            BigDecimal.MaxSigFigs = 50;
            var bd2 = BigDecimal.Sqrt(i);
            Console.WriteLine($"√{i} = {bd2}");
            BigDecimalAssert.AreFuzzyEqual(bd1, bd2);
            Console.WriteLine("");
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
            Console.WriteLine($"³√{i} = {BigDecimal.Cbrt(i):E100}");
            BigDecimal.MaxSigFigs = 50;
            Console.WriteLine($"³√{i} = {BigDecimal.Cbrt(i):E100}");
            Console.WriteLine("");
        }
    }

    /// <summary>
    /// This tests Pow() and RootN() with large exponents.
    /// It takes 37 seconds on my computer, so be patient.
    /// </summary>
    [TestMethod]
    public void TestRootNLargeNIntegerA()
    {
        var a = 5;
        var b = 500;

        // Test BigInteger.Pow().
        var powExpected = BigDecimal.Parse(
            "30549363634996046820519793932136176997894027405723266638936139092812916265247204577018572351080152282568751526935904671553178534278042839697351331142009178896307244205337728522220355888195318837008165086679301794879136633899370525163649789227021200352450820912190874482021196014946372110934030798550767828365183620409339937395998276770114898681640625");
        var powActual = BigInteger.Pow(a, b);
        BigDecimalAssert.AreEqual(powExpected, powActual);
        Console.WriteLine($"{powActual:E50}");

        // Test BigDecimal.RootN().
        var rootActual = BigDecimal.RootN(powActual, b);
        var rootExpected = a;
        BigDecimalAssert.AreFuzzyEqual(rootExpected, rootActual);
    }

    [TestMethod]
    public void TestRootNLargeNDecimalA()
    {
        var a = BigDecimal.Pi;
        var b = 50;
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

    [TestMethod]
    public void TestCbrtOfNegative()
    {
        BigDecimal expected;
        BigDecimal actual;
        BigDecimal delta;

        actual = BigDecimal.Cbrt(-27);
        BigDecimalAssert.AreFuzzyEqual(-3, actual);

        actual = BigDecimal.Cbrt(-16);
        expected = -2.519842099789746;
        delta = 0.000000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        actual = BigDecimal.Cbrt(-1234.5678);
        expected = -10.727659535728732;
        delta = 0.000000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    /// <summary>
    /// Test odd roots of some negative values.
    /// </summary>
    [TestMethod]
    public void TestOddRootOfNegative()
    {
        var actual = BigDecimal.RootN(-27, 5);
        var expected = -1.933182044931763;
        var delta = 0.000000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        actual = BigDecimal.RootN(-16, 7);
        expected = -1.485994289136948;
        delta = 0.000000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        actual = BigDecimal.RootN(-1234.5678, 17);
        expected = -1.52003581302643;
        delta = 0.00000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
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

    /// <summary>
    /// This can be slow and there are no asserts.
    /// </summary>
    [Ignore]
    [TestMethod]
    public void TestRootMaxNumberOfTerms()
    {
        double x;
        int n;
        var nTerms = 3;
        var rnd = new Random();
        BigDecimal y = 0;

        for (var i = 0; i < nTerms; i++)
        {
            x = double.Abs(rnd.GetDouble());
            n = rnd.Next(100);
            Console.WriteLine($"x = {x}, n = {n}");
            y = BigDecimal.RootN(x, n);
            Console.WriteLine($"RootN({x}, {n}) = {y:E10}");
        }
    }
}
