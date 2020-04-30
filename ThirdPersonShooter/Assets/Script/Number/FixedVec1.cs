﻿using System;

/// <summary>
/// 定点数字
/// </summary>
[Serializable]
public struct FixedVec1
{
    public static int frac_bits = 16; // 小数使用16位
    public static FixedVec1 Zero = new FixedVec1(0);

    internal Int64 m_num;

    public FixedVec1(int x)
    {
        m_num = x << frac_bits;
    }

    public FixedVec1(float x)
    {
        m_num = (Int64)(x * (1 << frac_bits));
    }

    public FixedVec1(Int64 x)
    {
        m_num = x << frac_bits;
    }

    public Int64 GetValue()
    {
        return m_num;
    }

    public FixedVec1 SetValue(Int64 v)
    {
        m_num = v;
        return this;
    }

    public static FixedVec1 Lerp(FixedVec1 a, FixedVec1 b, float delta)
    {
        return a + (b - a) * delta;
    }

    public static FixedVec1 Lerp(FixedVec1 a, FixedVec1 b, FixedVec1 delta)
    {
        return a + (b - a) * delta;
    }

    public FixedVec1 Abs()
    {
        return FixedVec1.Abs(this);
    }

    public FixedVec1 Sqrt()
    {
        return FixedVec1.Sqrt(this);
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static FixedVec1 operator+(FixedVec1 f1, FixedVec1 f2)
    {
        return new FixedVec1(f1.GetValue() + f2.GetValue());
    }

    public static FixedVec1 operator +(FixedVec1 f1, int f2)
    {
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 + tmp;
    }

    public static FixedVec1 operator +(int f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp + f2;
    }

    public static FixedVec1 operator +(FixedVec1 f1, Int64 f2)
    {
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 + tmp;
    }

    public static FixedVec1 operator +(Int64 f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp + f2;
    }
    public static FixedVec1 operator +(FixedVec1 f1, float f2)
    {
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 + tmp;
    }

    public static FixedVec1 operator +(float f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp + f2;
    }

    //---------------------------------------------------------------------
    //---------------------------------------------------------------------
    public static FixedVec1 operator -(FixedVec1 f1, FixedVec1 f2)
    {
        return new FixedVec1(f1.GetValue() - f2.GetValue());
    }

    public static FixedVec1 operator -(FixedVec1 f1, int f2)
    {
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 - tmp;
    }

    public static FixedVec1 operator -(int f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp - f2;
    }

    public static FixedVec1 operator -(FixedVec1 f1, Int64 f2)
    {
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 - tmp;
    }

    public static FixedVec1 operator -(Int64 f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp - f2;
    }
    public static FixedVec1 operator -(FixedVec1 f1, float f2)
    {
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 - tmp;
    }

    public static FixedVec1 operator -(float f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp - f2;
    }
    public static FixedVec1 operator -(FixedVec1 f2)
    {
        return new FixedVec1(-f2.m_num);
    }

    //*********************************************************************
    public static FixedVec1 operator *(FixedVec1 f1, FixedVec1 f2)
    {
        return new FixedVec1(f1.GetValue() * f2.GetValue());
    }

    public static FixedVec1 operator *(FixedVec1 f1, int f2)
    {
        return f1 * f2;
    }

    public static FixedVec1 operator *(int f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp * f2;
    }

    public static FixedVec1 operator *(FixedVec1 f1, Int64 f2)
    {
        return f1 * f2;
    }

    public static FixedVec1 operator *(Int64 f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp * f2;
    }
    public static FixedVec1 operator *(FixedVec1 f1, float f2)
    {
        return f1 * f2;
    }

    public static FixedVec1 operator *(float f1, FixedVec1 f2)
    {
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp * f2;
    }

    /////////////////////////////////////////////////////////////
    public static FixedVec1 operator /(FixedVec1 f1, FixedVec1 f2)
    {
        if (f2 == FixedVec1.Zero)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        return new FixedVec1(f1.GetValue() / f2.GetValue());
    }

    public static FixedVec1 operator /(FixedVec1 f1, int f2)
    {
        if (f2 == 0)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 / tmp;
    }

    public static FixedVec1 operator /(int f1, FixedVec1 f2)
    {
        if (f2 == FixedVec1.Zero)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp / f2;
    }

    public static FixedVec1 operator /(FixedVec1 f1, Int64 f2)
    {
        if (f2 == 0)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 / tmp;
    }

    public static FixedVec1 operator /(Int64 f1, FixedVec1 f2)
    {
        if (f2 == FixedVec1.Zero)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp / f2;
    }

    public static FixedVec1 operator /(FixedVec1 f1, float f2)
    {
        if (f2 < 0.00001f || f2 > -0.00001f)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        FixedVec1 tmp = new FixedVec1(f2);
        return f1 / tmp;
    }

    public static FixedVec1 operator /(float f1, FixedVec1 f2)
    {
        if (f2 == FixedVec1.Zero)
        {
            UnityEngine.Debug.LogError("div zero");
            return FixedVec1.Zero;
        }
        FixedVec1 tmp = new FixedVec1(f1);
        return tmp / f2;
    }

    public static FixedVec1 Sqrt(FixedVec1 n)
    {
        return new FixedVec1((Int64)Math.Sqrt(n.m_num));
    }

    // 比较
    public static bool operator > (FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num > f2.m_num;
    }
    public static bool operator < (FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num < f2.m_num;
    }

    public static bool operator >= (FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num >= f2.m_num;
    }
    public static bool operator <=(FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num <= f2.m_num;
    }
    public static bool operator ==(FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num == f2.m_num;
    }
    public static bool operator != (FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num != f2.m_num;
    }
    public static bool Equals (FixedVec1 f1, FixedVec1 f2)
    {
        return f1.m_num == f2.m_num;
    }

    public bool Equals(FixedVec1 f1)
    {
        return this.m_num == f1.m_num;
    }
    public static bool operator >(FixedVec1 f1, float f2)
    {
        return f1.m_num > float_to_int64(f2);
    }
    public static bool operator <(FixedVec1 f1, float f2)
    {
        return f1.m_num < float_to_int64(f2);
    }

    public static bool operator >=(FixedVec1 f1, float f2)
    {
        return f1.m_num >= float_to_int64(f2);
    }
    public static bool operator <=(FixedVec1 f1, float f2)
    {
        return f1.m_num <= float_to_int64(f2);
    }
    public static bool operator ==(FixedVec1 f1, float f2)
    {
        return f1.m_num == float_to_int64(f2);
    }
    public static bool operator !=(FixedVec1 f1, float f2)
    {
        return f1.m_num != float_to_int64(f2);
    }

    public static FixedVec1 Max()
    {
        return new FixedVec1(Int64.MaxValue);
    }

    public static FixedVec1 Max(FixedVec1 f1, FixedVec1 f2)
    {
        return new FixedVec1(Math.Max(f1.m_num, f2.m_num));
    }
    public static FixedVec1 Min(FixedVec1 f1, FixedVec1 f2)
    {
        return new FixedVec1(Math.Min(f1.m_num, f2.m_num));
    }

    public static FixedVec1 Abs(FixedVec1 f1)
    {
        return new FixedVec1(Math.Abs(f1.m_num));
    }

    public float ToFloat()
    {
        return m_num / (float)(1 << frac_bits);
    }

    public int ToInt()
    {
        return (int)m_num / (1 << frac_bits);
    }

    public override string ToString()
    {
        double tmp = (double)m_num / (double)(1 << frac_bits);
        return tmp.ToString();
    }

    private static Int64 float_to_int64(float f)
    {
        return (Int64)(f * (1 << frac_bits));
    }
}