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

    /// <summary>A few test values for sbyte.</summary>
    private static object[][] _SbyteTestValues { get; } =
        new[]
        {
            new object[] { sbyte.MinValue },
            new object[] { (sbyte)(-1) },
            new object[] { (sbyte)0 },
            new object[] { (sbyte)1 },
            new object[] { sbyte.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to sbyte and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_SbyteTestValues))]
    public void ValidCastsToSbyte(sbyte i)
    {
        BigDecimal bd = i;
        var j = (sbyte)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_SbyteTestValues))]
    public void TryConvertFromCheckedSbyteInRangeTest(sbyte i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (sbyte)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to sbyte with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_SbyteTestValues))]
    public void TryConvertToCheckedSbyteInRangeTest(sbyte i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out sbyte j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_SbyteTestValues))]
    public void TryConvertToSaturatingSbyteInRangeTest(sbyte i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out sbyte j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for byte.</summary>
    private static object[][] _ByteTestValues { get; } =
        new[]
        {
            new object[] { (byte)0 },
            new object[] { (byte)1 },
            new object[] { byte.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to byte and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_ByteTestValues))]
    public void ValidCastsToByte(byte i)
    {
        BigDecimal bd = i;
        var j = (byte)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_ByteTestValues))]
    public void TryConvertFromCheckedByteInRangeTest(byte i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (byte)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to byte with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_ByteTestValues))]
    public void TryConvertToCheckedByteInRangeTest(byte i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out byte j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_ByteTestValues))]
    public void TryConvertToSaturatingByteInRangeTest(byte i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out byte j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for short.</summary>
    private static object[][] _ShortTestValues { get; } =
        new[]
        {
            new object[] { short.MinValue },
            new object[] { (short)(-1) },
            new object[] { (short)0 },
            new object[] { (short)1 },
            new object[] { short.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to short and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_ShortTestValues))]
    public void ValidCastsToShort(short i)
    {
        BigDecimal bd = i;
        var j = (short)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_ShortTestValues))]
    public void TryConvertFromCheckedShortInRangeTest(short i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (short)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to short with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_ShortTestValues))]
    public void TryConvertToCheckedShortInRangeTest(short i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out short j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_ShortTestValues))]
    public void TryConvertToSaturatingShortInRangeTest(short i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out short j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for ushort.</summary>
    private static object[][] _UshortTestValues { get; } =
        new[]
        {
            new object[] { (ushort)0 },
            new object[] { (ushort)1 },
            new object[] { ushort.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to ushort and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_UshortTestValues))]
    public void ValidCastsToUshort(ushort i)
    {
        BigDecimal bd = i;
        var j = (ushort)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_UshortTestValues))]
    public void TryConvertFromCheckedUshortInRangeTest(ushort i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (ushort)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to ushort with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_UshortTestValues))]
    public void TryConvertToCheckedUshortInRangeTest(ushort i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out ushort j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_UshortTestValues))]
    public void TryConvertToSaturatingUshortInRangeTest(ushort i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out ushort j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for int.</summary>
    private static object[][] _IntTestValues { get; } =
        new[]
        {
            new object[] { int.MinValue },
            new object[] { (int)(-1) },
            new object[] { (int)0 },
            new object[] { (int)1 },
            new object[] { int.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to int and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_IntTestValues))]
    public void ValidCastsToInt(int i)
    {
        BigDecimal bd = i;
        var j = (int)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_IntTestValues))]
    public void TryConvertFromCheckedIntInRangeTest(int i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (int)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to int with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_IntTestValues))]
    public void TryConvertToCheckedIntInRangeTest(int i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out int j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_IntTestValues))]
    public void TryConvertToSaturatingIntInRangeTest(int i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out int j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for uint.</summary>
    private static object[][] _UintTestValues { get; } =
        new[]
        {
            new object[] { (uint)0 },
            new object[] { (uint)1 },
            new object[] { uint.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to uint and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_UintTestValues))]
    public void ValidCastsToUint(uint i)
    {
        BigDecimal bd = i;
        var j = (uint)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_UintTestValues))]
    public void TryConvertFromCheckedUintInRangeTest(uint i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (uint)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to uint with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_UintTestValues))]
    public void TryConvertToCheckedUintInRangeTest(uint i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out uint j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_UintTestValues))]
    public void TryConvertToSaturatingUintInRangeTest(uint i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out uint j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for long.</summary>
    private static object[][] _LongTestValues { get; } =
        new[]
        {
            new object[] { long.MinValue },
            new object[] { (long)(-1) },
            new object[] { (long)0 },
            new object[] { (long)1 },
            new object[] { long.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to long and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_LongTestValues))]
    public void ValidCastsToLong(long i)
    {
        BigDecimal bd = i;
        var j = (long)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_LongTestValues))]
    public void TryConvertFromCheckedLongInRangeTest(long i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (long)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to long with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_LongTestValues))]
    public void TryConvertToCheckedLongInRangeTest(long i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out long j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_LongTestValues))]
    public void TryConvertToSaturatingLongInRangeTest(long i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out long j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for ulong.</summary>
    private static object[][] _UlongTestValues { get; } =
        new[]
        {
            new object[] { (ulong)0 },
            new object[] { (ulong)1 },
            new object[] { ulong.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to ulong and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_UlongTestValues))]
    public void ValidCastsToUlong(ulong i)
    {
        BigDecimal bd = i;
        var j = (ulong)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_UlongTestValues))]
    public void TryConvertFromCheckedUlongInRangeTest(ulong i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (ulong)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to ulong with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_UlongTestValues))]
    public void TryConvertToCheckedUlongInRangeTest(ulong i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out ulong j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_UlongTestValues))]
    public void TryConvertToSaturatingUlongInRangeTest(ulong i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out ulong j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for Int128.</summary>
    private static object[][] _Int128TestValues { get; } =
        new[]
        {
            new object[] { Int128.MinValue },
            new object[] { (Int128)(-1) },
            new object[] { (Int128)0 },
            new object[] { (Int128)1 },
            new object[] { Int128.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to Int128 and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_Int128TestValues))]
    public void ValidCastsToInt128(Int128 i)
    {
        BigDecimal bd = i;
        var j = (Int128)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_Int128TestValues))]
    public void TryConvertFromCheckedInt128InRangeTest(Int128 i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (Int128)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to Int128 with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_Int128TestValues))]
    public void TryConvertToCheckedInt128InRangeTest(Int128 i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out Int128 j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_Int128TestValues))]
    public void TryConvertToSaturatingInt128InRangeTest(Int128 i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out Int128 j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for UInt128.</summary>
    private static object[][] _Uint128TestValues { get; } =
        new[]
        {
            new object[] { (UInt128)0 },
            new object[] { (UInt128)1 },
            new object[] { UInt128.MaxValue }
        };

    /// <summary>
    /// Test casting from BigDecimal to UInt128 and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_Uint128TestValues))]
    public void ValidCastsToUInt128(UInt128 i)
    {
        BigDecimal bd = i;
        var j = (UInt128)bd;
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_Uint128TestValues))]
    public void TryConvertFromCheckedUInt128InRangeTest(UInt128 i)
    {
        var ok = BigDecimal.TryConvertFromChecked(i, out var bd);
        Assert.IsTrue(ok);
        var j = (UInt128)bd;
        Assert.AreEqual(i, j);
    }

    /// <summary>
    /// Test converting from BigDecimal to UInt128 with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_Uint128TestValues))]
    public void TryConvertToCheckedUInt128InRangeTest(UInt128 i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToChecked(bd, out UInt128 j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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
    [DynamicData(nameof(_Uint128TestValues))]
    public void TryConvertToSaturatingUInt128InRangeTest(UInt128 i)
    {
        BigDecimal bd = i;
        var ok = BigDecimal.TryConvertToSaturating(bd, out UInt128 j);
        Assert.IsTrue(ok);
        Assert.AreEqual(i, j);
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

    /// <summary>A few test values for Half.</summary>
    private static object[][] _HalfTestValues
    {
        get
        {
            var minPosNorm = XFloatingPoint.GetMinPosNormalValue<Half>();
            var maxPosSub = XFloatingPoint.GetMaxPosSubnormalValue<Half>();
            return new[]
            {
                new object[] { Half.MinValue },
                new object[] { (Half)(-1) },
                new object[] { -minPosNorm },
                new object[] { -maxPosSub },
                new object[] { -Half.Epsilon },
                new object[] { (Half)0 },
                new object[] { Half.Epsilon },
                new object[] { maxPosSub },
                new object[] { minPosNorm },
                new object[] { (Half)1 },
                new object[] { Half.MaxValue }
            };
        }
    }

    /// <summary>
    /// Test value for negative overflow for Half.
    /// </summary>
    private static readonly BigDecimal _HalfNegInf = (BigDecimal)Half.MinValue - 1;

    /// <summary>
    /// Test value for positive overflow for Half.
    /// </summary>
    private static readonly BigDecimal _HalfPosInf = (BigDecimal)Half.MaxValue + 1;

    /// <summary>
    /// Test casting from BigDecimal to Half and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_HalfTestValues))]
    public void ValidCastsToHalf(Half f)
    {
        Trace.WriteLine($"f = {f:E5}");
        BigDecimal bd = f;
        Trace.WriteLine($"bd = {bd:E5}");
        var g = (Half)bd;
        Trace.WriteLine($"g = {g:E5}");
        Assert.AreEqual(f, g);
        Trace.WriteLine("");
    }

    /// <summary>
    /// Test converting from Half to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_HalfTestValues))]
    public void TryConvertFromCheckedHalfInRangeTest(Half f)
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

    /// <summary>
    /// Test converting from BigDecimal to Half with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_HalfTestValues))]
    public void TryConvertToCheckedHalfInRangeTest(Half f)
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

        bd = _HalfNegInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.IsTrue(ok);
        Assert.AreEqual(Half.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _HalfPosInf;
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
    [DynamicData(nameof(_HalfTestValues))]
    public void TryConvertToSaturatingHalfInRangeTest(Half f)
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

        bd = _HalfNegInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(Half.MinValue, f);
        Trace.WriteLine("");

        bd = _HalfPosInf;
        Trace.WriteLine($"bd = {bd:E5}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E5}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(Half.MaxValue, f);
    }

    #endregion Conversion to and from Half

    #region Conversion to and from float

    /// <summary>A few test values for float.</summary>
    private static object[][] _FloatTestValues
    {
        get
        {
            var minPosNorm = XFloatingPoint.GetMinPosNormalValue<float>();
            var maxPosSub = XFloatingPoint.GetMaxPosSubnormalValue<float>();
            return new[]
            {
                new object[] { float.MinValue },
                new object[] { -1f },
                new object[] { -minPosNorm },
                new object[] { -maxPosSub },
                new object[] { -float.Epsilon },
                new object[] { 0f },
                new object[] { float.Epsilon },
                new object[] { maxPosSub },
                new object[] { minPosNorm },
                new object[] { 1f },
                new object[] { float.MaxValue }
            };
        }
    }

    /// <summary>
    /// Test value for negative overflow for float.
    /// </summary>
    private static readonly BigDecimal _FloatNegInf = (BigDecimal)float.MinValue - 1e29;

    /// <summary>
    /// Test value for positive overflow for float.
    /// </summary>
    private static readonly BigDecimal _FloatPosInf = (BigDecimal)float.MaxValue + 1e29;

    /// <summary>
    /// Test casting from BigDecimal to float and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_FloatTestValues))]
    public void ValidCastsToFloat(float f)
    {
        Trace.WriteLine($"f = {f:E9}");
        BigDecimal bd = f;
        Trace.WriteLine($"bd = {bd:E9}");
        var g = (float)bd;
        Trace.WriteLine($"g = {g:E9}");
        Assert.AreEqual(f, g);
        Trace.WriteLine("");
    }

    /// <summary>
    /// Test casting from BigDecimal to float, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ReturnsInfinityWhenCastingToFloatOutsideOfValidRange()
    {
        BigDecimal bd;
        float f;

        bd = _FloatNegInf;
        Trace.WriteLine($"bd = {bd:E9}");
        f = (float)bd;
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(float.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _FloatPosInf;
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
    [DynamicData(nameof(_FloatTestValues))]
    public void TryConvertFromCheckedFloatInRangeTest(float f)
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

    /// <summary>
    /// Test converting from BigDecimal to float with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_FloatTestValues))]
    public void TryConvertToCheckedFloatInRangeTest(float f)
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

        bd = _FloatNegInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.IsTrue(ok);
        Assert.AreEqual(float.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _FloatPosInf;
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
    [DynamicData(nameof(_FloatTestValues))]
    public void TryConvertToSaturatingFloatInRangeTest(float f)
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

        bd = _FloatNegInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(float.MinValue, f);
        Trace.WriteLine("");

        bd = _FloatPosInf;
        Trace.WriteLine($"bd = {bd:E9}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E9}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(float.MaxValue, f);
    }

    #endregion Conversion to and from float

    #region Conversion to and from double

    /// <summary>A few test values for double.</summary>
    private static object[][] _DoubleTestValues
    {
        get
        {
            var minPosNorm = XFloatingPoint.GetMinPosNormalValue<double>();
            var maxPosSub = XFloatingPoint.GetMaxPosSubnormalValue<double>();
            return new[]
            {
                new object[] { double.MinValue },
                new object[] { -1d },
                new object[] { -minPosNorm },
                new object[] { -maxPosSub },
                new object[] { -double.Epsilon },
                new object[] { 0d },
                new object[] { double.Epsilon },
                new object[] { maxPosSub },
                new object[] { minPosNorm },
                new object[] { 1d },
                new object[] { double.MaxValue }
            };
        }
    }

    /// <summary>
    /// Test value for negative overflow for double.
    /// </summary>
    private static readonly BigDecimal _DoubleNegInf = (BigDecimal)double.MinValue - 1e291;

    /// <summary>
    /// Test value for positive overflow for double.
    /// </summary>
    private static readonly BigDecimal _DoublePosInf = (BigDecimal)double.MaxValue + 1e291;

    /// <summary>
    /// Test casting from BigDecimal to double and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_DoubleTestValues))]
    public void ValidCastsToDouble(double f)
    {
        Trace.WriteLine($"f = {f:E17}");
        BigDecimal bd = f;
        Trace.WriteLine($"bd = {bd:E17}");
        var g = (double)bd;
        Trace.WriteLine($"g = {g:E17}");
        Assert.AreEqual(f, g);
        Trace.WriteLine("");
    }

    /// <summary>
    /// Test casting from BigDecimal to double, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ReturnsInfinityWhenCastingToDoubleOutsideOfValidRange()
    {
        BigDecimal bd;
        double f;

        bd = _DoubleNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        f = (double)bd;
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(double.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _DoublePosInf;
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
    [DynamicData(nameof(_DoubleTestValues))]
    public void TryConvertFromCheckedDoubleInRangeTest(double f)
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

    /// <summary>
    /// Test converting from BigDecimal to double with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_DoubleTestValues))]
    public void TryConvertToCheckedDoubleInRangeTest(double f)
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

        bd = _DoubleNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToChecked(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.IsTrue(ok);
        Assert.AreEqual(double.NegativeInfinity, f);
        Trace.WriteLine("");

        bd = _DoublePosInf;
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
    [DynamicData(nameof(_DoubleTestValues))]
    public void TryConvertToSaturatingDoubleInRangeTest(double f)
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

        bd = _DoubleNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(double.MinValue, f);
        Trace.WriteLine("");

        bd = _DoublePosInf;
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
    private static object[][] _DecimalTestValues { get; } = new[]
    {
        new object[] { decimal.MinValue },
        new object[] { -1m },
        new object[] { 0m },
        new object[] { 1m },
        new object[] { decimal.MaxValue }
    };

    /// <summary>
    /// Test value for negative overflow for decimal.
    /// </summary>
    private static readonly BigDecimal _DecimalNegInf = (BigDecimal)decimal.MinValue - 1;

    /// <summary>
    /// Test value for positive overflow for decimal.
    /// </summary>
    private static readonly BigDecimal _DecimalPosInf = (BigDecimal)decimal.MaxValue + 1;

    /// <summary>
    /// Test casting from BigDecimal to decimal and back.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_DecimalTestValues))]
    public void ValidCastsToDecimal(decimal f)
    {
        Trace.WriteLine($"f = {f:E17}");
        BigDecimal bd = f;
        Trace.WriteLine($"bd = {bd:E17}");
        var g = (decimal)bd;
        Trace.WriteLine($"g = {g:E17}");
        Assert.AreEqual(f, g);
        Trace.WriteLine("");
    }

    /// <summary>
    /// Test casting from BigDecimal to decimal, outside the valid range for that type.
    /// </summary>
    [TestMethod]
    public void ThrowsExceptionWhenCastingToDecimalOutsideOfValidRange()
    {
        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = _DecimalNegInf;
            return (decimal)bd;
        });

        Assert.ThrowsException<OverflowException>(() =>
        {
            var bd = _DecimalPosInf;
            return (decimal)bd;
        });
    }

    /// <summary>
    /// Test converting from decimal to BigDecimal with overflow check, within the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_DecimalTestValues))]
    public void TryConvertFromCheckedDecimalInRangeTest(decimal f)
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

    /// <summary>
    /// Test converting from BigDecimal to decimal with overflow check, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_DecimalTestValues))]
    public void TryConvertToCheckedDecimalInRangeTest(decimal f)
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

    /// <summary>
    /// Test converting from a BigDecimal to decimal with overflow check, outside the valid range
    /// for that type.
    /// </summary>
    [TestMethod]
    public void TryConvertToCheckedDecimalOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(_DecimalNegInf, out decimal _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(_DecimalPosInf, out decimal _));
    }

    /// <summary>
    /// Test converting from a BigDecimal to decimal, with saturation, within the valid range for
    /// that type.
    /// </summary>
    [TestMethod]
    [DynamicData(nameof(_DecimalTestValues))]
    public void TryConvertToSaturatingDecimalInRangeTest(decimal f)
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

        bd = _DecimalNegInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(decimal.MinValue, f);
        Trace.WriteLine("");

        bd = _DecimalPosInf;
        Trace.WriteLine($"bd = {bd:E17}");
        ok = BigDecimal.TryConvertToSaturating(bd, out f);
        Trace.WriteLine($"f = {f:E17}");
        Assert.AreEqual(true, ok);
        Assert.AreEqual(decimal.MaxValue, f);
    }

    #endregion Conversion to and from decimal
}
