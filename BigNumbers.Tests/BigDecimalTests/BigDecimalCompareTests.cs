using Galaxon.BigNumbers.TestTools;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

/// <summary>Test the methods in BigDecimalCompare.cs.</summary>
[TestClass]
public class BigDecimalCompareTests
{
    #region ULP tests

    [TestMethod]
    public void ULP_Decimal_Tests()
    {
        decimal m;
        BigDecimal ulp;

        // Integer.
        m = 123456789;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(1, ulp);

        // Multiple of 10 does not give 10 for the ULP. Proves maximum ULP for decimal is 1.
        m = 1234567890;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(1, ulp);

        m = 12345678.9m;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(0.1m, ulp);

        m = 123456.789m;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(0.001m, ulp);

        m = 1.23456789e-10m;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(1e-18m, ulp);

        m = -123456789;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(1, ulp);

        m = -12345678.9m;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(0.1m, ulp);

        m = -123456.789m;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(0.001m, ulp);

        m = -1.23456789e-10m;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(1e-18m, ulp);

        m = decimal.MaxValue;
        ulp = BigDecimal.UnitOfLeastPrecision(m);
        Assert.AreEqual(1, ulp);
    }

    #endregion ULP tests
    #region Data

    private static IEnumerable<object[]> _LessThanInts =>
        new[]
        {
            // Different sign.
            new object[] { -456, 0 },
            new object[] { 0, 123 },
            new object[] { -456, 123 },
            // Same sign, different exponent.
            new object[] { -123789, -456 },
            new object[] { 456, 123789 },
            // Same sign, same exponent.
            new object[] { -456, -123 },
            new object[] { 123, 456 }
        };

    private static IEnumerable<object[]> _GreaterThanInts =>
        new[]
        {
            // Different sign.
            new object[] { 0, -456 },
            new object[] { 123, 0 },
            new object[] { 123, -456 },
            // Same sign, different exponent.
            new object[] { -456, -123789 },
            new object[] { 123789, 456 },
            // Same sign, same exponent.
            new object[] { -123, -456 },
            new object[] { 456, 123 }
        };

    private static IEnumerable<object[]> _EqualInts =>
        new[]
        {
            new object[] { 0, 0 },
            new object[] { -123, -123 },
            new object[] { 456, 456 }
        };

    private static IEnumerable<object[]> _LessThanDoubles =>
        new[]
        {
            // Different sign.
            new object[] { -123.456, 0 },
            new object[] { 0, 123.456 },
            new object[] { -456.789, 123.456 },
            // Same sign, different exponent.
            new object[] { -123456.789, -987.654 },
            new object[] { 456.789, 987654.321 },
            // Same sign, same exponent.
            new object[] { -456.789, -123.456 },
            new object[] { 123.456, 456.789 }
        };

    private static IEnumerable<object[]> _GreaterThanDoubles =>
        new[]
        {
            // Different sign.
            new object[] { 0, -123.456 },
            new object[] { 123.456, 0 },
            new object[] { 123.456, -456.789 },
            // Same sign, different exponent.
            new object[] { -987.654, -123456.789 },
            new object[] { 987654.321, 456.789 },
            // Same sign, same exponent.
            new object[] { -123.456, -456.789 },
            new object[] { 456.789, 123.456 }
        };

    private static IEnumerable<object[]> _EqualDoubles =>
        new[]
        {
            new object[] { 0.0, 0.0 },
            new object[] { -123.456, -123.456 },
            new object[] { 123.456, 123.456 }
        };

    #endregion Data

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

    #region FuzzyEquals

    [TestMethod]
    public void FuzzyEquals_HalfValues_ReturnTrue()
    {
        var n = 10;
        var rnd = new Random();
        for (var i = 0; i < n; i++)
        {
            // A divide operation should introduce some diversion in precision.
            var f1 = rnd.NextHalf();
            var f2 = rnd.NextHalf();
            var f3 = f1 / f2;
            var bd3 = (BigDecimal)f1 / f2;

            if (!Half.IsFinite(f3))
            {
                continue;
            }

            Console.WriteLine($"      Half value: {f3:E20}");
            Console.WriteLine($"BigDecimal value: {bd3:E20}");
            Console.WriteLine();
            Assert.IsTrue(bd3.FuzzyEquals(f3));
        }
    }

    [TestMethod]
    public void FuzzyEquals_FloatValues_ReturnTrue()
    {
        var n = 10;
        var rnd = new Random();
        for (var i = 0; i < n; i++)
        {
            // A divide operation should introduce some diversion in precision.
            var f1 = rnd.NextFloat();
            var f2 = rnd.NextFloat();
            var f3 = f1 / f2;
            var bd3 = (BigDecimal)f1 / f2;

            if (!float.IsFinite(f3))
            {
                continue;
            }

            Console.WriteLine($"     float value: {f3:E20}");
            Console.WriteLine($"BigDecimal value: {bd3:E20}");
            Console.WriteLine();
            BigDecimalAssert.AreFuzzyEqual(f3, bd3);
        }
    }

