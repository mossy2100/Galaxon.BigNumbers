using System.Diagnostics;
using System.Numerics;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestArithmeticOperators
{
    [TestMethod]
    public void TestDivisionSmall()
    {
        BigDecimal a = 1;
        BigDecimal b = 2;
        BigDecimal c = a / b;
        Assert.AreEqual(5, c.Significand);
        Assert.AreEqual(-1, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionInts()
    {
        BigDecimal a = 10;
        BigDecimal b = 2;
        BigDecimal c = a / b;
        Assert.AreEqual(5, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionFraction()
    {
        BigDecimal a = 2;
        BigDecimal b = 3;
        BigDecimal c = a / b;
        Assert.AreEqual(
            BigInteger.Parse("66666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666666667"),
            c.Significand);
        Assert.AreEqual(-101, c.Exponent);
    }

    [TestMethod]
    public void TestDivisionFloats()
    {
        BigDecimal c = 40075016.6855785;
        BigDecimal r = c / BigDecimal.Tau;
        r = BigDecimal.RoundSigFigs(r, 7);
        Assert.AreEqual(6378137, r.Significand);
        Assert.AreEqual(0, r.Exponent);
    }

    [TestMethod]
    public void TestModSmallInts()
    {
        BigDecimal a = 7;
        BigDecimal b = 2;
        BigDecimal c = a % b;
        Assert.AreEqual(1, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestModSmallDecimals()
    {
        BigDecimal a = 7.6543m;
        BigDecimal b = 2.3456m;
        BigDecimal c = a % b;
        Assert.AreEqual(6175, c.Significand);
        Assert.AreEqual(-4, c.Exponent);
    }

    [TestMethod]
    public void TestModNegativeIntDivisor()
    {
        BigDecimal a = 8;
        BigDecimal b = -3;
        BigDecimal c = a % b;
        Assert.AreEqual(2, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestModNegativeIntDividend()
    {
        BigDecimal a = -8;
        BigDecimal b = 3;
        BigDecimal c = a % b;
        Assert.AreEqual(-2, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestModAngle1()
    {
        BigDecimal a = -BigDecimal.Tau  + 0.1m;
        BigDecimal b = BigDecimal.Tau;
        BigDecimal c = a % b;
        if (c < 0)
        {
            c += BigDecimal.Tau;
        }
        Trace.WriteLine(c);
    }
}
