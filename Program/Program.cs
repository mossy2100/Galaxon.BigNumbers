using Galaxon.Numerics.Types;

Console.WriteLine("Hello, World.");

for (int i = -10; i <= 10; i++)
{
    double d = i / 10.0;
    double asinD = double.Asin(d);
    BigDecimal bd = i / (BigDecimal)10;
    BigDecimal asinBD = BigDecimal.Asin(bd);
    Console.WriteLine(d);
    Console.WriteLine(bd);
    Console.WriteLine(asinD);
    Console.WriteLine(asinBD);
    Console.WriteLine();
}
