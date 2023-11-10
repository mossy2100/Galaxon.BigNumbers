using Galaxon.BigNumbers.TestTools;

namespace Galaxon.BigNumbers.Tests;

/// <summary>
/// Test all methods in BigDecimal.Exp.cs.
/// That means all methods for exponentiation, finding roots, and logarithms.
///
/// For a useful exponent calculator:
/// <see href="https://www.calculator.net/exponent-calculator.html"/>
/// </summary>
[TestClass]
public class BigDecimalExpLogTests
{
    #region Test Exp()

    /// <summary>Exp(0) returns 1.</summary>
    [TestMethod]
    public void Exp_0_Returns1()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = 1;
        actual = BigDecimal.Exp(0);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>Exp(1) returns E.</summary>
    [TestMethod]
    public void Exp_1_ReturnsE()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = BigDecimal.E;
        actual = BigDecimal.Exp(1);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>Test Exp() with positive integers.</summary>
    [TestMethod]
    public void Exp_PositiveIntegers()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = BigDecimal.Parse("7.3890560989306502272304274605750078131803155705518");
        actual = BigDecimal.Exp(2);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("20.085536923187667740928529654581717896987907838554");
        actual = BigDecimal.Exp(3);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("22026.465794806716516957900645284244366353512618557");
        actual = BigDecimal.Exp(10);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    /// <summary>Test Exp() with negative integers.</summary>
    [TestMethod]
    public void Exp_NegativeIntegers()
    {
        BigDecimal.MaxSigFigs = 50;

        double expected;
        BigDecimal actual;
        double delta;

        expected = 0.36787944117144;
        actual = BigDecimal.Exp(-1);
        delta = 0.00000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        expected = 0.13533528323661;
        actual = BigDecimal.Exp(-2);
        delta = 0.00000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        expected = 4.5399929762485E-5;
        actual = BigDecimal.Exp(-10);
        delta = 0.0000000000001E-5;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    /// <summary>Test Exp() with positive floating point values.</summary>
    [TestMethod]
    public void Exp_PositiveFloats()
    {
        BigDecimal.MaxSigFigs = 15;

        double expected;
        BigDecimal actual;
        double delta;

        expected = 1.6487212707001;
        delta = 0.0000000000001;
        actual = BigDecimal.Exp(0.5);
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        expected = 4.1329443527781E+53;
        actual = BigDecimal.Exp(123.456);
        delta = 0.0000000000001E+53;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        expected = 1.000000789;
        actual = BigDecimal.Exp(0.000000789);
        delta = 0.000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    /// <summary>Test Exp() with negative floating point values.</summary>
    [TestMethod]
    public void Exp_NegativeFloats()
    {
        BigDecimal.MaxSigFigs = 50;

        double expected;
        BigDecimal actual;
        double delta;

        expected = 0.60653065971263;
        delta = 0.00000000000001;
        actual = BigDecimal.Exp(-0.5);
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        expected = 2.4195825412646E-54;
        delta = 0.0000000000001E-54;
        actual = BigDecimal.Exp(-123.456);
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        expected = 0.99999921100031;
        delta = 0.00000000000001;
        actual = BigDecimal.Exp(-0.000000789);
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    /// <summary>
    /// I generated these expected results using Python.
    /// <code>
    /// from mpmath import mp, exp
    /// 
    /// # Set the precision (number of decimal places)
    /// mp.dps = 50  # 50 decimal places of precision
    /// 
    /// # Loop through integer values from -12 to 12
    /// for i in range(-12, 13):
    ///     theta = i * mp.pi / 6
    ///     ex = exp(theta)
    ///     print(f"{i} =&gt; \"{ex}\",")
    /// </code>
    /// </summary>
    [TestMethod]
    public void TestExpSmallInts()
    {
        for (var i = -12; i <= 12; i++)
        {
            var x = i * BigDecimal.Pi / 6;
            var actual = BigDecimal.Exp(x);
            var sExpected = i switch
            {
                -12 => "0.0018674427317079888144302129348270303934228050024753",
                -11 => "0.0031524147529622894000659384412839827640237784258691",
                -10 => "0.0053215654788005829730579384152591698054259594219172",
                -9 => "0.0089832910211294278896649519079252537213377662061819",
                -8 => "0.015164619864546569952540734219606183996568754950538",
                -7 => "0.025599270367096255967376662638216121162722038746372",
                -6 => "0.043213918263772249774417737171728011275728109810633",
                -5 => "0.072949060849339129604144188688201948172721952976131",
                -4 => "0.12314471107013313364153364972867151811420737440326",
                -3 => "0.20787957635076190854695561983497877003387784163177",
                -2 => "0.35091980717841096756573671599695305836257315362096",
                -1 => "0.59238484718838898366541633266191948741411414573546",
                0 => "1.0",
                1 => "1.6880917949644686006168476280967822941196811892872",
                2 => "2.8496539082263614974741273198529043939640061027811",
                3 => "4.8104773809653516554730356667038331263901708746645",
                4 => "8.1205273966697763158469976193555193318256024365142",
                5 => "13.708195669102426021133740729960389251438535725633",
                6 => "23.1406926327792690057290863679485473802661062426",
                7 => "39.063613363189410862731026734876707809698520050258",
                8 => "65.942965200064414660503586537916126310123668352993",
                9 => "111.3177784898562260268410079329888431712466750719",
                10 => "187.91462850239850943960738537822910245889661780777",
                11 => "317.21714252869519188997584575648157534191687913466",
                12 => "535.4916555247647365030493295890471814778057976033"
            };
            var expected = BigDecimal.Parse(sExpected);
            var diff = BigDecimal.Abs(expected - actual);
            Console.WriteLine($"expected = {expected}");
            Console.WriteLine($"actual   = {actual}");
            Console.WriteLine($"diff     = {diff}");
            Console.WriteLine((actual.FuzzyEquals(expected) ? "" : "NOT ") + "FUZZY EQUAL");
            Console.WriteLine();

            BigDecimalAssert.AreFuzzyEqual(expected, actual);
        }
    }

    #endregion Test Exp()

    #region Test Log() methods

    [TestMethod]
    public void TestLog()
    {
        BigDecimal.MaxSigFigs = 50;

        BigDecimal expected;
        BigDecimal actual;

        expected = 0;
        actual = BigDecimal.Log(1);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("0.69314718055994530941723212145817656807550013436026");
        actual = BigDecimal.Log(2);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = 1;
        actual = BigDecimal.Log(BigDecimal.E);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        expected = BigDecimal.Parse("2.3025850929940456840179914546843642076011014886288");
        actual = BigDecimal.Log(10);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void TestLogDouble()
    {
        BigDecimal.MaxSigFigs = 30;

        for (var i = 1; i < 100; i++)
        {
            Console.WriteLine("--------------------------------------------------");

            double d = i;
            var logD = double.Log(d);
            Console.WriteLine($"double.Log({d})     = {logD}");

            BigDecimal bd = i;
            var logBD = BigDecimal.Log(bd);
            Console.WriteLine($"BigDecimal.Log({bd}) = {logBD}");

            BigDecimalAssert.AreFuzzyEqual(logD, logBD);
        }
    }

    [TestMethod]
    public void LogSpeedTest()
    {
        for (var i = 1; i < 10; i++)
        {
            Console.WriteLine("--------------------------------------------------");

            var t1 = DateTime.Now.Ticks;
            var log = BigDecimal.Log(i);
            var t2 = DateTime.Now.Ticks;
            var tLog = t2 - t1;
            Console.WriteLine($"Log({i}) == {log}");
            Console.WriteLine($"{tLog} ticks.");
        }
    }

    #endregion Test Log() methods
}
