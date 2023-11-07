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
        // There are 4 bytes in an 32-bit integer.
        return 4;
    }

    /// <inheritdoc/>
    public int GetExponentShortestBitLength()
    {
        return GetExponentByteCount() * _BITS_PER_BYTE;
    }

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandLittleEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteBigInteger(Significand, destination, out bytesWritten, false);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandBigEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteBigInteger(Significand, destination, out bytesWritten, true);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteExponentLittleEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteInt(Exponent, destination, out bytesWritten, false);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
    {
        return TryWriteInt(Exponent, destination, out bytesWritten, true);
    }

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

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteSignificandLittleEndian"/>
    /// <see cref="TryWriteSignificandBigEndian"/>
    /// </summary>
    private static bool TryWriteBigInteger(BigInteger bi, Span<byte> destination,
        out int bytesWritten, bool isBigEndian)
    {
        var bytes = bi.ToByteArray(false, isBigEndian);
        return TryWriteBytes(bytes, destination, out bytesWritten);
    }

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteExponentLittleEndian"/>
    /// <see cref="TryWriteExponentBigEndian"/>
    /// </summary>
    private static bool TryWriteInt(int i, Span<byte> destination, out int bytesWritten,
        bool isBigEndian)
    {
        var bytes = BitConverter.GetBytes(i);

        // If big-endian is desired, reverse the order of the bytes in the array.
        if (isBigEndian) Array.Reverse(bytes);

        return TryWriteBytes(bytes, destination, out bytesWritten);
    }
}
