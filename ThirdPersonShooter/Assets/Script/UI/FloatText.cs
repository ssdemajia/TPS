using Shaoshuai.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Shaoshuai.UI
{
    public class FloatText: MonoBehaviour
    {
        public Text text;
        float offset = 4f;
        public void OnUse(Transform trans, string s)
        {
            var randOffset = Random.Range(offset, offset * 2);
            text.text = s;
            Vector3 pos = trans.position;
            transform.position = FloatBarManager.Instance.camera.WorldToScreenPoint(
                new Vector3(pos.x, pos.y + offset, pos.z - randOffset));
            gameObject.SetActive(true);
        }

        private void Start()
        {
            Destroy(this.gameObject, .5f);
        }
        
        private void Update()
        {
            var pos = transform.position;
            pos.y += 40 * Time.deltaTime;
            transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

    }
}
