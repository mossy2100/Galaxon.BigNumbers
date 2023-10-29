namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigRationalExpTests
{
    [TestMethod]
    public void TestPowWithIntegerExponent()
    {
        BigRational f = new (2, 3);
        var g = f ^ 2;
        Assert.AreEqual(4, g.Numerator);
        Assert.AreEqual(9, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithNegativeOneExponent()
    {
        BigRational f = new (2, 3);
        var g = f ^ -1;
        Assert.AreEqual(3, g.Numerator);
        Assert.AreEqual(2, g.Denominator);
    }

    [TestMethod]
    public void TestPowWithNegativeIntegerExponent()
    {
        BigRational f = new (2, 3);
        var g = f ^ -2;
        Assert.AreEqual(9, g.Numerator);
        Assert.AreEqual(4, g.Denominator);
    }
}
