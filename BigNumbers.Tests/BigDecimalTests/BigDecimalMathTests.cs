using System.Numerics;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalMathTests
{
    #region Numeric methods tests




    #endregion Numeric methods tests

    [TestMethod]
    public void TestMultiplySmallInts()
    {
        BigDecimal a = 2;
        BigDecimal b = 3;
        var c = a * b;
        Assert.AreEqual(6, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestMultiplySmallFloats()
    {
        BigDecimal a = 1.2345m;
        BigDecimal b = 6.789m;
        var c = a * b;
        Assert.AreEqual(83810205, c.Significand);
        Assert.AreEqual(-7, c.Exponent);
    }

    [TestMethod]
    public void TestMultiplyLargeValues()
    {
        // Calculate radius of Earth.
        BigDecimal r = 6378137; // meters
        var c = BigDecimal.RoundSigFigs(BigDecimal.Tau * r, 15);
        // Should be 40075016.6855785
        Assert.AreEqual(400750166855785, c.Significand);
        Assert.AreEqual(-7, c.Exponent);
    }

    [TestMethod]
    public void TestMultiplyNegatives()
    {
        BigDecimal a = -1.23456789m;
        BigDecimal b = 9.87654321m;
        var c = a * b;
        Assert.AreEqual(-121932631112635269, c.Significand);
        Assert.AreEqual(-16, c.Exponent);

        b = -b;
        c = a * b;
        Assert.AreEqual(121932631112635269, c.Significand);
        Assert.AreEqual(-16, c.Exponent);

        a = -a;
        c = a * b;
        Assert.AreEqual(-121932631112635269, c.Significand);
        Assert.AreEqual(-16, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionSmall()
    {
        BigDecimal a = 1;
        BigDecimal b = 2;
        var c = a / b;
        Assert.AreEqual(5, c.Significand);
        Assert.AreEqual(-1, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionInts()
    {
        BigDecimal a = 10;
        BigDecimal b = 2;
        var c = a / b;
        Assert.AreEqual(5, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionFraction()
    {
        BigDecimal.MaxSigFigs = 50;
        BigDecimal a = 2;
        BigDecimal b = 3;
        var c = a / b;
        Assert.AreEqual(
            BigInteger.Parse("66666666666666666666666666666666666666666666666667"),
            c.Significand);
        Assert.AreEqual(-50, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionFloats()
    {
        BigDecimal c = 40075016.6855785m;
        var r = c / BigDecimal.Tau;
        r = BigDecimal.RoundSigFigs(r, 7);
        Assert.AreEqual(6378137, r.Significand);
        Assert.AreEqual(0, r.Exponent);
    }

    [TestMethod]
    public void TestModSmallInts()
    {
        BigDecimal a = 7;
        BigDecimal b = 2;
        var c = a % b;
        Assert.AreEqual(1, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestModSmallDecimals()
    {
        BigDecimal a = 7.6543m;
        BigDecimal b = 2.3456m;
        var c = a % b;
        Assert.AreEqual(6175, c.Significand);
        Assert.AreEqual(-4, c.Exponent);
    }

    [TestMethod]
    public void TestModNegativeIntDivisor()
    {
        BigDecimal a = 8;
        BigDecimal b = -3;
        var c = a % b;
        Assert.AreEqual(-1, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestModNegativeIntDividend()
    {
        BigDecimal a = -8;
        BigDecimal b = 3;
        var c = a % b;
        Assert.AreEqual(1, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestModAngle1()
    {
        var a = -BigDecimal.Tau + 0.1m;
        var b = BigDecimal.Tau;
        var c = a % b;
        Assert.AreEqual(1, c.Significand);
        Assert.AreEqual(-1, c.Exponent);
    }

    [TestMethod]
    public void TestAgm1()
    {
        BigDecimal a;
        BigDecimal b;
        BigDecimal expected;
        BigDecimal actual;

        BigDecimal.MaxSigFigs = 30;

        a = 4;
        b = 5;
        expected = 4.4860572m;
        actual = BigDecimal.RoundSigFigs(BigDecimal.ArithmeticGeometricMean(a, b), 8);
        Assert.AreEqual(expected, actual);

        a = 7;
        b = 100;
        expected = 38.7918476m;
        actual = BigDecimal.RoundSigFigs(BigDecimal.ArithmeticGeometricMean(a, b), 9);
        Assert.AreEqual(expected, actual);
    }
}
