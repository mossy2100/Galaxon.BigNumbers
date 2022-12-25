using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Strings;

namespace Galaxon.Numerics.Types;

public partial struct BigDecimal
{
    /// <summary>
    /// Format the BigDecimal as a string.
    ///
    /// Supported formats are the usual: D, E, F, G, N, P, and R.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings" />
    ///
    /// Although "D" is normally only used by integral types, in this case both the significand and
    /// exponent will be formatted as integers.
    ///
    /// An secondary code "U" is provided.
    ///   - If omitted, the exponent (if present) will be formatted with the usual E[-+]999 format.
    ///   - If present, the exponent is formatted with "×10" instead of "E" and the exponent digits
    ///     will be rendered as superscript. Also, a "+" sign is not used for positive exponents.
    ///
    /// In either case, unlike with "E" and "G" when used with float, double, and decimal, exponent
    /// digits are not left-padded with 0s. I don't see this as necessary.
    ///
    /// Codes "R" and "D" will produce the same output. However, the Unicode flag is undefined with
    /// "R", because Parse() doesn't support superscript exponents.
    /// </summary>
    /// <param name="specifier">The format specifier (default "G").</param>
    /// <param name="provider">The format provider (default null).</param>
    /// <returns>The formatted string.</returns>
    /// <exception cref="ArgumentInvalidException">If the format specifier is invalid.</exception>
    public string ToString(string? specifier, IFormatProvider? provider = null)
    {
        // Set defaults.
        string format = "G";
        string ucFormat = format;
        int? precision = null;
        bool unicode = false;

        // Parse the format specifier.
        if (!string.IsNullOrEmpty(specifier))
        {
            Match match = Regex.Match(specifier,
                @"^(?<format>[DEFGNPR])(?<precision>\d*)(?<unicode>U?)$", RegexOptions.IgnoreCase);

            // Check format is valid.
            if (!match.Success)
            {
                throw new ArgumentInvalidException(nameof(specifier),
                    $"Invalid format specifier \"{specifier}\".");
            }

            // Extract parts.
            format = match.Groups["format"].Value;
            ucFormat = format.ToUpper();
            string strPrecision = match.Groups["precision"].Value;
            precision = strPrecision == "" ? null : int.Parse(strPrecision);
            unicode = match.Groups["unicode"].Value.ToUpper() == "U";

            // Check if Unicode flag is present when it shouldn't be.
            if (unicode && ucFormat is "F" or "N" or "P" or "R")
            {
                throw new ArgumentInvalidException(nameof(specifier),
                    $"The Unicode flag is invalid with format \"{format}\".");
            }
        }

        // Format the significand.
        int exp = Exponent;
        switch (ucFormat)
        {
            case "D" or "R":
                string strSig = Significand.ToString("D", provider);
                if (exp == 0)
                {
                    return strSig;
                }
                return strSig + FormatExponent(format, exp, unicode, 1, provider);

            case "E":
                return FormatScientific(format, precision, unicode, 3, provider);

            case "F" or "N":
                return FormatFixed(format, precision, provider);

            case "G":
            {
                // Get the format using E with 2-digit exponent.
                string strFormatE = FormatScientific(format, precision, unicode, 2, provider);

                // Get the fixed point format, specifying the maximum number of significant figures.
                string strFormatF = FormatFixedSigFigs(precision, provider);

                // Return the shorter, preferring F.
                return strFormatF.Length <= strFormatE.Length ? strFormatF : strFormatE;
            }

            case "P":
                return (this * 100).FormatFixed("F", precision, provider) + "%";

            default:
                return "";
        }
    }

