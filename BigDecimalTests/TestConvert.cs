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
        double x;
        BigDecimal bd;

        // Ordinary value.
        x = 123.456789;
        bd = x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 9);
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(-6, bd.Exponent);

        // Integer value.
        x = 12345;
        bd = x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 5);
        Assert.AreEqual(12345, (int)bd.Significand);
        Assert.AreEqual(0, bd.Exponent);

        // Approximate minimum positive subnormal value.
        x = 4.94e-324;
        bd = x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 3);
        Assert.AreEqual(494, (int)bd.Significand);
        Assert.AreEqual(-326, bd.Exponent);

        // Approximate maximum positive subnormal value.
        x = 2.225e-308;
        bd = x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(2225, (int)bd.Significand);
        Assert.AreEqual(-311, bd.Exponent);

        // Approximate minimum positive normal value.
        x = 2.226e-308;
        bd = x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(2226, (int)bd.Significand);
        Assert.AreEqual(-311, bd.Exponent);

        // Maximum positive normal value.
        x = double.MaxValue;
        bd = x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 17);
        Assert.AreEqual(17976931348623157, (long)bd.Significand);
        Assert.AreEqual(292, bd.Exponent);
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

    [TestMethod]
    public void TestCastToDouble()
    {
        BigDecimal bd;
        double x, y;

        x = 123.456789;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (double)bd;
        Assert.AreEqual(x, y);

        x = 0.00123456789;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (double)bd;
        Assert.AreEqual(x, y);

        x = 12345678900;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (double)bd;
        Assert.AreEqual(x, y);

        x = -123.456789;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (double)bd;
        Assert.AreEqual(x, y);

        x = -0.00123456789;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (double)bd;
        Assert.AreEqual(x, y);

        x = -12345678900;
        bd = BigDecimal.Parse(x.ToString("G30"));
        y = (double)bd;
        Assert.AreEqual(x, y);
    }
}
