using System.Globalization;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;

namespace Galaxon.BigNumbers;

public partial struct BigComplex
{
    #region Parse methods

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigComplex Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        Parse(s, provider);

    /// <inheritdoc/>
    /// <remarks>
    /// Supported formats:
    /// 1. Ordinary real number (integer or floating point), e.g. 12345, 123.45, 123.45e67 etc. The
    ///    'e' for exponent can be lower or upper-case, as with normal floating point syntax.
    /// 2. Format used by System.Complex.ToString(), with angle brackets and a semicolon. Square
    ///    brackets, curly braces, parentheses, or no brackets at all are also supported.
    /// 3. Standard notation used in maths is supported (e.g. a + bi), with either i or j. The i or
    ///    j can come before or after the imaginary number, and it can be lower or upper-case.
    ///
    /// Also note:
    /// - Leading, trailing, or any other whitespace is ignored.
    /// - Digit grouping characters (e.g. thousands separators) are allowed, including commas or
    ///   periods (culture-specific), underscores, and thin spaces.
    /// - The real part must come before the imaginary part. In the math notation, either the real
    ///   or imaginary part can be omitted.
    /// - The format (x, y) is NOT supported, because we need to allow the commas as a decimal
    ///   points (and they could also appear as digit grouping characters).
    /// - A null or empty string is taken to be 0.
    /// </remarks>
    public static BigComplex Parse(string s, IFormatProvider? provider)
    {
        // Optimization.
        if (string.IsNullOrWhiteSpace(s)) return 0;

        // Get a NumberFormatInfo object so we know what decimal point character to accept.
        var nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Remove ignored characters from the string.
        s = BigDecimal.RemoveIgnoredCharacters(s, nfi);

        // Different components of the patterns.
        var rxUnsignedReal = $@"\d+({nfi.NumberDecimalSeparator}\d+)?(e[+\-]?\d+)?";
        var rxSignedReal = $@"[+\-]?{rxUnsignedReal}";
        var rxUnsignedImag = $@"({rxUnsignedReal}[ij]|[ij]{rxUnsignedReal})";
        var rxSignedImag = $@"[+\-]?{rxUnsignedImag}";
        var rxLeftBracket = @"[<\(\[{]";
        var rxRightBracket = @"[>\)\]}]";

        // Supported patterns.
        var patterns = new[]
        {
            // Brackets and semicolon style, e.g. x;y, <x;y>, (x;y), [x;y], or {x;y}.
            $@"(?<left>{rxLeftBracket})?(?<real>{rxSignedReal});(?<imag>{rxSignedImag})(?<right>{rxRightBracket})?",
            // Real part only, e.g. 123.45, -6, 7.8e9, etc.
            $@"(?<real>{rxSignedReal})",
            // Imaginary part only, e.g. 12i, -j34, etc.
            $@"(?<imag>{rxSignedImag})",
            // Both real and imaginary part, e.g. 12+34i, 5.6-j7.8, etc.
            $@"(?<real>{rxSignedReal})[+\-](?<imag>{rxUnsignedImag})"
        };

        // Test each pattern.
        foreach (var pattern in patterns)
        {
            // Test the pattern.
            var match = Regex.Match(s, pattern, RegexOptions.IgnoreCase);
            if (!match.Success) continue;

            // Extract the components.
            var sReal = match.Groups["real"].Value;
            var sImag = match.Groups["imag"].Value;
            var sLeft = match.Groups["left"].Value;
            var sRight = match.Groups["right"].Value;

            // Check the brackets match.
            if ((sLeft == "" && sRight != "") || (sLeft == "<" && sRight != ">")
                || (sLeft == "(" && sRight != ")") || (sLeft == "[" && sRight != "]")
                || (sLeft == "{" && sRight != "}"))
            {
                throw new ArgumentFormatException(nameof(s),
                    "Invalid format for complex number (non-matching brackets).");
            }

            // Construct the BigComplex.
            var real = sReal == "" ? 0 : BigDecimal.Parse(sReal);
            var imag = sImag == "" ? 0 : BigDecimal.Parse(sImag);
            return new BigComplex(real, imag);
        }

        // The provided string does not match any supported pattern.
        throw new ArgumentFormatException(nameof(s), "Invalid format for complex number.");
    }

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
    /// <remarks>
    /// I've added support for an alternate format to the one used by Complex.ToString(), namely the
    /// conventional "a + bi" notation (and it's variations).
    /// Both 'i' and 'j' are supported for the imaginary unit, hopefully to keep both mathematicians
    /// and engineers happy.
    ///
    /// Upper-case 'I' means the 'i' is placed after the imaginary part, e.g. 12 + 34i
    /// Lower-case 'i' means the 'i' is placed before the imaginary part, e.g. 12 + i34
    /// Upper-case 'J' means the 'j' is placed after the imaginary part, e.g. 12 + 34j
    /// Lower-case 'j' means the 'j' is placed before the imaginary part, e.g. 12 + j34
    ///
    /// If there's no imaginary part, the real part will be formatted like a normal real value.
    /// If there's no real part, the imaginary part will be formatted like a normal real value with
    /// the i or j placed as a prefix or suffix according to the format specifier.
    /// If the value is negative, the sign will come before the number with i or j.
    /// e.g. -1.23i, -i1.23, -1.23j, -j1.23
    ///
    /// If you want to use this format, the code ('I', 'i', 'J', or 'j') comes *before* the normal
    /// format code, or it can be by itself. So, these would all be valid:
    /// - I
    /// - j
    /// - IG
    /// - IF0
    /// - iE3
    /// - jN
    /// etc.
    ///
    /// If you don't include this prefix, then the same format used by Complex will be used, with
    /// angle brackets, and a semicolon separating the real and imaginary parts. Each part will be
    /// formatted according to the format string (e.g. G, F2, E3, etc.).
    ///
    /// The default format code is "G".
    /// </remarks>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        // Default format.
        if (string.IsNullOrWhiteSpace(format))
        {
            format = "G";
        }

        // Check if math format is specified.
        if (format[0] == 'I' || format[0] == 'i' || format[0] == 'J' || format[0] == 'j')
        {
            // Strip the leading character from the format to get a standard one.
            format = format.Length == 1 ? "G" : format[1..];

            // Handle real numbers.
            if (Imaginary == 0) return Real.ToString(format);

            // Construct the "a + bi" string.
            var sReal = Real == 0 ? "" : Real.ToString(format);
            var sSign = Real == 0 ? (Imaginary < 0 ? "-" : "") : (Imaginary < 0 ? " - " : " + ");
            var sImaginary = BigDecimal.Abs(Imaginary).ToString(format);
            switch (format[0])
            {
                case 'I':
                    sImaginary = sImaginary + 'i';
                    break;

                case 'i':
                    sImaginary = 'i' + sImaginary;
                    break;

                case 'J':
                    sImaginary = sImaginary + 'j';
                    break;

                case 'j':
                    sImaginary = 'j' + sImaginary;
                    break;
            }
            return $"{sReal}{sSign}{sImaginary}";
        }

        // Use the same format as Complex, e.g. <1.23; 4.56>
        return $"<{Real.ToString(format)}; {Imaginary.ToString(format)}>";
    }

    /// <summary>
    /// Format the BigComplex as a string.
    /// </summary>
    public readonly string ToString(string format) =>
        ToString(format, CultureInfo.InvariantCulture);

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
