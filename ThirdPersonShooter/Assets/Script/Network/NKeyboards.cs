using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NKeyboards
{
    public enum KeyNum: byte
    {
        Up = 2,
        Down = 4,
        Left = 8,
        Right = 16,
        Fire = 32,
        Jump = 64
    }
    /// <summary>
    /// 前一帧按键值
    /// </summary>
    KeyNum lastKey;

    /// <summary>
    /// 当前帧按键值
    /// </summary>
    KeyNum curKey;
    public NKeyboards()
    {
        lastKey = 0;
        curKey = 0;
    }

    public bool GetKey(KeyNum key)
    {
        if ((curKey & key) != 0)
            return true;
        return false;
    }

    public bool IsKeyDown(KeyNum key)
    {
        if ((curKey & key) != 0)
            return true;
        return false;
    }

    public bool IsKeyUp(KeyNum key)
    {
        if ((lastKey & key) != 0 && (curKey & key) == 0)
            return true;
        return false;
    }

    public void SetKey(bool isDown, KeyNum key)
    {
        key = isDown ? key : 0;
        lastKey = curKey;
        curKey |= key;
    }

    public byte GetByte()
    {
        byte b = (byte)curKey;
        Reset();
        return b;
    }

    public void Parse(Protocol msg)
    {
        Reset();
        curKey = (KeyNum)msg.getByte();
    }
    public void Reset()
    {
        lastKey = curKey;
        curKey = 0;
    }
}
