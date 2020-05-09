using System;
using UnityEngine;

namespace Shaoshuai.Core
{
    [Serializable]
    public abstract class BaseManager: MonoBehaviour
    {
        public virtual void DoAwake() { }
        public virtual void DoStart() { }
        public virtual void DoUpdate(FixedVec1 deltaTime) { }
        public virtual void DoDestroy() { }
    }
}
