using Shaoshuai.Core;
using Shaoshuai.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shaoshuai.View
{
    public class PlayerView: MonoBehaviour
    {
        public PlayerEntity player;

        public void BindEntity(BaseEntity baseEntity)
        {
            player = baseEntity as PlayerEntity;
            transform.position = player.getPos();

        }

        private void Awake()
        {
            
        }

        private void Update()
        {
            var position = player.getPos();
            transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime);
            // Todo 旋转
        }
    }
}
