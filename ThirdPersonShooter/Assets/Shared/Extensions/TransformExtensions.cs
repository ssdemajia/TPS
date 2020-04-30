using UnityEngine;

namespace Shared.Extensions
{
    // 必须是public static的类与方法来拓展原类型
    public static class TransformExtensions
    {
        /// <summary>
        ///  检查target在当前origin的视线中
        /// </summary>
        /// <param name="origin">视线观察者</param>
        /// <param name="target">被观察者</param>
        /// <param name="fieldOfView">视角范围，角度</param>
        /// <param name="collisionMask">检查player layer</param>
        /// <param name="offset">偏移</param>
        /// <returns>是否观察到目标</returns>
        public static bool IsInLineOfSight(this Transform origin, Vector3 target,
            float fieldOfView, LayerMask collisionMask, Vector3 offset)
        {
            Vector3 direction = target - origin.position;
            // 判断目标在视野范围内
            if (Vector3.Angle(origin.forward, direction.normalized) < fieldOfView / 2)
            {
                float distance = Vector3.Distance(origin.position, target);

                // 有其他物体阻挡了视线
                RaycastHit hit;
                if (Physics.Raycast(origin.position + offset, direction.normalized, out hit, distance, collisionMask))
                {
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}
