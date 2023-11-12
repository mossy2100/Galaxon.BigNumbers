using Galaxon.BigNumbers.TestTools;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalPowTests
{
    [TestMethod]
    public void CubeRootOfNegative_VariousMethods()
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
        Assert.ThrowsException<OverflowException>(() =>
        {
            var y2 = (BigDecimal)1 / 3;
            return BigDecimal.Pow(x, y2);
        });
    }

    #region Base 0

    [TestMethod]
    public void Pow_Base0Exponent0_Returns1()
    {
        BigDecimal x = 0;
        BigDecimal y = 0;
        var actual = BigDecimal.Pow(x, y);
        BigDecimal expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base0Exponent1_Returns0()
    {
        BigDecimal x = 0;
        BigDecimal y = 1;
        var actual = BigDecimal.Pow(x, y);
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
        var actual = BigDecimal.Pow(x, y);
        BigDecimal expected = 1;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Pow_Base1Exponent1_Returns1()
    {
        BigDecimal x = 1;
        BigDecimal y = 1;
        var actual = BigDecimal.Pow(x, y);
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
        expected = 0.25m;
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
}
