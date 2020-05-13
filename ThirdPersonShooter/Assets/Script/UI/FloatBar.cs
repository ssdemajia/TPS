using Shaoshuai.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Shaoshuai.UI
{
    public class FloatBar : MonoBehaviour
    {
        //public Image volume;  // 血条
        //public Image background;  // 背景图片
        //public Text info;   // 数值内容
        public Slider slider;
        RectTransform rectTransform;
        Transform targetTransform; // 玩家位置
        int currentValue;
        int maxValue;

        private void Awake()
        {
            //volume = transform.Find("Health").GetComponent<Image>();
            //background = transform.Find("Background").GetComponent<Image>();
            //info = transform.Find("HealthInfo").GetComponent<Text>();
            slider = transform.Find("Slider").GetComponent<Slider>();
        }

        public void OnUse(Transform trans, int val, int maxVal)
        {
            Vector3 pos = trans.position;
            transform.position = FloatBarManager.Instance.camera.WorldToScreenPoint(pos);
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

        public void UpdateHP(int currentVal)
        {
            if (currentVal < 0)
                currentVal = 0;
            if (currentVal > maxValue)
                currentVal = maxValue;
            slider.value = currentVal;
        }
    }
}
