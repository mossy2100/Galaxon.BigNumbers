using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestConvert
{
    [TestMethod]
    public void TestTryConvertFromCheckedInt()
    {
        int x = 100;
        bool ok = BigDecimal.TryConvertFromChecked(x, out BigDecimal bd);
        Assert.IsTrue(ok);
        Assert.AreEqual(1, (int)bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestTryConvertFromCheckedDouble()
    {
        double x = 123.456789;
        bool ok = BigDecimal.TryConvertFromChecked(x, out BigDecimal bd);
        Assert.IsTrue(ok);
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(-6, bd.Exponent);
    }

    [TestMethod]
    public void TestCastFromDouble()
    {
        double x = 123.456789;
        BigDecimal bd = x;
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(-6, bd.Exponent);
    }

    [TestMethod]
    public void TestCastFromDecimal()
    {
        decimal x;
        BigDecimal bd;

        x = 123.456789m;
        bd = x;
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(-6, bd.Exponent);

        x = 0.00123456789m;
        bd = x;
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(-11, bd.Exponent);

        x = 12345678900m;
        bd = x;
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(2, bd.Exponent);

        x = -123.456789m;
        bd = x;
        Assert.AreEqual(-123456789, (int)bd.Significand);
        Assert.AreEqual(-6, bd.Exponent);

        x = -0.00123456789m;
        bd = x;
        Assert.AreEqual(-123456789, (int)bd.Significand);
        Assert.AreEqual(-11, bd.Exponent);

        x = -12345678900m;
        bd = x;
        Assert.AreEqual(-123456789, (int)bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestCastToDecimal()
    {
        BigDecimal bd;
        decimal x, y;

        x = 123.456789m;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (decimal)bd;
        Assert.AreEqual(x, y);

        x = 0.00123456789m;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (decimal)bd;
        Assert.AreEqual(x, y);

        x = 12345678900m;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (decimal)bd;
        Assert.AreEqual(x, y);

        x = -123.456789m;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (decimal)bd;
        Assert.AreEqual(x, y);

        x = -0.00123456789m;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (decimal)bd;
        Assert.AreEqual(x, y);

        x = -12345678900m;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (decimal)bd;
        Assert.AreEqual(x, y);
    }
}
