using System.Globalization;
using System.Numerics;

namespace Galaxon.BigNumbers.Tests;

[TestClass]
public class BigComplexStringsTests
{
    /// <summary>
    /// This is not actually a test method. I want to see how Complex numbers are formatted by .NET,
    /// so I can do the same thing with BigComplex.
    /// </summary>
    [TestMethod]
    public void TestComplexToString()
    {
        Complex z;

        z = new Complex(0, 0);
        Console.WriteLine(z.ToString());

        z = new Complex(12, 0);
        Console.WriteLine(z.ToString());

        z = new Complex(-12, 0);
        Console.WriteLine(z.ToString());

        z = new Complex(0, 34);
        Console.WriteLine(z.ToString());

        z = new Complex(0, -34);
        Console.WriteLine(z.ToString());

        z = new Complex(12, 34);
        Console.WriteLine(z.ToString());

        z = new Complex(-12, 34);
        Console.WriteLine(z.ToString());

        z = new Complex(12, -34);
        Console.WriteLine(z.ToString());

        z = new Complex(-12, -34);
        Console.WriteLine(z.ToString());

        z = new Complex(12.3456, 34.5678);
        Console.WriteLine(z.ToString());

        z = new Complex(12.3456, 34.5678);
        Console.WriteLine(z.ToString("F2"));

        z = new Complex(12.3456, 34.5678);
        Console.WriteLine(z.ToString("E2"));
    }

    /// <summary>
    /// This uses the example from the documentation.
    /// </summary>
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.numerics.complex.tostring?view=net-7.0#system-numerics-complex-tostring"/>
    [TestMethod]
    public void TestComplexToString2()
    {
        Complex[] c =
        {
            new (17.3, 14.1),
            new (-18.9, 147.2),
            new (13.472, -18.115),
            new (-11.154, -17.002)
        };
        var ci = new CultureInfo("en-US", false);
        foreach (var c1 in c)
        {
            Console.WriteLine(c1.ToString(ci));
        }
    }
}
