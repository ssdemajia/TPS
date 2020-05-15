using System.Collections.Generic;
using UnityEngine;
using Shaoshuai.UI;

namespace Shaoshuai.Core
{
    public class FloatBarManager: BaseManager
    {
        public static FloatBarManager Instance { get; private set; }
        List<FloatBar> healthbars = new List<FloatBar>();
        Transform cameraTF;
        Transform healthBarRoot;
        public Camera camera;
        public GameObject prefab;
        public Canvas canvas;

        private void Awake()
        {
            Instance = this;
            camera = Camera.main; // 主摄像头
            cameraTF = camera.transform;

            //for (int i = 0; i < transform.childCount; i++)
            //{
            //    healthbars.Add(transform.GetChild(i));
            //}
            healthBarRoot = CreateTransform("HealthBarRoot");  // 生命值栏所处
        }

        Transform CreateTransform(string name)
        {
            var trans = new GameObject(name).transform;
            trans.SetParent(canvas.transform);  // 挂在canvas底下
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
            return trans;
        }

        public static FloatBar CreateFloatBar(Transform trans, int val, int maxValue)
        {
            return Instance.CreateBar(trans, val, maxValue);
        }

        // 创建生命条
        FloatBar CreateBar(Transform trans, int val, int maxValue)
        {
            GameObject gameObject = Instantiate(prefab, healthBarRoot, false);
            FloatBar floatBar = gameObject.GetComponent<FloatBar>();
            healthbars.Add(floatBar);
            //floatBar.OnUse(trans, val, maxValue);
            return floatBar;
        }

        // 更新每一个生命条
        private void Update()
        {
            
        }
    }
}
