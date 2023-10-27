using System.Numerics;

namespace Galaxon.BigNumbers.Tests.BigComplexTests;

[TestClass]
public class CoreTests
{
    /// <summary>
    /// Test the Equals() method and the equality and inequality operators.
    /// </summary>
    [TestMethod]
    public void EqualsTest()
    {
        BigComplex? z1, z2;

        z1 = null;
        z2 = null;
        Assert.IsTrue(z1.Equals(z2));
        Assert.IsTrue(z1 == z2);
        Assert.IsFalse(z1 != z2);

        z1 = null;
        z2 = BigComplex.I;
        Assert.IsFalse(z1.Equals(z2));
        Assert.IsTrue(z1 != z2);
        Assert.IsFalse(z1 == z2);

        z1 = new BigComplex(6, 23);
        z2 = null;
        Assert.IsFalse(z1.Equals(z2));
        Assert.IsTrue(z1 != z2);
        Assert.IsFalse(z1 == z2);

        z1 = BigComplex.I;
        z2 = BigComplex.I;
        Assert.IsTrue(z1.Equals(z2));
        Assert.IsTrue(z1 == z2);
        Assert.IsFalse(z1 != z2);

        z1 = 3 + 5 * BigComplex.I;
        z2 = 2 + 8 * BigComplex.I;
        Assert.IsFalse(z1.Equals(z2));
        Assert.IsTrue(z1 != z2);
        Assert.IsFalse(z1 == z2);

        z1 = 3 + 5 * BigComplex.I;
        z2 = new BigComplex(3, 5);
        Assert.IsTrue(z1.Equals(z2));
        Assert.IsTrue(z1 == z2);
        Assert.IsFalse(z1 != z2);
    }

    [TestMethod]
    public void ToStringTest()
    {
        BigComplex z;

        z = BigComplex.Zero;
        Assert.AreEqual("0", z.ToString());

        z = BigComplex.One;
        Assert.AreEqual("1", z.ToString());

        z = -BigComplex.One;
        Assert.AreEqual("-1", z.ToString());

        z = BigComplex.I;
        Assert.AreEqual("i", z.ToString());

        z = -BigComplex.I;
        Assert.AreEqual("-i", z.ToString());

        z = new BigComplex(5.123m, 0);
        Assert.AreEqual("5.123", z.ToString());

        z = new BigComplex(-6.789m, 0);
        Assert.AreEqual("-6.789", z.ToString());

        z = new BigComplex(0, 5.123m);
        Assert.AreEqual("5.123i", z.ToString());

        z = new BigComplex(0, -6.789m);
        Assert.AreEqual("-6.789i", z.ToString());

        z = new BigComplex(3, 6);
        Assert.AreEqual("3 + 6i", z.ToString());

        z = new BigComplex(3.14m, 6.88m);
        Assert.AreEqual("3.14 + 6.88i", z.ToString());

        z = new BigComplex(-2, 5);
        Assert.AreEqual("-2 + 5i", z.ToString());

        z = new BigComplex(9, -2);
        Assert.AreEqual("9 - 2i", z.ToString());

        z = new BigComplex(-17, -7);
        Assert.AreEqual("-17 - 7i", z.ToString());
    }

    [TestMethod]
    public void MagnitudePhaseTest()
    {
        BigComplex z1;
        Complex z2;

        z1 = BigComplex.Zero;
        z2 = Complex.Zero;
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = BigComplex.One;
        z2 = Complex.One;
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = BigComplex.ImaginaryOne;
        z2 = Complex.ImaginaryOne;
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = -BigComplex.One;
        z2 = new Complex(-1, 0);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = -BigComplex.ImaginaryOne;
        z2 = -Complex.ImaginaryOne;
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = new BigComplex(1, 1);
        z2 = new Complex(1, 1);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = new BigComplex(-1, -1);
        z2 = new Complex(-1, -1);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = new BigComplex(3, 4);
        z2 = new Complex(3, 4);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = new BigComplex(-5, 6);
        z2 = new Complex(-5, 6);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = new BigComplex(3.14m, 2.81m);
        z2 = new Complex(3.14, 2.81);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);

        z1 = new BigComplex(-3.14m, -2.81m);
        z2 = new Complex(-3.14, -2.81);
        BigDecimal.AssertAreEqual(Complex.Abs(z2), BigComplex.Abs(z1));
        BigDecimal.AssertAreEqual(z2.Magnitude, z1.Magnitude);
        BigDecimal.AssertAreEqual(z2.Phase, z1.Phase);
    }

    [TestMethod]
    public void FromPolarCoordinatesTest()
    {
        BigComplex z1;
        Complex z2;

        z1 = BigComplex.FromPolarCoordinates(0, 0);
        z2 = Complex.FromPolarCoordinates(0, 0);
        BigComplex.AssertAreEqual(z2, z1);

        z1 = BigComplex.FromPolarCoordinates(1, 0);
        z2 = Complex.FromPolarCoordinates(1, 0);
        BigComplex.AssertAreEqual(z2, z1);

        z1 = BigComplex.FromPolarCoordinates(1, BigDecimal.Pi / 2);
        z2 = Complex.FromPolarCoordinates(1, Math.PI / 2);
        BigComplex.AssertAreEqual(z2, z1);

        z1 = BigComplex.FromPolarCoordinates(1, -BigDecimal.Pi / 2);
        z2 = Complex.FromPolarCoordinates(1, -Math.PI / 2);
        BigComplex.AssertAreEqual(z2, z1);

        z1 = BigComplex.FromPolarCoordinates(1.23456789m, 1.23456789m);
        z2 = Complex.FromPolarCoordinates(1.23456789, 1.23456789);
        BigComplex.AssertAreEqual(z2, z1);

        z1 = BigComplex.FromPolarCoordinates(1.23456789m, -1.23456789m);
        z2 = Complex.FromPolarCoordinates(1.23456789, -1.23456789);
        BigComplex.AssertAreEqual(z2, z1);
    }
}
