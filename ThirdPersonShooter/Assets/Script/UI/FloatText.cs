using Shaoshuai.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Shaoshuai.UI
{
    public class FloatText: MonoBehaviour
    {
        Transform targetTransform;
        public Text text;
        float offset = 4f;
        public void OnUse(Transform trans, string s)
        {
            text.text = s;
            Vector3 pos = trans.position;
            transform.position = FloatBarManager.Instance.camera.WorldToScreenPoint(new Vector3(pos.x, pos.y + offset, pos.z-offset));
            targetTransform = trans;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            Destroy(this.gameObject, 1);
        }
        
        private void Update()
        {
            var pos = transform.position;
            pos.y += 30 * Time.deltaTime;
            transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

    }
}
