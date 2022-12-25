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
}
