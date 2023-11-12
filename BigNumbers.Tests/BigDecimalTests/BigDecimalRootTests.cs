using System.Numerics;
using Galaxon.BigNumbers.TestTools;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalRootTests
{
    #region Sqrt() tests

    [TestMethod]
    public void Sqrt_0_Returns0()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Zero, BigDecimal.Sqrt(0));
    }

    [TestMethod]
    public void Sqrt_1_Returns1()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.One, BigDecimal.Sqrt(1));
    }

    [TestMethod]
    public void Sqrt_Negative1_ThrowsException()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Sqrt(-1));
    }

    [TestMethod]
    public void Sqrt_PositiveNumberSquared_ReturnsNumber()
    {
        BigDecimal x, y;

        x = 12345;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(x, y);

        x = 6.22e23;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(x, y);

        x = BigDecimal.Pi;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(x, y);

        x = BigDecimal.E;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(x, y);
    }

    [TestMethod]
    public void Sqrt_NegativeNumberSquared_ReturnsPositiveNumber()
    {
        BigDecimal x, y;

        x = -12345;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(-x, y);

        x = -6.22e23;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(-x, y);

        x = -BigDecimal.Pi;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(-x, y);

        x = -BigDecimal.E;
        y = BigDecimal.Sqrt(BigDecimal.Sqr(x));
        BigDecimalAssert.AreFuzzyEqual(-x, y);
    }

    [TestMethod]
    public void Sqrt_SmallInts_ReturnsCorrectResult()
    {
        for (var i = 1; i <= 100; i++)
        {
            var d = double.Sqrt(i);
            BigDecimal.MaxSigFigs = 55;
            var bd1 = BigDecimal.Sqrt(i);
            Console.WriteLine($"√{i} = {bd1}");
            BigDecimalAssert.AreFuzzyEqual(d, bd1);

            BigDecimal.MaxSigFigs = 50;
            var bd2 = BigDecimal.Sqrt(i);
            Console.WriteLine($"√{i} = {bd2}");
            BigDecimalAssert.AreFuzzyEqual(bd1, bd2);
            Console.WriteLine("");
        }
    }

    [TestMethod]
    public void Sqrt_LargeNumber_ReturnsCorrectResult()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("6.02214076E23");
        var expected = BigDecimal.Parse("776024533117.34932546664032837511112530578432706889"
            + "69571576562989126786337996022194015376088918609909491309813595319711937386010926");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void Sqrt_SmallNumber_ReturnsCorrectResult()
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
    /// </summary>
    [TestMethod]
    public void Sqrt_NumberLargerThanLargestDouble_ReturnsCorrectResult()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.2345678E789");
        var expected = BigDecimal.Parse("3.5136417005722140080009539858670683706660895438958"
            + "9865958869460824551868009859293464600836861863229496438492388219814058056172706E+394");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    /// <summary>
    /// In this test, both the initial argument and the square root are smaller than the smallest
    /// possible double value.
    /// </summary>
    [TestMethod]
    public void Sqrt_NumberSmallerThanSmallestDouble_ReturnsCorrectResult()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.2345678E-789");
        var expected = BigDecimal.Parse("3.5136417005722140080009539858670683706660895438958"
            + "9865958869460824551868009859293464600836861863229496438492388219814058056172706E-395");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void Sqrt_NegativeNumber_ThrowsException()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Sqrt(-123.456));
    }

    #endregion Sqrt() tests

    #region Cbrt() tests

    [TestMethod]
    public void Cbrt_0_ReturnsCorrectResult()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Zero, BigDecimal.Cbrt(0));
    }

    [TestMethod]
    public void Cbrt_1_ReturnsCorrectResult()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.One, BigDecimal.Cbrt(1));
    }

    [TestMethod]
    public void Cbrt_Negative1_ReturnsCorrectResult()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.NegativeOne, BigDecimal.Cbrt(-1));
    }

    /// <summary>Test cube root of values up to 1000.</summary>
    [TestMethod]
    public void Cbrt_SmallIntegers_ReturnsCorrectResult()
    {
        for (var i = 1; i <= 1000; i++)
        {
            // Compare with double.
            BigDecimal.MaxSigFigs = 50;
            double d = double.Cbrt(i);
            BigDecimal bd = BigDecimal.Cbrt(i);
            Console.WriteLine($"³√{i} = {bd:E49}");
            BigDecimalAssert.AreFuzzyEqual(d, bd);

            // Compare with greater sig figs (visual check).
            BigDecimal.MaxSigFigs = 55;
            bd = BigDecimal.Cbrt(i);
            Console.WriteLine($"³√{i} = {bd:E54}");
            Console.WriteLine("");
        }
    }

    [TestMethod]
    public void Cbrt_NegativeNumbers_ReturnsCorrectResult()
    {
        BigDecimal expected;
        BigDecimal actual;

        actual = BigDecimal.Cbrt(-27);
        BigDecimalAssert.AreFuzzyEqual(-3, actual);

        actual = BigDecimal.Cbrt(-16);
        expected = -2.519842099789746m;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        actual = BigDecimal.Cbrt(-1234.5678);
        expected = -10.727659535728732m;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    #endregion Cbrt() tests

    #region RootN() tests

    /// <summary>
    /// This tests Pow() and RootN() with large exponents.
    /// It takes 37 seconds on my computer, so be patient.
    /// </summary>
    [TestMethod]
    public void RootN_PositiveIntegerParameter_LargeN_ReturnsCorrectValue()
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
    public void RootN_PositiveFloatingPointParameter_LargeN_ReturnsCorrectValue()
    {
        var x = BigDecimal.Pi;
        var n = 50;

        // Test BigDecimal.RootN().
        var rootActual = BigDecimal.RootN(x, n);
        Console.WriteLine($"rootActual   = {rootActual}");
        var rootExpected = 1.0231586906017m;
        Console.WriteLine($"rootExpected = {rootExpected}");
        var delta = 0.0000000000001m;
        BigDecimalAssert.AreFuzzyEqual(rootExpected, rootActual, delta);

        // Test BigInteger.Pow().
        var powActual = BigDecimal.Pow(rootActual, n);
        Console.WriteLine($"powActual   = {powActual}");
        var powExpected = x;
        Console.WriteLine($"powExpected = {x}");

        var delta2 = BigDecimal.Parse("0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001");
        BigDecimalAssert.AreFuzzyEqual(powExpected, powActual, delta2);
    }

    /// <summary>
    /// Test odd roots of some negative values.
    /// </summary>
    [TestMethod]
    public void RootN_NegativeParameter_OddN_ReturnsCorrectResult()
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
    public void RootN_NegativeParameter_EvenN_Throws()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-27, 4));
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-16, 6));
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-1234.5678, 16));
    }

    #endregion RootN() tests
}
