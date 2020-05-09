using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaoshuai.Core
{
    public class BaseComponent
    {
        public BaseEntity entity;
        public Transform3D transform;

        public virtual void BindEntity(BaseEntity be)
        {
            entity = be;
            transform = be.transform;
        }
        public virtual void DoAwake() { }
        public virtual void DoStart() { }
        public virtual void DoUpdate(FixedVec1 deltaTime) { }
        public virtual void DoDestroy() { }
    }
}
