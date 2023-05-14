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
            var strSinD = sinD.ToString("F14");
            if (strSinD == "-0.00000000000000")
            {
                strSinD = "0.00000000000000";
            }

            var bd = i * BigDecimal.Pi / 180;
            var sinBD = BigDecimal.Sin(bd);
            var strSinBD = sinBD.ToString("F14");

            Assert.AreEqual(strSinD, strSinBD);
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
            var strCosD = cosD.ToString("F14");
            if (strCosD == "-0.00000000000000")
            {
                strCosD = "0.00000000000000";
            }

            var bd = i * BigDecimal.Pi / 180;
            var cosBD = BigDecimal.Cos(bd);
            var strCosBD = cosBD.ToString("F14");

            Assert.AreEqual(strCosD, strCosBD);
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
            var tanBD = BigDecimal.Tan(bd);
            var strTanBD = tanBD.ToString("F13");

            Assert.AreEqual(strTanD, strTanBD);
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
            var strAsinD = asinD.ToString("F14");
            if (strAsinD == "-0.00000000000000")
            {
                strAsinD = "0.00000000000000";
            }

            BigDecimal bd = d;
            var asinBD = BigDecimal.Asin(bd);
            var strAsinBD = asinBD.ToString("F14");

            Assert.AreEqual(strAsinD, strAsinBD);
        }
    }

    // [TestMethod]
    // public void TestAsin2()
    // {
    //     BigDecimal.MaxSigFigs = 30;
    //     for (int i = 2; i <= 10; i++)
    //     {
    //         double d = i / 10.0;
    //         if (d > 1) continue;
    //         Trace.WriteLine("");
    //         Trace.WriteLine(d);
    //         double asinD = double.Asin(d);
    //         string strAsinD = asinD.ToString("F14");
    //         if (strAsinD == "-0.00000000000000")
    //         {
    //             strAsinD = "0.00000000000000";
    //         }
    //
    //         BigDecimal bd = d;
    //
    //         long t1 = DateTime.Now.Ticks;
    //         BigDecimal asinBD = BigDecimal.Asin(bd);
    //         long t2 = DateTime.Now.Ticks;
    //         long tAsin = t2 - t1;
    //         Trace.WriteLine($"Time for Asin = {tAsin} ticks.");
    //         string strAsinBD = asinBD.ToString("F14");
    //
    //         long t3 = DateTime.Now.Ticks;
    //         BigDecimal asin2BD = BigDecimal.Asin2(bd);
    //         long t4 = DateTime.Now.Ticks;
    //         long tAsin2 = t4 - t3;
    //         Trace.WriteLine($"Time for Asin2 = {tAsin2} ticks.");
    //         string strAsin2BD = asin2BD.ToString("F14");
    //
    //         if (tAsin < tAsin2)
    //             Trace.WriteLine($"Asin() is faster when d = {d}");
    //         else
    //             Trace.WriteLine($"Asin2() is faster when d = {d}");
    //
    //         Assert.AreEqual(strAsinD, strAsinBD);
    //         Assert.AreEqual(strAsinD, strAsin2BD);
    //     }
    // }

    [TestMethod]
    public void TestAcos()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -10; i <= 10; i++)
        {
            var d = i / 10.0;
            var acosD = double.Acos(d);
            var strAcosD = acosD.ToString("F14");
            if (strAcosD == "-0.00000000000000")
            {
                strAcosD = "0.00000000000000";
            }

            BigDecimal bd = d;
            var acosBD = BigDecimal.Acos(bd);
            var strAcosBD = acosBD.ToString("F14");

            Assert.AreEqual(strAcosD, strAcosBD);
        }
    }

    [TestMethod]
    public void TestAtan()
    {
        BigDecimal.MaxSigFigs = 30;
        for (var i = -50; i <= 50; i++)
        {
            var d = i / 10.0;
            var atanD = double.Atan(d);
            var strAtanD = atanD.ToString("F14");
            if (strAtanD == "-0.00000000000000")
            {
                strAtanD = "0.00000000000000";
            }

            BigDecimal bd = d;
            var atanBD = BigDecimal.Atan(bd);
            var strAtanBD = atanBD.ToString("F14");

            Assert.AreEqual(strAtanD, strAtanBD);
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
            if (strSinhD == "-0.00000000000000")
            {
                strSinhD = "0.00000000000000";
            }

            BigDecimal bd = i;
            var sinhBD = BigDecimal.Sinh(bd);
            var strSinhBD = sinhBD.ToString("G14");

            Assert.AreEqual(strSinhD, strSinhBD);
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
            if (strCoshD == "-0.00000000000000")
            {
                strCoshD = "0.00000000000000";
            }

            BigDecimal bd = i;
            var coshBD = BigDecimal.Cosh(bd);
            var strCoshBD = coshBD.ToString("G14");

            Assert.AreEqual(strCoshD, strCoshBD);
        }
    }
}
