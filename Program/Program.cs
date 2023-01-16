using Galaxon.Core.Numbers;
using Galaxon.Numerics;

Console.WriteLine("Hello, World.");

BigDecimal.MaxSigFigs = 1000;
long maxlen = 0;
for (int i = 0; i < 1000; i++)
{
    double f = XDouble.GetRandom();
    BigDecimal bd = f;
    string s = bd.ToString("E")[..^5].TrimEnd('0');
    if (s.Length > maxlen)
    {
        maxlen = s.Length;
    }
    Console.WriteLine(s);
}
Console.WriteLine();
Console.WriteLine($"max len = {maxlen}");
