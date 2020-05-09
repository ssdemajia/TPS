using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shaoshuai.Core
{
    public class BaseEntity: BaseLifeCycle
    {
        public static int idCounter { get; private set; } // 用于获得entityId值
        public int EntityId;
        public int PrefabId;
        public object UnityTransform;
        public Transform3D transform = new Transform3D();
        public FixedVec1 speed = new FixedVec1(10);
        public List<BaseComponent> components = new List<BaseComponent>(); // 当前实体上所有组件

        public BaseEntity()
        {
            EntityId = ++idCounter;
        }

        public void RegisterComp(BaseComponent comp)
        {
            components.Add(comp);
            comp.BindEntity(this);
        }

        public Vector3 getPos()
        {
            return transform.Position.ToVector3();
        }

        public override void DoAwake()
        {
            foreach (var comp in components)
            {
                comp.DoAwake();
            }
        }

        public override void DoStart()
        {
            foreach (var comp in components)
            {
                comp.DoStart();
            }
        }

        public override void DoUpdate(FixedVec1 deltaTime)
        {
            foreach (var comp in components)
            {
                comp.DoUpdate(deltaTime);
            }
        }

        public override void DoDestroy()
        {
            foreach (var comp in components)
            {
                comp.DoDestroy();
            }
        }
    }
}
