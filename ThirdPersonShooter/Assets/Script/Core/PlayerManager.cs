using UnityEngine;

namespace Shaoshuai.Core
{
    class PlayerManager: BaseManager
    {
        public static GameObject InstantiateLocalPlayer(int prefabId, Vector3 position)
        {
            var prefab = ResourceManager.Instance.LoadPrefab(prefabId);
            var obj = (GameObject)GameObject.Instantiate(prefab, position, Quaternion.identity);
            return obj;
        }
    }
}
