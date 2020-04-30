using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TransformComponent
{
    FixedVec3 _position;
    FixedVec3 _lastPosition;

    FixedVec3 _rotation;
    FixedVec3 _lastRotation;
    NetData _data;

    public TransformComponent()
    {
        _postion = new FixedVec3();
        _lastPosition = new FixedVec3();
        _rotation = new FixedVec3();
        _lastRotation = new FixedVec3();
    }

    public void Init(NetData data)
    {
        _data = data;
    }

    public FixedVec3 Position
    {
        get { return _position; }
        set
        {
            if (_position == value) return;
            if (_data.Shape == null)
            {
                _position = value;
                _lastPosition = value;
            }
            else
            {
                _position = value;
            }
        }
    }


    public void Reset(FixedVec3 postion, FixedVec3 rotation)
    {
        _position = postion;
        _rotation = rotation;
    }
}

