namespace Galaxon.BigNumbers.BigDecimalTests;

[TestClass]
public class RoundTests
{
    /// <summary>
    /// Check BigDecimal.Round() matches decimal.Round() for all rounding methods.
    /// </summary>
    [TestMethod]
    public void TestRoundingMethods()
    {
        decimal[] values =
        {
            -2.6m, -2.51m, -2.5m, -2.49m, -2.4m, -1.6m, -1.51m, -1.5m, -1.49m, -1.4m,
            1.4m, 1.49m, 1.5m, 1.51m, 1.6m, 2.4m, 2.49m, 2.5m, 2.51m, 2.6m,
        };
        foreach (var value in values)
        {
            foreach (var method in Enum.GetValues<MidpointRounding>())
            {
                BigDecimal expected = decimal.Round(value, method);
                var actual = BigDecimal.Round(value, 0, method);
                Assert.AreEqual(expected, actual);
            }
        }
    }

    [TestMethod]
    public void TestRoundPi()
    {
        var pi = BigDecimal.Round(BigDecimal.Pi, 4);
        Assert.AreEqual(3.1416m, pi);
    }

    [TestMethod]
    public void TestRoundSigFigsToEven()
    {
        BigDecimal bd = 1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3);
        Assert.AreEqual(123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5);
        Assert.AreEqual(12346, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = 12345;
        r = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(1234, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = 12355;
        r = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(1236, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToEvenNegatives()
    {
        BigDecimal bd = -1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3);
        Assert.AreEqual(-123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5);
        Assert.AreEqual(-12346, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = -12345;
        r = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(-1234, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = -12355;
        r = BigDecimal.RoundSigFigs(bd, 4);
        Assert.AreEqual(-1236, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsAwayFromZero()
    {
        BigDecimal bd = 1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.AwayFromZero);
        Assert.AreEqual(123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.AwayFromZero);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.AwayFromZero);
        Assert.AreEqual(12346, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = 12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.AwayFromZero);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = 12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.AwayFromZero);
        Assert.AreEqual(1236, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsAwayFromZeroNegatives()
    {
        BigDecimal bd = -1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.AwayFromZero);
        Assert.AreEqual(-123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.AwayFromZero);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.AwayFromZero);
        Assert.AreEqual(-12346, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = -12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.AwayFromZero);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = -12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.AwayFromZero);
        Assert.AreEqual(-1236, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToNegativeInfinity()
    {
        BigDecimal bd = 1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(1234, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(12345, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = 12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(1234, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = 12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToNegativeInfinityNegatives()
    {
        BigDecimal bd = -1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(-124, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(-12346, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = -12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = -12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToNegativeInfinity);
        Assert.AreEqual(-1236, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToPositiveInfinity()
    {
        BigDecimal bd = 1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(124, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(12346, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = 12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = 12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(1236, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToPositiveInfinityPositives()
    {
        BigDecimal bd = -1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(-123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(-1234, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(-12345, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = -12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(-1234, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = -12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToPositiveInfinity);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToZero()
    {
        BigDecimal bd = 1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.ToZero);
        Assert.AreEqual(123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToZero);
        Assert.AreEqual(1234, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.ToZero);
        Assert.AreEqual(12345, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = 12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToZero);
        Assert.AreEqual(1234, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = 12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToZero);
        Assert.AreEqual(1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }

    [TestMethod]
    public void TestRoundSigFigsToZeroPositives()
    {
        BigDecimal bd = -1234567890;
        var r = BigDecimal.RoundSigFigs(bd, 3, MidpointRounding.ToZero);
        Assert.AreEqual(-123, r.Significand);
        Assert.AreEqual(7, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToZero);
        Assert.AreEqual(-1234, r.Significand);
        Assert.AreEqual(6, r.Exponent);

        r = BigDecimal.RoundSigFigs(bd, 5, MidpointRounding.ToZero);
        Assert.AreEqual(-12345, r.Significand);
        Assert.AreEqual(5, r.Exponent);

        bd = -12345;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToZero);
        Assert.AreEqual(-1234, r.Significand);
        Assert.AreEqual(1, r.Exponent);

        bd = -12355;
        r = BigDecimal.RoundSigFigs(bd, 4, MidpointRounding.ToZero);
        Assert.AreEqual(-1235, r.Significand);
        Assert.AreEqual(1, r.Exponent);
    }
}