    /// <inheritdoc />
    /// <remarks>
    /// Override of ValueType.ToString(). Needed for debugging and string interpolation.
    /// </remarks>
    /// <see cref="ValueType.ToString" />
    public override string ToString() =>
        ToString("G");

    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
    {
        string formattedValue = ToString(new string(format), provider);
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

    /// <inheritdoc />
    public static BigDecimal Parse(string str, IFormatProvider? provider)
    {
        // Get a NumberFormatInfo object so we know what characters to look for.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Remove any whitespace, underscore, or group separator characters from the string.
        str = Regex.Replace(str, $@"[\s_{nfi.NumberGroupSeparator}]", "");

        // Check the string format and extract salient info.
        string strSign = $"[{nfi.NegativeSign}{nfi.PositiveSign}]?";
        Match match = Regex.Match(str, $@"^(?<integer>{strSign}\d+)"
            + $@"({nfi.NumberDecimalSeparator}(?<fraction>\d+))?(e(?<exponent>{strSign}\d+))?$",
            RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(str), "Invalid format.");
        }

        // Get the parts.
        string strInt = match.Groups["integer"].Value;
        string strFrac = match.Groups["fraction"].Value;
        string strExp = match.Groups["exponent"].Value;

        // Construct the result object.
        BigInteger sig = BigInteger.Parse(strInt + strFrac, provider);
        int exp = strExp == "" ? 0 : int.Parse(strExp, provider);
        exp -= strFrac.Length;
        return new BigDecimal(sig, exp);
    }

    /// <summary>
    /// More convenient version of Parse().
    /// </summary>
    public static BigDecimal Parse(string str) =>
        Parse(str, NumberFormatInfo.InvariantInfo);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(string str, NumberStyles style, IFormatProvider? provider) =>
        Parse(str, provider);

    /// <inheritdoc />
    public static BigDecimal Parse(ReadOnlySpan<char> span, IFormatProvider? provider) =>
        Parse(new string(span), provider);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(ReadOnlySpan<char> span, NumberStyles style,
        IFormatProvider? provider) =>
        Parse(new string(span), provider);

    /// <inheritdoc />
    public static bool TryParse(string? str, IFormatProvider? provider, out BigDecimal result)
    {
        if (str == null)
        {
            result = Zero;
            return false;
        }

        try
        {
            result = Parse(str, provider);
            return true;
        }
        catch (Exception)
        {
            result = Zero;
            return false;
        }
    }

