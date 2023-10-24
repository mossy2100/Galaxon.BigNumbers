using System.Diagnostics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.BigDecimalTests;

[TestClass]
public class TrigTests
{
    public const int Denominator = 12;

    public static IEnumerable<object[]> Numerators
    {
        get
        {
            List<object[]> fractions = new ();
            for (var i = -Denominator; i <= Denominator; i++)
            {
                fractions.Add(new object[] { i });
            }
            return fractions;
        }
    }

    [ClassInitialize]
    public static void Initialize(TestContext context)
    {
        BigDecimal.MaxSigFigs = 30;
    }

    private static bool DoubleEqualsBigDecimal(double d, BigDecimal bd)
    {
        return d.FuzzyEquals((double)bd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestSin(int i)
    {
        var d = i * double.Tau / Denominator;
        var sinD = double.Sin(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var sinBd = BigDecimal.Sin(bd);

        BigDecimal.AssertAreEqual(sinD, sinBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestCos(int i)
    {
        var d = i * double.Tau / Denominator;
        var cosD = double.Cos(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var cosBd = BigDecimal.Cos(bd);

        BigDecimal.AssertAreEqual(cosD, cosBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestTan(int i)
    {
        var d = i * double.Tau / Denominator;
        try
        {
            var tanD = double.Tan(d);

            var bd = i * BigDecimal.Tau / Denominator;
            var tanBd = BigDecimal.Tan(bd);

            BigDecimal.AssertAreEqual(tanD, tanBd);
        }
        catch (Exception)
        {
            var deg = (int)(d * 180 / double.Pi);
            Trace.WriteLine($"tan({deg}°) is undefined.");
        }
    }

    [TestMethod]
    public void TestTanException()
    {
        for (var x = -3; x <= 3; x += 2)
        {
            Assert.ThrowsException<ArithmeticException>(() =>
                BigDecimal.Tan(x * BigDecimal.Pi / 2));
        }
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestAsin(int i)
    {
        var d = (double)i / Denominator;
        var asinD = double.Asin(d);

        var bd = (BigDecimal)i / Denominator;
        var asinBd = BigDecimal.Asin(bd);

        BigDecimal.AssertAreEqual(asinD, asinBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestAcos(int i)
    {
        var d = (double)i / Denominator;
        var acosD = double.Acos(d);

        var bd = (BigDecimal)i / Denominator;
        var acosBd = BigDecimal.Acos(bd);

        BigDecimal.AssertAreEqual(acosD, acosBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestAtan(int i)
    {
        var d = (double)i;
        var atanD = double.Atan(d);

        var bd = (BigDecimal)i;
        var atanBd = BigDecimal.Atan(bd);

        BigDecimal.AssertAreEqual(atanD, atanBd);
    }

    [TestMethod]
    public void TestAtan2ThrowsExceptionWhenBothParamsZero()
    {
        Assert.ThrowsException<ArgumentInvalidException>(() => BigDecimal.Atan2(0, 0));
    }

    [TestMethod]
    public void TestAtan2()
    {
        const int n = 12;
        for (var x = -n; x <= n; x++)
        {
            for (var y = -n; y <= n; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                var atan2D = double.Atan2(y, x);
                var atan2Bd = BigDecimal.Atan2(y, x);

                BigDecimal.AssertAreEqual(atan2D, atan2Bd);
            }
        }
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestSinh(int i)
    {
        var d = i * double.Tau / Denominator;
        var sinhD = double.Sinh(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var sinhBd = BigDecimal.Sinh(bd);

        BigDecimal.AssertAreEqual(sinhD, sinhBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestCosh(int i)
    {
        var d = i * double.Tau / Denominator;
        var coshD = double.Cosh(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var coshBd = BigDecimal.Cosh(bd);

        BigDecimal.AssertAreEqual(coshD, coshBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestTanh(int i)
    {
        var d = i * double.Tau / Denominator;
        var tanhD = double.Tanh(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var tanhBd = BigDecimal.Tanh(bd);

        BigDecimal.AssertAreEqual(tanhD, tanhBd);
    }

    [TestMethod]
    public void TestPolarToCartesian()
    {
        BigDecimal r, a, x, y;
        var oneOnSqrt2 = 1 / BigDecimal.Sqrt(2);

        r = 1;
        a = 0;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimal.AssertAreEqual(1, x);
        BigDecimal.AssertAreEqual(0, y);

        r = 1;
        a = BigDecimal.Pi / 4;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimal.AssertAreEqual(oneOnSqrt2, x);
        BigDecimal.AssertAreEqual(oneOnSqrt2, y);

        r = 1;
        a = BigDecimal.Pi / 2;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimal.AssertAreEqual(0, x);
        BigDecimal.AssertAreEqual(1, y);

        r = 1;
        a = BigDecimal.Pi;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimal.AssertAreEqual(-1, x);
        BigDecimal.AssertAreEqual(0, y);

        r = 1;
        a = 3 * BigDecimal.Pi / 2;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimal.AssertAreEqual(0, x);
        BigDecimal.AssertAreEqual(-1, y);

        r = 1;
        a = BigDecimal.Tau;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimal.AssertAreEqual(1, x);
        BigDecimal.AssertAreEqual(0, y);
    }
}
