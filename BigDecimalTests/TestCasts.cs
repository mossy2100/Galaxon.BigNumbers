using Galaxon.Numerics.Types;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestCasts
{
    [TestMethod]
    public void TestCastToInt()
    {
        BigDecimal bd = 0;
        Assert.AreEqual(0, (int)bd);

        bd = 1;
        Assert.AreEqual(1, (int)bd);

        bd = int.MaxValue;
        Assert.AreEqual(int.MaxValue, (int)bd);

        bd = int.MinValue;
        Assert.AreEqual(int.MinValue, (int)bd);
    }

    [TestMethod]
    public void TestCastToIntTooBig()
    {
        BigDecimal bd = (BigDecimal)int.MaxValue + 1;
        Assert.ThrowsException<OverflowException>(() => (int)bd);
    }

    [TestMethod]
    public void TestCastToIntTooBigNegative()
    {
        BigDecimal bd = (BigDecimal)int.MinValue - 1;
        Assert.ThrowsException<OverflowException>(() => (int)bd);
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
    public void TestCastToDecimal1()
    {
        decimal m;
        BigDecimal bd;

        bd = 0;
        m = 0;
        Assert.AreEqual(m, (decimal)bd);

        bd = 1;
        m = 1;
        Assert.AreEqual(m, (decimal)bd);

        bd = decimal.MinValue;
        m = decimal.MinValue;
        Assert.AreEqual(m, (decimal)bd);

        bd = decimal.MaxValue;
        m = decimal.MaxValue;
        Assert.AreEqual(m, (decimal)bd);
    }

    [TestMethod]
    public void TestCastToDecimal2()
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
    public void TestCastToDecimalTooBig()
    {
        BigDecimal bd = (BigDecimal)decimal.MaxValue + 1;
        Assert.ThrowsException<OverflowException>(() => (decimal)bd);
    }

    [TestMethod]
    public void TestCastToDecimalTooSmall()
    {
        BigDecimal bd = new (1, -29);
        Assert.ThrowsException<OverflowException>(() => (decimal)bd);
    }

    [TestMethod]
    public void TestCastToDecimalTooBigNegative()
    {
        BigDecimal bd = (BigDecimal)decimal.MinValue - 1;
        Assert.ThrowsException<OverflowException>(() => (decimal)bd);
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

    [TestMethod]
    public void TestCastFromDouble()
    {
        double x;
        BigDecimal bd;

        // Ordinary value.
        x = 123.456789;
        bd = (BigDecimal)x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 9);
        Assert.AreEqual(123456789, (int)bd.Significand);
        Assert.AreEqual(-6, bd.Exponent);

        // Integer value.
        x = 12345;
        bd = (BigDecimal)x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 5);
        Assert.AreEqual(12345, (int)bd.Significand);
        Assert.AreEqual(0, bd.Exponent);

        // Approximate minimum positive subnormal value.
        x = 4.94e-324;
        bd = (BigDecimal)x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 3);
        Assert.AreEqual(494, (int)bd.Significand);
        Assert.AreEqual(-326, bd.Exponent);

        // Approximate maximum positive subnormal value.
        x = 2.225e-308;
        bd = (BigDecimal)x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(2225, (int)bd.Significand);
        Assert.AreEqual(-311, bd.Exponent);

        // Approximate minimum positive normal value.
        x = 2.226e-308;
        bd = (BigDecimal)x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(2226, (int)bd.Significand);
        Assert.AreEqual(-311, bd.Exponent);

        // Maximum positive normal value.
        x = double.MaxValue;
        bd = (BigDecimal)x;
        Assert.AreEqual(x.ToString("E16"), bd.ToString("E16"));
        bd = BigDecimal.RoundSigFigs(bd, BigDecimal.DoubleMaxSigFigs);
        Assert.AreEqual(17976931348623157, (long)bd.Significand);
        Assert.AreEqual(292, bd.Exponent);
    }
}
