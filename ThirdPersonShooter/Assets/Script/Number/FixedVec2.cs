using UnityEngine;

public struct FixedVec2
{
    public FixedVec1 x;
    public FixedVec1 y;


    public FixedVec2(float x, float y)
    {

        this.x = new FixedVec1(x);
        this.y = new FixedVec1(y);
    }

    public FixedVec2(FixedVec1 x, FixedVec1 y)
    {
        this.x = x;
        this.y = y;
    }

    public FixedVec2(FixedVec2 a)
    {
        this.x = a.x;
        this.y = a.y;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x.ToFloat(), 0, y.ToFloat());
    }

    public static FixedVec2 GetV2(FixedVec1 x, FixedVec1 y)
    {
        return new FixedVec2(x, y);
    }

    public static FixedVec2 operator +(FixedVec2 a, FixedVec2 b)
    {
        return new FixedVec2(a.x + b.x, a.y + b.y);
    }

    public static FixedVec2 operator -(FixedVec2 a, FixedVec2 b)
    {
        return new FixedVec2(a.x - b.x, a.y - b.y);
    }

    public static FixedVec2 operator *(FixedVec2 a, FixedVec1 b)
    {
        return new FixedVec2(a.x * b, a.y * b);
    }

    public FixedVec2 Rotate(FixedVec1 value)
    {
        FixedVec1 tx, ty;
        tx = FixedMath.CosAngle(value) * x - y * FixedMath.SinAngle(value);
        ty = FixedMath.CosAngle(value) * y + x * FixedMath.SinAngle(value);
        return new FixedVec2(tx, ty);
    }

    public FixedVec1 ToRotation()
    {
        if (x == 0 && y == 0)
        {
            return new FixedVec1();
        }
        FixedVec1 sin = this.normalized.y;
        if (this.x >= 0)
        {
            return FixedMath.Asin(sin) / FixedMath.PI * 180;
        }
        else
        {
            return FixedMath.Asin(-sin) / FixedMath.PI * 180 + 180;
        }
    }
    public static FixedVec2 Parse(FixedVec1 ratio)
    {
        return new FixedVec2(FixedMath.CosAngle(ratio), FixedMath.SinAngle(ratio));
    }
    public FixedVec2 normalized
    {

        get
        {
            if (x == 0 && y == 0)
            {
                return new FixedVec2();
            }
            float fx = x.ToFloat();
            float fy = y.ToFloat();
            float fn = Mathf.Sqrt(fx * fx + fy * fy);
            var value = new FixedVec2(fx / fn, fy / fn);
            return value;
        }
    }

    public static FixedVec2 left = new FixedVec2(-1, 0);
    public static FixedVec2 right = new FixedVec2(1, 0);
    public static FixedVec2 up = new FixedVec2(0, 1);
    public static FixedVec2 down = new FixedVec2(0, -1);
    public static FixedVec2 zero = new FixedVec2(0, 0);

    public FixedVec1 Dot(FixedVec2 b)
    {
        return Dot(this, b);
    }
    public static FixedVec1 Dot(FixedVec2 a, FixedVec2 b)
    {
        return a.x * b.x + b.y * a.y;
    }

    public static FixedVec2 operator -(FixedVec2 a)
    {
        return new FixedVec2(-a.x, -a.y);
    }
    public static FixedVec3 operator *(FixedVec2 a, FixedVec2 b)
    {
        return new FixedVec3(new FixedVec1(), new FixedVec1(), a.x * b.y - a.y * b.x);
    }
    public static bool operator ==(FixedVec2 a, FixedVec2 b)
    {
        return a.x == b.x && a.y == b.y;
    }
    public static bool operator !=(FixedVec2 a, FixedVec2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public bool Equals(FixedVec2 f)
    {
        return this.x == f.x && this.y == f.y;
    }

    public override string ToString()
    {
        return "{" + x.ToString() + "," + y.ToString() + "}";// + ":" + ToVector3().ToString();
    }

    public override int GetHashCode()
    {
        var hashCode = 1502939027;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }

    public void Normalize()
    {
        var a = x * x + y * y;
        if (a.m_num == 0)
            return;
        var b = FixedVec1.Sqrt(a);
        x /= b;
        y /= b;
    }

}