    [TestMethod]
    public void FuzzyEquals_DoubleValues_ReturnTrue()
    {
        var n = 10;
        var rnd = new Random();
        for (var i = 0; i < n; i++)
        {
            // A divide operation should introduce some diversion in precision.
            var f1 = rnd.NextDoubleFullRange();
            var f2 = rnd.NextDoubleFullRange();
            var f3 = f1 / f2;
            var bd3 = (BigDecimal)f1 / f2;

            if (!double.IsFinite(f3))
            {
                continue;
            }

            Console.WriteLine($"BigDecimal value: {bd3:E20}");
            Console.WriteLine($"    double value: {f3:E20}");
            Assert.IsTrue(bd3.FuzzyEquals(f3));
            Console.WriteLine();
        }
    }

    [TestMethod]
    public void FuzzyEquals_Pi_ReturnTrue()
    {
        var bdPi = BigDecimal.Pi;
        var hPi = Half.Pi;
        var fPi = float.Pi;
        var dPi = double.Pi;
        Assert.IsTrue(bdPi.FuzzyEquals(hPi));
        Assert.IsTrue(bdPi.FuzzyEquals(fPi));
        Assert.IsTrue(bdPi.FuzzyEquals(dPi));
    }

    [TestMethod]
    public void FuzzyEquals_E_ReturnTrue()
    {
        var bdE = BigDecimal.E;
        var hE = Half.E;
        var fE = float.E;
        var dE = double.E;
        Assert.IsTrue(bdE.FuzzyEquals(hE));
        Assert.IsTrue(bdE.FuzzyEquals(fE));
        Assert.IsTrue(bdE.FuzzyEquals(dE));
    }

    #endregion FuzzyEquals

    #region CompareTo tests

    [TestMethod]
    [DynamicData(nameof(_LessThanInts))]
    public void CompareTo_LessThan_Ints(int x, int y)
    {
        Assert.AreEqual(-1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(_GreaterThanInts))]
    public void CompareTo_GreaterThan_Ints(int x, int y)
    {
        Assert.AreEqual(1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(_EqualInts))]
    public void CompareTo_Equal_Ints(int x, int y)
    {
        Assert.AreEqual(0, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(_LessThanDoubles))]
    public void CompareTo_LessThan_Doubles(double x, double y)
    {
        Assert.AreEqual(-1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(_GreaterThanDoubles))]
    public void CompareTo_GreaterThan_Doubles(double x, double y)
    {
        Assert.AreEqual(1, ((BigDecimal)x).CompareTo(y));
    }

    [TestMethod]
    [DynamicData(nameof(_EqualDoubles))]
    public void CompareTo_Equal_Doubles(double x, double y)
    {
        Assert.AreEqual(0, ((BigDecimal)x).CompareTo(y));
    }

    #endregion CompareTo tests

    #region Comparison operator tests

    [TestMethod]
    [DynamicData(nameof(_LessThanInts))]
    public void LessThanOp_Ints(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w < z);
    }

    [TestMethod]
    [DynamicData(nameof(_GreaterThanInts))]
    public void GreaterThanOp_Ints(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w > z);
    }

    [TestMethod]
    [DynamicData(nameof(_EqualInts))]
    public void EqualOp_Ints(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w == z);
    }

    [TestMethod]
    [DynamicData(nameof(_LessThanInts))]
    [DynamicData(nameof(_EqualInts))]
    public void LessThanOrEqualOp_Ints(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w <= z);
    }

    [TestMethod]
    [DynamicData(nameof(_GreaterThanInts))]
    [DynamicData(nameof(_EqualInts))]
    public void GreaterThanOrEqualOp_Ints(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w >= z);
    }

    [TestMethod]
    [DynamicData(nameof(_LessThanInts))]
    [DynamicData(nameof(_GreaterThanInts))]
    public void NotEqualOp_Ints(int x, int y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w != z);
    }

    [TestMethod]
    [DynamicData(nameof(_LessThanDoubles))]
    public void LessThanOp_Doubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w < z);
    }

    [TestMethod]
    [DynamicData(nameof(_GreaterThanDoubles))]
    public void GreaterThanOp_Doubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w > z);
    }

    [TestMethod]
    [DynamicData(nameof(_EqualDoubles))]
    public void EqualOp_Doubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w == z);
    }

    [TestMethod]
    [DynamicData(nameof(_LessThanDoubles))]
    [DynamicData(nameof(_EqualDoubles))]
    public void LessThanOrEqualOp_Doubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w <= z);
    }

    [TestMethod]
    [DynamicData(nameof(_GreaterThanDoubles))]
    [DynamicData(nameof(_EqualDoubles))]
    public void GreaterThanOrEqualOp_Doubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w >= z);
    }

    [TestMethod]
    [DynamicData(nameof(_LessThanDoubles))]
    [DynamicData(nameof(_GreaterThanDoubles))]
    public void NotEqualOp_Doubles(double x, double y)
    {
        BigDecimal w = x;
        BigDecimal z = y;
        Assert.IsTrue(w != z);
    }

    #endregion Comparison operator tests
}
