using System.Numerics;

namespace Galaxon.BigNumbers.Tests.BigComplexTests;

[TestClass]
public class ExpLogTests
{
    [TestMethod]
    public void SqrtTest()
    {
        BigComplex z1;
        Complex z2;

        z1 = BigComplex.Zero;
        z2 = Complex.Zero;
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = BigComplex.One;
        z2 = Complex.One;
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = BigComplex.I;
        z2 = Complex.ImaginaryOne;
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(1, 1);
        z2 = new Complex(1, 1);
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(3, 4);
        z2 = new Complex(3, 4);
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(-5, 6);
        z2 = new Complex(-5, 6);
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));

        z1 = new BigComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        BigComplex.AssertAreEqual(Complex.Sqrt(z2), BigComplex.Sqrt(z1));
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
        BigComplex.AssertAreEqual(expected, actual);

        z1 = BigComplex.I;
        z2 = Complex.ImaginaryOne;
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = -BigComplex.One;
        z2 = -Complex.One;
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = -BigComplex.I;
        z2 = -Complex.ImaginaryOne;
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(1, 1);
        z2 = new Complex(1, 1);
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(3, 4);
        z2 = new Complex(3, 4);
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(-5, 6);
        z2 = new Complex(-5, 6);
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));

        z1 = new BigComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        BigComplex.AssertAreEqual(Complex.Reciprocal(z2), BigComplex.Reciprocal(z1));
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
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // i
        bc = BigComplex.I;
        c = Complex.ImaginaryOne;
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -1
        bc = -BigComplex.One;
        // NB: Setting c = -Complex.One doesn't work here.
        // The Complex unary negation operator negates a 0 real or imaginary
        // part to -0, which is valid for double, but it causes Atan2() to
        // return -π instead of π for the phase, thus causing Log() to return
        // the wrong result.
        c = new Complex(-1, 0);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -i
        bc = -BigComplex.I;
        // Cannot use -Complex.ImaginaryOne; See above note.
        c = new Complex(0, -1);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 1+i
        bc = new BigComplex(1, 1);
        c = new Complex(1, 1);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 1-i
        bc = new BigComplex(1, -1);
        c = new Complex(1, -1);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 3.14+2.81i
        bc = new BigComplex(3.14m, 2.81m);
        c = new Complex(3.14, 2.81);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // 3.14-2.81i
        bc = new BigComplex(3.14m, -2.81m);
        c = new Complex(3.14, -2.81);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -3.14+2.81i
        bc = new BigComplex(-3.14m, 2.81m);
        c = new Complex(-3.14, 2.81);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));

        // -3.14-2.81i
        bc = new BigComplex(-3.14m, -2.81m);
        c = new Complex(-3.14, -2.81);
        BigComplex.AssertAreEqual(Complex.Log(c), BigComplex.Log(bc));
    }

    [TestMethod]
    public void ExpTest()
    {
        BigComplex z;

        z = 0;
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = 1;
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = -1;
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = BigComplex.I;
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = -BigComplex.I;
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = BigDecimal.Ln10;
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        // These throw OverflowExceptions.
        // z = decimal.MaxValue;
        // BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));
        // z = decimal.MinValue;
        // BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        // z = BigDecimals.SmallestNonZeroDec;
        // BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(0, BigDecimal.Pi);
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(1, 1);
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(3, 4);
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(-5, 6);
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(3.14m, 2.81m);
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));

        z = new BigComplex(-3.14m, -2.81m);
        BigComplex.AssertAreEqual(Complex.Exp((Complex)z), BigComplex.Exp(z));
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
        BigComplex z1;
        BigComplex w1 = new (3, 4);
        Complex z2;
        Complex w2 = new (3, 4);

        z1 = BigComplex.Zero;
        z2 = Complex.Zero;
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));

        z1 = BigComplex.One;
        z2 = Complex.One;
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, -1), BigComplex.Pow(z1, -1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = BigComplex.I;
        z2 = Complex.ImaginaryOne;
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, -1), BigComplex.Pow(z1, -1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = -BigComplex.One;
        z2 = new Complex(-1, 0);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, -1), BigComplex.Pow(z1, -1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = -BigComplex.I;
        z2 = new Complex(0, -1);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, -1), BigComplex.Pow(z1, -1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = new BigComplex(1, 1);
        z2 = new Complex(1, 1);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = new BigComplex(3, 4);
        z2 = new Complex(3, 4);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = new BigComplex(-5, 6);
        z2 = new Complex(-5, 6);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = new BigComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));

        z1 = new BigComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        BigComplex.AssertAreEqual(Complex.Pow(z2, 0), BigComplex.Pow(z1, 0));
        BigComplex.AssertAreEqual(Complex.Pow(z2, 1), BigComplex.Pow(z1, 1));
        BigComplex.AssertAreEqual(Complex.Pow(z2, Complex.ImaginaryOne),
            BigComplex.Pow(z1, BigComplex.I));
        BigComplex.AssertAreEqual(Complex.Pow(z2, w2), BigComplex.Pow(z1, w1));
    }
}
