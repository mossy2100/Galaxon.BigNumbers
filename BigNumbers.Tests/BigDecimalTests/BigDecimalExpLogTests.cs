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
