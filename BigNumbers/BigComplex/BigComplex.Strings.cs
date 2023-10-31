using System.Globalization;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Parse methods

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigComplex Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        Parse(s, provider);

    /// <inheritdoc/>
    public static BigComplex Parse(string s, IFormatProvider? provider) =>
        throw
            // Check how it's formatted:
            // 1. With the format used by System.Complex.ToString(), with angle brackets and semicolon, e.g. <x; y>
            // 2. Using the standard notation used in maths, e.g. a + bi, a + ib, etc.
            // 3. As an ordinary integer, decimal, or floating point real number, e.g. 12345 or 123.45 or 123.45e67 etc.
            new NotImplementedException();

    /// <summary>Simplest version of Parse().</summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The BigComplex value represented by the string.</returns>
    public static BigComplex Parse(string s) => Parse(s, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public static BigComplex Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
        Parse(new string(s), provider);

    /// <inheritdoc/>
    public static BigComplex Parse(ReadOnlySpan<char> s, NumberStyles style,
        IFormatProvider? provider) =>
        Parse(new string(s), provider);

    /// <inheritdoc/>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigComplex result) =>
        TryParse(s, provider, out result);

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out BigComplex result)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            result = 0;
            return false;
        }

        // Try to parse the provided string.
        try
        {
            result = Parse(s, provider);
            return true;
        }
        catch (Exception)
        {
            result = 0;
            return false;
        }
    }

    /// <summary>Simplest version of TryParse().</summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">The BigComplex value represented by the string.</param>
    /// <returns>If the attempt to parse the value succeeded.</returns>
    public static bool TryParse(string? s, out BigComplex result) =>
        TryParse(s, NumberFormatInfo.InvariantInfo, out result);

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        out BigComplex result) =>
        TryParse(new string(s), provider, out result);

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigComplex result) =>
        TryParse(new string(s), provider, out result);

    #endregion Parse methods

    #region Format methods

    /// <inheritdoc/>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
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

    /// <summary>
    /// Format the BigComplex as a string.
    /// </summary>
    public readonly string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

    /// <summary>
    /// Express the complex number as a string in the usual algebraic format.
    /// This differs from Complex.ToString(), which outputs strings like (x, y).
    /// </summary>
    /// <returns>The complex number as a string.</returns>
    public readonly override string ToString() => ToString("G");

    /// <inheritdoc/>
    public readonly bool TryFormat(Span<char> destination, out int charsWritten,
        ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var formattedValue = ToString(new string(format), provider);
        try
        {
            formattedValue.CopyTo(destination);
            charsWritten = formattedValue.Length;
            return true;
        }
        catch
        {
            charsWritten = 0;
            return false;
        }
    }

    #endregion Format methods
}
