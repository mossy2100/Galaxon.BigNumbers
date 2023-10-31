using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    #region Parse methods

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigRational Parse(string s, NumberStyles style, IFormatProvider? provider) =>
        Parse(s, provider);

    /// <summary>
    /// Parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    ///
    /// Notes:
    /// - The numerator can have a sign (+ or -) but not the denominator.
    /// - The divide sign and denominator can either both be present or both be omitted.
    /// </summary>
    /// <exception cref="ArgumentFormatException"></exception>
    public static BigRational Parse(string s, IFormatProvider? provider)
    {
        // Optimization.
        if (string.IsNullOrWhiteSpace(s)) return 0;

        // Get a NumberFormatInfo object so we know what decimal point character to accept.
        var nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Remove ignored characters from the string.
        s = BigDecimal.RemoveIgnoredCharacters(s, nfi);

        // I may support the superscript/subscript version later (as generated by ToString()).
        // Just support ASCII for now.
        var match = Regex.Match(s, @"^(?<num>[+\-]?\d+)(/(?<den>\d+))?$");
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s),
                "Incorrect format. The correct format is int/int (e.g. 22/7 or -3/4), or just int (e.g. 567).");
        }

        // Extract parts:
        var sNum = match.Groups["num"].Value;
        var sDen = match.Groups["den"].Value;

        // Construct the BigRational.
        var num = sNum == "" ? 0 : BigInteger.Parse(sNum);
        var den = sDen == "" ? 1 : BigInteger.Parse(sDen);
        return new BigRational(num, den);
    }

    /// <summary>Simplest version of Parse().</summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The BigRational value represented by the string.</returns>
    public static BigRational Parse(string s) => Parse(s, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigRational Parse(ReadOnlySpan<char> s, NumberStyles style,
        IFormatProvider? provider) =>
        Parse(new string(s), provider);

    /// <inheritdoc/>
    public static BigRational Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
        Parse(new string(s), provider);

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigRational result) =>
        TryParse(s, provider, out result);

    /// <inheritdoc/>
    public static bool TryParse(string? s, IFormatProvider? provider, out BigRational result)
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
    /// <param name="result">The BigRational value represented by the string.</param>
    /// <returns>If the attempt to parse the value succeeded.</returns>
    public static bool TryParse(string? s, out BigRational result) =>
        TryParse(s, CultureInfo.InvariantCulture, out result);

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigRational result) =>
        TryParse(new string(s), provider, out result);

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        out BigRational result) =>
        TryParse(new string(s), provider, out result);

    #endregion Parse methods

    #region Format methods

    /// <inheritdoc/>
    /// <todo>
    /// Update to support standard format strings for integers, namely D, N, R, with the optional U
    /// code, same as for BigDecimal. Remove "A", keep "M" for mixed.
    /// </todo>
    public readonly string ToString(string? format, IFormatProvider? formatProvider)
    {
        // Default to the Unicode version.
        if (string.IsNullOrEmpty(format))
        {
            format = "U";
        }

        // If the denominator is 1 then just return the numerator as a string.
        if (Denominator == 1)
        {
            return Numerator.ToString();
        }

        // Get the parts we need.
        var sign = this < 0 ? "-" : "";
        var num = BigInteger.Abs(Numerator);
        var den = BigInteger.Abs(Denominator);

        switch (format.ToUpper())
        {
            // ASCII.
            case "A":
                return $"{sign}{num}/{den}";

            // Unicode.
            case "U":
                return $"{sign}{num.ToSuperscript()}/{den.ToSubscript()}";

            // Mixed.
            case "M":
                // Format improper fractions (or rationals that have a numerator with a larger
                // magnitude than the denominator) with a quotient and remainder, e.g 2³/₄
                if (num > den)
                {
                    var quot = num / den;
                    var rem = num % den;
                    return $"{sign}{quot}{rem.ToSuperscript()}/{den.ToSubscript()}";
                }
                return $"{sign}{num.ToSuperscript()}/{den.ToSubscript()}";

            default:
                throw new ArgumentFormatException(nameof(format),
                    "The provided format string is not supported.");
        }
    }

    /// <summary>
    /// Format the rational as a string.
    /// </summary>
    public readonly string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

    /// <summary>
    /// Format the rational as a string.
    /// The is the default override version, which uses Unicode characters for a nicer format.
    /// </summary>
    public readonly override string ToString() => ToString("U");

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
