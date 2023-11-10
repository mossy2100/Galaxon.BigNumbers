using System.Numerics;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalConstructorTests
{
    [TestMethod]
    public void TestMainConstructor()
    {
        var x = new BigDecimal(123, 456);
        Assert.AreEqual(123, x.Significand);
        Assert.AreEqual(456, x.Exponent);
    }

    [TestMethod]
    public void TestConstructorMakesValueCanonical()
    {
        var x = new BigDecimal(123000, 456);
        Assert.AreEqual(123, x.Significand);
        Assert.AreEqual(459, x.Exponent);

        x = new BigDecimal(123000, -12);
        Assert.AreEqual(123, x.Significand);
        Assert.AreEqual(-9, x.Exponent);
    }

    [TestMethod]
    public void TestConstructorMakesValueCanonicalZero()
    {
        var x = new BigDecimal(0, 456);
        Assert.AreEqual(0, x.Significand);
        Assert.AreEqual(0, x.Exponent);
    }

    [TestMethod]
    public void TestIntegerConstructor()
    {
        var x = new BigDecimal(123);
        Assert.AreEqual(123, x.Significand);
        Assert.AreEqual(0, x.Exponent);

        var y = BigInteger.Parse("-12345678901234567890123456789012345678901234567890");
        x = new BigDecimal(y);
        Assert.AreEqual(y / 10, x.Significand);
        Assert.AreEqual(1, x.Exponent);
    }

    [TestMethod]
    public void TestZeroConstructor()
    {
        var x = new BigDecimal();
        Assert.AreEqual(0, x.Significand);
        Assert.AreEqual(0, x.Exponent);
    }
}
