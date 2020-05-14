using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] Text text;
    
    // 显示子弹数
    public void Display(int ammo, int maxAmmo)
    {
        text.text = $"子弹数:{ammo}/{maxAmmo}";
    }
}
