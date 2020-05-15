using Shaoshuai.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Shaoshuai.UI
{
    public class FloatBar : MonoBehaviour
    {
        Slider slider;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void UpdateHP(int currentVal, int maxValue)
        {
            if (currentVal < 0)
                currentVal = 0;
            if (currentVal > maxValue)
                currentVal = maxValue;
            slider.value = currentVal;
            slider.maxValue = maxValue;
            transform.LookAt(Camera.main.transform);
        }
    }
}
