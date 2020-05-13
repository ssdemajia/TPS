using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shaoshuai.Core
{
    class ResourceManager: BaseManager
    {
        public static ResourceManager Instance { get; private set; }
        public string prefabsDir = "Prefabs/";

        // 保存id与gameobj的缓存
        public Dictionary<int, GameObject> id_prefab = new Dictionary<int, GameObject>();
        public override void DoAwake()
        {
            Instance = this;
        }

        public GameObject LoadPrefab(int id)
        {
            if (id_prefab.TryGetValue(id, out var value))
                return value;
            var prefab = (GameObject)Resources.Load(prefabsDir + "Player"); // 需要将资源放在Resources目录下
            id_prefab[id] = prefab;
            return prefab;
        }
    }
}
