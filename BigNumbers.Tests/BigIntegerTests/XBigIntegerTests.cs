using System.Numerics;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class XBigIntegerTests
{
    [TestMethod]
    public void Reverse_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.Reverse();

        // Assert
        Assert.AreEqual(987654321, result);
    }

    [TestMethod]
    public void IsPalindromic_ReturnsTrueForPalindromicNumber()
    {
        // Arrange
        BigInteger n = 123454321;

        // Act
        var result = n.IsPalindromic();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsPalindromic_ReturnsFalseForNonPalindromicNumber()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.IsPalindromic();

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void DigitSum_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.DigitSum();

        // Assert
        Assert.AreEqual(45, result);
    }

    [TestMethod]
    public void NumDigits_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.NumDigits();

        // Assert
        Assert.AreEqual(9, result);
    }

    [TestMethod]
    public void ToUnsigned_ReturnsCorrectValueForPositiveNumber()
    {
        // Arrange
        BigInteger n = 123456789;

        // Act
        var result = n.ToUnsigned();

        // Assert
        Assert.AreEqual(123456789, result);
    }

    [TestMethod]
    public void ToUnsigned_ReturnsCorrectValueForNegativeNumber()
    {
        // Arrange
        BigInteger n = -123456789;

        // Act
        var result = n.ToUnsigned();

        // Assert
        Assert.AreEqual(306039647, result);
    }

    [TestMethod]
    public void LeastCommonMultiple_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger a = 4;
        BigInteger b = 6;

        // Act
        var result = XBigInteger.LeastCommonMultiple(a, b);

        // Assert
        Assert.AreEqual(12, result);
    }

    [TestMethod]
    public void GreatestCommonDivisor_ReturnsCorrectValue()
    {
        // Arrange
        BigInteger a = 123456789;
        BigInteger b = 987654321;

        // Act
        var result = XBigInteger.GreatestCommonDivisor(a, b);

        // Assert
        Assert.AreEqual(9, result);
    }

    [TestMethod]
    public void Sum_ReturnsCorrectValue()
    {
        // Arrange
        List<BigInteger> nums = new () { 1, 2, 3, 4, 5 };

        // Act
        var result = nums.Sum();

        // Assert
        Assert.AreEqual(15, result);
    }

    [TestMethod]
    public void Sum_WithTransform_ReturnsCorrectValue()
    {
        // Arrange
        List<BigInteger> nums = new () { 1, 2, 3, 4, 5 };

        // Act
        var result = nums.Sum(n => n * n);

        // Assert
        Assert.AreEqual(55, result);
    }

    [TestMethod]
    public void TestToUnsignedZero()
    {
        BigInteger bi = 0;
        BigInteger expected = 0;
        var actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestToUnsignedPositive()
    {
        BigInteger bi;
        BigInteger expected;
        BigInteger actual;

        bi = 1;
        expected = 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = 123;
        expected = 123;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = sbyte.MaxValue;
        expected = sbyte.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = byte.MaxValue;
        expected = byte.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = int.MaxValue;
        expected = int.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = uint.MaxValue;
        expected = uint.MaxValue;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestToUnsignedNegative()
    {
        BigInteger bi;
        BigInteger expected;
        BigInteger actual;

        bi = -1;
        expected = 255;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -128;
        expected = 128;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -100;
        expected = bi + byte.MaxValue + 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -10000;
        expected = bi + ushort.MaxValue + 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);

        bi = -12345678;
        expected = bi + uint.MaxValue + 1;
        actual = bi.ToUnsigned();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestNumDigitsZero()
    {
        BigInteger bi;
        int nDigits;

        bi = 0;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);
    }

    [TestMethod]
    public void TestNumDigitsPositive()
    {
        BigInteger bi;
        int nDigits;

        bi = 1;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = 9;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = 10;
        nDigits = bi.NumDigits();
        Assert.AreEqual(2, nDigits);

        bi = 999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(3, nDigits);

        bi = 1000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(4, nDigits);

        bi = 999_999_999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(9, nDigits);

        bi = 1_000_000_000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(10, nDigits);

        bi = BigInteger.Parse("99999999999999999999");
        nDigits = bi.NumDigits();
        Assert.AreEqual(20, nDigits);
    }

    [TestMethod]
    public void TestNumDigitsNegative()
    {
        BigInteger bi;
        int nDigits;

        bi = -1;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = -9;
        nDigits = bi.NumDigits();
        Assert.AreEqual(1, nDigits);

        bi = -10;
        nDigits = bi.NumDigits();
        Assert.AreEqual(2, nDigits);

        bi = -999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(3, nDigits);

        bi = -1000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(4, nDigits);

        bi = -999_999_999;
        nDigits = bi.NumDigits();
        Assert.AreEqual(9, nDigits);

        bi = -1_000_000_000;
        nDigits = bi.NumDigits();
        Assert.AreEqual(10, nDigits);

        bi = BigInteger.Parse("-99999999999999999999");
        nDigits = bi.NumDigits();
        Assert.AreEqual(20, nDigits);
    }

    [TestMethod]
    public void TestGreatestCommonDivisor()
    {
        // Equal values. 0, 1, prime, composite.
        Assert.AreEqual(0, XBigInteger.GreatestCommonDivisor(0, 0));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(1, 1));
        Assert.AreEqual(5, XBigInteger.GreatestCommonDivisor(5, 5));
        Assert.AreEqual(10, XBigInteger.GreatestCommonDivisor(10, 10));

        // 0 and 1.
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(1, 0));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(0, 1));

        // 0 and prime.
        Assert.AreEqual(5, XBigInteger.GreatestCommonDivisor(5, 0));
        Assert.AreEqual(5, XBigInteger.GreatestCommonDivisor(0, 5));

        // 0 and composite.
        Assert.AreEqual(10, XBigInteger.GreatestCommonDivisor(10, 0));
        Assert.AreEqual(10, XBigInteger.GreatestCommonDivisor(0, 10));

        // 1 and prime.
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(1, 5));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(5, 1));

        // 1 and composite.
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(1, 10));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(10, 1));

        // Prime and prime.
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(3, 7));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(7, 3));

        // Prime and composite with a common factor.
        Assert.AreEqual(5, XBigInteger.GreatestCommonDivisor(10, 5));
        Assert.AreEqual(5, XBigInteger.GreatestCommonDivisor(5, 10));

        // Prime and composite without a common factor.
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(6, 5));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(5, 6));

        // Composite and composite with a prime common factor.
        Assert.AreEqual(2, XBigInteger.GreatestCommonDivisor(4, 6));
        Assert.AreEqual(2, XBigInteger.GreatestCommonDivisor(6, 4));

        // Composite and composite with a composite common factor.
        Assert.AreEqual(4, XBigInteger.GreatestCommonDivisor(4, 16));
        Assert.AreEqual(4, XBigInteger.GreatestCommonDivisor(16, 4));

        // Composite and composite without a common factor.
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(4, 9));
        Assert.AreEqual(1, XBigInteger.GreatestCommonDivisor(9, 4));
    }
}
