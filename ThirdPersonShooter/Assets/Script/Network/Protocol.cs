using System;
using System.Collections.Generic;
using System.Text;

public class Protocol
{
    List<Byte> byteList = new List<byte>();
    byte[] bytes;
    int index = 0;
    int lastOffset = 0;
    UInt16 strLen = 0;
    byte[] tempBytes;
    public int Length
    {
        get { return bytes.Length - (index + lastOffset); }
    }    
    
    public Protocol InitMsg(byte[] bytes)
    {
        this.bytes = bytes;
        index = 0;
        strLen = 0;
        lastOffset = 0;
        return this;
    }

    public byte[] GetByteStream()
    {
        return byteList.ToArray();
    }

    public UInt16 getUInt16()
    {
        index += lastOffset;
        lastOffset = 2;
        return BitConverter.ToUInt16(bytes, index);
    }

    public int getInt32()
    {
        index += lastOffset;
        lastOffset = 4;
        return BitConverter.ToInt32(bytes, index);
    }

    public Int64 getInt64()
    {
        index += lastOffset;
        lastOffset = 8;
        return BitConverter.ToInt64(bytes, index);
    }

    public string getString()
    {
        strLen = getUInt16();
        index += lastOffset;
        lastOffset = strLen;
        return Encoding.Unicode.GetString(bytes, index, strLen);
    }

    public byte getByte()
    {
        index += lastOffset;
        lastOffset = 1;
        return bytes[index];
    }

    public bool getBool()
    {
        return getByte() == 1;
    }

    public FixedVec1 getFixedVec1()
    {
        return new FixedVec1(getInt64());
    }

    public FixedVec2 getFixedVec2()
    {
        FixedVec1 v1 = getFixedVec1();
        FixedVec1 v2 = getFixedVec1();
        return new FixedVec2(v1, v2);
    }
    //--------------------------------------------------------------------------
    public void push(byte num)
    {
        byteList.Add(num);
    }

    public void push(int num)
    {
        byteList.AddRange(BitConverter.GetBytes(num));
    }

    public void push(Int64 num)
    {
        byteList.AddRange(BitConverter.GetBytes(num));
    }

    public void push(UInt16 num)
    {
        byteList.AddRange(BitConverter.GetBytes(num));
    }

    public void push(bool num)
    {
        byteList.Add(num ? (byte)1: (byte)0);
    }

    public void push(string s)
    {
        tempBytes = Encoding.Unicode.GetBytes(s);
        strLen = (UInt16)tempBytes.Length;
        push(strLen);
        byteList.AddRange(tempBytes);
    }
}
