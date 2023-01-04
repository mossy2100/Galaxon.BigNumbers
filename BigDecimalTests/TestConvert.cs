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
}
