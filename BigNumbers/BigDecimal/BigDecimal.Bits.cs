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
        return Exponent.GetByteCount();
    }

    /// <inheritdoc/>
    public int GetExponentShortestBitLength()
    {
        return GetExponentByteCount() * _BITS_PER_BYTE;
    }

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandBigEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteBigInteger(Significand, destination, out bytesWritten, true);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandLittleEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteBigInteger(Significand, destination, out bytesWritten, false);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten)
    {
        return TryWriteBigInteger(Exponent, destination, out bytesWritten, true);
    }

    /// <inheritdoc/>
    public readonly bool TryWriteExponentLittleEndian(Span<byte> destination,
        out int bytesWritten)
    {
        return TryWriteBigInteger(Exponent, destination, out bytesWritten, false);
    }

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteSignificandBigEndian"/>
    /// <see cref="TryWriteSignificandLittleEndian"/>
    /// </summary>
    private static bool TryWriteBigInteger(BigInteger bi, Span<byte> destination,
        out int bytesWritten, bool isBigEndian)
    {
        var bytes = bi.ToByteArray(false, isBigEndian);
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
}
