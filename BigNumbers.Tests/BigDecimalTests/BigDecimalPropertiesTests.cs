namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalPropertiesTests
{
    [TestMethod]
    public void TestSetSignificand()
    {
        var x = new BigDecimal(123, 456);
        x.Significand = 789;
        Assert.AreEqual(789, x.Significand);
        Assert.AreEqual(456, x.Exponent);
    }

    [TestMethod]
    public void TestSetSignificandMakesCanonical()
    {
        var x = new BigDecimal(123, 456);
        x.Significand = 789000;
        Assert.AreEqual(789, x.Significand);
        Assert.AreEqual(459, x.Exponent);
    }

    [TestMethod]
    public void TestSetExponent()
    {
        var x = new BigDecimal(123, 456);
        x.Exponent = 789;
        Assert.AreEqual(123, x.Significand);
        Assert.AreEqual(789, x.Exponent);
    }

    [TestMethod]
    public void TestGetDigitsStringAndGetNumSigFigs()
    {
        var x = new BigDecimal(123, 456);
        Assert.AreEqual("123", x.DigitsString);
        Assert.AreEqual(3, x.NumSigFigs);
    }

    [TestMethod]
    public void TestChangingSignificandChangesDigitsStringAndNumSigFigs()
    {
        var x = new BigDecimal(123, 456);
        x.Significand = 708090;
        Assert.AreEqual("70809", x.DigitsString);
        Assert.AreEqual(5, x.NumSigFigs);
    }
}
