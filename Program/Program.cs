using Galaxon.BigNumbers.Tests;

Console.WriteLine("BigNumbers Test Program.");

var result = PythonRunner.CallMathFunction("sin", 3.1416m);
Console.WriteLine(result);
