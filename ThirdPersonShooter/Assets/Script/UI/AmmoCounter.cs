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
        text.text = string.Format("{0}/{1}", ammo, maxAmmo);
    }
}
