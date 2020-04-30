using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class NClientInput
{
    NInputController _controller;
    NKeyboards _keyboards;
    public NClientInput(NInputController controller)
    {
        _controller = controller;
        _keyboards = new NKeyboards();
    }

    public void ReceiveFrame(Protocol protocol)
    {
        _keyboards.Parse(protocol);
    }
}
