// Find out how many significant figures we should compare to check for equality between a
// BigDecimal and other floating point types.
//
// using Galaxon.BigNumbers;
// using Galaxon.Core.Numbers;
//
// int n = 10;
// var rnd = new Random();
// decimal mHalfMin = (decimal)Half.MinValue;
// decimal mHalfMax = (decimal)Half.MaxValue;
// var minEqualSigFigs = 100000000;
// BigDecimal maxDiff = 0;
//
// var t1 = DateTime.Now;
//
// while (true)
// {
//     // Get a decimal value in the valid range for Half.
//     decimal m;
//     while (true)
//     {
//         m = rnd.NextDecimal();
//         if (m >= mHalfMin && m <= mHalfMax) break;
//     }
//
//     // Convert the value to a Half and a BigDecimal.
//     var h = (Half)m;
//     var bd = (BigDecimal)m;
//
//     Console.WriteLine($"decimal: {m}");
//     Console.WriteLine($"Half: {h}");
//     Console.WriteLine($"BigDecimal: {bd}");
//
//     var j = 5;
//     Console.WriteLine();
//     BigDecimal bd1 = BigDecimal.RoundSigFigs(bd, j);
//     BigDecimal bd2 = BigDecimal.RoundSigFigs(h, j);
//     var (bd3, bd4) = BigDecimal.Align(bd1, bd2);
//     Console.WriteLine($"BigDecimal rounded to {j} sig figs: {bd3}");
//     Console.WriteLine($"Half rounded to {j} sig figs: {bd4}");
//     if (bd1 == bd2)
//     {
//         Console.WriteLine($"Equal at {j} sig figs.");
//         // if (j < minEqualSigFigs)
//         // {
//         //     minEqualSigFigs = j;
//         // }
//         // break;
//     }
//     else
//     {
//         var diff = BigDecimal.Abs(bd3.Significand - bd4.Significand);
//         Console.WriteLine($"Unequal. Diff in significands = {diff}");
//         if (diff > maxDiff)
//         {
//             maxDiff = diff;
//         }
//     }
//
//     Console.WriteLine("--------------------------------------------------------------------------");
//
//     var t2 = DateTime.Now;
//     var dt = t2 - t1;
//     if (dt.TotalSeconds >= 10) break;
// }
//
// // Console.WriteLine($"Lowest number of sig figs required = {minEqualSigFigs}");
// Console.WriteLine($"Max difference in significands = {maxDiff}");

// using Galaxon.BigNumbers;
// using Galaxon.Core.Numbers;
//
// var n = 1;
// var rnd = new Random();
// decimal mHalfMin = (decimal)Half.MinValue;
// decimal mHalfMax = (decimal)Half.MaxValue;
//
// for (var i = 0; i < n; i++)
// {
//     // Get a decimal value in the valid range for Half.
//     decimal m;
//     Half h;
//     while (true)
//     {
//         m = rnd.NextDecimal();
//         if (m >= mHalfMin && m <= mHalfMax)
//         {
//             h = (Half)m;
//             // if (Half.IsSubnormal(h))
//             break;
//         }
//     }
//
//     // Convert the value to a Half and a BigDecimal.
//     var bd = (BigDecimal)m;
//
//     Console.WriteLine($"Value as decimal: {m}");
//     Console.WriteLine($"Value as Half: {h}");
//     Console.WriteLine($"Value as BigDecimal: {bd}");
//
//     // Calculate the value of the least significant bit in the Half.
//     BigDecimal lsb;
//
//     if (Half.IsSubnormal(h))
//     {
//         Console.WriteLine("Value is subnormal");
//         lsb = Half.Epsilon;
//         Console.WriteLine($"Value of LSB is {lsb}");
//     }
//     else
//     {
//         var expBias = XFloatingPoint.GetExpBias<Half>();
//         var nExpBits = XFloatingPoint.GetNumExpBits<Half>();
//         var nFracBits = XFloatingPoint.GetNumFracBits<Half>();
//         var (signBit, expBits, fracBits) = h.Disassemble<Half>();
//         Console.WriteLine($"Disassembled Half: {signBit} {expBits.ToBin(nExpBits)} {fracBits.ToBin(nFracBits)}");
//         Console.WriteLine("Value is normal");
//         lsb = BigDecimal.Exp2(expBits - expBias - nFracBits);
//         Console.WriteLine($"Value of LSB is {lsb}");
//     }
//
//     var delta = lsb / 2;
//     Console.WriteLine($"Delta is {delta}");
//
//     // Get the difference;
//     var bd2 = (BigDecimal)h;
//     var diff = BigDecimal.Abs(bd - bd2);
//     Console.WriteLine($"Diff is {diff}");
//
//     if (diff < delta)
//     {
//         Console.WriteLine($"Values are fuzzy equal.");
//     }
//     else
//     {
//         Console.WriteLine($"Values are not fuzzy equal.");
//     }
// }


