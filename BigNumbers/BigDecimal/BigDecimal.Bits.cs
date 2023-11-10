using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    /// <summary>Number of bits per byte.</summary>
    private const byte _BITS_PER_BYTE = 8;

    /// <inheritdoc/>
    public int GetSignificandByteCount()
    {
        return Significand.GetByteCount();
    }

    /// <inheritdoc/>
    public int GetSignificandBitLength()
    {
        return GetSignificandByteCount() * _BITS_PER_BYTE;
    }

    /// <inheritdoc/>
    public int GetExponentByteCount()
    {
        return sizeof(int);
    }

    /// <inheritdoc/>
    public int GetExponentShortestBitLength()
    {
        var exponent = Exponent;
        const int BITS_PER_INT = sizeof(int) * 8;

        // Non-negative exponent.
        if (exponent >= 0)
        {
            return BITS_PER_INT - int.LeadingZeroCount(exponent);
        }

        // Negative exponent.
        return BITS_PER_INT + 1 - int.LeadingZeroCount(~exponent);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandLittleEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteBigInteger(Significand, destination, out bytesWritten, false);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten)
    {
        return TryWriteBigInteger(Significand, destination, out bytesWritten, true);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten)
    {
        return TryWriteInt(Exponent, destination, out bytesWritten, false);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
    {
        return TryWriteInt(Exponent, destination, out bytesWritten, true);
    }

    /// <summary>Copy some bytes from one span to another.</summary>
    /// <param name="bytes">The bytes to copy.</param>
    /// <param name="destination">The span to copy the bytes to.</param>
    /// <param name="bytesWritten">How many bytes were written.</param>
    /// <returns>If the operation succeeded.</returns>
    private static bool TryWriteBytes(Span<byte> bytes, Span<byte> destination,
        out int bytesWritten)
    {
        try
        {
            bytes.CopyTo(destination);
            bytesWritten = bytes.Length;
            return true;
        }
        catch
        {
            bytesWritten = 0;
            return false;
        }
    }

    /// <summary>Write a BigInteger to a destination span of bytes.</summary>
    /// <param name="bi">The BigInteger to write.</param>
    /// <param name="destination">The span of bytes to copy the BigInteger bytes to.</param>
    /// <param name="bytesWritten">The number of bytes written.</param>
    /// <param name="isBigEndian">If the bytes should be copied in big-endian format or not.</param>
    /// <returns>If the operation succeeded.</returns>
    private static bool TryWriteBigInteger(BigInteger bi, Span<byte> destination,
        out int bytesWritten, bool isBigEndian)
    {
        var bytes = bi.ToByteArray(false, isBigEndian);
        return TryWriteBytes(bytes, destination, out bytesWritten);
    }

    /// <summary>Write an int to a destination span of bytes.</summary>
    /// <param name="i">The int to write.</param>
    /// <param name="destination">The span of bytes to copy the int bytes to.</param>
    /// <param name="bytesWritten">The number of bytes written.</param>
    /// <param name="isBigEndian">If the bytes should be copied in big-endian format or not.</param>
    /// <returns>If the operation succeeded.</returns>
    private static bool TryWriteInt(int i, Span<byte> destination, out int bytesWritten,
        bool isBigEndian)
    {
        var bytes = BitConverter.GetBytes(i);

        // If big-endian is desired, reverse the order of the bytes in the array.
        if (isBigEndian)
        {
            Array.Reverse(bytes);
        }

        return TryWriteBytes(bytes, destination, out bytesWritten);
    }
}
