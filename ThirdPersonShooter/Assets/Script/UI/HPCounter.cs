using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPCounter : MonoBehaviour
{
    [SerializeField] Text text;
    
    // 显示生命值
    public void Display(int hp, int maxHP)
    {
        text.text = $"生命值:{hp}/{maxHP}";
    }
}
