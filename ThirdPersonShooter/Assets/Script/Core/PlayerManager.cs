using UnityEngine;

namespace Shaoshuai.Core
{
    class PlayerManager: BaseManager
    {
        //public static GameObject InstantiatePlayer(PlayerEntity player, int prefabId, FixedVec3 position)
        //{
        //    var prefab = ResourceManager.Instance.LoadPrefab(prefabId);
        //    var obj = (GameObject)GameObject.Instantiate(prefab, position.ToVector3(), Quaternion.identity);

        //    var views = obj.GetComponents<OtherPlayer>();
        //    foreach (var view in views)
        //    {
        //        view.BindEntity(player);
        //    }            
        //    player.UnityTransform = obj.transform;
        //    player.transform.Position = position;
        //    player.PrefabId = prefabId;
           
        //    player.DoAwake();
        //    player.DoStart();
        //    return obj;
        //}

        public static GameObject InstantiateLocalPlayer(int prefabId, FixedVec3 position)
        {
            var prefab = ResourceManager.Instance.LoadPrefab(prefabId);
            var obj = (GameObject)GameObject.Instantiate(prefab, position.ToVector3(), Quaternion.identity);
            return obj;
        }
        //public override void DoUpdate(FixedVec1 deltaTime)
        //{
        //    foreach (var player in GameManager.allPlayers)
        //    {
        //        player.Value.DoUpdate(deltaTime);
        //    }
        //}
    }
}
