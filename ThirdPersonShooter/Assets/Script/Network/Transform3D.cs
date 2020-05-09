using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaoshuai
{
    public class Transform3D
    {
        /// <summary>
        /// 用来描述物体位置信息
        /// </summary>
        FixedVec3 _position;
        FixedVec3 _lastPosition;

        FixedVec3 _rotation;
        FixedVec3 _lastRotation;

        public Transform3D()
        {
            _position = new FixedVec3();
            _lastPosition = new FixedVec3();
            _rotation = new FixedVec3();
            _lastRotation = new FixedVec3();
        }

        public FixedVec3 Position
        {
            get { return _position; }
            set
            {
                if (_position == value) return;
                 _position = value;
            }
        }


        public void Reset(FixedVec3 postion, FixedVec3 rotation)
        {
            _position = postion;
            _rotation = rotation;
        }
    }

}

