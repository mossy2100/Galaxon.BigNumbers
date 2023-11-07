using System.Diagnostics;
using System.Numerics;
using Galaxon.BigNumbers.TestTools;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

/// <summary>
/// Test all methods in BigDecimal.Exp.cs.
/// That means all methods for exponentiation, finding roots, and logarithms.
///
/// For a useful exponent calculator:
/// <see href="https://www.calculator.net/exponent-calculator.html"/>
/// </summary>
[TestClass]
public class BigDecimalExpTests
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

    #region Test Pow()

    #region Base 0

    [TestMethod]
    public void Pow_Base0Exponent0_Returns1()
    {
        BigDecimal x = 0;
        BigDecimal y = 0;
        BigDecimal actual = BigDecimal.Pow(x, y);
        BigDecimal expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base0Exponent1_Returns0()
    {
        BigDecimal x = 0;
        BigDecimal y = 1;
        BigDecimal actual = BigDecimal.Pow(x, y);
        BigDecimal expected = 0;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base0ExponentPos_Returns0()
    {
        BigDecimal x = 0;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;

        y = 2;
        actual = BigDecimal.Pow(x, y);
        expected = 0;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        y = 10;
        actual = BigDecimal.Pow(x, y);
        expected = 0;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        y = 1.234;
        actual = BigDecimal.Pow(x, y);
        expected = 0;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        y = 4.567e307;
        actual = BigDecimal.Pow(x, y);
        expected = 0;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base0ExponentNeg_Throws()
    {
        BigDecimal x = 0;
        BigDecimal y = -2;
        Assert.ThrowsException<DivideByZeroException>(() => BigDecimal.Pow(x, y));

        y = -12345.6789;
        Assert.ThrowsException<DivideByZeroException>(() => BigDecimal.Pow(x, y));
    }

    #endregion Base 0

    #region Base 1

    [TestMethod]
    public void Pow_Base1Exponent0_Returns1()
    {
        BigDecimal x = 1;
        BigDecimal y = 0;
        BigDecimal actual = BigDecimal.Pow(x, y);
        BigDecimal expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base1Exponent1_Returns1()
    {
        BigDecimal x = 1;
        BigDecimal y = 1;
        BigDecimal actual = BigDecimal.Pow(x, y);
        BigDecimal expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base1ExponentPos_Returns1()
    {
        BigDecimal x = 1;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected = 1;

        y = 5;
        actual = BigDecimal.Pow(x, y);
        Assert.AreEqual(expected, actual);

        y = 1.23;
        actual = BigDecimal.Pow(x, y);
        Assert.AreEqual(expected, actual);

        y = 4.56e123;
        actual = BigDecimal.Pow(x, y);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base1ExponentNeg_Returns1()
    {
        BigDecimal x = 1;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected = 1;

        y = -5;
        actual = BigDecimal.Pow(x, y);
        Assert.AreEqual(expected, actual);

        y = -1.23;
        actual = BigDecimal.Pow(x, y);
        Assert.AreEqual(expected, actual);

        y = -4.56e123;
        actual = BigDecimal.Pow(x, y);
        Assert.AreEqual(expected, actual);
    }

    #endregion Base 1

    #region Base positive

    [TestMethod]
    public void Pow_BasePosExponent0_Returns1()
    {
        BigDecimal x;
        BigDecimal y = 0;
        BigDecimal actual;
        BigDecimal expected;

        x = 1;
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);

        x = new BigDecimal(1, 1000);
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);

        x = 123.4567;
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);

        x = BigDecimal.Pi;
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_BasePosExponent1_ReturnsItself()
    {
        BigDecimal x;
        BigDecimal y = 1;
        BigDecimal actual;
        BigDecimal expected;

        x = 1;
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);

        x = new BigDecimal(1, 1000);
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);

        x = 123.4567;
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);

        x = BigDecimal.Pi;
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_BasePosExponentPos()
    {
        BigDecimal x;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;
        BigDecimal delta;

        x = 2;
        y = 5;
        actual = BigDecimal.Pow(x, y);
        expected = 32;
        Assert.AreEqual(expected, actual);

        x = 123;
        y = 4;
        actual = BigDecimal.Pow(x, y);
        expected = 228886641;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        x = BigDecimal.E;
        y = 123;
        actual = BigDecimal.Pow(x, y);
        expected = 2.6195173187491E+53;
        delta = 0.0000000000001E+53;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        x = 1.23;
        y = 4.56;
        actual = BigDecimal.Pow(x, y);
        expected = 2.5702023016193;
        delta = 0.0000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    [TestMethod]
    public void Pow_BasePosExponentNeg()
    {
        BigDecimal x;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;
        BigDecimal delta;

        x = 2;
        y = -5;
        actual = BigDecimal.Pow(x, y);
        expected = 0.03125;
        Assert.AreEqual(expected, actual);

        x = 123;
        y = -4;
        actual = BigDecimal.Pow(x, y);
        expected = 4.3689749459865E-9;
        delta = 0.0000000000001E-9;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        x = BigDecimal.E;
        y = -123;
        actual = BigDecimal.Pow(x, y);
        expected = 3.8174971886712E-54;
        delta = 0.0000000000001E-54;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        x = 1.23;
        y = -4.56;
        actual = BigDecimal.Pow(x, y);
        expected = 0.38907443175581;
        delta = 0.00000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    #endregion Base positive

    #region Base negative

    [TestMethod]
    public void Pow_BaseNegExponent0_Returns1()
    {
        BigDecimal x;
        BigDecimal y = 0;
        BigDecimal actual;
        BigDecimal expected;

        x = -1;
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);

        x = new BigDecimal(-1, 1000);
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);

        x = -123.4567;
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);

        x = -BigDecimal.Pi;
        actual = BigDecimal.Pow(x, y);
        expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_BaseNegExponent1_ReturnsSelf()
    {
        BigDecimal x;
        BigDecimal y = 1;
        BigDecimal actual;
        BigDecimal expected;

        x = -1;
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);

        x = new BigDecimal(-1, 1000);
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);

        x = -123.4567;
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);

        x = -BigDecimal.Pi;
        actual = BigDecimal.Pow(x, y);
        expected = x;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_BaseNegExponentPosInt()
    {
        BigDecimal x;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;
        BigDecimal delta;

        x = -2;
        y = 5;
        actual = BigDecimal.Pow(x, y);
        expected = -32;
        BigDecimalAssert.AreEqual(expected, actual);

        x = -123;
        y = 4;
        actual = BigDecimal.Pow(x, y);
        expected = 228886641;
        BigDecimalAssert.AreEqual(expected, actual);

        x = -BigDecimal.E;
        y = 123;
        actual = BigDecimal.Pow(x, y);
        expected = -2.619517318749E+53;
        delta = 0.000000000001E+53;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    [TestMethod]
    public void Pow_BaseNegExponentNegInt()
    {
        BigDecimal x;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;
        BigDecimal delta;

        x = -2;
        y = -5;
        actual = BigDecimal.Pow(x, y);
        expected = -0.03125;
        BigDecimalAssert.AreEqual(expected, actual);

        x = -BigDecimal.E;
        y = -123;
        actual = BigDecimal.Pow(x, y);
        expected = -3.8174971886712E-54;
        delta = 0.0000000000001E-54;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    [TestMethod]
    public void Pow_BaseNegExponentPosFloat_SolvableWorks()
    {
        BigDecimal x;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;

        x = -32;
        y = 0.2m;
        actual = BigDecimal.Pow(x, y);
        expected = -2;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_BaseNegExponentPosFloat_UnsolvableThrows()
    {
        BigDecimal x;
        BigDecimal y;

        x = -123;
        y = 0.5;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, y));
    }

    [TestMethod]
    public void Pow_BaseNegExponentNegFloat_SolvableWorks()
    {
        BigDecimal x;
        BigDecimal y;
        BigDecimal actual;
        BigDecimal expected;

        x = -32;
        y = -0.4m;
        actual = BigDecimal.Pow(x, y);
        expected = 0.00048828125m;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_BaseNegExponentNegFloat_UnsolvableThrows()
    {
        BigDecimal x;
        BigDecimal y;

        x = -123;
        y = -0.5;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, y));
    }

    [TestMethod]
    public void CubeRootOfNegative()
    {
        BigDecimal x;
        BigRational y;
        BigDecimal actual;
        BigDecimal expected;

        // Cube root of -27 using the Pow(BigDecimal, BigRational) method.
        x = -27;
        y = new BigRational(1, 3);
        actual = BigDecimal.Pow(x, y);
        expected = -3;
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        // Compare with the Cbrt() method.
        actual = BigDecimal.Cbrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);

        // Test how this doesn't work for a BigDecimal value because we can't represent 1/3 exactly.
        BigDecimal z = (BigDecimal)1 / 3;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, z));
    }

    [TestMethod]
    public void Pow_BaseNegExponentRationalOddDenimonator_SolvableWorks()
    {
        BigDecimal x;
        BigRational y;
        BigDecimal actual;
        BigDecimal expected;
        BigDecimal delta;

        // Test that -27^(1/3) = 1/3
        x = -27;
        y = new BigRational(1, 3);
        actual = BigDecimal.Pow(x, y);
        expected = -3;
        BigDecimalAssert.AreEqual(expected, actual);

        // Test that -27^(-1/3) = -1/3
        x = -27;
        y = new BigRational(-1, 3);
        actual = BigDecimal.Pow(x, y);
        expected = -0.33333333333333;
        delta = 0.00000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        x = -123.456;
        y = new BigRational(4, 5);
        actual = BigDecimal.Pow(x, y);
        expected = 47.120486020959;
        delta = 0.000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        x = -123.456;
        y = new BigRational(-4, 5);
        actual = BigDecimal.Pow(x, y);
        expected = 0.0212221919688;
        delta = 0.0000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    /// <summary>
    /// Test that attempting to calculate x^y where x is negative and y is a rational with an even
    /// denominator will throw an exception.
    /// </summary>
    [TestMethod]
    public void Pow_BaseNegExponentRationalEvenDenominator_UnsolvableThrows()
    {
        BigDecimal x;
        BigRational y;

        // Test that -27^(1/2) throws exception.
        x = -27;
        y = new BigRational(1, 2);
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, y));

        // Test that -27^(-1/2) throws exception.
        x = -27;
        y = new BigRational(-1, 2);
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, y));

        // Test throws exception.
        x = -123.456;
        y = new BigRational(5, 6);
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, y));

        // Test throws exception.
        x = -123.456;
        y = new BigRational(-5, 6);
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(x, y));
    }

    #endregion Base negative

    #endregion Test Pow()

    #region Test methods for finding roots

    [TestMethod]
    public void TestSqrt0()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Zero, BigDecimal.Sqrt(0));
    }

    [TestMethod]
    public void TestSqrt1()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.One, BigDecimal.Sqrt(1));
    }

    [TestMethod]
    public void TestSqrtPiSquared()
    {
        var bd = BigDecimal.Sqrt(BigDecimal.Pi * BigDecimal.Pi);
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Pi, bd);
    }

    // No asserts, just want to make sure the method calls complete fast enough and without error.
    // Also testing rounding to required sig figs.
    [TestMethod]
    public void TestSqrtSmallInts()
    {
        for (var i = 1; i <= 10; i++)
        {
            BigDecimal.MaxSigFigs = 55;
            var bd1 = BigDecimal.Sqrt(i);
            Trace.WriteLine($"√{i} = {bd1}");
            BigDecimal.MaxSigFigs = 50;
            var bd2 = BigDecimal.Sqrt(i);
            Trace.WriteLine($"√{i} = {bd2}");
            BigDecimalAssert.AreFuzzyEqual(bd1, bd2);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtBig()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("6.02214076E23");
        var expected = BigDecimal.Parse("776024533117.34932546664032837511112530578432706889"
            + "69571576562989126786337996022194015376088918609909491309813595319711937386010926");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    /// <summary>
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtSmall()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.602176634E-19");
        var expected = BigDecimal.Parse("4.0027198677899006825970388239053767545702786298616"
            + "66648707342924009987437927221345536742635143445476302206435987095958590772815416E-10");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    /// <summary>
    /// In this test, both the initial argument and the square root are larger than the largest
    /// possible double value.
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtBiggerThanBiggestDouble()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.2345678E789");
        var expected = BigDecimal.Parse("3.5136417005722140080009539858670683706660895438958"
            + "9865958869460824551868009859293464600836861863229496438492388219814058056172706E+394");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    /// <summary>
    /// In this test, both the initial argument and the square root are smaller than the largest
    /// possible double value.
    /// Used https://keisan.casio.com/calculator to get expected result.
    /// </summary>
    [TestMethod]
    public void TestSqrtSmallerThanSmallestDouble()
    {
        BigDecimal.MaxSigFigs = 130;
        var x = BigDecimal.Parse("1.2345678E-789");
        var expected = BigDecimal.Parse("3.5136417005722140080009539858670683706660895438958"
            + "9865958869460824551868009859293464600836861863229496438492388219814058056172706E-395");
        var actual = BigDecimal.Sqrt(x);
        BigDecimalAssert.AreFuzzyEqual(expected, actual);
    }

    [TestMethod]
    public void TestSqrtNegative()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Sqrt(-1));
    }

    [TestMethod]
    public void TestCbrt0()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.Zero, BigDecimal.Cbrt(0));
    }

    [TestMethod]
    public void TestCbrt1()
    {
        BigDecimalAssert.AreFuzzyEqual(BigDecimal.One, BigDecimal.Cbrt(1));
    }

    // No asserts, just want to make sure the method calls complete fast enough and without error or
    // infinite looping.
    [TestMethod]
    public void TestCbrtSmallValues()
    {
        for (var i = 1; i <= 1000; i++)
        {
            BigDecimal.MaxSigFigs = 54;
            Console.WriteLine($"³√{i} = " + BigDecimal.Cbrt(i));
            BigDecimal.MaxSigFigs = 50;
            Console.WriteLine($"³√{i} = " + BigDecimal.Cbrt(i));
            Console.WriteLine("");
        }
    }

    [TestMethod]
    public void TestRootNLargeNIntegerA()
    {
        BigInteger a = 5;
        var b = 500;
        var c = BigInteger.Pow(a, b);
        BigDecimalAssert.AreFuzzyEqual(a, BigDecimal.RootN(c, b));
    }

    [TestMethod]
    public void TestRootNLargeNDecimalA()
    {
        var a = BigDecimal.Pi;
        var b = 500;
        var c = BigDecimal.Pow(a, b);
        BigDecimalAssert.AreFuzzyEqual(a, BigDecimal.RootN(c, b));
    }

    [TestMethod]
    public void TestRootNWithNegativeArgumentAndOddRoot()
    {
        BigDecimal x = -123;
        var y = 71;
        var z = BigDecimal.RootN(x, y);
        Assert.IsTrue(z < 0);
    }

    [TestMethod]
    public void TestRootNWithNegativeArgumentAndEvenRoot()
    {
        BigDecimal x = -123;
        var y = 70;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(x, y));
    }

    /// <summary>
    /// This shows how the cube root of a negative value cannot be achieved using Pow(), because a
    /// BigDecimal cannot represent 1/3 exactly. You have to use Cbrt() or RootN().
    /// </summary>
    [TestMethod]
    public void TestCubeRootOfNegativeValueUsingPow()
    {
        var oneThird = BigDecimal.One / 3;
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.Pow(-27, oneThird));
    }

    [TestMethod]
    public void TestCbrtOfNegative()
    {
        BigDecimal expected;
        BigDecimal actual;
        BigDecimal delta;

        actual = BigDecimal.Cbrt(-27);
        BigDecimalAssert.AreFuzzyEqual(-3, actual);

        actual = BigDecimal.Cbrt(-16);
        expected = -2.519842099789746;
        delta = 0.000000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);

        actual = BigDecimal.Cbrt(-1234.5678);
        expected = -10.727659535728732;
        delta = 0.000000000000001;
        BigDecimalAssert.AreFuzzyEqual(expected, actual, delta);
    }

    /// <summary>
    /// Test odd roots of some negative values.
    /// </summary>
    [TestMethod]
    public void TestOddRootOfNegative()
    {
        BigDecimal c;
        c = BigDecimal.RootN(-27, 5);
        BigDecimalAssert.AreFuzzyEqual(-1.933182044931763, c);

        c = BigDecimal.RootN(-16, 7);
        BigDecimalAssert.AreFuzzyEqual(-1.485994289136948, c);

        c = BigDecimal.RootN(-1234.5678, 17);
        BigDecimalAssert.AreFuzzyEqual(-1.52003581302643, c);
    }

    /// <summary>
    /// Test even roots of negative values throw exceptions.
    /// </summary>
    [TestMethod]
    public void TestEvenRootOfNegativeThrowsException()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-27, 4));
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-16, 6));
        Assert.ThrowsException<ArithmeticException>(() => BigDecimal.RootN(-1234.5678, 16));
    }

    [TestMethod]
    public void TestRootMaxNumberOfTerms()
    {
        double x;
        uint n;
        int nTerms = 1;
        var rnd = new Random();
        BigDecimal y = 0;

        // for (var i = 0; i < nTerms; i++)
        // {
        //     x = double.Abs(rnd.NextDoubleFullRange());
        //     n = rnd.NextUint();
        //     Console.WriteLine($"x = {x}, n = {n}");
        //     y = BigDecimal.RootN(x, n);
        //     Console.WriteLine($"RootN({x}, {n}) = {y:E10}");
        // }

        x = 1.23456789e5;
        n = 3;
        Console.WriteLine($"x = {x}, n = {n}");
        y = BigDecimal.RootN(x, n);
        Console.WriteLine($"RootN({x}, {n}) = {y:E10}");
    }

    #endregion Test methods for finding roots

    #region Test logarithm methods

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
            Trace.WriteLine("--------------------------------------------------");

            double d = i;
            var logD = double.Log(d);
            Trace.WriteLine($"double.Log({d})     = {logD}");

            BigDecimal bd = i;
            var logBD = BigDecimal.Log(bd);
            Trace.WriteLine($"BigDecimal.Log({bd}) = {logBD}");

            BigDecimalAssert.AreFuzzyEqual(logD, logBD);
        }
    }

    [TestMethod]
    public void LogSpeedTest()
    {
        for (var i = 1; i < 10; i++)
        {
            Trace.WriteLine("--------------------------------------------------");

            var t1 = DateTime.Now.Ticks;
            var log = BigDecimal.Log(i);
            var t2 = DateTime.Now.Ticks;
            var tLog = t2 - t1;
            Trace.WriteLine($"Log({i}) == {log}");
            Trace.WriteLine($"{tLog} ticks.");
        }
    }

    #endregion Test logarithm methods
}
