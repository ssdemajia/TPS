using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BaseComponent
{
    public NetData data;
    public BaseComponent(NetData data)
    {
        this.data = data;
    }

    public virtual void Update() { }
}

