namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigRationalStringsTests
{
    [TestMethod]
    public void TestToString()
    {
        BigRational f = new (3, 4);
        Assert.AreEqual("3/4", f.ToString("A"));
        Assert.AreEqual("³/₄", f.ToString("U"));
        Assert.AreEqual("³/₄", f.ToString("M"));

        f += 2;
        Assert.AreEqual("11/4", f.ToString("A"));
        Assert.AreEqual("¹¹/₄", f.ToString("U"));
        Assert.AreEqual("2³/₄", f.ToString("M"));
    }
}
