namespace Galaxon.BigNumbers.Tests;

/// <summary>
/// Test the Align() method. This method should probably be private, but I want to make sure it
/// works.
/// </summary>
[TestClass]
public class BigDecimalAlignTests
{
    [TestMethod]
    public void Align_EqualExponents_NoChange()
    {
        var x = new BigDecimal(123, 456);
        var y = new BigDecimal(789, 456);
        var (a, b, c) = BigDecimal.Align(x, y);
        Assert.AreEqual(a, 123);
        Assert.AreEqual(b, 789);
        Assert.AreEqual(c, 456);
    }

    [TestMethod]
    public void Align_LargerExponentFirst_FirstOperandAdjusted()
    {
        var x = new BigDecimal(123, 5);
        var y = new BigDecimal(789, 3);
        var (a, b, c) = BigDecimal.Align(x, y);
        Assert.AreEqual(a, 12300);
        Assert.AreEqual(b, 789);
        Assert.AreEqual(c, 3);
    }

    [TestMethod]
    public void Align_SmallerExponentFirst_SecondOperandAdjusted()
    {
        var x = new BigDecimal(123, 4);
        var y = new BigDecimal(789, 8);
        var (a, b, c) = BigDecimal.Align(x, y);
        Assert.AreEqual(a, 123);
        Assert.AreEqual(b, 7890000);
        Assert.AreEqual(c, 4);
    }
}
