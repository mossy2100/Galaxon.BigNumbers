namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalCompareTests
{
    private static IEnumerable<object[]> LessThanInts =>
        new[]
        {
            new object[] { -456, -123 },
            new object[] { -456, 0 },
            new object[] { -456, 123 },
            new object[] { 0, 123 },
            new object[] { 123, 456 }
        };

    private static IEnumerable<object[]> GreaterThanInts =>
        new[]
        {
            new object[] { -123, -456 },
            new object[] { 0, -456 },
            new object[] { 123, -456 },
            new object[] { 123, 0 },
            new object[] { 456, 123 }
        };

    private static IEnumerable<object[]> EqualInts =>
        new[]
        {
            new object[] { 0, 0 },
            new object[] { -123, -123 },
            new object[] { 123, 123 }
        };

    private static IEnumerable<object[]> LessThanDoubles =>
        new[]
        {
            new object[] { -456.789, -123.456 },
            new object[] { -456.789, 0.0 },
            new object[] { -456.789, 123.456 },
            new object[] { 0.0, 123.456 },
            new object[] { 123.456, 456.789 }
        };

    private static IEnumerable<object[]> GreaterThanDoubles =>
        new[]
        {
            new object[] { -123.456, -456.789 },
            new object[] { 0.0, -456.789 },
            new object[] { 123.456, -456.789 },
            new object[] { 123.456, 0.0 },
            new object[] { 456.789, 123.456 }
        };

    private static IEnumerable<object[]> EqualDoubles =>
        new[]
        {
            new object[] { 0.0, 0.0 },
            new object[] { -123.456, -123.456 },
            new object[] { 123.456, 123.456 }
        };

    [TestMethod]
    [DynamicData(nameof(LessThanInts))]
    public void TestCompareToLessThanWithInts(int x, int y)
    {
        Assert.AreEqual(-1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(GreaterThanInts))]
    public void TestCompareToGreaterThanWithInts(int x, int y)
    {
        Assert.AreEqual(1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(EqualInts))]
    public void TestCompareToEqualWithInts(int x, int y)
    {
        Assert.AreEqual(0, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(LessThanDoubles))]
    public void TestCompareToLessThanWithDoubles(double x, double y)
    {
        Assert.AreEqual(-1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(GreaterThanDoubles))]
    public void TestCompareToGreaterThanWithDoubles(double x, double y)
    {
        Assert.AreEqual(1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(EqualDoubles))]
    public void TestCompareToEqualWithDoubles(double x, double y)
    {
        Assert.AreEqual(0, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(LessThanInts))]
    public void TestLessThanWithInts(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w < z);
    }

    [TestMethod]
    [DynamicData(nameof(GreaterThanInts))]
    public void TestGreaterThanWithInts(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w > z);
    }

    [TestMethod]
    [DynamicData(nameof(EqualInts))]
    public void TestEqualWithInts(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w == z);
    }

    [TestMethod]
    [DynamicData(nameof(LessThanInts))]
    [DynamicData(nameof(EqualInts))]
    public void TestLessThanOrEqualWithInts(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w <= z);
    }

    [TestMethod]
    [DynamicData(nameof(GreaterThanInts))]
    [DynamicData(nameof(EqualInts))]
    public void TestGreaterThanOrEqualWithInts(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w >= z);
    }

    [TestMethod]
    [DynamicData(nameof(LessThanInts))]
    [DynamicData(nameof(GreaterThanInts))]
    public void TestNotEqualWithInts(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w != z);
    }

    [TestMethod]
    [DynamicData(nameof(LessThanDoubles))]
    public void TestLessThanWithDoubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w < z);
    }

    [TestMethod]
    [DynamicData(nameof(GreaterThanDoubles))]
    public void TestGreaterThanWithDoubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w > z);
    }

    [TestMethod]
    [DynamicData(nameof(EqualDoubles))]
    public void TestEqualWithDoubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w == z);
    }

    [TestMethod]
    [DynamicData(nameof(LessThanDoubles))]
    [DynamicData(nameof(EqualDoubles))]
    public void TestLessThanOrEqualWithDoubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w <= z);
    }

    [TestMethod]
    [DynamicData(nameof(GreaterThanDoubles))]
    [DynamicData(nameof(EqualDoubles))]
    public void TestGreaterThanOrEqualWithDoubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w >= z);
    }

    [TestMethod]
    [DynamicData(nameof(LessThanDoubles))]
    [DynamicData(nameof(GreaterThanDoubles))]
    public void TestNotEqualWithDoubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w != z);
    }

    #region Equals

    [TestMethod]
    public void Equals_EqualValues_ReturnsTrue()
    {
        BigDecimal x = 123;
        BigDecimal y = 123;
        Assert.IsTrue(x.Equals(y));
    }

    [TestMethod]
    public void Equals_InequalValues_ReturnsTrue()
    {
        BigDecimal x = 123;
        BigDecimal y = -456;
        Assert.IsFalse(x.Equals(y));
    }

    [TestMethod]
    public void Equals_Null_ReturnsFalse()
    {
        BigDecimal x = 123;
        Assert.IsFalse(x.Equals(null));
    }

    [TestMethod]
    public void Equals_NotBigDecimal_ReturnsFalse()
    {
        BigDecimal x = 123;
        var f = 456.78f;
        Assert.IsFalse(x.Equals(f));
    }

    #endregion Equals
}
