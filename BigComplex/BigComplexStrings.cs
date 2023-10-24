using System.Globalization;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    /// <inheritdoc />
    public static BigComplex Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static BigComplex Parse(ReadOnlySpan<char> s, NumberStyles style,
        IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(string? s, IFormatProvider? provider, out BigComplex result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        out BigComplex result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigComplex result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigComplex result)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Express the complex number as a string in the usual algebraic format.
    /// This differs from Complex.ToString(), which outputs strings like (x, y).
    /// </summary>
    /// <returns>The complex number as a string.</returns>
    public readonly override string ToString()
    {
        var realPart = Real == 0 && Imaginary != 0 ? "" : $"{Real}";

        var sign = "";
        if (Real == 0)
        {
            if (Imaginary < 0)
            {
                sign = "-";
            }
        }
        else
        {
            if (Imaginary < 0)
            {
                sign = " - ";
            }
            else if (Imaginary > 0)
            {
                sign = " + ";
            }
        }

        string imagPart;
        var absImag = BigDecimal.Abs(Imaginary);
        if (absImag == 0)
        {
            imagPart = "";
        }
        else if (absImag == 1)
        {
            imagPart = "i";
        }
        else
        {
            imagPart = $"{absImag}i";
        }

        return $"{realPart}{sign}{imagPart}";
    }

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }
}
