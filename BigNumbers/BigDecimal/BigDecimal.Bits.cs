using System.Numerics;

namespace Galaxon.BigNumbers;

public partial struct BigDecimal
{
    /// <inheritdoc/>
    public int GetSignificandByteCount() => Significand.GetByteCount();

    /// <inheritdoc/>
    public int GetSignificandBitLength() => GetSignificandByteCount() * 8;

    /// <inheritdoc/>
    public int GetExponentByteCount() => 4;

    /// <inheritdoc/>
    public int GetExponentShortestBitLength() => 32;

    /// <inheritdoc/>
    public readonly bool
        TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Significand, destination, out bytesWritten, true);

    /// <inheritdoc/>
    public readonly bool TryWriteSignificandLittleEndian(Span<byte> destination,
        out int bytesWritten) =>
        TryWriteBigInteger(Significand, destination, out bytesWritten, false);

    /// <inheritdoc/>
    public readonly bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Exponent, destination, out bytesWritten, true);

    /// <inheritdoc/>
    public readonly bool
        TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) =>
        TryWriteBigInteger(Exponent, destination, out bytesWritten, false);

    /// <summary>
    /// Shared logic for:
    /// <see cref="TryWriteBigInteger"/>
    /// </summary>
    private static bool TryWrite(byte[] bytes, Span<byte> destination, out int bytesWritten)
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
    /// <see cref="TryWriteSignificandBigEndian"/>
    /// <see cref="TryWriteSignificandLittleEndian"/>
    /// </summary>
    private static bool TryWriteBigInteger(BigInteger bi, Span<byte> destination,
        out int bytesWritten,
        bool isBigEndian)
    {
        var bytes = bi.ToByteArray(false, isBigEndian);
        return TryWrite(bytes, destination, out bytesWritten);
    }
}
