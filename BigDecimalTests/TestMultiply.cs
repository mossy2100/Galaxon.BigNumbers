using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestMultiply
{
    [TestMethod]
    public void TestMultiplySmallInts()
    {
        BigDecimal a = 2;
        BigDecimal b = 3;
        BigDecimal c = a * b;
        Assert.AreEqual(6, c.Significand);
        Assert.AreEqual(0, c.Exponent);
    }

    [TestMethod]
    public void TestMultiplySmallFloats()
    {
        BigDecimal a = 1.2345m;
        BigDecimal b = 6.789m;
        BigDecimal c = a * b;
        Assert.AreEqual(83810205, c.Significand);
        Assert.AreEqual(-7, c.Exponent);
    }

    [TestMethod]
    public void TestMultiplyLargeValues()
    {
        // Calculate radius of Earth.
        BigDecimal r = 6378137; // meters
        BigDecimal c = BigDecimal.RoundSigFigs(BigDecimal.Tau * r, 15);
        // Should be 40075016.6855785
        Assert.AreEqual(400750166855785, c.Significand);
        Assert.AreEqual(-7, c.Exponent);
    }

    [TestMethod]
    public void TestMultiplyNegatives()
    {
        BigDecimal a = -1.23456789m;
        BigDecimal b = 9.87654321m;
        BigDecimal c = a * b;
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
}
