using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMath
{
    static int tabCount = 18 * 4;
    /// <summary>
    /// sin值对应表
    /// </summary>
    static readonly List<FixedVec1> _m_SinTab = new List<FixedVec1>();
    public static readonly FixedVec1 PI = new FixedVec1(3.14159265f);
    protected static FixedVec1 GetSinTab(FixedVec1 r)
    {

        FixedVec1 i = new FixedVec1(r.ToInt());
        //UnityEngine.Debug.Log(i.ToInt());
        if (i.ToInt() == _m_SinTab.Count - 1)
        {
            return _m_SinTab[(int)i.ToInt()];
        }
        else
        {
            // UnityEngine.Debug.Log(i.ToInt()+":"+ _m_SinTab[i.ToInt()]+":"+ Ratio.Lerp(_m_SinTab[i.ToInt()], _m_SinTab[(i + 1).ToInt()], r - i));
            return FixedVec1.Lerp(_m_SinTab[(int)i.ToInt()], _m_SinTab[(int)(i + 1).ToInt()], r - i);
        }

    }
    public static FixedVec1 GetAsinTab(FixedVec1 sin)
    {
        FixedMath math = Instance;
        //UnityEngine.Debug.Log("GetAsinTab");
        for (int i = _m_SinTab.Count - 1; i >= 0; i--)
        {

            if (sin > _m_SinTab[i])
            {
                if (i == _m_SinTab.Count - 1)
                {
                    return new FixedVec1(i) / (tabCount / 4) * (PI / 2);
                }
                else
                {
                    //return new Ratio(i);
                    return FixedVec1.Lerp(new FixedVec1(i), new FixedVec1(i + 1), (sin - _m_SinTab[i]) / (_m_SinTab[i + 1] - _m_SinTab[i])) / (tabCount / 4) * (PI / 2);
                }
            }
        }
        return new FixedVec1();
    }
    protected static FixedMath Instance
    {
        get
        {
            if (_m_instance == null)
            {
                _m_instance = new FixedMath();

            }
            return _m_instance;
        }
    }
    protected static FixedMath _m_instance;
    protected FixedMath()
    {
        if (_m_instance == null)
        {

            _m_SinTab.Add(new FixedVec1(0f));//0
            _m_SinTab.Add(new FixedVec1(0.08715f));
            _m_SinTab.Add(new FixedVec1(0.17364f));
            _m_SinTab.Add(new FixedVec1(0.25881f));
            _m_SinTab.Add(new FixedVec1(0.34202f));//20
            _m_SinTab.Add(new FixedVec1(0.42261f));
            _m_SinTab.Add(new FixedVec1(0.5f));

            _m_SinTab.Add(new FixedVec1(0.57357f));//35
            _m_SinTab.Add(new FixedVec1(0.64278f));
            _m_SinTab.Add(new FixedVec1(0.70710f));
            _m_SinTab.Add(new FixedVec1(0.76604f));
            _m_SinTab.Add(new FixedVec1(0.81915f));//55
            _m_SinTab.Add(new FixedVec1(0.86602f));//60

            _m_SinTab.Add(new FixedVec1(0.90630f));
            _m_SinTab.Add(new FixedVec1(0.93969f));
            _m_SinTab.Add(new FixedVec1(0.96592f));
            _m_SinTab.Add(new FixedVec1(0.98480f));//80
            _m_SinTab.Add(new FixedVec1(0.99619f));

            _m_SinTab.Add(new FixedVec1(1f));


        }
    }
    public static FixedVec1 PiToAngel(FixedVec1 pi)
    {
        return pi / PI * 180;
    }
    public static FixedVec1 Asin(FixedVec1 sin)
    {
        if (sin < -1 || sin > 1) { return new FixedVec1(); }
        if (sin >= 0)
        {
            return GetAsinTab(sin);
        }
        else
        {
            return -GetAsinTab(-sin);
        }
    }
    public static FixedVec1 Sin(FixedVec1 r)
    {

        FixedMath math = Instance;
        //int tabCount = SinTab.Count*4;
        FixedVec1 result = new FixedVec1();
        r = (r * tabCount / 2 / PI);
        //int n = r.ToInt();
        while (r < 0)
        {
            r += tabCount;
        }
        while (r > tabCount)
        {
            r -= tabCount;
        }
        if (r >= 0 && r <= tabCount / 4)                // 0 ~ PI/2
        {
            result = GetSinTab(r);
        }
        else if (r > tabCount / 4 && r < tabCount / 2)       // PI/2 ~ PI
        {
            r -= new FixedVec1(tabCount / 4);
            result = GetSinTab(new FixedVec1(tabCount / 4) - r);
        }
        else if (r >= tabCount / 2 && r < 3 * tabCount / 4)    // PI ~ 3/4*PI
        {
            r -= new FixedVec1(tabCount / 2);
            result = -GetSinTab(r);
        }
        else if (r >= 3 * tabCount / 4 && r < tabCount)      // 3/4*PI ~ 2*PI
        {
            r = new FixedVec1(tabCount) - r;
            result = -GetSinTab(r);
        }

        return result;
    }
    public static FixedVec1 Abs(FixedVec1 ratio)
    {
        return FixedVec1.Abs(ratio);
    }
    public static FixedVec1 Sqrt(FixedVec1 r)
    {
        return FixedVec1.Sqrt(r);
    }

    public static FixedVec1 Cos(FixedVec1 r)
    {
        return Sin(r + PI / 2);
    }
    public static FixedVec1 SinAngle(FixedVec1 angle)
    {
        return Sin(angle / 180 * PI);
    }
    public static FixedVec1 CosAngle(FixedVec1 angle)
    {
        return Cos(angle / 180 * PI);
    }
}
