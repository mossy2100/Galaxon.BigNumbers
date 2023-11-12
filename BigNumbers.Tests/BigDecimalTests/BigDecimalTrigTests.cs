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

    [TestMethod]
    public void TestNormalizeAngle()
    {
        for (var i = -10; i <= 10; i++)
        {
            var thetaDeg = (BigDecimal)45 * i;
            var thetaRad = thetaDeg * BigDecimal.Pi / 180;
            var betaRad = BigDecimal.NormalizeAngle(thetaRad);
            var betaDeg = betaRad * 180 / BigDecimal.Pi;
            Console.WriteLine($"NormalizeAngle({thetaDeg:F2}) = {betaDeg:F2}");
        }
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestSin(int i)
    {
        var d = i * double.Tau / Denominator;
        var sinD = double.Sin(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var sinBd = BigDecimal.Sin(bd);

        BigDecimalAssert.AreFuzzyEqual(sinD, sinBd, 1e-15);
    }

    [TestMethod]
    public void TestSinOneThirdPi()
    {
        BigDecimal.MaxSigFigs = 200;
        var theta = BigDecimal.Pi / 3;

        var t1 = DateTime.Now;
        var sin1 = BigDecimal.Sin(theta);
        var t2 = DateTime.Now;
        Console.WriteLine($"Time for method 1: {(t2 - t1).Ticks} ticks.");

        var t3 = DateTime.Now;
        var sin2 = BigDecimal.Sqrt(3) / 2;
        var t4 = DateTime.Now;
        Console.WriteLine($"Time for method 2: {(t4 - t3).Ticks} ticks.");

        BigDecimalAssert.AreFuzzyEqual(sin1, sin2);
    }

    [TestMethod]
    public void TestSinOneQuarterPi()
    {
        BigDecimal.MaxSigFigs = 200;
        var theta = BigDecimal.Pi / 4;

        var t1 = DateTime.Now;
        var sin1 = BigDecimal.Sin(theta);
        var t2 = DateTime.Now;
        Console.WriteLine($"Time for method 1: {(t2 - t1).Ticks} ticks.");

        var t3 = DateTime.Now;
        var sin2 = BigDecimal.Sqrt(2) / 2;
        var t4 = DateTime.Now;
        Console.WriteLine($"Time for method 2: {(t4 - t3).Ticks} ticks.");

        BigDecimalAssert.AreFuzzyEqual(sin1, sin2);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestCos(int i)
    {
        var d = i * double.Tau / Denominator;
        var cosD = double.Cos(d);

        var bd = i * BigDecimal.Tau / Denominator;
        var cosBd = BigDecimal.Cos(bd);

        BigDecimalAssert.AreFuzzyEqual(cosD, cosBd, 1e-15);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestTan(int i)
    {
        var t1 = DateTime.Now;
        var d = i * double.Tau / Denominator;
        var bd = i * BigDecimal.Tau / Denominator;

        if (i == 3 || i == 9 || i == -3 || i == -9)
        {
            Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Tan(bd));
        }
        else
        {
            var tanD = double.Tan(d);
            var tanBd = BigDecimal.Tan(bd);
            BigDecimalAssert.AreFuzzyEqual(tanD, tanBd, 1e-14);
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
        var bd = i * BigDecimal.Tau / Denominator;
        var actual = BigDecimal.Sinh(bd);

        var sResult = i switch
        {
            -12 => "-267.74489404101651425711744968805617722370618739915",
            -11 => "-158.60699505697111480028788990902014567957642767811",
            -10 => "-93.954653468459854428317163719906921644545595924175",
            -9 => "-55.654397599417548299475671490540458958762668652846",
            -8 => "-32.963900290099934045275522901848260063063549799021",
            -7 => "-19.519007046411157303381825036119245844267899005756",
            -6 => "-11.548739357257748377977334315388409684495189066395",
            -5 => "-6.8176233041265434457647982706360936516329068863285",
            -4 => "-3.9986913427998215911027319848134239068556975310555",
            -3 => "-2.3012989023072948734630400234344271781781465165164",
            -2 => "-1.2493670505239752649541953019279756678007164745801",
            -1 => "-0.54785347388803980847571564771743140335278352177586",
            0 => "0.0",
            1 => "0.54785347388803980847571564771743140335278352177586",
            2 => "1.2493670505239752649541953019279756678007164745801",
            3 => "2.3012989023072948734630400234344271781781465165164",
            4 => "3.9986913427998215911027319848134239068556975310555",
            5 => "6.8176233041265434457647982706360936516329068863285",
            6 => "11.548739357257748377977334315388409684495189066395",
            7 => "19.519007046411157303381825036119245844267899005756",
            8 => "32.963900290099934045275522901848260063063549799021",
            9 => "55.654397599417548299475671490540458958762668652846",
            10 => "93.954653468459854428317163719906921644545595924175",
            11 => "158.60699505697111480028788990902014567957642767811",
            12 => "267.74489404101651425711744968805617722370618739915"
        };
        var expected = BigDecimal.Parse(sResult);

        if (!actual.FuzzyEquals(expected))
        {
            Console.WriteLine($"i = {i}");
            Console.WriteLine($"  actual = {actual}");
            Console.WriteLine($"expected = {expected}");
        }

        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestCosh(int i)
    {
        var bd = i * BigDecimal.Tau / Denominator;
        var actual = BigDecimal.Cosh(bd);

        var sResult = i switch
        {
            -12 => "267.74676148374822224593187990099100425409961020415",
            -11 => "158.61014747172407708968795584746142966234045145654",
            -10 => "93.959975033938655011290221658322180814351021883597",
            -9 => "55.663380890438677727365336442448384212484006419052",
            -8 => "32.979064909964480615228063636067866247060118553972",
            -7 => "19.544606316778253559349201698757461965430621044502",
            -6 => "11.591953275521520627751752052560137695770917176205",
            -5 => "6.8905723649758825753689424593242955998056288393046",
            -4 => "4.1218360538699547247442656345420954249699049054587",
            -3 => "2.5091784786580567820099956432694059482120243581482",
            -2 => "1.600286857702386232519932017924928726163289628201",
            -1 => "1.1402383210764287921411319803793508907668976675113",
            0 => "1.0",
            1 => "1.1402383210764287921411319803793508907668976675113",
            2 => "1.600286857702386232519932017924928726163289628201",
            3 => "2.5091784786580567820099956432694059482120243581482",
            4 => "4.1218360538699547247442656345420954249699049054587",
            5 => "6.8905723649758825753689424593242955998056288393046",
            6 => "11.591953275521520627751752052560137695770917176205",
            7 => "19.544606316778253559349201698757461965430621044502",
            8 => "32.979064909964480615228063636067866247060118553972",
            9 => "55.663380890438677727365336442448384212484006419052",
            10 => "93.959975033938655011290221658322180814351021883597",
            11 => "158.61014747172407708968795584746142966234045145654",
            12 => "267.74676148374822224593187990099100425409961020415"
        };
        var expected = BigDecimal.Parse(sResult);

        if (!actual.FuzzyEquals(expected))
        {
            Console.WriteLine($"i = {i}");
            Console.WriteLine($"  actual = {actual}");
            Console.WriteLine($"expected = {expected}");
        }

        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    [DynamicData(nameof(Numerators))]
    public void TestTanh(int i)
    {
        var bd = i * BigDecimal.Tau / Denominator;
        var actual = BigDecimal.Tanh(bd);

        var sResult = i switch
        {
            -12 => "-0.99999302533961061060510721183234574642771937737571",
            -11 => "-0.99998012475996515746159863388486198275063429879461",
            -10 => "-0.99994336348560247684346575838967997004954141370184",
            -9 => "-0.99983861398863265074230079223665552043801446853656",
            -8 => "-0.99954017435285241680078218524995671274284367824213",
            -7 => "-0.99869021304639324874244025879224659393497035329873",
            -6 => "-0.99627207622074994426469058001253671189689919080459",
            -5 => "-0.98941320735268202124484900984327402380700669710019",
            -4 => "-0.97012382116593073268760346172618581780745368768497",
            -3 => "-0.91715233566727434637309292144261877536792714860109",
            -2 => "-0.78071443535926771285622903482745131894613240055138",
            -1 => "-0.48047277815645160586084528707524322599092233129085",
            0 => "0.0",
            1 => "0.48047277815645160586084528707524322599092233129085",
            2 => "0.78071443535926771285622903482745131894613240055138",
            3 => "0.91715233566727434637309292144261877536792714860109",
            4 => "0.97012382116593073268760346172618581780745368768497",
            5 => "0.98941320735268202124484900984327402380700669710019",
            6 => "0.99627207622074994426469058001253671189689919080459",
            7 => "0.99869021304639324874244025879224659393497035329873",
            8 => "0.99954017435285241680078218524995671274284367824213",
            9 => "0.99983861398863265074230079223665552043801446853656",
            10 => "0.99994336348560247684346575838967997004954141370184",
            11 => "0.99998012475996515746159863388486198275063429879461",
            12 => "0.99999302533961061060510721183234574642771937737571"
        };
        var expected = BigDecimal.Parse(sResult);

        if (!actual.FuzzyEquals(expected))
        {
            Console.WriteLine($"i = {i}");
            Console.WriteLine($"  actual = {actual}");
            Console.WriteLine($"expected = {expected}");
        }

        BigDecimalAssert.AreFuzzyEqual(expected, actual);
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
