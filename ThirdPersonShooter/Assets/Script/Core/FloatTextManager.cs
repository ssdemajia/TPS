using System.Collections.Generic;
using UnityEngine;
using Shaoshuai.UI;

namespace Shaoshuai.Core
{
    public class FloatTextManager: BaseManager
    {
        public static FloatTextManager Instance { get; private set; }
        //List<FloatBar> healthbars = new List<FloatBar>();
        Transform cameraTF;
        Transform Root;
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
            Root = CreateTransform("FloatTextRoot");  // 生命值栏所处
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

        public static FloatText CreateFloatText(Transform trans, int val)
        {
            return Instance.createFloatText(trans, val);
        }

        // 创建生命条
        FloatText createFloatText(Transform trans, int val)
        {
            GameObject gameObject = Instantiate(prefab, Root, false);
            FloatText floatText = gameObject.GetComponent<FloatText>();
            floatText.OnUse(trans, val.ToString());
            return floatText;
        }


    }
}
