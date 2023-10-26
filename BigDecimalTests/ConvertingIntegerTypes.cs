namespace Galaxon.BigNumbers.BigDecimalTests;

[TestClass]
public class ConvertingIntegerTypes
{
    #region Conversion to and from sbyte

    private BigDecimal[] _sbyteTestValues = { sbyte.MinValue, -1, 0, 1, sbyte.MaxValue };

    [TestMethod]
    public void ValidCastsToSbyte()
    {
        foreach (var bd in _sbyteTestValues)
        {
            var i = (sbyte)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedSbyteInRangeTest()
    {
        foreach (var bd in _sbyteTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out sbyte i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedSbyteOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(sbyte.MinValue - 1, out sbyte _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(sbyte.MaxValue + 1, out sbyte _));
    }

    [TestMethod]
    public void TryConvertToSaturatingSbyteInRangeTest()
    {
        foreach (var bd in _sbyteTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out sbyte i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToSaturatingSbyteOverflowTest()
    {
        bool ok;
        sbyte i;

        ok = BigDecimal.TryConvertToSaturating(sbyte.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(sbyte.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating(sbyte.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(sbyte.MaxValue, i);
    }

    #endregion Conversion to and from sbyte

    #region Conversion to and from byte

    private BigDecimal[] _byteTestValues = { 0, 1, byte.MaxValue };

    [TestMethod]
    public void ValidCastsToByte()
    {
        foreach (var bd in _byteTestValues)
        {
            var i = (byte)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedByteInRangeTest()
    {
        foreach (var bd in _byteTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out byte i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedByteOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(byte.MinValue - 1, out byte _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(byte.MaxValue + 1, out byte _));
    }

    [TestMethod]
    public void TryConvertToSaturatingByteInRangeTest()
    {
        foreach (var bd in _byteTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out byte i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToSaturatingByteOverflowTest()
    {
        bool ok;
        byte i;

        ok = BigDecimal.TryConvertToSaturating(byte.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(byte.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating(byte.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(byte.MaxValue, i);
    }

    #endregion Conversion to and from byte

    #region Conversion to and from short

    private BigDecimal[] _shortTestValues = { short.MinValue, -1, 0, 1, short.MaxValue };

    [TestMethod]
    public void ValidCastsToShort()
    {
        foreach (var bd in _shortTestValues)
        {
            var i = (short)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedShortInRangeTest()
    {
        foreach (var bd in _shortTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out short i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedShortOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(short.MinValue - 1, out short _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(short.MaxValue + 1, out short _));
    }

    [TestMethod]
    public void TryConvertToSaturatingShortInRangeTest()
    {
        foreach (var bd in _shortTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out short i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToSaturatingShortOverflowTest()
    {
        bool ok;
        short i;

        ok = BigDecimal.TryConvertToSaturating(short.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(short.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating(short.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(short.MaxValue, i);
    }

    #endregion Conversion to and from short

    #region Conversion to and from ushort

    private BigDecimal[] _ushortTestValues = { 0, 1, ushort.MaxValue };

    [TestMethod]
    public void ValidCastsToUshort()
    {
        foreach (var bd in _ushortTestValues)
        {
            var i = (ushort)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedUshortInRangeTest()
    {
        foreach (var bd in _ushortTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out ushort i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedUshortOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(ushort.MinValue - 1, out ushort _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked(ushort.MaxValue + 1, out ushort _));
    }

    [TestMethod]
    public void TryConvertToSaturatingUshortInRangeTest()
    {
        foreach (var bd in _ushortTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out ushort i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToSaturatingUshortOverflowTest()
    {
        bool ok;
        ushort i;

        ok = BigDecimal.TryConvertToSaturating(ushort.MinValue - 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(ushort.MinValue, i);

        ok = BigDecimal.TryConvertToSaturating(ushort.MaxValue + 1, out i);
        Assert.AreEqual(true, ok);
        Assert.AreEqual(ushort.MaxValue, i);
    }

    #endregion Conversion to and from ushort

    #region Conversion to and from int

    private BigDecimal[] _intTestValues = { int.MinValue, -1, 0, 1, int.MaxValue };

    [TestMethod]
    public void ValidCastsToInt()
    {
        foreach (var bd in _intTestValues)
        {
            var i = (int)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedIntInRangeTest()
    {
        foreach (var bd in _intTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out int i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedIntOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)int.MinValue - 1, out int _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)int.MaxValue + 1, out int _));
    }

    [TestMethod]
    public void TryConvertToSaturatingIntInRangeTest()
    {
        foreach (var bd in _intTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out int i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

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

    private BigDecimal[] _uintTestValues = { 0, 1, uint.MaxValue };

    [TestMethod]
    public void ValidCastsToUint()
    {
        foreach (var bd in _uintTestValues)
        {
            var i = (uint)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedUintInRangeTest()
    {
        foreach (var bd in _uintTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out uint i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedUintOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)uint.MinValue - 1, out uint _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)uint.MaxValue + 1, out uint _));
    }

    [TestMethod]
    public void TryConvertToSaturatingUintInRangeTest()
    {
        foreach (var bd in _uintTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out uint i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

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

    private BigDecimal[] _longTestValues = { long.MinValue, -1, 0, 1, long.MaxValue };

    [TestMethod]
    public void ValidCastsToLong()
    {
        foreach (var bd in _longTestValues)
        {
            var i = (long)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedLongInRangeTest()
    {
        foreach (var bd in _longTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out long i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedLongOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)long.MinValue - 1, out long _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)long.MaxValue + 1, out long _));
    }

    [TestMethod]
    public void TryConvertToSaturatingLongInRangeTest()
    {
        foreach (var bd in _longTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out long i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

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

    private BigDecimal[] _ulongTestValues = { 0, 1, ulong.MaxValue };

    [TestMethod]
    public void ValidCastsToUlong()
    {
        foreach (var bd in _ulongTestValues)
        {
            var i = (ulong)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedUlongInRangeTest()
    {
        foreach (var bd in _ulongTestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out ulong i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedUlongOverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)ulong.MinValue - 1, out ulong _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)ulong.MaxValue + 1, out ulong _));
    }

    [TestMethod]
    public void TryConvertToSaturatingUlongInRangeTest()
    {
        foreach (var bd in _ulongTestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out ulong i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

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

    private BigDecimal[] _Int128TestValues = { Int128.MinValue, -1, 0, 1, Int128.MaxValue };

    [TestMethod]
    public void ValidCastsToInt128()
    {
        foreach (var bd in _Int128TestValues)
        {
            var i = (Int128)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedInt128InRangeTest()
    {
        foreach (var bd in _Int128TestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out Int128 i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedInt128OverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)Int128.MinValue - 1, out Int128 _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)Int128.MaxValue + 1, out Int128 _));
    }

    [TestMethod]
    public void TryConvertToSaturatingInt128InRangeTest()
    {
        foreach (var bd in _Int128TestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out Int128 i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

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

    private BigDecimal[] _UInt128TestValues = { 0, 1, UInt128.MaxValue };

    [TestMethod]
    public void ValidCastsToUInt128()
    {
        foreach (var bd in _UInt128TestValues)
        {
            var i = (UInt128)bd;
            BigDecimal bd2 = i;
            Assert.AreEqual(bd, bd2);
        }
    }

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

    [TestMethod]
    public void TryConvertToCheckedUInt128InRangeTest()
    {
        foreach (var bd in _UInt128TestValues)
        {
            var ok = BigDecimal.TryConvertToChecked(bd, out UInt128 i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

    [TestMethod]
    public void TryConvertToCheckedUInt128OverflowTest()
    {
        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)UInt128.MinValue - 1, out UInt128 _));

        Assert.ThrowsException<OverflowException>(() =>
            BigDecimal.TryConvertToChecked((BigDecimal)UInt128.MaxValue + 1, out UInt128 _));
    }

    [TestMethod]
    public void TryConvertToSaturatingUInt128InRangeTest()
    {
        foreach (var bd in _UInt128TestValues)
        {
            var ok = BigDecimal.TryConvertToSaturating(bd, out UInt128 i);
            Assert.IsTrue(ok);
            Assert.AreEqual(bd, i);
        }
    }

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
}
