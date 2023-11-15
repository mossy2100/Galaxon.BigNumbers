namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigDecimalTestsInitialize
{
    /// <summary>
    /// This code will run before every test in the project.
    /// </summary>
    /// <param name="context"></param>
    [TestInitialize]
    public static void Initialize(TestContext context)
    {
        BigDecimal.MaxSigFigs = 50;
    }
}
