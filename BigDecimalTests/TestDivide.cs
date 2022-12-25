using System.Numerics;
using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestDivide
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
}
