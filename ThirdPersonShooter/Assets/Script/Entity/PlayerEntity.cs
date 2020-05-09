using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shaoshuai.Component;
using Shaoshuai.Core;
using Shaoshuai.Message;

namespace Shaoshuai.Entity
{
    public class PlayerEntity: BaseEntity
    {
        public PlayerInput input;
        public MoveComponent moveComp;
        public int localPlayerId;

        public PlayerEntity()
        {
            input = new PlayerInput();
            moveComp = new MoveComponent();
            RegisterComp(moveComp);
        }

    }
}
