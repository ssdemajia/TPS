using Shaoshuai.Message;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 传输协议，包括类型序列化、反序列化操作
/// </summary>
public class Packet
{
    List<Byte> byteList = new List<byte>();
    byte[] bytes;
    int index = 0;
    int lastOffset = 0;
    Int16 strLen = 0;
    byte[] tempBytes;
    public int Length
    {
        get { return bytes.Length - (index + lastOffset); }
    }    
    
    public Packet InitMsg(byte[] bytes)
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

    public Int16 getInt16()
    {
        index += lastOffset;
        lastOffset = 2;
        return BitConverter.ToInt16(bytes, index);
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
        strLen = getInt16();
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

    public FixedVec3 getFixedVec3()
    {
        FixedVec1 v1 = getFixedVec1();
        FixedVec1 v2 = getFixedVec1();
        FixedVec1 v3 = getFixedVec1();
        return new FixedVec3(v1, v2, v3);
    }

    public T[] getArray<T>(T[] arr) where T: BaseFormat, new()
    {
        Int16 len = getInt16();
        if (len == 0)
            return null;
        T[] result = new T[len];
        for (int i = 0; i < len; i++)
        {
            if (getBool())
                result[i] = null;
            else
            {
                T value = new T();
                value.Deserialize(this);
                result[i] = value;
            }
        }
        return result;
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
        strLen = (Int16)tempBytes.Length;
        push(strLen);
        byteList.AddRange(tempBytes);
    }

    public void push(FixedVec1 f)
    {
        Int64 v = f.GetValue();
        push(v);
    }
    public void push(FixedVec2 f)
    {
        push(f.x);
        push(f.y);
    }
    public void push(FixedVec3 f)
    {
        push(f.x);
        push(f.y);
        push(f.z);
    }

    public void push<T>(T[] arr) where T: BaseFormat
    {
        ushort len = 0;
        if (arr != null)
            len = (ushort)arr.Length;
        push(len);
        foreach (T a in arr)
        {
            push(a == null);
            if (a != null)
                a.Deserialize(this);
        }
    }
}
