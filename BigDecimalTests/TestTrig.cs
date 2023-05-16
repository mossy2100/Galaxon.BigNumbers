using System.Diagnostics;
using Galaxon.Core.Exceptions;

namespace Galaxon.Numerics.BigDecimalTests;

[TestClass]
public class TestTrig
{
    [TestMethod]
    public void TestSin()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -360; i <= 360; i += 15)
        {
            var d = i * double.Pi / 180.0;
            var sinD = double.Sin(d);
            var strSinD = sinD.ToString("F13");
            if (strSinD == "-0.0000000000000")
            {
                strSinD = "0.0000000000000";
            }

            var bd = i * BigDecimal.Pi / 180;
            var sinBd = BigDecimal.Sin(bd);
            var strSinBd = sinBd.ToString("F13");

            Assert.AreEqual(strSinD, strSinBd);
        }
    }

    [TestMethod]
    public void TestCos()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -360; i <= 360; i += 15)
        {
            var d = i * double.Pi / 180.0;
            var cosD = double.Cos(d);
            var strCosD = cosD.ToString("F13");
            if (strCosD == "-0.0000000000000")
            {
                strCosD = "0.0000000000000";
            }

            var bd = i * BigDecimal.Pi / 180;
            var cosBd = BigDecimal.Cos(bd);
            var strCosBd = cosBd.ToString("F13");

            Assert.AreEqual(strCosD, strCosBd);
        }
    }

    [TestMethod]
    public void TestTan()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -360; i <= 360; i += 15)
        {
            // Skip values for which tan is undefined.
            if (i is -270 or -90 or 90 or 270)
            {
                continue;
            }

            var d = i * double.Pi / 180.0;
            var tanD = double.Tan(d);
            var strTanD = tanD.ToString("F13");
            if (strTanD == "-0.0000000000000")
            {
                strTanD = "0.0000000000000";
            }

            var bd = i * BigDecimal.Pi / 180;
            var tanBd = BigDecimal.Tan(bd);
            var strTanBd = tanBd.ToString("F13");

            Assert.AreEqual(strTanD, strTanBd);
        }
    }

    [TestMethod]
    public void TestAsin()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -10; i <= 10; i++)
        {
            var d = i / 10.0;
            var asinD = double.Asin(d);
            var strAsinD = asinD.ToString("F13");
            if (strAsinD == "-0.0000000000000")
            {
                strAsinD = "0.0000000000000";
            }

            BigDecimal bd = d;
            var asinBb = BigDecimal.Asin(bd);
            var strAsinBd = asinBb.ToString("F13");

            Assert.AreEqual(strAsinD, strAsinBd);
        }
    }

    [TestMethod]
    public void TestAcos()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -10; i <= 10; i++)
        {
            var d = i / 10.0;
            var acosD = double.Acos(d);
            var strAcosD = acosD.ToString("F13");
            if (strAcosD == "-0.0000000000000")
            {
                strAcosD = "0.0000000000000";
            }

            BigDecimal bd = d;
            var acosBd = BigDecimal.Acos(bd);
            var strAcosBd = acosBd.ToString("F13");

            Assert.AreEqual(strAcosD, strAcosBd);
        }
    }

    [TestMethod]
    public void TestAtan()
    {
        BigDecimal.MaxSigFigs = 30;
        const int n = 12;
        for (var i = -n; i <= n; i++)
        {
            var x = i * double.Tau / n;
            var atanD = double.Atan(x);
            var strAtanD = atanD.ToString("F13");
            if (strAtanD == "-0.0000000000000")
            {
                strAtanD = "0.0000000000000";
            }

            var bd = i * BigDecimal.Tau / n;
            var atanBd = BigDecimal.Atan(bd);
            var strAtanBd = atanBd.ToString("F13");

            Assert.AreEqual(strAtanD, strAtanBd);
        }
    }

    [TestMethod]
    public void TestAtan2ThrowsExceptionWhenBothParamsZero() =>
        Assert.ThrowsException<ArgumentInvalidException>(() => BigDecimal.Atan2(0, 0));

    [TestMethod]
    public void TestAtan2()
    {
        BigDecimal.MaxSigFigs = 30;
        const int n = 12;
        for (var i = -n; i <= n; i++)
        {
            for (var j = -n; j <= n; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                // Do the calc with doubles.
                var x = i * double.Tau / n;
                var y = j * double.Tau / n;
                var atan2D = double.Atan2(y, x);
                var strAtan2D = atan2D.ToString("F13");
                // Matching 13 decimal places with the double calculations is the best I can get.
                // Could be due to limitations of doubles in storing exact values.
                if (strAtan2D == "-0.0000000000000")
                {
                    strAtan2D = "0.0000000000000";
                }

                // Do the calc with BigDecimals.
                var bdX = i * BigDecimal.Tau / n;
                var bdY = j * BigDecimal.Tau / n;
                var atan2Bd = BigDecimal.Atan2(bdY, bdX);
                var strAtan2Bd = atan2Bd.ToString("F13");

                // Compare.
                Assert.AreEqual(strAtan2D, strAtan2Bd);
                Trace.WriteLine($"{strAtan2D} == {strAtan2Bd}");
            }
        }
    }

    [TestMethod]
    public void TestSinh()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -10; i <= 10; i++)
        {
            double d = i;
            var sinhD = double.Sinh(d);
            var strSinhD = sinhD.ToString("G14");
            if (strSinhD == "-0.0000000000000")
            {
                strSinhD = "0.0000000000000";
            }

            BigDecimal bd = i;
            var sinhBd = BigDecimal.Sinh(bd);
            var strSinhBd = sinhBd.ToString("G14");

            Assert.AreEqual(strSinhD, strSinhBd);
        }
    }

    [TestMethod]
    public void TestCosh()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -10; i <= 10; i++)
        {
            double d = i;
            var coshD = double.Cosh(d);
            var strCoshD = coshD.ToString("G14");
            if (strCoshD == "-0.0000000000000")
            {
                strCoshD = "0.0000000000000";
            }

            BigDecimal bd = i;
            var coshBd = BigDecimal.Cosh(bd);
            var strCoshBd = coshBd.ToString("G14");

            Assert.AreEqual(strCoshD, strCoshBd);
        }
    }

    [TestMethod]
    public void TestTanh()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -10; i <= 10; i++)
        {
            double d = i;
            var tanhD = double.Tanh(d);
            var strTanhD = tanhD.ToString("G14");
            if (strTanhD == "-0.0000000000000")
            {
                strTanhD = "0.0000000000000";
            }

            BigDecimal bd = i;
            var tanhBd = BigDecimal.Tanh(bd);
            var strTanhBd = tanhBd.ToString("G14");

            Assert.AreEqual(strTanhD, strTanhBd);
        }
    }

    [TestMethod]
    public void TestPolarToCartesian()
    {
    }
}
