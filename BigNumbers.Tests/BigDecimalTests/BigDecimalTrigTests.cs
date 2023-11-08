using Galaxon.BigNumbers.TestTools;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalTrigTests
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

        BigDecimalAssert.AreFuzzyEqual(sinD, sinBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestCos(int i)
    {
        var d = i * double.Tau / Denominator;
        var cosD = double.Cos(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var cosBd = BigDecimal.Cos(bd);

        BigDecimalAssert.AreFuzzyEqual(cosD, cosBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestTan(int i)
    {
        var t1 = DateTime.Now;
        var d = i * double.Tau / Denominator;
        try
        {
            var tanD = double.Tan(d);
            var bd = i * BigDecimal.Tau / Denominator;
            var tanBd = BigDecimal.Tan(bd);
            BigDecimalAssert.AreFuzzyEqual(tanD, tanBd);
        }
        catch (Exception)
        {
            var deg = (int)(d * 180 / double.Pi);
            Console.WriteLine($"tan({deg}°) is undefined.");
        }
        var t2 = DateTime.Now;
        Console.WriteLine($"Time elapsed: {t2 - t1} ticks.");
    }

    [TestMethod]
    public void TestTanRandom()
    {
        int i;
        DateTime t1, t2;

        // Step 1. Get a set of random numbers and calculate tan(x).
        var n = 10;
        var rnd = new Random();
        var inputs = new double[10];
        var results = new double[10];
        for (i = 0; i < n; i++)
        {
            inputs[i] = rnd.NextDouble();
            results[i] = double.Tan(inputs[i]);
            Console.WriteLine($"double.Tan({inputs[i]}) = {results[i]}");
        }

        // Step 2. Test performance of BigDecimal.Tan().
        t1 = DateTime.Now;
        for (i = 0; i < n; i++)
        {
            var bd = (BigDecimal)inputs[i];
            var tanBd = BigDecimal.Tan(bd);
            Console.WriteLine($"BigDecimal.Tan({bd}) = {tanBd}");
            BigDecimalAssert.AreFuzzyEqual(results[i], tanBd);
        }
        t2 = DateTime.Now;
        var dt = t2 - t1;
        Console.WriteLine($"Tan(x) total time elapsed: {dt.TotalSeconds} ticks.");
        var avg = dt.TotalSeconds / n;
        Console.WriteLine($"Average is {avg} seconds per call.");

        // Step 3. Test performance of BigDecimal.Tan2().
        // t1 = DateTime.Now;
        // for (i = 0; i < n; i++)
        // {
        //     var bd = (BigDecimal)inputs[i];
        //     var tanBd = BigDecimal.Tan2(bd);
        //     Console.WriteLine($"BigDecimal.Tan({bd}) = {tanBd}");
        //     // BigDecimalAssert.AreEqual(results[i], tanBd);
        // }
        // t2 = DateTime.Now;
        // Console.WriteLine($"Tan2(x) total time elapsed: {(t2 - t1).Ticks} ticks.");
    }

    [TestMethod]
    public void TestTanException()
    {
        for (var x = -3; x <= 3; x += 2)
        {
            Assert.ThrowsException<ArithmeticException>(() =>
                BigDecimal.Tan(x * BigDecimal.HalfPi));
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

        BigDecimalAssert.AreFuzzyEqual(asinD, asinBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestAcos(int i)
    {
        var d = (double)i / Denominator;
        var acosD = double.Acos(d);

        var bd = (BigDecimal)i / Denominator;
        var acosBd = BigDecimal.Acos(bd);

        BigDecimalAssert.AreFuzzyEqual(acosD, acosBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestAtan(int i)
    {
        var d = (double)i;
        var atanD = double.Atan(d);

        var bd = (BigDecimal)i;
        var atanBd = BigDecimal.Atan(bd);

        BigDecimalAssert.AreFuzzyEqual(atanD, atanBd);
    }

    [TestMethod]
    public void TestAtan2Returns0WhenBothParams0()
    {
        Assert.AreEqual(0, BigDecimal.Atan2(0, 0));
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

                BigDecimalAssert.AreFuzzyEqual(atan2D, atan2Bd);
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

        BigDecimalAssert.AreFuzzyEqual(sinhD, sinhBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestCosh(int i)
    {
        var d = i * double.Tau / Denominator;
        var coshD = double.Cosh(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var coshBd = BigDecimal.Cosh(bd);

        BigDecimalAssert.AreFuzzyEqual(coshD, coshBd);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestTanh(int i)
    {
        var d = i * double.Tau / Denominator;
        var tanhD = double.Tanh(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var tanhBd = BigDecimal.Tanh(bd);

        BigDecimalAssert.AreFuzzyEqual(tanhD, tanhBd);
    }

    [TestMethod]
    public void TestPolarToCartesian()
    {
        BigDecimal r, a, x, y;
        var oneOnSqrt2 = 1 / BigDecimal.Sqrt(2);

        r = 1;
        a = 0;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimalAssert.AreFuzzyEqual(1, x);
        BigDecimalAssert.AreFuzzyEqual(0, y);

        r = 1;
        a = BigDecimal.Pi / 4;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimalAssert.AreFuzzyEqual(oneOnSqrt2, x);
        BigDecimalAssert.AreFuzzyEqual(oneOnSqrt2, y);

        r = 1;
        a = BigDecimal.HalfPi;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimalAssert.AreFuzzyEqual(0, x);
        BigDecimalAssert.AreFuzzyEqual(1, y);

        r = 1;
        a = BigDecimal.Pi;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimalAssert.AreFuzzyEqual(-1, x);
        BigDecimalAssert.AreFuzzyEqual(0, y);

        r = 1;
        a = 3 * BigDecimal.HalfPi;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimalAssert.AreFuzzyEqual(0, x);
        BigDecimalAssert.AreFuzzyEqual(-1, y);

        r = 1;
        a = BigDecimal.Tau;
        (x, y) = BigDecimal.PolarToCartesian(r, a);
        BigDecimalAssert.AreFuzzyEqual(1, x);
        BigDecimalAssert.AreFuzzyEqual(0, y);
    }
}
