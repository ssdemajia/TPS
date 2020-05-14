using Shaoshuai.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Shaoshuai.UI
{
    public class FloatBar : MonoBehaviour
    {
        public Slider slider;
        RectTransform rectTransform;
        Transform targetTransform; // 玩家位置
        int currentValue;
        int maxValue;
        float offset = 2.5f;
        private void Awake()
        {
            slider = transform.Find("Slider").GetComponent<Slider>();
        }

        public void OnUse(Transform trans, int val, int maxVal)
        {
            Vector3 pos = trans.position;
            transform.position = FloatBarManager.Instance.camera.WorldToScreenPoint(new Vector3(pos.x, pos.y+offset, pos.z));
            targetTransform = trans;
            currentValue = val;
            maxValue = maxVal;
            gameObject.SetActive(true);
            slider.value = val;
            slider.maxValue = maxVal;
        }

        public void DoUpdate()
        {

        }

        public void UpdateHP(int currentVal, int maxValue)
        {
            this.maxValue = maxValue;
            if (currentVal < 0)
                currentVal = 0;
            if (currentVal > maxValue)
                currentVal = maxValue;
            slider.value = currentVal;
            var pos = targetTransform.position;
            transform.position = FloatBarManager.Instance.camera.WorldToScreenPoint(new Vector3(pos.x, pos.y + offset, pos.z));
        }
    }
}
