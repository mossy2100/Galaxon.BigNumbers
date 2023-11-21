namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    /// <inheritdoc/>
    public static BigDecimal operator +(BigDecimal x)
    {
        return x;
    }

    /// <inheritdoc/>
    public static BigDecimal operator -(BigDecimal x)
    {
        return new BigDecimal(-x.Significand, x.Exponent);
    }

    /// <inheritdoc/>
    public static BigDecimal operator +(BigDecimal x, BigDecimal y)
    {
        // If the orders of magnitude of the two values are different enough, adding them will have
        // no effect after rounding off to the maximum number of significant figures. We want to
        // detect this situation to save time, mainly by avoiding the call to Align().
        var xMaxExp = x.Exponent + x.NumSigFigs - 1;
        var yMaxExp = y.Exponent + y.NumSigFigs - 1;
        if (xMaxExp - MaxSigFigs > yMaxExp)
        {
            return x;
        }
        if (yMaxExp - MaxSigFigs > xMaxExp)
        {
            return y;
        }

        // Align the values to the same exponent.
        var (x2Sig, y2Sig, exp) = Align(x, y);

        // Sum the significands.
        var sum = new BigDecimal(x2Sig + y2Sig, exp);

        return RoundSigFigs(sum);
    }

    /// <inheritdoc/>
    public static BigDecimal operator -(BigDecimal x, BigDecimal y)
    {
        return x + -y;
    }

    /// <inheritdoc/>
    public static BigDecimal operator ++(BigDecimal x)
    {
        return x + 1;
    }

    /// <inheritdoc/>
    public static BigDecimal operator --(BigDecimal x)
    {
        return x - 1;
    }

    /// <inheritdoc/>
    public static BigDecimal operator *(BigDecimal x, BigDecimal y)
    {
        var prod = new BigDecimal(x.Significand * y.Significand, x.Exponent + y.Exponent);
        return RoundSigFigs(prod);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Computes division using the Goldschmidt algorithm:
    /// <see href="https://en.wikipedia.org/wiki/Division_algorithm#Goldschmidt_division"/>
    /// </remarks>
    /// <exception cref="System.DivideByZeroException">If y == 0</exception>
    public static BigDecimal operator /(BigDecimal x, BigDecimal y)
    {
        // Guard.
        if (y == 0)
        {
            throw new DivideByZeroException("Division by 0 is undefined.");
        }

        // Shortcuts.
        if (y == 1)
        {
            return x;
        }
        if (x == y)
        {
            return 1;
        }

        // Add guard digits to ensure a correct result.
        var sf = AddGuardDigits(7);

        // Find f ~= 1/y as an initial estimate of the multiplication factor.
        // We can quickly get a very good initial estimate by leveraging the decimal type.
        // In other places we've used the double type for calculating estimates, both for speed and
        // to access methods that the decimal type doesn't provide. However, because division may be
        // needed when casting a double to BigDecimal, using double here causes infinite recursion.
        // Casting from decimal to BigDecimal doesn't require division so it doesn't have that
        // problem.
        var yRounded = RoundSigFigs(y, _DECIMAL_PRECISION);
        BigDecimal f = 1 / (decimal)(yRounded.Significand);
        f.Exponent -= yRounded.Exponent;

        while (true)
        {
            x *= f;
            y *= f;

            // If y is 1, then n is the result.
            if (y == 1)
            {
                break;
            }

            f = 2 - y;

            // If y is not 1, but is close to 1, then f can be 1 due to rounding after the
            // subtraction. If it is, there's no point continuing.
            if (f == 1)
            {
                break;
            }
        }

        // Restore maximum sig figs and round off.
        return RemoveGuardDigits(x, sf);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// There are various ways to implement the modulo operator:
    /// <see href="https://en.wikipedia.org/wiki/Modulo"/>
    /// This method uses truncated division, to match the behaviour of the operator as used with the
    /// standard number types in .NET.
    /// It means the result (the remainder) will have the same sign as the dividend (x).
    /// </remarks>
    /// <exception cref="DivideByZeroException">if the divisor is 0.</exception>
    public static BigDecimal operator %(BigDecimal x, BigDecimal y)
    {
        var mod = x - Truncate(x / y) * y;
        return RoundSigFigs(mod);
    }

    /// <summary>Exponentiation operator.</summary>
    /// <remarks>
    /// Overloads the ^ operator to perform exponentiation, consistent with common mathematical
    /// usage.
    /// While C-based languages traditionally use ^ for bitwise XOR, operator overloading in C#
    /// allows for a more intuitive use in the context of custom numerical types like BigDecimal,
    /// BigRational, and BigComplex.
    /// Many C-inspired languages use ** for the exponentiation operator, but this hasn't been done
    /// in C# (yet) and isn't possible with operator overloading, as only a small set of standard
    /// operator tokens can be overloaded.
    /// </remarks>
    /// <param name="x">The base.</param>
    /// <param name="y">The exponent.</param>
    /// <returns>The first operand raised to the power of the second.</returns>
    /// <exception cref="ArithmeticException">
    /// If the base is 0 and the exponent is negative.
    /// </exception>
    public static BigDecimal operator ^(BigDecimal x, BigDecimal y)
    {
        return Pow(x, y);
    }
}
