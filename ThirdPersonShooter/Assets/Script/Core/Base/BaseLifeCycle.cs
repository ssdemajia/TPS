using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaoshuai.Core
{
    public class BaseLifeCycle
    {
        public virtual void DoAwake() { }
        public virtual void DoStart() { }
        public virtual void DoUpdate(float deltaTime) { }
        public virtual void DoDestroy() { }
    }
}