    /// <summary>
    /// More convenient version of TryParse().
    /// </summary>
    public static bool TryParse(string? str, out BigDecimal result) =>
        TryParse(str, NumberFormatInfo.InvariantInfo, out result);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(string? str, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(str, provider, out result);

    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<char> span, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(new string(span), provider, out result);

    /// <inheritdoc />
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(ReadOnlySpan<char> span, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(new string(span), provider, out result);

    /// <summary>
    /// From a BigDecimal, extract two strings of digits that would appear if the number was written
    /// in fixed-point format (i.e. without an exponent).
    /// Sign is ignored.
    /// </summary>
    private (string strInt, string strFrac) PreformatFixed()
    {
        string strAbsSig = BigInteger.Abs(Significand).ToString();

        if (Exponent == 0)
        {
            return (strAbsSig, "");
        }

        if (Exponent > 0)
        {
            return (strAbsSig.PadRight(strAbsSig.Length + Exponent, '0'), "");
        }

        if (-Exponent == strAbsSig.Length)
        {
            return ("0", strAbsSig);
        }

        if (-Exponent > strAbsSig.Length)
        {
            return ("0", strAbsSig.PadLeft(-Exponent, '0'));
        }

        return (strAbsSig[..^-Exponent], strAbsSig[^-Exponent..]);
    }

    /// <summary>
    /// From a BigDecimal, extract the significand and the exponent needed to write the number using
    /// scientific notation.
    /// Sign is ignored.
    /// </summary>
    private (string strSig, int exp) PreformatScientific()
    {
        string strAbsSig = BigInteger.Abs(Significand).ToString();
        if (strAbsSig.Length == 1)
        {
            return (strAbsSig, Exponent);
        }
        string strSig = strAbsSig[..1] + '.' + strAbsSig[1..];
        int exp = Exponent + strAbsSig.Length - 1;
        return (strSig, exp);
    }

    private string FormatFixed(string format, int? precision, IFormatProvider? provider = null)
    {
        // Get a NumberFormatInfo we can use for special characters.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Get the parts of the string.
        BigDecimal bd = precision.HasValue ? Round(this, precision.Value) : this;
        string strSign = bd.Significand < 0 ? nfi.NegativeSign : "";
        (string strInt, string strFrac) = bd.PreformatFixed();

        // Add group separators to the integer part if necessary.
        if (format == "N")
        {
            strInt = BigInteger.Parse(strInt).ToString("N", provider);
        }

        // Format the fractional part.
        if (strFrac != "" || precision is > 0)
        {
            // Add trailing 0s if the precision was specified and they're needed.
            if (precision.HasValue && precision.Value > strFrac.Length)
            {
                strFrac = strFrac.PadRight(precision.Value, '0');
            }
            strFrac = nfi.NumberDecimalSeparator + strFrac;
        }

        return strSign + strInt + strFrac;
    }

    /// <summary>
    /// Format as fixed point, except in this case the precision is the number of significant
    /// figures, not the number of decimal places.
    /// Note, this is not technically formatting as significant figures, since trailing 0s following
    /// the decimal point are not retained, as per the usual format for "G".
    /// </summary>
    private string FormatFixedSigFigs(int? nSigFigs, IFormatProvider? provider = null)
    {
        // Get a NumberFormatInfo we can use for special characters.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Get the significant figures.
        string strAbsSig = BigInteger.Abs(Significand).ToString();
        int sigLen = strAbsSig.Length;

        // Get the sign.
        string strSign = Significand < 0 ? nfi.NegativeSign : "";

        // If we don't have to remove any digits, use default fixed-point format.
        if (nSigFigs is null or 0 || sigLen <= nSigFigs.Value)
        {
            return FormatFixed("F", null, provider);
        }

        // Shift the decimal point so we have the requested number of significant digits
        // before it. Any digits to the right will be discarded, but with rounding.
        string strInt = strAbsSig[..nSigFigs.Value];
        string strFrac = strAbsSig[nSigFigs.Value..];

        // Get the new value and round it off.
        BigDecimal rounded = Round(Parse(strSign + strInt + "." + strFrac));

        // Shifting the decimal point left a certain number of places means incrementing
        // the exponent that many times.
        rounded.Exponent = Exponent + sigLen - nSigFigs.Value;
        return rounded.FormatFixed("F", null, provider);
    }

    /// <summary>
    /// Format the value using scientific notation.
    /// </summary>
    private string FormatScientific(string format, int? precision, bool unicode, int expWidth,
        IFormatProvider? provider = null)
    {
        // Get the significand as a fixed point number with one digit preceding the decimal point,
        // and the adjusted exponent.
        (string strSig, int exp) = PreformatScientific();

        // Round it off if necessary.
        if (precision is > 0)
        {
            strSig = Parse(strSig).FormatFixed("F", precision - 1, provider);
        }

        // Format the exponent.
        string strExp = FormatExponent(format, exp, unicode, expWidth, provider);

        return strSig + strExp;
    }

    private static string FormatExponent(string format, int exp, bool unicode, int expWidth,
        IFormatProvider? provider = null)
    {
        // Get a NumberFormatInfo we can use for special characters.
        NumberFormatInfo nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Get the exponent digits as a string, which may include a negative sign.
        string strExp = exp.ToString("D", provider);

        // Use Unicode format if requested.
        if (unicode)
        {
            // Prepend the 10x part and superscript the exponent.
            return "×10" + strExp.ToSuperscript();
        }

        // Standard format.
        // Add a plus sign if necessary.
        if (exp >= 0)
        {
            strExp = nfi.PositiveSign + strExp;
        }
        // Pad digits if requested.
        if (expWidth > 1)
        {
            strExp = strExp.PadLeft(expWidth + 1, '0');
        }
        // Prepend 'E' or 'e' depending on whether the requested format is upper or lower case.
        return (char.IsLower(format[0]) ? 'e' : 'E') + strExp;
    }
}
