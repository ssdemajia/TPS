using Shaoshuai.Entity;
using Shaoshuai.Core;

namespace Shaoshuai.Component
{
    public class MoveComponent: BaseComponent
    {
        public PlayerEntity player;

        public override void BindEntity(BaseEntity be)
        {
            base.BindEntity(be);
            player = (PlayerEntity)be;
        }
    }
}
