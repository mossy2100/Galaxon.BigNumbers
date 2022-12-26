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

    // [TestMethod]
    // public void TestSqrt2()
    // {
    //     BigDecimal bd = BigDecimal.Sqrt(2);
    //     Assert.AreEqual(BigDecimal.Sqrt2, bd);
    // }
    //
    // [TestMethod]
    // public void TestSqrt10()
    // {
    //     BigDecimal bd = BigDecimal.Sqrt(10);
    //     Assert.AreEqual(BigDecimal.Sqrt10, bd);
    // }

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
        for (int i = 1; i <= 1000; i++)
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
        Trace.WriteLine(BigDecimal.Sqrt(6.02214076E23).ToString());
    }

    [TestMethod]
    public void TestSqrtSmall()
    {
        Trace.WriteLine(BigDecimal.Sqrt(1.602176634E-19).ToString());
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
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigDecimal.Sqrt(-1));
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

    [TestMethod]
    public void TestExp()
    {
        BigDecimal.MaxSigFigs = 50;
        Assert.AreEqual(1, BigDecimal.Exp(0));
        Assert.AreEqual(BigDecimal.E, BigDecimal.Exp(1));
        Assert.AreEqual(BigDecimal.Parse("1.64872127070012814684865078781416357165377610071014801"),
            BigDecimal.Exp(0.5));
        Assert.AreEqual(BigDecimal.Parse("7.38905609893065022723042746057500781318031557055184732"),
            BigDecimal.Exp(2));
        Assert.AreEqual(BigDecimal.Parse("20.0855369231876677409285296545817178969879078385541501"),
            BigDecimal.Exp(3));
        Assert.AreEqual(BigDecimal.Parse("22026.4657948067165169579006452842443663535126185567811"),
            BigDecimal.Exp(10));
        Assert.AreEqual(BigDecimal.Parse("0.135335283236612691893999494972484403407631545909575882"),
            BigDecimal.Exp(-2));
    }

    [TestMethod]
    public void TestPowNegativeBaseFractionalExp()
    {
        BigDecimal x = -32;
        BigDecimal y = 0.2;
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
        Assert.ThrowsException<ArgumentInvalidException>(() => BigDecimal.RootN(x, y));
    }

    /// <summary>
    /// This shows how the cube root of a negative value cannot be achieved using Pow(), because a
    /// BigDecimal cannot represent 1/3 exactly. You have to use Cbrt() or RootN().
    /// </summary>
    [TestMethod]
    public void TestCubeRootOfNegativeValueUsingPow()
    {
        BigDecimal oneThird = BigDecimal.One / 3;
        Assert.ThrowsException<ArgumentInvalidException>(() => BigDecimal.Pow(-27, oneThird));
    }

    [TestMethod]
    public void TestECalculation()
    {
        // Use the built-in value.
        BigDecimal.MaxSigFigs = 50;
        BigDecimal expected = BigDecimal.RoundSigFigsMax(BigDecimal.E);
        Trace.WriteLine(expected);
        BigDecimal actual = BigDecimal.Exp(1);
        Trace.WriteLine(actual);
        Assert.AreEqual(expected, actual);

        // Calculate the value.
        BigDecimal.MaxSigFigs = 200;
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
}
