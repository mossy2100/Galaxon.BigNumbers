using System.Diagnostics;
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
        Trace.WriteLine(z.ToString());

        z = new Complex(12, 0);
        Trace.WriteLine(z.ToString());

        z = new Complex(-12, 0);
        Trace.WriteLine(z.ToString());

        z = new Complex(0, 34);
        Trace.WriteLine(z.ToString());

        z = new Complex(0, -34);
        Trace.WriteLine(z.ToString());

        z = new Complex(12, 34);
        Trace.WriteLine(z.ToString());

        z = new Complex(-12, 34);
        Trace.WriteLine(z.ToString());

        z = new Complex(12, -34);
        Trace.WriteLine(z.ToString());

        z = new Complex(-12, -34);
        Trace.WriteLine(z.ToString());

        z = new Complex(12.3456, 34.5678);
        Trace.WriteLine(z.ToString());

        z = new Complex(12.3456, 34.5678);
        Trace.WriteLine(z.ToString("F2"));

        z = new Complex(12.3456, 34.5678);
        Trace.WriteLine(z.ToString("E2"));
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
            new Complex(17.3, 14.1),
            new Complex(-18.9, 147.2),
            new Complex(13.472, -18.115),
            new Complex(-11.154, -17.002)
        };
        CultureInfo ci = new CultureInfo("en-US", false);
        foreach (Complex c1 in c)
        {
            Console.WriteLine(c1.ToString(ci));
        }
    }
}