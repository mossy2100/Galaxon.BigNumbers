using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Galaxon.Core.Exceptions;
using Galaxon.Core.Strings;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    /// <summary>
    /// Format the BigDecimal as a string.
    /// Supported formats are the usual: D, E, F, G, N, P, and R.
    /// <see
    ///     href="https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings"/>
    /// Although "D" is normally only used by integral types, in this case both the significand and
    /// exponent will be formatted as integers.
    /// An secondary code "U" is provided, which follows the precision (if given).
    /// - If omitted, the exponent (if present) will be formatted with the usual E[-+]999 format.
    /// - If present, the exponent is formatted with "×10" instead of "E" and the exponent digits
    /// will be rendered as superscript. Also, a "+" sign is not used for positive exponents,
    /// and the exponent digits are not zero-padded.
    /// Example: "E7U" will format as per usual (E with 7 decimal digits), except using Unicode
    /// characters for the exponent part.
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
        var format = "G";
        var ucFormat = format;
        int? precision = null;
        var unicode = false;

        // Parse the format specifier.
        if (!string.IsNullOrEmpty(specifier))
        {
            var match = FormatRegex().Match(specifier);

            // Check format is valid.
            if (!match.Success)
            {
                throw new ArgumentOutOfRangeException(nameof(specifier),
                    $"Invalid format specifier \"{specifier}\".");
            }

            // Extract parts.
            format = match.Groups["format"].Value;
            ucFormat = format.ToUpper();
            var strPrecision = match.Groups["precision"].Value;
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
        var exp = Exponent;
        switch (ucFormat)
        {
            case "D" or "R":
                var strSig = Significand.ToString($"D{precision}", provider);
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
                // Default precision is unlimited, same as for BigInteger.
                // See: https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#GFormatString

                // Get the format using E with 2-digit exponent.
                var strFormatE = FormatScientific(format, precision - 1, unicode, 2, provider);

                // Get the fixed point format, specifying the precision as the maximum number of
                // significant figures.
                var strFormatF = FormatFixedSigFigs(precision, provider);

                // Return the shorter, preferring F.
                return strFormatF.Length <= strFormatE.Length ? strFormatF : strFormatE;
            }

            case "P":
                var nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;
                precision ??= nfi.PercentDecimalDigits;
                return (this * 100).FormatFixed("F", precision, provider) + nfi.PercentSymbol;

            default:
                return "";
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Override of ValueType.ToString(). Needed for debugging and string interpolation.
    /// </remarks>
    /// <see cref="ValueType.ToString"/>
    public override string ToString() => ToString("G");

    /// <inheritdoc/>
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
        IFormatProvider? provider)
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

    /// <inheritdoc/>
    public static BigDecimal Parse(string strBigDecimal, IFormatProvider? provider)
    {
        // Get a NumberFormatInfo object so we know what characters to look for.
        var nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Remove whitespace and group separator characters from the string. This includes:
        //   - commas or periods (depending on locale)
        //   - underscores
        //   - thin spaces
        strBigDecimal = Regex.Replace(strBigDecimal, $@"[\s{nfi.NumberGroupSeparator}_\u2009]", "");

        // Check the string format and extract salient info.
        var strRxSign = $"[{nfi.NegativeSign}{nfi.PositiveSign}]?";
        var strRxInt = $@"(?<int>{strRxSign}\d+)";
        var strRxFrac = $@"(\{nfi.NumberDecimalSeparator}(?<frac>\d+))?";
        var strRxExp = $@"(e(?<exp>{strRxSign}\d+))?";
        var strRx = $"^{strRxInt}{strRxFrac}{strRxExp}$";
        var match = Regex.Match(strBigDecimal, strRx, RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            throw new ArgumentFormatException(nameof(strBigDecimal), "Invalid BigDecimal format.");
        }

        // Get the digits.
        var strInt = match.Groups["int"].Value;
        var strFrac = match.Groups["frac"].Value;
        var strExp = match.Groups["exp"].Value;

        // Construct the result.
        var sig = BigInteger.Parse(strInt + strFrac, provider);
        var exp = strExp == "" ? 0 : int.Parse(strExp, provider);
        exp -= strFrac.Length;
        return new BigDecimal(sig, exp);
    }

    /// <summary>
    /// More convenient version of Parse().
    /// </summary>
    public static BigDecimal Parse(string str) => Parse(str, NumberFormatInfo.InvariantInfo);

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(string str, NumberStyles style, IFormatProvider? provider) =>
        Parse(str, provider);

    /// <inheritdoc/>
    public static BigDecimal Parse(ReadOnlySpan<char> span, IFormatProvider? provider) =>
        Parse(new string(span), provider);

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static BigDecimal Parse(ReadOnlySpan<char> span, NumberStyles style,
        IFormatProvider? provider) =>
        Parse(new string(span), provider);

    /// <inheritdoc/>
    public static bool TryParse(string? str, IFormatProvider? provider, out BigDecimal result)
    {
        if (str == null)
        {
            result = 0;
            return false;
        }

        try
        {
            result = Parse(str, provider);
            return true;
        }
        catch (Exception)
        {
            result = 0;
            return false;
        }
    }

    /// <summary>
    /// More convenient version of TryParse().
    /// </summary>
    public static bool TryParse(string? str, out BigDecimal result) =>
        TryParse(str, NumberFormatInfo.InvariantInfo, out result);

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(string? str, NumberStyles style, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(str, provider, out result);

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> span, IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(new string(span), provider, out result);

    /// <inheritdoc/>
    /// <remarks>Ignoring style parameter for now.</remarks>
    public static bool TryParse(ReadOnlySpan<char> span, NumberStyles style,
        IFormatProvider? provider,
        out BigDecimal result) =>
        TryParse(new string(span), provider, out result);

    /// <summary>
    /// From a BigDecimal, extract two strings of digits that would appear if the number was written
    /// in fixed-point format (i.e. without an exponent).
    /// Sign is ignored.
    /// </summary>
    private (string strInt, string strFrac) PreformatFixed()
    {
        var strAbsSig = BigInteger.Abs(Significand).ToString();

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

    private string FormatFixed(string format, int? precision, IFormatProvider? provider = null)
    {
        // Get a NumberFormatInfo we can use for special characters.
        var nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Get the parts of the string.
        var bd = precision.HasValue ? Round(this, precision.Value) : this;
        var strSign = bd.Significand < 0 ? nfi.NegativeSign : "";
        var (strInt, strFrac) = bd.PreformatFixed();

        // Add group separators to the integer part if necessary.
        if (format == "N")
        {
            strInt = BigInteger.Parse(strInt).ToString("N0", provider);
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

        // If zero, omit sign. We don't want to render -0.0000...
        var strAbs = strInt + strFrac;
        // if (decimal.Parse(strAbs) == 0m)
        // {
        //     return strAbs;
        // }

        return strSign + strAbs;
    }

    /// <summary>
    /// Format as fixed point, except in this case the precision is the number of significant
    /// figures, not the number of decimal places.
    /// Note, this is not technically formatting as significant figures, since trailing 0s following
    /// the decimal point are not retained, as per the usual format for "G".
    /// </summary>
    private string FormatFixedSigFigs(int? nSigFigs, IFormatProvider? provider = null)
    {
        // If we don't have to remove any digits, use default fixed-point format.
        var nDigitsToCut = nSigFigs is null or 0 ? 0 : NumSigFigs - nSigFigs.Value;
        if (nDigitsToCut <= 0)
        {
            return FormatFixed("F", null, provider);
        }

        // Round the value.
        var rounded = Round(new BigDecimal(Significand, -nDigitsToCut));
        rounded.Exponent += Exponent + nDigitsToCut;

        // Format as fixed-point without trailing zeros.
        return rounded.FormatFixed("F", null, provider);
    }

    /// <summary>
    /// Format the value using scientific notation.
    /// </summary>
    private string FormatScientific(string format, int? precision, bool unicode, int expWidth,
        IFormatProvider? provider = null)
    {
        // Format the significand.
        var nDecimalPlacesToShift = NumSigFigs - 1;
        BigDecimal sig = new (Significand, -nDecimalPlacesToShift);
        var strSig = sig.FormatFixed("F", precision, provider);

        // Format the exponent.
        var exp = Exponent + nDecimalPlacesToShift;
        var strExp = FormatExponent(format, exp, unicode, expWidth, provider);

        return strSig + strExp;
    }

    /// <summary>
    /// Format the exponent part of scientific notation.
    /// </summary>
    /// <param name="format">
    /// The original format code (e.g. E, e, G, or g). We need to know this to determine whether to
    /// use an upper- or lower-case 'E'.
    /// </param>
    /// <param name="exp">The exponent value.</param>
    /// <param name="unicode">Whether to use Unicode or standard format.</param>
    /// <param name="expWidth">
    /// The minimum number of digits in the exponent (typically 3 for E and 2 for G).
    /// Relevant for standard (non-Unicode) format only.
    /// </param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The formatted exponent.</returns>
    private static string FormatExponent(string format, int exp, bool unicode, int expWidth,
        IFormatProvider? provider = null)
    {
        // Get a NumberFormatInfo we can use for special characters.
        var nfi = provider as NumberFormatInfo ?? NumberFormatInfo.InvariantInfo;

        // Use Unicode format if requested.
        if (unicode)
        {
            // Prepend the x10 part and superscript the exponent.
            return "×10" + exp.ToString("D", provider).ToSuperscript();
        }

        // Standard format.
        return (char.IsLower(format[0]) ? 'e' : 'E')
            + (exp < 0 ? nfi.NegativeSign : nfi.PositiveSign)
            + int.Abs(exp).ToString("D", provider).ZeroPad(expWidth);
    }

    [GeneratedRegex("^(?<format>[DEFGNPR])(?<precision>\\d*)(?<unicode>U?)$",
        RegexOptions.IgnoreCase, "en-AU")]
    private static partial Regex FormatRegex();

    #region Helper methods

    /// <summary>
    /// Generate a string of '0' characters using doubling.
    /// </summary>
    /// <remarks>
    /// Rather than using a simple loop, the methods saves time by using doubling.
    /// Thus, a string of ~1000 characters requires fewer than 10 iterations instead of ~1000.
    /// </remarks>
    /// <param name="n">The number of '0' characters in the string.</param>
    /// <returns>The string of '0' characters.</returns>
    private static string StringOfZeros(BigInteger n)
    {
        // Terminating conditions and quick answers.
        // This will handle most (if not all) practical uses of the method.
        if (n == 0) return "";
        if (n == 1) return "0";
        if (n == 2) return "00";
        if (n == 3) return "000";
        if (n == 4) return "0000";
        if (n == 5) return "00000";
        if (n == 6) return "000000";
        if (n == 7) return "0000000";
        if (n == 8) return "00000000";
        if (n == 9) return "000000000";

        // See if n is even.
        string str2;
        if (BigInteger.IsEvenInteger(n))
        {
            str2 = StringOfZeros(n / 2);
            return str2 + str2;
        }

        // n is odd.
        str2 = StringOfZeros((n - 1) / 2);
        return '0' + str2 + str2;
    }

    /// <summary>
    /// Pad a string on the left with '0' characters up to a minimum width.
    /// </summary>
    /// <remarks>
    /// I created this method rather than using String.PadRight() because of the need to support
    /// BigInteger string widths, which comes from exponents being BigIntegers. I realise it's
    /// ridiculous to imagine a string of more than int.MaxValue zeros, but it seemed like a better
    /// solution to implement this method than throw an exception if the string width is too big.
    /// </remarks>
    /// <param name="str">The string.</param>
    /// <param name="width">The minimum number of characters in the the result.</param>
    /// <returns>The zero-padded string.</returns>
    public static string ZeroPadLeft(string str, BigInteger width)
    {
        var nZerosNeeded = width - str.Length;
        if (nZerosNeeded <= 0) return str;

        return StringOfZeros(nZerosNeeded) + str;
    }

    #endregion Helper methods
}
