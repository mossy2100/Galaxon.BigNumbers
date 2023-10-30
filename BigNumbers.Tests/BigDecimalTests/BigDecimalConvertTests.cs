using System.Diagnostics;
using Galaxon.Core.Numbers;

namespace Galaxon.BigNumbers.Tests;

/// <summary>
/// Test conversions between BigDecimal and the standard number types.
/// </summary>
[TestClass]
public class BigDecimalConvertTests
{
    #region Conversion to and from sbyte

    /// <summary>
    /// A few test values for sbyte.
    /// </summary>
    private readonly sbyte[] _sbyteTestValues = { sbyte.MinValue, -1, 0, 1, sbyte.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to sbyte and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToSbyte()
    {
        foreach (var i in _sbyteTestValues)
        {
            BigDecimal bd = i;
            var j = (sbyte)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to sbyte, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToSbyteOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)sbyte.MaxValue + 1;
            return (sbyte)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)sbyte.MinValue - 1;
            return (sbyte)bd;
        });
    }

    /// <summary>
    /// Test converting from sbyte to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedSbyteInRangeTest()
    {
        foreach (var i in _sbyteTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (sbyte)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to sbyte with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedSbyteInRangeTest()
    {
        foreach (var i in _sbyteTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out sbyte j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to sbyte with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedSbyteOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)sbyte.MinValue - 1, out sbyte _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)sbyte.MaxValue + 1, out sbyte _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to sbyte, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingSbyteInRangeTest()
    {
        foreach (var i in _sbyteTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out sbyte j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to sbyte, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingSbyteOverflowTest()
    {
        bool ok;
        sbyte i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)sbyte.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(sbyte.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)sbyte.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(sbyte.MaxValue, i);
    }

    #endregion Conversion to and from sbyte

    #region Conversion to and from byte

    /// <summary>
    /// A few test values for byte.
    /// </summary>
    private readonly byte[] _byteTestValues = { 0, 1, byte.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to byte and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToByte()
    {
        foreach (var i in _byteTestValues)
        {
            BigDecimal bd = i;
            var j = (byte)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to byte, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToByteOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)byte.MaxValue + 1;
            return (byte)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)byte.MinValue - 1;
            return (byte)bd;
        });
    }

    /// <summary>
    /// Test converting from byte to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedByteInRangeTest()
    {
        foreach (var i in _byteTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (byte)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to byte with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedByteInRangeTest()
    {
        foreach (var i in _byteTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out byte j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to byte with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedByteOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)byte.MinValue - 1, out byte _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)byte.MaxValue + 1, out byte _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to byte, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingByteInRangeTest()
    {
        foreach (var i in _byteTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out byte j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to byte, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingByteOverflowTest()
    {
        bool ok;
        byte i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)byte.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(byte.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)byte.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(byte.MaxValue, i);
    }

    #endregion Conversion to and from byte

    #region Conversion to and from short

    /// <summary>
    /// A few test values for short.
    /// </summary>
    private readonly short[] _shortTestValues = { short.MinValue, -1, 0, 1, short.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to short and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToShort()
    {
        foreach (var i in _shortTestValues)
        {
            BigDecimal bd = i;
            var j = (short)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to short, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToShortOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)short.MaxValue + 1;
            return (short)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)short.MinValue - 1;
            return (short)bd;
        });
    }

    /// <summary>
    /// Test converting from short to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedShortInRangeTest()
    {
        foreach (var i in _shortTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (short)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to short with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedShortInRangeTest()
    {
        foreach (var i in _shortTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out short j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to short with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedShortOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)short.MinValue - 1, out short _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)short.MaxValue + 1, out short _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to short, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingShortInRangeTest()
    {
        foreach (var i in _shortTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out short j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to short, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingShortOverflowTest()
    {
        bool ok;
        short i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)short.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(short.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)short.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(short.MaxValue, i);
    }

    #endregion Conversion to and from short

    #region Conversion to and from ushort

    /// <summary>
    /// A few test values for ushort.
    /// </summary>
    private readonly ushort[] _ushortTestValues = { 0, 1, ushort.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to ushort and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToUshort()
    {
        foreach (var i in _ushortTestValues)
        {
            BigDecimal bd = i;
            var j = (ushort)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to ushort, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToUshortOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)ushort.MaxValue + 1;
            return (ushort)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)ushort.MinValue - 1;
            return (ushort)bd;
        });
    }

    /// <summary>
    /// Test converting from ushort to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedUshortInRangeTest()
    {
        foreach (var i in _ushortTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (ushort)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to ushort with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUshortInRangeTest()
    {
        foreach (var i in _ushortTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out ushort j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to ushort with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUshortOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)ushort.MinValue - 1, out ushort _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)ushort.MaxValue + 1, out ushort _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to ushort, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUshortInRangeTest()
    {
        foreach (var i in _ushortTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out ushort j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to ushort, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUshortOverflowTest()
    {
        bool ok;
        ushort i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)ushort.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(ushort.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)ushort.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(ushort.MaxValue, i);
    }

    #endregion Conversion to and from ushort

    #region Conversion to and from int

    /// <summary>
    /// A few test values for int.
    /// </summary>
    private readonly int[] _intTestValues = { int.MinValue, -1, 0, 1, int.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to int and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToInt()
    {
        foreach (var i in _intTestValues)
        {
            BigDecimal bd = i;
            var j = (int)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to int, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToIntOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)int.MaxValue + 1;
            return (int)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)int.MinValue - 1;
            return (int)bd;
        });
    }

    /// <summary>
    /// Test converting from int to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedIntInRangeTest()
    {
        foreach (var i in _intTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (int)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to int with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedIntInRangeTest()
    {
        foreach (var i in _intTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out int j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to int with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedIntOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)int.MinValue - 1, out int _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)int.MaxValue + 1, out int _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to int, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingIntInRangeTest()
    {
        foreach (var i in _intTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out int j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to int, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingIntOverflowTest()
    {
        bool ok;
        int i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)int.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(int.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)int.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(int.MaxValue, i);
    }

    #endregion Conversion to and from int

    #region Conversion to and from uint

    /// <summary>
    /// A few test values for uint.
    /// </summary>
    private readonly uint[] _uintTestValues = { 0, 1, uint.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to uint and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToUint()
    {
        foreach (var i in _uintTestValues)
        {
            BigDecimal bd = i;
            var j = (uint)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to long, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToUintOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)uint.MaxValue + 1;
            return (uint)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)uint.MinValue - 1;
            return (uint)bd;
        });
    }

    /// <summary>
    /// Test converting from uint to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedUintInRangeTest()
    {
        foreach (var i in _uintTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (uint)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to uint with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUintInRangeTest()
    {
        foreach (var i in _uintTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out uint j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to uint with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUintOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)uint.MinValue - 1, out uint _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)uint.MaxValue + 1, out uint _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to uint, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUintInRangeTest()
    {
        foreach (var i in _uintTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out uint j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to uint, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUintOverflowTest()
    {
        bool ok;
        uint i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)uint.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(uint.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)uint.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(uint.MaxValue, i);
    }

    #endregion Conversion to and from uint

    #region Conversion to and from long

    /// <summary>
    /// A few test values for long.
    /// </summary>
    private readonly long[] _longTestValues = { long.MinValue, -1, 0, 1, long.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to long and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToLong()
    {
        foreach (var i in _longTestValues)
        {
            BigDecimal bd = i;
            var j = (long)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test an OverflowException is thrown when casting from BigDecimal to long, outside the
    /// valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToLongOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)long.MaxValue + 1;
            return (long)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)long.MinValue - 1;
            return (long)bd;
        });
    }

    /// <summary>
    /// Test converting from long to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedLongInRangeTest()
    {
        foreach (var i in _longTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (long)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to long with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedLongInRangeTest()
    {
        foreach (var i in _longTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out long j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to long with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedLongOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)long.MinValue - 1, out long _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)long.MaxValue + 1, out long _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to long, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingLongInRangeTest()
    {
        foreach (var i in _longTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out long j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to long, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingLongOverflowTest()
    {
        bool ok;
        long i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)long.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(long.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)long.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(long.MaxValue, i);
    }

    #endregion Conversion to and from long

    #region Conversion to and from ulong

    /// <summary>
    /// A few test values for ulong.
    /// </summary>
    private readonly ulong[] _ulongTestValues = { 0, 1, ulong.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to ulong and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToUlong()
    {
        foreach (var i in _ulongTestValues)
        {
            BigDecimal bd = i;
            var j = (ulong)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to ulong, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToUlongOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)ulong.MaxValue + 1;
            return (ulong)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)ulong.MinValue - 1;
            return (ulong)bd;
        });
    }

    /// <summary>
    /// Test converting from ulong to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedUlongInRangeTest()
    {
        foreach (var i in _ulongTestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (ulong)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to ulong with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUlongInRangeTest()
    {
        foreach (var i in _ulongTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out ulong j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to ulong with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUlongOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)ulong.MinValue - 1, out ulong _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)ulong.MaxValue + 1, out ulong _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to ulong, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUlongInRangeTest()
    {
        foreach (var i in _ulongTestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out ulong j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to ulong, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUlongOverflowTest()
    {
        bool ok;
        ulong i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)ulong.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(ulong.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)ulong.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(ulong.MaxValue, i);
    }

    #endregion Conversion to and from ulong

    #region Conversion to and from Int128

    /// <summary>
    /// A few test values for Int128.
    /// </summary>
    private readonly Int128[] _int128TestValues = { Int128.MinValue, -1, 0, 1, Int128.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to Int128 and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToInt128()
    {
        foreach (var i in _int128TestValues)
        {
            BigDecimal bd = i;
            var j = (Int128)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to Int128, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToInt128OutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)Int128.MaxValue + 1;
            return (Int128)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)Int128.MinValue - 1;
            return (Int128)bd;
        });
    }

    /// <summary>
    /// Test converting from Int128 to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedInt128InRangeTest()
    {
        foreach (var i in _int128TestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (Int128)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to Int128 with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedInt128InRangeTest()
    {
        foreach (var i in _int128TestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out Int128 j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to Int128 with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedInt128OverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)Int128.MinValue - 1, out Int128 _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)Int128.MaxValue + 1, out Int128 _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to Int128, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingInt128InRangeTest()
    {
        foreach (var i in _int128TestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out Int128 j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to Int128, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingInt128OverflowTest()
    {
        bool ok;
        Int128 i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)Int128.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(Int128.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)Int128.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(Int128.MaxValue, i);
    }

    #endregion Conversion to and from Int128

    #region Conversion to and from UInt128

    /// <summary>
    /// A few test values for UInt128.
    /// </summary>
    private readonly UInt128[] _uint128TestValues = { 0, 1, UInt128.MaxValue };

    /// <summary>
    /// Test casting from BigDecimal to UInt128 and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToUInt128()
    {
        foreach (var i in _uint128TestValues)
        {
            BigDecimal bd = i;
            var j = (UInt128)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to UInt128, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToUInt128OutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)UInt128.MaxValue + 1;
            return (UInt128)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = (BigDecimal)UInt128.MinValue - 1;
            return (UInt128)bd;
        });
    }

    /// <summary>
    /// Test converting from UInt128 to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedUInt128InRangeTest()
    {
        foreach (var i in _uint128TestValues)
        {
            var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
            Assert.IsTrue(ok);
            var j = (UInt128)bd;
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to UInt128 with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUInt128InRangeTest()
    {
        foreach (var i in _uint128TestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToChecked(bd, out UInt128 j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to UInt128 with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedUInt128OverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)UInt128.MinValue - 1, out UInt128 _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)UInt128.MaxValue + 1, out UInt128 _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to UInt128, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUInt128InRangeTest()
    {
        foreach (var i in _uint128TestValues)
        {
            BigDecimal bd = i;
            var ok = BigDecimal.TryConvertToSaturating(bd, out UInt128 j);
            Assert.IsTrue(ok);
            Assert.AreEqual(i, j);
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to UInt128, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingUInt128OverflowTest()
    {
        bool ok;
        UInt128 i;

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)UInt128.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(UInt128.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating((BigDecimal)UInt128.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(UInt128.MaxValue, i);
    }

    #endregion Conversion to and from UInt128

    #region Conversion to and from Half

    /// <summary>
    /// A few test values for Half.
    /// </summary>
    private static Half[] GetHalfTestValues()
    {
        var minPosNorm = XFloatingPoint.GetMinPosNormalValue<Half>();
        var maxPosSub = XFloatingPoint.GetMaxPosSubnormalValue<Half>();
        return new[]
        {
            Half.MinValue,
            (Half)(-1),
            -minPosNorm,
            -maxPosSub,
            -Half.Epsilon,
            (Half)0,
            Half.Epsilon,
            maxPosSub,
            minPosNorm,
            (Half)1,
            Half.MaxValue
        };
    }

    /// <summary>
    /// Test value for negative overflow for Half.
    /// </summary>
    private static readonly BigDecimal _halfNegInf = (BigDecimal)Half.MinValue - 1;

    /// <summary>
    /// Test value for positive overflow for Half.
    /// </summary>
    private static readonly BigDecimal _halfPosInf = (BigDecimal)Half.MaxValue + 1;

    /// <summary>
    /// Test casting from BigDecimal to Half and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToHalf()
    {
        foreach (var f in GetHalfTestValues())
        {
            Trace.WriteLine($"f = {f:E5}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E5}");
            var g = (Half)bd;
            Trace.WriteLine($"g = {g:E5}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from Half to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedHalfInRangeTest()
    {
        foreach (var f in GetHalfTestValues())
        {
            Trace.WriteLine($"f = {f:E5}");
            var ok = BigDecimal.TryConvertFromChecked(f, out var bd);
            Trace.WriteLine($"bd = {bd:E5}");
            Assert.IsTrue(ok);
            var g = (Half)bd;
            Trace.WriteLine($"g = {g:E5}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to Half with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedHalfInRangeTest()
    {
        foreach (var f in GetHalfTestValues())
        {
            Trace.WriteLine($"f = {f:E5}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E5}");
            var ok = BigDecimal.TryConvertToChecked(bd, out Half g);
            Trace.WriteLine($"g = {g:E5}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to Half with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedHalfOverflowTest()
    {
        BigDecimal bd;
        Half f;
        bool ok;

        bd = _halfNegInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.IsTrue(ok);
        Assert.AreEqual(Half.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _halfPosInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.IsTrue(ok);
        Assert.AreEqual(Half.PositiveInfinity, f);
    }

    /// <summary>
    /// Test converting from a BigDecimal to Half, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingHalfInRangeTest()
    {
        foreach (var f in GetHalfTestValues())
        {
            Trace.WriteLine($"f = {f:E5}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E5}");
            var ok = BigDecimal.TryConvertToSaturating(bd, out Half g);
            Trace.WriteLine($"g = {g:E5}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to Half, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingHalfOverflowTest()
    {
        BigDecimal bd;
        Half f;
        bool ok;

        bd = _halfNegInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(Half.MinValue, f);
        Trace.WriteLine("");

        bd = _halfPosInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(Half.MaxValue, f);
    }

    #endregion Conversion to and from Half

    #region Conversion to and from float

    /// <summary>
    /// A few test values for float.
    /// </summary>
    private static float[] GetFloatTestValues()
    {
        var minPosNorm = XFloatingPoint.GetMinPosNormalValue<float>();
        var maxPosSub = XFloatingPoint.GetMaxPosSubnormalValue<float>();
        return new[]
        {
            float.MinValue,
            -1,
            -minPosNorm,
            -maxPosSub,
            -float.Epsilon,
            0,
            float.Epsilon,
            maxPosSub,
            minPosNorm,
            1,
            float.MaxValue
        };
    }

    /// <summary>
    /// Test value for negative overflow for float.
    /// </summary>
    private static readonly BigDecimal _floatNegInf = (BigDecimal)float.MinValue - 1e29;

    /// <summary>
    /// Test value for positive overflow for float.
    /// </summary>
    private static readonly BigDecimal _floatPosInf = (BigDecimal)float.MaxValue + 1e29;

    /// <summary>
    /// Test casting from BigDecimal to float and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToFloat()
    {
        foreach (var f in GetFloatTestValues())
        {
            Trace.WriteLine($"f = {f:E9}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E9}");
            var g = (float)bd;
            Trace.WriteLine($"g = {g:E9}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to float, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ReturnsInfinityWhenCastingToFloatOutsideOfValidRange()
    {
        BigDecimal bd;
        float f;

        bd = _floatNegInf;
        Trace.WriteLine($"bd = {bd:E9}");
        f = (float)bd;
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(float.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _floatPosInf;
        Trace.WriteLine($"bd = {bd:E9}");
        f = (float)bd;
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(float.PositiveInfinity, f);
    }

    /// <summary>
    /// Test converting from float to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedFloatInRangeTest()
    {
        foreach (var f in GetFloatTestValues())
        {
            Trace.WriteLine($"f = {f:E9}");
            var ok = BigDecimal.TryConvertFromChecked(f, out var bd);
            Trace.WriteLine($"bd = {bd:E9}");
            Assert.IsTrue(ok);
            var g = (float)bd;
            Trace.WriteLine($"g = {g:E9}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to float with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedFloatInRangeTest()
    {
        foreach (var f in GetFloatTestValues())
        {
            Trace.WriteLine($"f = {f:E9}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E9}");
            var ok = BigDecimal.TryConvertToChecked(bd, out float g);
            Trace.WriteLine($"g = {g:E9}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to float with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedFloatOverflowTest()
    {
        BigDecimal bd;
        float f;
        bool ok;

        bd = _floatNegInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.IsTrue(ok);
        Assert.AreEqual(float.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _floatPosInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.IsTrue(ok);
        Assert.AreEqual(float.PositiveInfinity, f);
    }

    /// <summary>
    /// Test converting from a BigDecimal to float, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingFloatInRangeTest()
    {
        foreach (var f in GetFloatTestValues())
        {
            Trace.WriteLine($"f = {f:E9}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E9}");
            var ok = BigDecimal.TryConvertToSaturating(bd, out float g);
            Trace.WriteLine($"g = {g:E9}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to float, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingFloatOverflowTest()
    {
        BigDecimal bd;
        float f;
        bool ok;

        bd = _floatNegInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(float.MinValue, f);
        Trace.WriteLine("");

        bd = _floatPosInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(float.MaxValue, f);
    }

    #endregion Conversion to and from float

    #region Conversion to and from double

    /// <summary>
    /// A few test values for double.
    /// </summary>
    private static double[] GetDoubleTestValues()
    {
        var minPosNorm = XFloatingPoint.GetMinPosNormalValue<double>();
        var maxPosSub = XFloatingPoint.GetMaxPosSubnormalValue<double>();
        return new[]
        {
            double.MinValue,
            -1,
            -minPosNorm,
            -maxPosSub,
            -double.Epsilon,
            0,
            double.Epsilon,
            maxPosSub,
            minPosNorm,
            1,
            double.MaxValue
        };
    }

    /// <summary>
    /// Test value for negative overflow for double.
    /// </summary>
    private static readonly BigDecimal _doubleNegInf = (BigDecimal)double.MinValue - 1e291;

    /// <summary>
    /// Test value for positive overflow for double.
    /// </summary>
    private static readonly BigDecimal _doublePosInf = (BigDecimal)double.MaxValue + 1e291;

    /// <summary>
    /// Test casting from BigDecimal to double and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToDouble()
    {
        foreach (var f in GetDoubleTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E17}");
            var g = (double)bd;
            Trace.WriteLine($"g = {g:E17}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to double, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ReturnsInfinityWhenCastingToDoubleOutsideOfValidRange()
    {
        BigDecimal bd;
        double f;

        bd = _doubleNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        f = (double)bd;
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(double.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _doublePosInf;
        Trace.WriteLine($"bd = {bd:E17}");
        f = (double)bd;
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(double.PositiveInfinity, f);
    }

    /// <summary>
    /// Test converting from double to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedDoubleInRangeTest()
    {
        foreach (var f in GetDoubleTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            var ok = BigDecimal.TryConvertFromChecked(f, out var bd);
            Trace.WriteLine($"bd = {bd:E17}");
            Assert.IsTrue(ok);
            var g = (double)bd;
            Trace.WriteLine($"g = {g:E17}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to double with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedDoubleInRangeTest()
    {
        foreach (var f in GetDoubleTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E17}");
            var ok = BigDecimal.TryConvertToChecked(bd, out double g);
            Trace.WriteLine($"g = {g:E17}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to double with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedDoubleOverflowTest()
    {
        BigDecimal bd;
        double f;
        bool ok;

        bd = _doubleNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.IsTrue(ok);
        Assert.AreEqual(double.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _doublePosInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.IsTrue(ok);
        Assert.AreEqual(double.PositiveInfinity, f);
    }

    /// <summary>
    /// Test converting from a BigDecimal to double, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingDoubleInRangeTest()
    {
        foreach (var f in GetDoubleTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E17}");
            var ok = BigDecimal.TryConvertToSaturating(bd, out double g);
            Trace.WriteLine($"g = {g:E17}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to double, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingDoubleOverflowTest()
    {
        BigDecimal bd;
        double f;
        bool ok;

        bd = _doubleNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(double.MinValue, f);
        Trace.WriteLine("");

        bd = _doublePosInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(double.MaxValue, f);
    }

    #endregion Conversion to and from double

    #region Conversion to and from decimal

    /// <summary>
    /// A few test values for decimal.
    /// </summary>
    private static decimal[] GetDecimalTestValues() =>
        new[]
        {
            decimal.MinValue,
            -1,
            0,
            1,
            decimal.MaxValue
        };

    /// <summary>
    /// Test value for negative overflow for decimal.
    /// </summary>
    private static readonly BigDecimal _decimalNegInf = (BigDecimal)decimal.MinValue - 1;

    /// <summary>
    /// Test value for positive overflow for decimal.
    /// </summary>
    private static readonly BigDecimal _decimalPosInf = (BigDecimal)decimal.MaxValue + 1;

    /// <summary>
    /// Test casting from BigDecimal to decimal and back.
    /// </summary>
    [TestMethod]
    public void ValidCastsToDecimal()
    {
        foreach (var f in GetDecimalTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E17}");
            var g = (decimal)bd;
            Trace.WriteLine($"g = {g:E17}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test casting from BigDecimal to decimal, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToDecimalOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = _decimalNegInf;
            return (decimal)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = _decimalPosInf;
            return (decimal)bd;
        });
    }

    /// <summary>
    /// Test converting from decimal to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertFromCheckedDecimalInRangeTest()
    {
        foreach (var f in GetDecimalTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            var ok = BigDecimal.TryConvertFromChecked(f, out var bd);
            Trace.WriteLine($"bd = {bd:E17}");
            Assert.IsTrue(ok);
            var g = (decimal)bd;
            Trace.WriteLine($"g = {g:E17}");
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from BigDecimal to decimal with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedDecimalInRangeTest()
    {
        foreach (var f in GetDecimalTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E17}");
            var ok = BigDecimal.TryConvertToChecked(bd, out decimal g);
            Trace.WriteLine($"g = {g:E17}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to decimal with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedDecimalOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(_decimalNegInf, out decimal _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(_decimalPosInf, out decimal _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to decimal, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingDecimalInRangeTest()
    {
        foreach (var f in GetDecimalTestValues())
        {
            Trace.WriteLine($"f = {f:E17}");
            BigDecimal bd = f;
            Trace.WriteLine($"bd = {bd:E17}");
            var ok = BigDecimal.TryConvertToSaturating(bd, out decimal g);
            Trace.WriteLine($"g = {g:E17}");
            Assert.IsTrue(ok);
            Assert.AreEqual(f, g);
            Trace.WriteLine("");
        }
    }

    /// <summary>
    /// Test converting from a BigDecimal to decimal, with saturation, outside the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToSaturatingDecimalOverflowTest()
    {
        BigDecimal bd;
        decimal f;
        bool ok;

        bd = _decimalNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(decimal.MinValue, f);
        Trace.WriteLine("");

        bd = _decimalPosInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(decimal.MaxValue, f);
    }

    #endregion Conversion to and from decimal
}
