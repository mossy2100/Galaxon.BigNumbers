using System.Numerics;
using Galaxon.BigNumbers.TestTools;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigComplexExpTests
{
    [TestMethod]
    public void SqrtTest()
    {
        BigComplex z1;
        Complex z2;

        z1 = BigComplex.Zero;
        z2 = Complex.Zero;
        Assert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = BigComplex.One;
        z2 = Complex.One;
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = BigComplex.I;
        z2 = Complex.ImaginaryOne;
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(1, 1);
        z2 = new Complex(1, 1);
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(3, 4);
        z2 = new Complex(3, 4);
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(-5, 6);
        z2 = new Complex(-5, 6);
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        BigComplexAssert.AreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));
    }

    [TestMethod]
    public void ReciprocalTest()
    {
        BigComplex z1, actual;
        Complex z2, expected;

        z1 = BigComplex.One;
        z2 = Complex.One;
        actual = BigComplex.Reciprocal(z1);
        expected = Complex.Reciprocal(z2);
        BigComplexAssert.AreEqual(expected, actual);

        z1 = BigComplex.I;
        z2 = Complex.ImaginaryOne;
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = -BigComplex.One;
        z2 = -Complex.One;
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = -BigComplex.I;
        z2 = -Complex.ImaginaryOne;
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(1, 1);
        z2 = new Complex(1, 1);
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(3, 4);
        z2 = new Complex(3, 4);
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(-5, 6);
        z2 = new Complex(-5, 6);
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        BigComplexAssert.AreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));
    }

    [TestMethod]
    public void LnThrowsExceptionIfArgZero()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BigComplex.Log(BigComplex.Zero));
    }

    [TestMethod]
    public void DecimalComplexLnMatchesComplexLog()
    {
        BigComplex bc;
        Complex c;

        // 1
        bc = BigComplex.One;
        c = Complex.One;
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // i
        bc = BigComplex.I;
        c = Complex.ImaginaryOne;
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -1
        bc = -BigComplex.One;
        // NB: Setting c = -Complex.One doesn't work here.
        // The Complex unary negation operator negates a 0 real or imaginary
        // part to -0, which is valid for double, but it causes Atan2() to
        // return -π instead of π for the phase, thus causing Log() to return
        // the wrong result.
        c = new Complex(-1, 0);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -i
        bc = -BigComplex.I;
        // Cannot use -Complex.ImaginaryOne; See above note.
        c = new Complex(0, -1);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 1+i
        bc = new BigComplex(1, 1);
        c = new Complex(1, 1);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 1-i
        bc = new BigComplex(1, -1);
        c = new Complex(1, -1);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 3.14+2.81i
        bc = new BigComplex(3.14m, 2.81m);
        c = new Complex(3.14, 2.81);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 3.14-2.81i
        bc = new BigComplex(3.14m, -2.81m);
        c = new Complex(3.14, -2.81);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -3.14+2.81i
        bc = new BigComplex(-3.14m, 2.81m);
        c = new Complex(-3.14, 2.81);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -3.14-2.81i
        bc = new BigComplex(-3.14m, -2.81m);
        c = new Complex(-3.14, -2.81);
        BigComplexAssert.AreEqual(Complex.Log(c), BigComplex.Log(bc));
    }

    [TestMethod]
    public void ExpTest()
    {
        BigComplex z;

        z = 0;
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = 1;
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = -1;
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = BigComplex.I;
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = -BigComplex.I;
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = BigDecimal.Ln10;
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        // These throw OverflowExceptions.
        // z = decimal.MaxValue;
        // BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));
        // z = decimal.MinValue;
        // BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        // z = BigDecimal.SmallestNonZeroDec;
        // BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(0, BigDecimal.Pi);
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(1, 1);
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(3, 4);
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(-5, 6);
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(3.14m, 2.81m);
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(-3.14m, -2.81m);
        BigComplexAssert.AreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));
    }

    [TestMethod]
    [ExpectedException(typeof(ArithmeticException))]
    public void PowThrowsWhenZeroRaisedToImagNum()
    {
        BigComplex.Pow(0, BigComplex.I);
    }

    [TestMethod]
    public void PowThrowsWhenZeroRaisedToNegNum()
    {
        Assert.ThrowsException<ArithmeticException>(() => BigComplex.Pow(0, -1));
    }

    [TestMethod]
    public void PowTest()
    {
        BigComplex bc1;
        Complex c1;

        bc1 = BigComplex.Zero;
        c1 = Complex.Zero;
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));

        bc1 = BigComplex.One;
        c1 = Complex.One;
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, -1), BigComplex.Pow(bc1, -1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = BigComplex.I;
        c1 = Complex.ImaginaryOne;
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, -1), BigComplex.Pow(bc1, -1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = -BigComplex.One;
        c1 = new Complex(-1, 0);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, -1), BigComplex.Pow(bc1, -1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = -BigComplex.I;
        c1 = new Complex(0, -1);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, -1), BigComplex.Pow(bc1, -1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = new BigComplex(1, 1);
        c1 = new Complex(1, 1);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = new BigComplex(3, 4);
        c1 = new Complex(3, 4);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = new BigComplex(-5, 6);
        c1 = new Complex(-5, 6);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = new BigComplex(3.14, 2.81);
        c1 = new Complex(3.14, 2.81);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));

        bc1 = new BigComplex(-3.14, -2.81);
        c1 = new Complex(-3.14, -2.81);
        BigComplexAssert.AreEqual(Complex.Pow(c1, 0), BigComplex.Pow(bc1, 0));
        BigComplexAssert.AreEqual(Complex.Pow(c1, 1), BigComplex.Pow(bc1, 1));
        BigComplexAssert.AreEqual(Complex.Pow(c1, Complex.ImaginaryOne),
            BigComplex.Pow(bc1, BigComplex.I));
    }

    [TestMethod]
    public void TestFirstComplexRootOfNegative()
    {
        BigComplex root;

        // I got the answers to test from this online calculator:
        // https://www.emathhelp.net/en/calculators/algebra-2/nth-roots-of-complex-number-calculator/

        root = BigComplex.RootN(-16, 2);
        BigDecimalAssert.AreEqual(0, root.Real);
        BigDecimalAssert.AreEqual(4, root.Imaginary);

        root = BigComplex.RootN(-16, 3);
        BigDecimalAssert.AreEqual(1.259921049894873, root.Real);
        BigDecimalAssert.AreEqual(2.182247271943443, root.Imaginary);

        root = BigComplex.RootN(-16, 4);
        BigDecimalAssert.AreEqual(1.414213562373095, root.Real);
        BigDecimalAssert.AreEqual(1.414213562373095, root.Imaginary);
    }
}
