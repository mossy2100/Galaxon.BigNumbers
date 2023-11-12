namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class _BigDecimalInitializeTests
{
    [TestInitialize]
    public static void Initialize(TestContext context)
    {
        BigDecimal.MaxSigFigs = 50;
    }
}
