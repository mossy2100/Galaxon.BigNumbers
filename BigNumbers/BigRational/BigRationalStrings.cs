using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers;

public partial struct BigRational
{
    /// <inheritdoc/>
    public static BigRational Parse(string s, NumberStyles style, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentFormatException"></exception>
    public static BigRational Parse(string s, IFormatProvider? provider = null)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Just support ASCII for now.
        // I may support the superscript/subscript version later (as generated by ToString()), but
        // it's probably unnecessary.
        var match = Regex.Match(s, @"^(?<num>-?\d+)/(?<den>-?\d+)$");
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(s),
                "Incorrect format. The correct format is int/int, e.g. 22/7 or -3/4.");
        }

        var num = BigInteger.Parse(match.Groups["num"].Value);
        var den = BigInteger.Parse(match.Groups["den"].Value);
        return new BigRational(num, den);
    }

    /// <inheritdoc/>
    public static BigRational Parse(ReadOnlySpan<char> s, NumberStyles style,
        IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static BigRational Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to parse a string into a rational.
    /// This version of the method is required to implement IParsable[BigRational], but it's more
    /// likely people will call the version that doesn't have the provider parameter.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, IFormatProvider? provider, out BigRational result)
    {
        // Check a value was provided.
        if (string.IsNullOrWhiteSpace(s))
        {
            throw new ArgumentNullException(nameof(s), "Cannot parse a null or empty string.");
        }

        // Try to parse the provided string.
        try
        {
            result = Parse(s, provider);
        }
        catch (Exception)
        {
            result = default(BigRational);
            return false;
        }

        // All good.
        return true;
    }

    /// <summary>
    /// Try to parse a string into a rational.
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public static bool TryParse(string? s, out BigRational result)
    {
        return TryParse(s, null, out result);
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider,
        out BigRational result)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Format the rational as a string.
    /// </summary>
    /// <todo>
    /// Update to support standard format strings for integers, namely D, N, R, with the optional U
    /// code, same as for BigDecimal. Remove "A", keep "M" for mixed.
    /// </todo>
    /// <param name="format"></param>
    /// <param name="formatProvider"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentFormatException"></exception>
    public string ToString(string? format, IFormatProvider? formatProvider)
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
    public string ToString(string format)
    {
        return ToString(format, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Format the rational as a string.
    /// The is the default override version, which uses Unicode characters for a nicer format.
    /// </summary>
    public override string ToString()
    {
        return ToString("U", CultureInfo.CurrentCulture);
    }

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }
}
