using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
class Shape
{
    FixedVec1 left;
    FixedVec1 right;
    FixedVec1 up;
    FixedVec1 down;
    FixedVec1 height;
    FixedVec1 width;
    FixedVec3 _position;
    protected FixedVec2[] _points;

    public NetData data;
    public FixedVec3 Position
    {
        get
        {
            if (data != null)
            {
                return data.transform.Position;
            }
            else
            {
                return _position;
            }
        }
    }
    public Shape()
    {
        _position = FixedVec3.zero;
    }
}

