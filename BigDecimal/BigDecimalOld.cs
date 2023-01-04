using System.Numerics;
using Galaxon.Core.Numbers;

namespace Galaxon.Numerics.Types;

public partial struct BigDecimal
{
    private static BigDecimal ConvertFromFloatingPoint<T>(T n) where T : IFloatingPoint<T>
    {
        if (T.IsInfinity(n) || T.IsNaN(n))
        {
            throw new InvalidCastException("Cannot convert ±∞ or NaN to BigDecimal.");
        }

        // Get the value's parts.
        (byte signBit, ushort expBits, ulong fracBits) = n.Disassemble();

        // Check for 0.
        if (expBits == 0 && fracBits == 0)
        {
            return 0;
        }

        // Get the value's structure.
        (byte nExpBits, byte nFracBits, ushort expOffset) = n.GetStructure();

        // Check if the number is normal or subnormal.
        bool isSubnormal = expBits == 0;

        // Get sign.
        int sign = signBit == 1 ? -1 : 1;

        // Get the significand.
        // The bit values are taken to have the value 1..2^(nFracBits - 1) and the exponent is
        // correspondingly shifted. Doing this avoids division operations.
        BigDecimal sig = 0;
        BigDecimal pow = 1;
        for (int i = 0; i < nFracBits; i++)
        {
            bool set = (fracBits & (1ul << i)) != 0;
            if (set)
            {
                sig += pow;
            }
            pow *= 2;
        }

        // One more addition for normal numbers.
        if (!isSubnormal)
        {
            sig += pow;
        }

        // Get the exponent.
        int exp = (isSubnormal ? 1 : expBits) - expOffset - nFracBits;

        // Calculate the result.
        return sign * sig * Exp2(exp);
    }
}
