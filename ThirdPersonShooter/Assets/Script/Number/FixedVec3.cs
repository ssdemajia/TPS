using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct FixedVec3
{
    public static FixedVec3 left = new FixedVec3(-1, 0);
    public static FixedVec3 right = new FixedVec3(1, 0);
    public static FixedVec3 up = new FixedVec3(0, 1);
    public static FixedVec3 down = new FixedVec3(0, -1);
    public static FixedVec3 zero = new FixedVec3(0, 0);

    public FixedVec1 x
    {
        get;
        private set;
    }
    public FixedVec1 y
    {
        get;
        private set;
    }
    public FixedVec1 z
    {
        get;
        private set;
    }

    public FixedVec3(int x = 0, int y = 0, int z = 0)
    {
        this.x = new FixedVec1(x);
        this.y = new FixedVec1(y);
        this.z = new FixedVec1(z);

    }
    public FixedVec3(float x, float y, float z)
    {

        this.x = new FixedVec1(x);
        this.y = new FixedVec1(y);
        this.z = new FixedVec1(z);

    }
    public FixedVec3(FixedVec1 x, FixedVec1 y, FixedVec1 z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 ToVector3()
    {
        Vector3 v = new Vector3(x.ToFloat(), y.ToFloat(), z.ToFloat());
        return v;
    }

    public static FixedVec3 operator +(FixedVec3 a, FixedVec3 b)
    {
        return new FixedVec3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static FixedVec3 operator -(FixedVec3 a, FixedVec3 b)
    {
        return new FixedVec3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public FixedVec1 Dot(FixedVec3 b)
    {
        return Dot(this, b);
    }
    public static FixedVec1 Dot(FixedVec3 a, FixedVec3 b)
    {
        return a.x * b.x + b.y * a.y;
    }

    public static FixedVec3 operator -(FixedVec3 a)
    {
        return new FixedVec3(-a.x, -a.y, -a.z);
    }
    public static FixedVec2 operator *(FixedVec3 a, FixedVec2 b)
    {
        return new FixedVec2(-a.z * b.y, a.z * b.x);
    }
    public override string ToString()
    {
        return "{" + x.ToString() + "," + y.ToString() + "}";// + ":" + ToVector3().ToString();
    }

    public static bool operator ==(FixedVec3 a, FixedVec3 b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }
    public static bool operator !=(FixedVec3 a, FixedVec3 b)
    {
        return a.x != b.x || a.y != b.y || a.z != b.z;
    }

    public bool Equals(FixedVec3 f)
    {
        return this.x == f.x && this.y == f.y && this.z == f.z;
    }

}
