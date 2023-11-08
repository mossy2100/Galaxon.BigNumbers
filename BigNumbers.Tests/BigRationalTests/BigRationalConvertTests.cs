namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigRationalConvertTests
{
    [TestMethod]
    public void TestCastFromInt()
    {
        var a = 5;
        BigRational f = a;
        Assert.AreEqual(5, f.Numerator);
        Assert.AreEqual(1, f.Denominator);
    }

    [TestMethod]
    public void TestCastFromDoubleExact()
    {
        double d = 5;
        BigRational f = d;

        Console.WriteLine(d);
        Console.WriteLine(f.Numerator);
        Console.WriteLine(f.Denominator);
        Console.WriteLine((double)f.Numerator / (double)f.Denominator);

        Assert.AreEqual(5, f.Numerator);
        Assert.AreEqual(1, f.Denominator);
    }

    /// <summary>
    /// This example shows how a double converts exactly to a BigRational, but also how the double
    /// only stores an approximation of the original decimal fraction.
    /// </summary>
    [TestMethod]
    public void TestCastFromDoubleFuzzy()
    {
        var d = 0.1;
        BigRational f = d;

        Console.WriteLine(d);
        Console.WriteLine(f.Numerator);
        Console.WriteLine(f.Denominator);

        var d2 = (double)f.Numerator / (double)f.Denominator;
        Console.WriteLine(d2);

        Assert.AreEqual(3602879701896397, f.Numerator);
        Assert.AreEqual(36028797018963968, f.Denominator);
        Assert.AreEqual(d, d2);
    }
}
