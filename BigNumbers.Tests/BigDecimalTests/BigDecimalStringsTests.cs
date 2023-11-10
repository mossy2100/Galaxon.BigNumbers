using System.Numerics;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Strings;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalStringsTests
{
    [TestMethod]
    public void TestToStringD()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("D");
        Assert.AreEqual("12345", str);

        bd = BigDecimal.Parse("12345e67");
        str = bd.ToString("D");
        Assert.AreEqual("12345E+67", str);

        bd = BigDecimal.Parse("12345e-67");
        str = bd.ToString("D");
        Assert.AreEqual("12345E-67", str);

        bd = BigDecimal.Parse("-12345e67");
        str = bd.ToString("D");
        Assert.AreEqual("-12345E+67", str);

        bd = BigDecimal.Parse("-12345e-67");
        str = bd.ToString("D");
        Assert.AreEqual("-12345E-67", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("D");
        Assert.AreEqual(
            "314159265358979323846264338327950288419716939937510"
            + "5820974944592307816406286208998628034825342117068E-99", str);
    }

    [TestMethod]
    public void TestToStringDU()
    {
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("DU");
        Assert.AreEqual("12345", str);

        bd = BigDecimal.Parse("12345e67");
        str = bd.ToString("DU");
        Assert.AreEqual("12345×10⁶⁷", str);

        bd = BigDecimal.Parse("12345e-67");
        str = bd.ToString("DU");
        Assert.AreEqual("12345×10⁻⁶⁷", str);

        bd = BigDecimal.Parse("-12345e67");
        str = bd.ToString("DU");
        Assert.AreEqual("-12345×10⁶⁷", str);

        bd = BigDecimal.Parse("-12345e-67");
        str = bd.ToString("DU");
        Assert.AreEqual("-12345×10⁻⁶⁷", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("DU");
        Assert.AreEqual("31415926535897932384626433832795028841971693993751058209749445923078164"
            + "06286208998628034825342117068×10⁻" + "99".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringE()
    {
        BigDecimal.MaxSigFigs = 100;
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E+004", str);

        bd = BigDecimal.Parse("12345e67");
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E+071", str);

        bd = BigDecimal.Parse("12345e-67");
        str = bd.ToString("E");
        Assert.AreEqual("1.2345E-063", str);

        bd = BigDecimal.Parse("-12345e67");
        str = bd.ToString("E");
        Assert.AreEqual("-1.2345E+071", str);

        bd = BigDecimal.Parse("-12345e-67");
        str = bd.ToString("E");
        Assert.AreEqual("-1.2345E-063", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E");
        Assert.AreEqual("3.141592653589793238462643383279502884197169399375105820974944592307816406"
            + "286208998628034825342117068E+000", str);
    }

    [TestMethod]
    public void TestToStringE2()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E+000", str);

        bd = 12345;
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E+004", str);

        bd = 12345e67;
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E+071", str);

        bd = 12345e-67;
        str = bd.ToString("E2");
        Assert.AreEqual("1.23E-063", str);

        bd = -12345e67;
        str = bd.ToString("E2");
        Assert.AreEqual("-1.23E+071", str);

        bd = -12345e-67;
        str = bd.ToString("E2");
        Assert.AreEqual("-1.23E-063", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E2");
        Assert.AreEqual("3.14E+000", str);
    }

    [TestMethod]
    public void TestToStringEU()
    {
        BigDecimal.MaxSigFigs = 100;
        BigDecimal bd;
        string str;

        bd = 12345;
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "4".ToSuperscript(), str);

        bd = BigDecimal.Parse("12345e67");
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "71".ToSuperscript(), str);

        bd = BigDecimal.Parse("12345e-67");
        str = bd.ToString("EU");
        Assert.AreEqual("1.2345×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Parse("-12345e67");
        str = bd.ToString("EU");
        Assert.AreEqual("-1.2345×10" + "71".ToSuperscript(), str);

        bd = BigDecimal.Parse("-12345e-67");
        str = bd.ToString("EU");
        Assert.AreEqual("-1.2345×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Pi;
        str = bd.ToString("EU");
        Assert.AreEqual("3.141592653589793238462643383279502884197169399375105820974944592307816406"
            + "286208998628034825342117068×10" + "0".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringE2U()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "0".ToSuperscript(), str);

        bd = 12345;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "4".ToSuperscript(), str);

        bd = 12345e67;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "71".ToSuperscript(), str);

        bd = 12345e-67;
        str = bd.ToString("E2U");
        Assert.AreEqual("1.23×10" + "-63".ToSuperscript(), str);

        bd = -12345e67;
        str = bd.ToString("E2U");
        Assert.AreEqual("-1.23×10" + "71".ToSuperscript(), str);

        bd = -12345e-67;
        str = bd.ToString("E2U");
        Assert.AreEqual("-1.23×10" + "-63".ToSuperscript(), str);

        bd = BigDecimal.Pi;
        str = bd.ToString("E2U");
        Assert.AreEqual("3.14×10" + "0".ToSuperscript(), str);
    }

    [TestMethod]
    public void TestToStringF()
    {
        BigDecimal.MaxSigFigs = 100;
        BigDecimal bd;
        string str;

        bd = 1.2345m;
        str = bd.ToString("F");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("F");
        Assert.AreEqual("12345", str);

        bd = BigDecimal.Parse("12345e2");
        str = bd.ToString("F");
        Assert.AreEqual("1234500", str);

        bd = BigDecimal.Parse("1.2345e7");
        str = bd.ToString("F");
        Assert.AreEqual("12345000", str);

        bd = BigDecimal.Parse("1.2345e-2");
        str = bd.ToString("F");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("12345e-6");
        str = bd.ToString("F");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("-12345e2");
        str = bd.ToString("F");
        Assert.AreEqual("-1234500", str);

        bd = BigDecimal.Parse("-12345e-6");
        str = bd.ToString("F");
        Assert.AreEqual("-0.012345", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F");
        Assert.AreEqual("3.141592653589793238462643383279502884197169399375105820974944592307816406"
            + "286208998628034825342117068", str);
    }

    [TestMethod]
    public void TestToStringF0()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("F0");
        Assert.AreEqual("1", str);

        bd = 12345.5;
        str = bd.ToString("F0");
        Assert.AreEqual("12346", str);

        bd = 123.45;
        str = bd.ToString("F0");
        Assert.AreEqual("123", str);

        bd = 12345;
        str = bd.ToString("F0");
        Assert.AreEqual("12345", str);

        bd = 12345e2;
        str = bd.ToString("F0");
        Assert.AreEqual("1234500", str);

        bd = 1.2345e7;
        str = bd.ToString("F0");
        Assert.AreEqual("12345000", str);

        bd = 1.2345e-2;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = 12345e-6;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = -12345e2;
        str = bd.ToString("F0");
        Assert.AreEqual("-1234500", str);

        bd = -12345e-6;
        str = bd.ToString("F0");
        Assert.AreEqual("0", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F0");
        Assert.AreEqual("3", str);

        bd = BigDecimal.E;
        str = bd.ToString("F0");
        Assert.AreEqual("3", str);
    }

    [TestMethod]
    public void TestToStringF2()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("F2");
        Assert.AreEqual("1.23", str);

        bd = 123.45;
        str = bd.ToString("F2");
        Assert.AreEqual("123.45", str);

        bd = 12345;
        str = bd.ToString("F2");
        Assert.AreEqual("12345.00", str);

        bd = 12345e2;
        str = bd.ToString("F2");
        Assert.AreEqual("1234500.00", str);

        bd = 1.2345e7;
        str = bd.ToString("F2");
        Assert.AreEqual("12345000.00", str);

        bd = 1.2345e-2;
        str = bd.ToString("F2");
        Assert.AreEqual("0.01", str);

        bd = 12345e-6;
        str = bd.ToString("F2");
        Assert.AreEqual("0.01", str);

        bd = -12345e2;
        str = bd.ToString("F2");
        Assert.AreEqual("-1234500.00", str);

        bd = -12345e-6;
        str = bd.ToString("F2");
        Assert.AreEqual("-0.01", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("F2");
        Assert.AreEqual("3.14", str);
    }

    [TestMethod]
    public void TestToStringG()
    {
        BigDecimal.MaxSigFigs = 100;
        BigDecimal bd;
        string str;

        bd = BigDecimal.Parse("1.2345");
        str = bd.ToString("G");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("G");
        Assert.AreEqual("12345", str);

        bd = BigDecimal.Parse("12345e2");
        str = bd.ToString("G");
        Assert.AreEqual("1234500", str);

        bd = BigDecimal.Parse("1.2345e7");
        str = bd.ToString("G");
        Assert.AreEqual("12345000", str);

        bd = BigDecimal.Parse("1.2345e-2");
        str = bd.ToString("G");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("12345e-6");
        str = bd.ToString("G");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("-12345e2");
        str = bd.ToString("G");
        Assert.AreEqual("-1234500", str);

        bd = BigDecimal.Parse("-12345e-6");
        str = bd.ToString("G");
        Assert.AreEqual("-0.012345", str);

        bd = BigDecimal.Parse("12345e67");
        str = bd.ToString("G");
        Assert.AreEqual("1.2345E+71", str);

        bd = BigDecimal.Parse("12345e-67");
        str = bd.ToString("G");
        Assert.AreEqual("1.2345E-63", str);

        bd = BigDecimal.Parse("-12345e67");
        str = bd.ToString("G");
        Assert.AreEqual("-1.2345E+71", str);

        bd = BigDecimal.Parse("-12345e-67");
        str = bd.ToString("G");
        Assert.AreEqual("-1.2345E-63", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("G");
        Assert.AreEqual("3.141592653589793238462643383279502884197169399375105820974944592307816406"
            + "286208998628034825342117068", str);
    }

    [TestMethod]
    public void TestToStringG3()
    {
        BigDecimal bd;
        string str;

        bd = 123;
        str = bd.ToString("G3");
        Assert.AreEqual("123", str);

        bd = 123.45;
        str = bd.ToString("G3");
        Assert.AreEqual("123", str);

        bd = 1.2345;
        str = bd.ToString("G3");
        Assert.AreEqual("1.23", str);

        bd = 12345;
        str = bd.ToString("G3");
        Assert.AreEqual("12300", str);

        bd = 12345e2;
        str = bd.ToString("G3");
        Assert.AreEqual("1230000", str);

        bd = 1.2345e7;
        str = bd.ToString("G3");
        Assert.AreEqual("12300000", str);

        bd = 1.2345e-2;
        str = bd.ToString("G3");
        Assert.AreEqual("0.0123", str);

        bd = 12345e-6;
        str = bd.ToString("G3");
        Assert.AreEqual("0.0123", str);

        bd = -12345e2;
        str = bd.ToString("G3");
        Assert.AreEqual("-1230000", str);

        bd = -12345e-6;
        str = bd.ToString("G3");
        Assert.AreEqual("-0.0123", str);

        bd = 12345e67;
        str = bd.ToString("G3");
        Assert.AreEqual("1.23E+71", str);

        bd = 12345e-67;
        str = bd.ToString("G3");
        Assert.AreEqual("1.23E-63", str);

        bd = -12345e67;
        str = bd.ToString("G3");
        Assert.AreEqual("-1.23E+71", str);

        bd = -12345e-67;
        str = bd.ToString("G3");
        Assert.AreEqual("-1.23E-63", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("G3");
        Assert.AreEqual("3.14", str);
    }

    [TestMethod]
    public void TestToStringGU()
    {
        BigDecimal.MaxSigFigs = 100;
        BigDecimal bd;
        string str;

        bd = BigDecimal.Parse("1.2345");
        str = bd.ToString("GU");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("GU");
        Assert.AreEqual("12345", str);

        bd = BigDecimal.Parse("12345e2");
        str = bd.ToString("GU");
        Assert.AreEqual("1234500", str);

        bd = BigDecimal.Parse("1.2345e7");
        str = bd.ToString("GU");
        Assert.AreEqual("12345000", str);

        bd = BigDecimal.Parse("1.2345e-2");
        str = bd.ToString("GU");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("12345e-6");
        str = bd.ToString("GU");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("-12345e2");
        str = bd.ToString("GU");
        Assert.AreEqual("-1234500", str);

        bd = BigDecimal.Parse("-12345e-6");
        str = bd.ToString("GU");
        Assert.AreEqual("-0.012345", str);

        bd = BigDecimal.Parse("12345e67");
        str = bd.ToString("GU");
        Assert.AreEqual("1.2345×10⁷¹", str);

        bd = BigDecimal.Parse("12345e-67");
        str = bd.ToString("GU");
        Assert.AreEqual("1.2345×10⁻⁶³", str);

        bd = BigDecimal.Parse("-12345e67");
        str = bd.ToString("GU");
        Assert.AreEqual("-1.2345×10⁷¹", str);

        bd = BigDecimal.Parse("-12345e-67");
        str = bd.ToString("GU");
        Assert.AreEqual("-1.2345×10⁻⁶³", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("GU");
        Assert.AreEqual("3.141592653589793238462643383279502884197169399375105820974944592307816406"
            + "286208998628034825342117068", str);
    }

    [TestMethod]
    public void TestToStringN()
    {
        BigDecimal.MaxSigFigs = 100;
        BigDecimal bd;
        string str;

        bd = 1.2345m;
        str = bd.ToString("N");
        Assert.AreEqual("1.2345", str);

        bd = 12345;
        str = bd.ToString("N");
        Assert.AreEqual("12,345", str);

        bd = BigDecimal.Parse("12345e2");
        str = bd.ToString("N");
        Assert.AreEqual("1,234,500", str);

        bd = BigDecimal.Parse("1.2345e7");
        str = bd.ToString("N");
        Assert.AreEqual("12,345,000", str);

        bd = BigDecimal.Parse("1.2345e-2");
        str = bd.ToString("N");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("12345e-6");
        str = bd.ToString("N");
        Assert.AreEqual("0.012345", str);

        bd = BigDecimal.Parse("-12345e2");
        str = bd.ToString("N");
        Assert.AreEqual("-1,234,500", str);

        bd = BigDecimal.Parse("-12345e-6");
        str = bd.ToString("N");
        Assert.AreEqual("-0.012345", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("N");
        Assert.AreEqual("3.141592653589793238462643383279502884197169399375105820974944592307816406"
            + "286208998628034825342117068", str);
    }

    [TestMethod]
    public void TestToStringN0()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345m;
        str = bd.ToString("N0");
        Assert.AreEqual("1", str);

        bd = 12345;
        str = bd.ToString("N0");
        Assert.AreEqual("12,345", str);

        bd = BigDecimal.Parse("12345e2");
        str = bd.ToString("N0");
        Assert.AreEqual("1,234,500", str);

        bd = BigDecimal.Parse("1.2345e7");
        str = bd.ToString("N0");
        Assert.AreEqual("12,345,000", str);

        bd = BigDecimal.Parse("-12345e2");
        str = bd.ToString("N0");
        Assert.AreEqual("-1,234,500", str);
    }

    [TestMethod]
    public void TestToStringN2()
    {
        BigDecimal bd;
        string str;

        bd = 1.2345;
        str = bd.ToString("N2");
        Assert.AreEqual("1.23", str);

        bd = 123.45;
        str = bd.ToString("N2");
        Assert.AreEqual("123.45", str);

        bd = 12345;
        str = bd.ToString("N2");
        Assert.AreEqual("12,345.00", str);

        bd = 12345e2;
        str = bd.ToString("N2");
        Assert.AreEqual("1,234,500.00", str);

        bd = 1.2345e7;
        str = bd.ToString("N2");
        Assert.AreEqual("12,345,000.00", str);

        bd = 1.2345e-2;
        str = bd.ToString("N2");
        Assert.AreEqual("0.01", str);

        bd = 12345e-6;
        str = bd.ToString("N2");
        Assert.AreEqual("0.01", str);

        bd = -12345e2;
        str = bd.ToString("N2");
        Assert.AreEqual("-1,234,500.00", str);

        bd = -12345e-6;
        str = bd.ToString("N2");
        Assert.AreEqual("-0.01", str);

        bd = BigDecimal.Pi;
        str = bd.ToString("N2");
        Assert.AreEqual("3.14", str);
    }

    [TestMethod]
    public void TestParse0()
    {
        var bd = BigDecimal.Parse("0");
        Assert.AreEqual(0, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParse1()
    {
        var bd = BigDecimal.Parse("1");
        Assert.AreEqual(1, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParse2()
    {
        var bd = BigDecimal.Parse("2");
        Assert.AreEqual(2, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParseMinus1()
    {
        var bd = BigDecimal.Parse("-1");
        Assert.AreEqual(-1, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParsePlus1()
    {
        var bd = BigDecimal.Parse("+1");
        Assert.AreEqual(1, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParse10()
    {
        var bd = BigDecimal.Parse("10");
        Assert.AreEqual(1, bd.Significand);
        Assert.AreEqual(1, bd.Exponent);
    }

    [TestMethod]
    public void TestParseMinus200()
    {
        var bd = BigDecimal.Parse("-200");
        Assert.AreEqual(-2, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParsePositiveFloat()
    {
        var bd = BigDecimal.Parse("3.14");
        Assert.AreEqual(314, bd.Significand);
        Assert.AreEqual(-2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNegativeFloat()
    {
        var bd = BigDecimal.Parse("-6.28");
        Assert.AreEqual(-628, bd.Significand);
        Assert.AreEqual(-2, bd.Exponent);
    }

    [TestMethod]
    public void TestParsePi()
    {
        BigDecimal.MaxSigFigs = 101;
        var bd = BigDecimal.Pi;
        var expectedSignificand = BigInteger.Parse("3"
            + "14159265358979323846264338327950288419716939937510"
            + "5820974944592307816406286208998628034825342117068");
        Assert.AreEqual(expectedSignificand, bd.Significand);
        Assert.AreEqual(-99, bd.Exponent);
    }

    [TestMethod]
    public void TestParseFloatWithPositiveExponent()
    {
        // Avagadro's number.
        var bd = BigDecimal.Parse("6.0221408e+23");
        Assert.AreEqual(60221408, bd.Significand);
        Assert.AreEqual(16, bd.Exponent);
    }

    [TestMethod]
    public void TestParseFloatWithPositiveExponentNoE()
    {
        // Astronomical unit in meters.
        var bd = BigDecimal.Parse("1.496e11");
        Assert.AreEqual(1496, bd.Significand);
        Assert.AreEqual(8, bd.Exponent);
    }

    [TestMethod]
    public void TestParseFloatWithNegativeExponent()
    {
        // Charge on an electron.
        var bd = BigDecimal.Parse("1.60217663e-19");
        Assert.AreEqual(160217663, bd.Significand);
        Assert.AreEqual(-27, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNumberWithCommasForThousandsSeparators()
    {
        // Astronomical unit in meters.
        var bd = BigDecimal.Parse("149,597,870,700");
        Assert.AreEqual(1495978707, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNumberWithSpacesForThousandsSeparators()
    {
        // Astronomical unit in meters.
        var bd = BigDecimal.Parse("149 597 870 700");
        Assert.AreEqual(1495978707, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseNumberWithUnderscoresForThousandsSeparators()
    {
        // Astronomical unit in meters.
        var bd = BigDecimal.Parse("149_597_870_700");
        Assert.AreEqual(1495978707, bd.Significand);
        Assert.AreEqual(2, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroFraction()
    {
        var bd = BigDecimal.Parse("427.0000");
        Assert.AreEqual(427, bd.Significand);
        Assert.AreEqual(0, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroFraction2()
    {
        var bd = BigDecimal.Parse("42.700");
        Assert.AreEqual(427, bd.Significand);
        Assert.AreEqual(-1, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroInteger()
    {
        var bd = BigDecimal.Parse("0.7");
        Assert.AreEqual(7, bd.Significand);
        Assert.AreEqual(-1, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroInteger2()
    {
        var bd = BigDecimal.Parse("0000.735");
        Assert.AreEqual(735, bd.Significand);
        Assert.AreEqual(-3, bd.Exponent);
    }

    [TestMethod]
    public void TestParseZeroExponent()
    {
        var bd = BigDecimal.Parse("3.1416e0");
        Assert.AreEqual(31416, bd.Significand);
        Assert.AreEqual(-4, bd.Exponent);
    }

    [TestMethod]
    public void TestParseEmptyString()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse(""));
    }

    [TestMethod]
    public void TestParseInvalidFormat2()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("e15"));
    }

    [TestMethod]
    public void TestParseInvalidFormat3()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("1.445345.8"));
    }

    [TestMethod]
    public void TestParseMissingIntegerDigits()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse(".1414"));
    }

    [TestMethod]
    public void TestParseMissingFractionDigits()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("612."));
    }

    [TestMethod]
    public void TestParseMissingExponentDigits()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("6.02e"));
    }

    [TestMethod]
    public void TestParseWord()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("cat"));
    }

    [TestMethod]
    public void TestParseQuantity()
    {
        Assert.ThrowsException<ArgumentFormatException>(() => BigDecimal.Parse("3891.6 km"));
    }
}
