namespace Galaxon.BigNumbers;

[TestClass]
public class BigRationalConvertTests2
{
    [TestMethod]
    public void TestFindSingleDigit()
    {
        for (var n = 0; n < 10; n++)
        {
            for (var d = 1; d < 10; d++)
            {
                BigRational f = new (n, d);
                var x = (double)n / d;
                var f2 = (BigRational)x;
                Console.WriteLine($"Testing that {f} == {x}");
                Assert.AreEqual(f, f2);
            }
        }
    }

    [TestMethod]
    public void TestFindHalf()
    {
        var x = 0.5;
        var f = (BigRational)x;
        Assert.AreEqual(1, f.Numerator);
        Assert.AreEqual(2, f.Denominator);
    }

    [TestMethod]
    public void TestFindThird()
    {
        var x = 0.333333333333333;
        var f = (BigRational)x;
        Assert.AreEqual(1, f.Numerator);
        Assert.AreEqual(3, f.Denominator);
    }

    [TestMethod]
    public void TestFindRandom()
    {
        Random rnd = new ();

        // Get a random numerator.
        var n = rnd.Next();

        // Get a random denominator but not 0.
        var d = 0;
        while (d == 0)
        {
            d = rnd.Next();
        }

        BigRational f = new (n, d);
        var x = (double)n / d;
        var f2 = (BigRational)x;
        Console.WriteLine($"f = {f}, x = {x}, f2 = {f2}");
        Assert.AreEqual(f, f2);
    }

    [TestMethod]
    public void TestFindPi()
    {
        var x = PI;
        var f = (BigRational)x;
        var y = (double)f;
        Assert.AreEqual(x, y);
        Assert.AreEqual(245850922, f.Numerator);
        Assert.AreEqual(78256779, f.Denominator);
    }

    // [TestMethod]
    // public void TestFindPiLowerPrecision()
    // {
    //     var x = PI;
    //     var f = (BigRational)x;
    //     Assert.AreEqual(355, f.Numerator);
    //     Assert.AreEqual(113, f.Denominator);
    // }
}
